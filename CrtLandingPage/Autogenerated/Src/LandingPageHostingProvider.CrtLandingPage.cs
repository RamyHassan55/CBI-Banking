namespace CrtLandingPage
{
	using System;
	using System.Net;
	using Common.Logging;
	using CrtLandingPage.Requests;
	using Terrasoft.Common;
	using Terrasoft.Common.Json;
	using Terrasoft.Configuration.BpmonlineCloudIntegration;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Requests;

	#region Class: LandingPageHostingProvider

	/// <summary>
	/// Implementation of the <see cref="ILandingPageHostingProvider"/> interface.
	/// Represents class that provides hosting actions for landing pages.
	/// </summary>
	[DefaultBinding(typeof(ILandingPageHostingProvider))]
	public class LandingPageHostingProvider : ILandingPageHostingProvider
	{

		#region Constants: Private

		private const string _serviceApiScope = "landing_page_hosting.service";
		private const string _hostingServiceUrlSysSettingName = "LandingPageHostingServiceUrl";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for the <see cref="LandingPageHostingProvider"/> class.
		/// </summary>
		/// <param name="userConnection"></param>
		public LandingPageHostingProvider(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Properties: Private

		private IdentityServerProvider _identityServerProvider;
		/// <summary>
		/// Identity server provider instance. Returns established provider.
		/// If established is null then create instance of <see cref="IdentityServerProvider"/> 
		/// from <see cref="ClassFactory"/>.
		/// </summary>
		private IdentityServerProvider IdentityServerProvider {
			get {
				if (_identityServerProvider == null) {
					var userConnectionArg = new ConstructorArgument("userConnection", UserConnection);
					_identityServerProvider = ClassFactory.Get<IdentityServerProvider>(userConnectionArg);
				}
				return _identityServerProvider;
			}
			set {
				_identityServerProvider = value;
			}
		}

		private string _hostingServiceUrl;
		protected string HostingServiceUrl {
			get {
				if (string.IsNullOrWhiteSpace(_hostingServiceUrl)) {
					var result = SysSettings.TryGetValue(UserConnection, _hostingServiceUrlSysSettingName,
						out var urlValue);
					if (result) {
						_hostingServiceUrl = urlValue?.ToString();
					}
					if (string.IsNullOrWhiteSpace(_hostingServiceUrl)) {
						var message = new LocalizableString(UserConnection.ResourceStorage, nameof(LandingPageHostingProvider),
							"LocalizableStrings.HostingServiceUrlIsNotValid.Value");
						Logger.Error(message);
						throw new Exception(message);
					}
				}
				return _hostingServiceUrl;
			}
			set {
				_hostingServiceUrl = value;
			}
		}

		private Uri PublishPageUrl => new Uri(new Uri(HostingServiceUrl, UriKind.Absolute), "api/landing/publish");

		private Uri UpdatePageUrl => new Uri(new Uri(HostingServiceUrl, UriKind.Absolute), "api/landing/update");

		private Uri UnpublishPageUrl => new Uri(new Uri(HostingServiceUrl, UriKind.Absolute), "api/landing/unpublish");

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Instance of the <see cref="UserConnection"/> class."/>
		/// </summary>
		protected UserConnection UserConnection { get; }

		private IHttpRequestClient _httpRequestClient;
		/// <summary>
		/// Gets or sets the rest client for Api calls.
		/// </summary>
		protected IHttpRequestClient HttpRequestClient {
			get {
				if (_httpRequestClient == null) {
					_httpRequestClient = ClassFactory.Get<IHttpRequestClient>();
				}
				return _httpRequestClient;
			}
			set {
				_httpRequestClient = value;
			}
		}

		/// <summary>
		/// Instance of the <see cref="ILog"/> class.
		/// </summary>
		private ILog _logger;
		protected ILog Logger {
			get {
				return _logger ?? (_logger = LogManager.GetLogger("LandingPageHosting"));
			}
		}

		#endregion

		#region Methods: Private

		private HttpRequestConfig CreateRequestConfig(HttpRequestMethod method, Uri url) {
			string token = IdentityServerProvider.GetAccessTokenWithExceptionHandle(_serviceApiScope);
			return new HttpRequestConfig {
				Url = url,
				Method = method,
				Headers = {
					{ "Authorization", $"Bearer {token}" },
					{ "ContentType", "application/json" }
				}
			};
		}

		private LandingHostingActionResult GetFailedActionResult(Exception ex, string message) =>
			new LandingHostingActionResult {
				Success = false,
				ErrorMessage = message ?? ex?.Message,
				Exception = ex
			};

		#endregion

		#region Methods: Protected

		protected virtual LandingHostingActionResult SendRequest(HttpRequestConfig requestConfig) {
			var response = HttpRequestClient.SendWithJsonBody(requestConfig);
			if (response == null || response.StatusCode != HttpStatusCode.OK) {
				var message = response?.Exception?.Message ?? response?.Content ?? response?.ReasonPhrase;
				Logger.Error($"LandingPageHostingProvider.SendRequest: url '{requestConfig.Url}'" +
					$", exception: {message}");
				return GetFailedActionResult(response?.Exception, message);
			}
			return new LandingHostingActionResult {
				HostingInfo = Json.Deserialize<LandingPageHostingInfo>(response.Content)
			};
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Sends request to the hosting service to publish the landing page.
		/// </summary>
		/// <param name="request">Instance of the <see cref="LandingPagePublishRequest"/></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		public virtual LandingHostingActionResult Publish(LandingPagePublishRequest request) {
			try {
				var requestConfig = CreateRequestConfig(HttpRequestMethod.POST, PublishPageUrl);
				requestConfig.Body = request;
				return SendRequest(requestConfig);
			} catch (Exception ex) {
				return GetFailedActionResult(ex, ex.Message);
			}
		}

		/// <summary>
		/// Sends request to the hosting service to unpublish the landing page.
		/// </summary>
		/// <param name="request">Instance of the <see cref="LandingPageUnpublishRequest"/></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		public virtual LandingHostingActionResult Unpublish(LandingPageUnpublishRequest request) {
			try {
				var requestConfig = CreateRequestConfig(HttpRequestMethod.POST, UnpublishPageUrl);
				requestConfig.Body = request;
				return SendRequest(requestConfig);
			} catch (Exception ex) {
				return GetFailedActionResult(ex, ex.Message);
			}
		}

		#endregion

	}

	#endregion

}

