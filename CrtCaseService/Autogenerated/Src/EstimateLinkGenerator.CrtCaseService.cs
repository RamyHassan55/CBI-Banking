namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Web.Common;
	using Terrasoft.Web.Http.Abstractions;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: EstimateLinksGenerator

	/// <summary>
	/// Satisfaction level link generator.
	/// </summary>
	public class EstimateLinkGenerator : IMacrosInvokable
	{

		#region Constants: Private

		private const string TokenLinkFormat = "{0}/ServiceModel/CaseRatingManagementService.svc/EstimateCase/[?CaseId={1}?]";


		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="EstimateLinkGenerator"/> class.
		/// </summary>
		public EstimateLinkGenerator() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EstimateLinkGenerator"/> class.
		/// <param name="httpContextAccessor">Instance of the <see cref="IHttpContextAccessor"/>.</param>
		/// </summary>
		public EstimateLinkGenerator(IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
		}

		#endregion

		#region Properties : Private

		private IHttpContextAccessor _httpContextAccessor;
		private IHttpContextAccessor HttpContextAccessor  => _httpContextAccessor ??
				(_httpContextAccessor = HttpContext.HttpContextAccessor);

		#endregion

		#region Properties : Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection { get; set; }

		#endregion
		
		#region Methods : Protected

		/// <summary>
		/// Returns application url.
		/// </summary>
		/// <returns>Url string.</returns>
		protected virtual string GetApplicationUrl() {
			var portalSiteUrl = SystemSettings.GetValue(UserConnection, "PortalSiteUrl", string.Empty);
			if (string.IsNullOrEmpty(portalSiteUrl)) {
				HttpContext httpContext = HttpContextAccessor.GetInstance();
				return httpContext != null
					? WebUtilities.GetBaseApplicationUrl(httpContext.Request)
					: SystemSettings.GetValue(UserConnection, "SiteUrl", string.Empty);
			}		
			return portalSiteUrl;
		}

		/// <summary>
		/// Gets record identifier from argument object.
		/// </summary>
		/// <param name="argument">Argument object.</param>
		/// <returns>Record id.</returns>
		protected virtual Guid GetRecordId(object argument) {
			var recordArgument = argument as KeyValuePair<string, Guid>?;
			return recordArgument.HasValue ? recordArgument.Value.Value : Guid.Empty;
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Returns string value for macros.
		/// </summary>
		/// <param name="arguments">Arguments object.</param>
		/// <returns>Result string.</returns>
		public virtual string GetMacrosValue(object arguments) {
			Guid caseId = GetRecordId(arguments);
			return string.Format(TokenLinkFormat, GetApplicationUrl(), caseId);
		}

		#endregion

	}

	#endregion

}

