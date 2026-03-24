namespace MatomoConnector.API
{
	using System.Collections.Generic;
	using System.Linq;

	#region Class: SitesManagerModule

	/// <summary>
	/// Implementation of SitesManager module API.
	/// For more information, read https://developer.matomo.org/api-reference/reporting-api
	/// </summary>
	public class SitesManagerModule : BaseModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL and AuthToken.
		/// </summary>
		/// <param name="url">URL address of matomo service starting from http(s)://</param>
		/// <param name="apiKey">Auth token.</param>
		public SitesManagerModule(string url, string apiKey) : base(url, apiKey) {
		}

		#endregion

		#region Properties: Protected

		protected override string GetModuleName() => "SitesManager";

		#endregion

		#region Methods: Public

		/// <summary>
		/// Find site from it's URL.
		/// </summary>
		/// <returns>null - if site was not found.</returns>
		public List<int> GetSiteIdFromSiteUrl(string url) {
			var urlParameter = new SimpleParameter("url", url);
			var siteDetails = SendRequest<BaseSiteDetails>("getSitesIdFromSiteUrl", urlParameter);
			if (siteDetails == null || siteDetails.Count == 0) {
				return new List<int>();
			}
			return siteDetails.Select(x => int.Parse(x.idsite)).ToList();

		}

		/// <summary>
		/// Returns all sites for current account.
		/// </summary>
		/// <returns>null - if sites were not found.</returns>
		public List<SiteDetails> GetAllSites() {
			var siteDetails = SendRequest<SiteDetails>("getAllSites", new ParametersCollection());
			if (siteDetails == null || siteDetails.Count == 0) {
				return new List<SiteDetails>();
			}
			return siteDetails;

		}

		#endregion

	}

	#endregion

}
