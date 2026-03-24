namespace MatomoConnector.API
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core.Factories;

	#region Class: MatomoService

	[DefaultBinding(typeof(IMatomoService))]
	public class MatomoService : IMatomoService
	{

		#region Fields: Private

		private List<BaseModule> Modules = new List<BaseModule>();

		#endregion

		#region Constructors: Public

		public MatomoService(string apiUrl, string apiKey) => InitModules(apiUrl, apiKey);

		#endregion

		#region Methods: Private

		private void InitModules(string apiUrl, string apiKey) =>
			Modules = new List<BaseModule> {
				new LiveModule(apiUrl, apiKey),
				new SitesManagerModule(apiUrl, apiKey),
				new UserIdModule(apiUrl, apiKey)
			};

		#endregion

		#region Methods: Public

		/// <summary>
		/// <see cref="IMatomoService.GetUsersBySiteUrl(string)"/>.
		/// </summary>
		public IEnumerable<UserDetails> GetUsersBySiteUrl(string siteUrl, MatomoPeriod period, MatomoDate date,
				MatomoSegment segment) {
			var sitesManagerModule = Modules.Get<SitesManagerModule>();
			var userIdModule = Modules.Get<UserIdModule>();
			var siteIds = sitesManagerModule.GetSiteIdFromSiteUrl(siteUrl);
			var result = new List<UserDetails>();
			foreach (var id in siteIds) {
				var users = userIdModule.GetUsers(id, period, date, segment);
				result.AddRange(users);
			}
			return result;
		}

		/// <summary>
		/// <see cref="IMatomoService.GetLastVisitDetails(string, string, DateTime, string, IEnumerable[string])"/>.
		/// </summary>
		public LastVisitDetails GetLastVisitDetails(string siteUrl, MatomoPeriod period, MatomoDate date,
				string segmentName, IEnumerable<string> segmentValues) {
			var sitesManagerModule = Modules.Get<SitesManagerModule>();
			var siteIds = sitesManagerModule.GetSiteIdFromSiteUrl(siteUrl);
			return GetLastVisitDetails(siteIds, period, date, segmentName, segmentValues);
		}

		/// <summary>
		/// <see cref="IMatomoService.GetLastVisitDetails(List[int], string, DateTime, string, IEnumerable[string])"/>.
		/// </summary>
		public LastVisitDetails GetLastVisitDetails(List<int> siteIds, MatomoPeriod period, MatomoDate date,
				string segmentName, IEnumerable<string> segmentValues) {
			var liveModule = Modules.Get<LiveModule>();
			var segments = SegmentBuilder.Create(segmentName, SegmentOperator.EQUALS, segmentValues);
			var result = new List<VisitDetails>();
			foreach (var segment in segments) {
				var details = liveModule.GetLastVisitsDetails(siteIds, period, date, segment);
				if (details != default) {
					result.AddRange(details);
				}
			}
			return new LastVisitDetails {
				VisitDetails = result.ToArray()
			};
		}

		/// <summary>
		/// <see cref="IMatomoService.GetAllSites()"/>.
		/// </summary>
		public IEnumerable<SiteDetails> GetAllSites() {
			var sitesManagerModule = Modules.Get<SitesManagerModule>();
			return sitesManagerModule.GetAllSites();
		}

		#endregion

	}

	#endregion

}
