namespace MatomoConnector.API
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	#region Class: LiveModule

	/// <summary>
	/// Implementation of Live module API.
	/// For more information, read https://developer.matomo.org/api-reference/reporting-api
	/// </summary>
	public class LiveModule : BaseModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL and AuthToken.
		/// </summary>
		/// <param name="url">URL address of matomo service starting from http(s)://</param>
		/// <param name="apiKey">Auth token.</param>
		public LiveModule(string url, string apiKey): base(url, apiKey) {
		}

		#endregion

		#region Methods: Protected

		protected override string GetModuleName() => "Live";

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns extensive data for each visit, which includes: server time, visitId, visitorId, visitorType
		/// (new or returning), number of pages, list of all pages (and events, file downloaded and outlinks clicked),
		/// custom variables names and values set to this visit, number of goal conversions (and list of all Goal
		/// conversions for this visit, with time of conversion, revenue, URL, etc.), but also other attributes
		/// such as: days since last visit, days since first visit, country, continent, visitor IP, provider,
		/// referrer used (referrer name, keyword if it was a search engine, full URL), campaign name and keyword,
		/// operating system, browser, type of screen, resolution, supported browser plugins (flash, java,
		/// silverlight, pdf, etc.), various dates & times format to make it easier for API users... and more!
		/// With the parameter <paramref name="segment"/> you can filter the returned visits by any criteria
		/// (visitor IP, visitor ID, country, keyword used, time of day, etc.)
		/// </summary>
		/// <param name="segment">userId==someUserId</param>
		public List<VisitDetails> GetLastVisitsDetails(List<int> idSites, MatomoPeriod period, MatomoDate date,
				MatomoSegment segment) {
			var siteIds = (idSites != default && idSites.Count > 0)
				? idSites.Select(x => x.ToString()).Aggregate((x, y) => x + ',' + y)
				: "all";
			var parameters = new ParametersCollection {
				new SimpleParameter("idSite", siteIds),
				new SimpleParameter("period", period.ToString()),
				new DateParameter("date", date),
				new SegmentParameter(segment)
			};
			return SendRequest<VisitDetails>("getLastVisitsDetails", parameters);
		}

		#endregion

	}

	#endregion

}
