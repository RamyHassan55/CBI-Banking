namespace CrtDigitalAdsApp.Providers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Common.Logging;
	using CrtDigitalAdsApp.Enums;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using CrtDigitalAdsApp.Providers.Exceptions;
	using CrtDigitalAdsApp.Providers.Responses;
	using Newtonsoft.Json;
	using Polly;
	using Polly.Retry;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Requests;
	using Terrasoft.OAuthIntegration;
	using SysSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: DigitalAdsAdAccountsProvider

	/// <inheritdoc cref="IDigitalAdsAdAccountsProvider"/>
	[DefaultBinding(typeof(IDigitalAdsAdAccountsProvider))]
	public class DigitalAdsAdAccountsProvider : IDigitalAdsAdAccountsProvider
	{

		#region Constants: Private

		private const string CurrentRequest = "CurrentRequest";

		private const string DigitalAdsAdAccountsProviderRequestExceptionTemplate =
			"DigitalAdsAdAccountsProvider. Send request failed. " +
			"RequestUri: {0}, ResponseStatusCode: {1}, " +
			"ResponseContent: {2}, Exception: {3}";

		private const string GetAdAccountsUrl = "/api/digitalads/getAdAccounts";
		private const string GetCampaignDailyInsights = "/api/digitalads/getCampaignDailyInsights";
		private const string GetCampaignInsights = "/api/digitalads/getCampaignInsights";
		private const string GetCampaignsInfosUrl = "/api/digitalads/getCampaignsInfos";

		private const string ReLoginMessage =
			"No valid tokens found for the user. Please reauthenticate to the social platforms to restore access.";

		private const int RetryCount = 3;
		private const string Scopes = "digital_ads.service.full_access";
		private const string SmPlatformUrlSysSettingCode = "SocialPlatformServiceUrl";

		#endregion

		#region Fields: Private

		private ILog _logger;

		private readonly ErrorReason[] _loggedErrorReasons =
			Enum.GetValues(typeof(ErrorReason)).Cast<ErrorReason>().ToArray();

		private bool _reLoginLogged;
		private IHttpRequestClient _httpRequestClient;
		private HttpClient _httpClient;
		private readonly UserConnection _userConnection;
		private readonly RetryPolicy<IHttpResponse> _retryPolicy;
		private readonly Context _retryContext;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="DigitalAdsAdAccountsProvider"/>
		/// </summary>
		/// <param name="userConnection">User connection</param>
		public DigitalAdsAdAccountsProvider(UserConnection userConnection) {
			_userConnection = userConnection;
			_retryPolicy = Policy.Handle<Exception>()
				.OrResult<IHttpResponse>(x => !x.IsSuccessStatusCode)
				.WaitAndRetry(RetryCount,
					retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					(exception, timeSpan, retryIteration, context) => OnRetry(context, retryIteration));
			_retryContext = new Context("DigitalAdsAdAccountsProvider", new Dictionary<string, object>());
		}

		#endregion

		#region Properties: Private

		private ILog Logger => _logger ?? (_logger = LogManager.GetLogger(nameof(DigitalAdsAdAccountsProvider)));

		#endregion

		#region Properties: Public

		[Obsolete("Use HttpRequestClient property instead")]
		public HttpClient HttpClient {
			get {
				if (_httpClient != null) {
					return _httpClient;
				}
				var platformServiceUrl = SysSettings.GetValue(_userConnection, SmPlatformUrlSysSettingCode);
				var baseAddress = new Uri(platformServiceUrl.ToString());
				_httpClient = new HttpClient {
					BaseAddress = baseAddress
				};
				return _httpClient;
			}
			set => _httpClient = value;
		}

		public IHttpRequestClient HttpRequestClient {
			get {
				if (_httpRequestClient != null) {
					return _httpRequestClient;
				}
				_httpRequestClient = ClassFactory.Get<IHttpRequestClient>();
				return _httpRequestClient;
			}
			set => _httpRequestClient = value;
		}

		#endregion

		#region Methods: Private

		private static string GetAccessToken() {
			var identityServiceWrapper = GlobalAppSettings.FeatureUseSeparateSettingsForOAuth20
				? ClassFactory.Get<IIdentityServiceWrapper>("ExternalAccess")
				: ClassFactory.Get<IIdentityServiceWrapper>();
			var token = identityServiceWrapper.GetAccessToken(Scopes);
			return token;
		}

		private ResponseResult<string> CreateErrorResponse(ErrorReason reason) {
			return new ResponseResult<string> {
				Success = false,
				ErrorReason = reason,
				Response = string.Empty
			};
		}

		private HttpRequestConfig CreateHttpRequestConfigWithToken(string endPoint) {
			var platformServiceUrl = SysSettings.GetValue(UserConnection.Current, SmPlatformUrlSysSettingCode);
			var request = new HttpRequestConfig {
				Url = new Uri(platformServiceUrl + endPoint)
			};
			request.Headers.Add("Authorization", $"Bearer {GetAccessToken()}");
			return request;
		}

		private ResponseResult<string> HandleErrorResponse(IHttpResponse httpResponse) {
			var error = httpResponse.GetResult<AdPlatformErrorResponse>();
			if (error == null) {
				LogRequestError(httpResponse.StatusCode, httpResponse.RequestConfig.Url.ToString(),
					httpResponse.Content);
				return CreateErrorResponse(ErrorReason.UnknownError);
			}
			if (!_loggedErrorReasons.Contains(error.Reason)) {
				return CreateErrorResponse(error.Reason);
			}
			switch (error.Reason) {
				case ErrorReason.NoTokensFound:
					return HandleNoTokensFoundError();
				default:
					LogRequestError(httpResponse.StatusCode, httpResponse.RequestConfig.Url.ToString(),
						httpResponse.Content);
					return CreateErrorResponse(error.Reason);
			}
		}

		private ResponseResult<string> HandleNoTokensFoundError() {
			if (_reLoginLogged) {
				return CreateErrorResponse(ErrorReason.NoTokensFound);
			}
			Logger.Warn(ReLoginMessage);
			_reLoginLogged = true;
			return CreateErrorResponse(ErrorReason.NoTokensFound);
		}

		private void LogRequestError(HttpStatusCode statusCode, string endPoint, string content,
			Exception exception = null) {
			Logger.Error(string.Format(DigitalAdsAdAccountsProviderRequestExceptionTemplate, endPoint,
				statusCode,
				content,
				exception));
		}

		private void OnRetry(Context context, int retryIteration) {
			Logger.Error(
				$"DigitalAdsAdAccountsProvider. Retry attempt {retryIteration} failed. Request: {context[CurrentRequest]}");
		}

		private IHttpResponse SendRequest(HttpRequestMethod method, string endPoint, object bodyObject) {
			var jsonContent = JsonConvert.SerializeObject(bodyObject);
			_retryContext[CurrentRequest] = jsonContent;
			var httpRequestConfig = CreateHttpRequestConfigWithToken(endPoint);
			httpRequestConfig.Method = method;
			var response =
				_retryPolicy.Execute(() => {
						var httpResponse = HttpRequestClient.SendWithJsonBody(httpRequestConfig, jsonContent);
						return httpResponse;
					},
					_retryContext);
			return response;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IDigitalAdsAdAccountsProvider.GetAdAccounts"/>
		public async Task<List<AdAccountResponse>> GetAdAccounts(GetAdAccountsRequest getAdAccountsRequest) {
			var result = await Task.Run(() =>
				SendRequest(HttpRequestMethod.POST, GetAdAccountsUrl, getAdAccountsRequest));
			if (result.IsSuccessStatusCode) {
				return result.GetResult<List<AdAccountResponse>>();
			}
			HandleErrorResponse(result);
			throw new AdPlatformException(
				$"Error occured while retrieving {getAdAccountsRequest.PlatformName} ad accounts for {getAdAccountsRequest.PlatformUserId}");
		}

		/// <inheritdoc cref="IDigitalAdsAdAccountsProvider.GetCampaignsDailyInsights"/>
		public async Task<ResponseResult<IEnumerable<AdCampaignDailyInsightsResponse>>> GetCampaignsDailyInsights(
			GetAdCampaignsDailyInsightsRequest campaignsDailyInsightRequest) {
			try {
				var result = await Task.Run(() =>
					SendRequest(HttpRequestMethod.POST, GetCampaignDailyInsights, campaignsDailyInsightRequest));
				if (result.IsSuccessStatusCode) {
					var response = result.GetResult<List<AdCampaignDailyInsightsResponse>>();
					return new ResponseResult<IEnumerable<AdCampaignDailyInsightsResponse>>
						{ Success = true, Response = response };
				}
				var errorResponse = HandleErrorResponse(result);
				return new ResponseResult<IEnumerable<AdCampaignDailyInsightsResponse>> {
					Success = false, Response = new List<AdCampaignDailyInsightsResponse>(),
					ErrorReason = errorResponse.ErrorReason
				};
			} catch (Exception e) {
				Logger.Error("DigitalAdsAdAccountsProvider_GetCampaignsDailyInsights. " +
					"Something went wrong while processing the request", e);
				return new ResponseResult<IEnumerable<AdCampaignDailyInsightsResponse>>
					{ Success = false, Response = new List<AdCampaignDailyInsightsResponse>() };
			}
		}

		/// <inheritdoc cref="IDigitalAdsAdAccountsProvider.GetCampaignsInfos"/>
		public async Task<ResponseResult<List<AdCampaignInfoResponse>>> GetCampaignsInfos(
			GetAdCampaignsInfosRequest getAdCampaignsInfosRequest) {
			try {
				var result = await Task.Run(() =>
					SendRequest(HttpRequestMethod.POST, GetCampaignsInfosUrl, getAdCampaignsInfosRequest));
				if (result.IsSuccessStatusCode) {
					var response = result.GetResult<List<AdCampaignInfoResponse>>();
					return new ResponseResult<List<AdCampaignInfoResponse>>
						{ Success = true, Response = response };
				}
				var errorResponse = HandleErrorResponse(result);
				return new ResponseResult<List<AdCampaignInfoResponse>> {
					Success = false,
					Response = new List<AdCampaignInfoResponse>(),
					ErrorReason = errorResponse.ErrorReason
				};
			} catch (Exception e) {
				Logger.Error("DigitalAdsAdAccountsProvider_GetCampaignsInfos. " +
					"Something went wrong while processing the request", e);
				return new ResponseResult<List<AdCampaignInfoResponse>>
					{ Success = false, Response = new List<AdCampaignInfoResponse>() };
			}
		}

		/// <inheritdoc cref="IDigitalAdsAdAccountsProvider.GetCampaignsInsight"/>
		public async Task<ResponseResult<AdCampaignInsightsResponse>> GetCampaignsInsight(
			GetAdCampaignsInsightsRequest getAdCampaignsInsightsRequest) {
			try {
				var result = await Task.Run(() =>
					SendRequest(HttpRequestMethod.POST, GetCampaignInsights, getAdCampaignsInsightsRequest));
				if (result.IsSuccessStatusCode) {
					var response = result.GetResult<AdCampaignInsightsResponse>();
					return new ResponseResult<AdCampaignInsightsResponse>
						{ Success = true, Response = response };
				}
				var errorResponse = HandleErrorResponse(result);
				return new ResponseResult<AdCampaignInsightsResponse> {
					Success = false,
					ErrorReason = errorResponse.ErrorReason
				};
			} catch (Exception e) {
				Logger.Error("DigitalAdsAdAccountsProvider_GetCampaignsInsight. " +
					"Something went wrong while processing the request", e);
				return new ResponseResult<AdCampaignInsightsResponse>
					{ Success = false, Response = new AdCampaignInsightsResponse() };
			}
		}

		#endregion

	}

	#endregion
}