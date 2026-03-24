namespace CrtEmailDesigner.Providers
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;
	using Common.Logging;
	using Creatio.ContentDesigner;
	using Newtonsoft.Json;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.OAuthIntegration;
	using GetBfTokenResponse = CrtEmailDesigner.Models.GetBfTokenResponse;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	/// <summary>
	/// Provides logic to work with Beefree tokens
	/// </summary>
	[DefaultBinding(typeof(IBfTokenProvider))]
	[Obsolete("Class will be removed in upcoming releases")]
	internal class BfTokenProvider : IBfTokenProvider
	{

		#region Constants: Private

		private const string BfTokenProviderRequestExceptionTemplate =
			"BfTokenProvider. Send request failed. " +
			"RequestUri: {0}, ResponseStatusCode: {1}, " +
			"ResponseContent: {2}, Exception: {3}";

		private const string GetBeefreeToken = "api/beefreetoken/access/";
		private const string Scope = "beefree_api";
		private const string SocialAccountUrlSysSettingCode = "SocialAccountServiceUrl";

		#endregion

		#region Fields: Private

		private HttpClient _httpClient;
		private ILog _logger;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="BfTokenProvider"/>
		/// </summary>
		/// <param name="userConnection">User connection instance</param>
		public BfTokenProvider(UserConnection userConnection) {
			var socialAccountServiceUrl = SystemSettings.GetValue(userConnection, SocialAccountUrlSysSettingCode);
			var baseAddress = new Uri(socialAccountServiceUrl.ToString());
			HttpClient = new HttpClient {
				BaseAddress = baseAddress
			};
		}

		#endregion

		#region Properties: Public

		public HttpClient HttpClient {
			get => _httpClient ?? (_httpClient = new HttpClient());
			set => _httpClient = value;
		}

		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(nameof(BfTokenProvider)));
			set => _logger = value;
		}

		#endregion

		#region Methods: Private

		private static string GetAccessToken() {
			var identityServiceWrapper = GlobalAppSettings.FeatureUseSeparateSettingsForOAuth20
				? ClassFactory.Get<IIdentityServiceWrapper>("ExternalAccess")
				: ClassFactory.Get<IIdentityServiceWrapper>();
			var token = identityServiceWrapper.GetAccessToken(Scope);
			return token;
		}

		private static HttpRequestMessage GetHttpRequestMessage(GetBfTokenRequest request, string requestUrl) {
			var token = GetAccessToken();
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
			httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(request));
			httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return httpRequestMessage;
		}

		private string GetUid() {
			var uid = SystemSettings.GetValue(UserConnection.Current,
				"EmailDesignerUId", string.Empty);
			if (!string.IsNullOrWhiteSpace(uid)) {
				return uid;
			}
			var newUid = Guid.NewGuid();
			SystemSettings.SetValue(UserConnection.Current, "EmailDesignerUId", newUid);
			return newUid.ToString();
		}

		private void LogError(string messageTemplate, HttpRequestMessage httpRequestMessage,
			string errorString, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
			Exception exception = default) {
			Logger.Error(string.Format(messageTemplate, httpRequestMessage.RequestUri, httpStatusCode,
				errorString,
				exception));
		}

		private async Task<GetBfTokenResponse> SendRequest(HttpRequestMessage httpRequestMessage) {
			HttpResponseMessage response = null;
			try {
				response = await HttpClient.SendAsync(httpRequestMessage);
				if (!response.IsSuccessStatusCode) {
					var errorContentString = response?.Content.ReadAsStringAsync().Result;
					LogError(BfTokenProviderRequestExceptionTemplate, httpRequestMessage, errorContentString,
						response.StatusCode);
					return new GetBfTokenResponse { Error = errorContentString };
				}
				var content = await response.Content.ReadAsStringAsync();
				var bfTokenResponse = JsonConvert.DeserializeObject<GetBfTokenResponse>(content);
				bfTokenResponse.IsSuccess = true;
				return bfTokenResponse;
			} catch (Exception e) {
				LogError(BfTokenProviderRequestExceptionTemplate, httpRequestMessage, e.Message, exception: e);
				return new GetBfTokenResponse { Error = e.Message };
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IBfTokenProvider.GetToken"/>
		public async Task<GetBfTokenResponse> GetToken(string designerCode) {
			var request = new GetBfTokenRequest {
				UId = GetUid()
			};
			var requestMessage = GetHttpRequestMessage(request, $"{GetBeefreeToken}{designerCode}");
			var content = await SendRequest(requestMessage);
			return content;
		}

		#endregion

	}
}