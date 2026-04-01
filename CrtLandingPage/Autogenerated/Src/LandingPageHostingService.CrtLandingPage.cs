namespace CrtLandingPage.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Common.Logging;
	using CrtHTMLPageDesigner;
	using CrtLandingPage.Requests;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.ServiceModelContract;
	using Terrasoft.Web.Common;
	using CoreConfig = Terrasoft.Core.Configuration;

	#region Class: LandingPageHostingService

	/// <summary>
	/// Service for landing page hosting.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class LandingPageHostingService: BaseService
	{

		#region Constants: Private

		private const string _hostingProviderSysSettingName = "LandingPageHostingProviderFullClassName";

		#endregion

		#region Properties: Private

		private readonly Guid _landingStatePublished = Guid.Parse("D121222C-0A28-4AE5-8EA9-ED4A64496AA0");
		private readonly Guid _landingStateDraft = Guid.Parse("C749B940-3BA3-45BE-B836-7C38ACB7B0C1");

		private ILandingPageRepository _landingPageRepository;
		private ILandingPageRepository LandingPageRepository =>
			_landingPageRepository ?? (_landingPageRepository = ClassFactory.Get<ILandingPageRepository>());

		private ILandingPageHostingProvider _hostingProvider;
		private ILandingPageHostingProvider HostingProvider =>
			_hostingProvider ?? (_hostingProvider = GetHostingProvider());

		private IClientScriptInjector _scriptInjector;
		private IClientScriptInjector ScriptInjector =>
			_scriptInjector ?? (_scriptInjector = ClassFactory.Get<IClientScriptInjector>(
				new ConstructorArgument("userConnection", UserConnection)));

		#endregion

		#region Properties: Public

		/// <summary>
		/// Instance of the <see cref="ILog"/> class.
		/// </summary>
		private ILog _logger;
		public ILog Logger {
			get {
				return _logger ?? (_logger = LogManager.GetLogger("LandingPageHosting"));
			}
		}

		#endregion

		#region Methods: Private

		private ILandingPageHostingProvider GetDefaultHostingProvider() {
			var arg = new ConstructorArgument("userConnection", UserConnection);
			return ClassFactory.Get<ILandingPageHostingProvider>(arg);
		}

		private ILandingPageHostingProvider GetHostingProvider() {
			try {
				CoreConfig.SysSettings.TryGetValue(UserConnection, _hostingProviderSysSettingName,
					out var classNameValue);
				var className = classNameValue?.ToString();
				if (!string.IsNullOrWhiteSpace(className)) {
					var workspaceTypeProvider = ClassFactory.Get<IWorkspaceTypeProvider>();
					var classType = workspaceTypeProvider.GetType(className);
					var parameters = new object[] { UserConnection };
					var instance = Activator.CreateInstance(classType, parameters);
					if (instance is ILandingPageHostingProvider hostingProvider) {
						return hostingProvider;
					}
				}
			} catch (Exception ex) {
				Logger.Error($"Error while creating hosting provider instance: {ex.Message}");
			}
			return GetDefaultHostingProvider();
		}

		private Entity GetLandingEntity(Guid id) {
			return LandingPageRepository.GetLandingPageById(id);
		}

		private LandingHostingActionResult SetFailedResponse(HttpStatusCode code, string message = "") {
			var responseObj = new BaseResponse {
				Success = false,
				ErrorInfo = new ErrorInfo {
					Message = message
				}
			};
			throw new WebFaultException<BaseResponse>(responseObj, code);
		}

		private LandingHostingActionResult HandleResponse(LandingHostingActionResult result) {
			if (!result.Success) {
				return SetFailedResponse(HttpStatusCode.InternalServerError, result.ErrorMessage);
			}
			return result;
		}

		private void UpdateLandingUrl(Entity landing, string url) {
			landing.SetColumnValue("Url", url);
			landing.Save();
		}

		private void UpdateHtmlPageState(Entity htmlPage, Guid state) {
			htmlPage.SetColumnValue("PublicationStateId", state);
			htmlPage.Save();
		}

		private void UpdatePublishedHash(Entity page) {
			var currentHash = page.GetTypedColumnValue<string>("Hash");
			if (string.IsNullOrWhiteSpace(currentHash)) {
				currentHash = GetCurrentHash(page);
			}
			page.SetColumnValue("PublishedHash", currentHash);
		}

		private string GetCurrentHash(Entity page) {
			var hashUpdater = ClassFactory.Get<IHtmlPageHashUpdater>();
			return hashUpdater.UpdateHash(page);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Provides the ability to publish a landing page.
		/// </summary>
		/// <param name="landingId"></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "publish", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		public LandingHostingActionResult Publish(Guid landingId) {
			Entity landing = GetLandingEntity(landingId);
			if (landing == null) {
				return SetFailedResponse(HttpStatusCode.NotFound, "Landing not found");
			}
			var page = LandingPageRepository.GetLandingHtmlPages(landingId).FirstOrDefault();
			if (page == null) {
				return SetFailedResponse(HttpStatusCode.NotFound, "No designed page found");
			}
			var html = page.GetTypedColumnValue<string>("PageHtml");
			if (html.IsNullOrEmpty()) {
				return SetFailedResponse(HttpStatusCode.NotFound, "No designed page found");
			}
			var request = new LandingPagePublishRequest {
				LandingId = landingId,
				Variants = new List<LandingPageHtml> {
					new LandingPageHtml {
						HtmlPageId = page.GetTypedColumnValue<Guid>("Id"),
						Html = ScriptInjector.Inject(landingId, html),
						ContentSlug = page.GetTypedColumnValue<string>("ContentSlug")
					}
				}
			};
			var result = HostingProvider.Publish(request);
			var response = HandleResponse(result);
			if (response.Success) {
				UpdatePublishedHash(page);
				page.SetColumnValue("PublishedOn", UserConnection.CurrentUser.GetCurrentDateTime());
				UpdateLandingUrl(landing,response.HostingInfo.Url);
				UpdateHtmlPageState(page, _landingStatePublished);
			}
			return response;
		}

		/// <summary>
		/// Provides the ability to unpublish the published landing page.
		/// </summary>
		/// <param name="landingId"></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "unpublish", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		public LandingHostingActionResult Unpublish(Guid landingId) {
			Entity landing = GetLandingEntity(landingId);
			if (landing == null) {
				return SetFailedResponse(HttpStatusCode.NotFound, "Landing not found");
			}
			var page = LandingPageRepository.GetLandingHtmlPages(landingId).FirstOrDefault();
			if (page == null) {
				return SetFailedResponse(HttpStatusCode.NotFound, "No designed page found");
			}
			var request = new LandingPageUnpublishRequest {
				LandingId = landingId,
				RemoveAll = true
			};
			var result = HostingProvider.Unpublish(request);
			var response = HandleResponse(result);
			if (response.Success) {
				page.SetColumnValue("PublishedOn", null);
				page.SetColumnValue("PublishedHash", string.Empty);
				UpdateLandingUrl(landing, string.Empty);
				UpdateHtmlPageState(page, _landingStateDraft);
			}
			return response;
		}

		#endregion

	}

	#endregion

}

