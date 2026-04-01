namespace Terrasoft.Configuration
{
	using System;
	using System.Linq;
	using MatomoConnector.API;
	using System.Collections.Generic;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core;
	using CoreSettings = Core.Configuration.SysSettings;

	/// <summary>
	/// Provides Creatio web-service to operate with Matomo API.
	/// </summary>
	public class MatomoServiceFacade
	{

		#region Constructors: Public

		public MatomoServiceFacade(UserConnection userConnection) => UserConnection = userConnection;

		#endregion

		#region Properties: Private

		public UserConnection UserConnection { get; set; }

		#endregion

		#region Methods: Private

		private string GetMatomoApiUrl() => CoreSettings.GetValue(UserConnection, "MatomoAPIAddress", "");
		private string GetMatomoApiKey() => CoreSettings.GetValue(UserConnection, "MatomoAPIKey", "anonymous");

		private IMatomoService GetMatomoService() {
			var apiUrl = GetMatomoApiUrl();
			var apiKey = GetMatomoApiKey();
			return ClassFactory.Get<IMatomoService>(
				new ConstructorArgument("apiUrl", apiUrl),
				new ConstructorArgument("apiKey", apiKey));
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns all details by list of <paramref name="siteIds"/>, list of <paramref name="segmentValues"/>
		/// starting from the <paramref name="startDate"/> and ending with <see cref="MatomoMagicDate.today"/>.
		/// </summary>
		public virtual LastVisitDetails GetLastVisitDetails(List<int> siteIds, DateTime startDate, string segmentName,
				IEnumerable<string> segmentValues) {
			var service = GetMatomoService();
			var period = MatomoPeriod.Range;
			var date = MatomoDate.Get(startDate, MatomoMagicDate.today);
			return service.GetLastVisitDetails(siteIds, period, date, segmentName, segmentValues);
		}

		/// <summary>
		/// Returns list of site ids filtered by urls.
		/// </summary>
		/// <param name="urls">List of site urls to find.</param>
		public virtual IEnumerable<int> GetSiteIds(IEnumerable<string> urls) {
			var service = GetMatomoService();
			var allSites = service.GetAllSites();
			return allSites
				.Where(x => !string.IsNullOrWhiteSpace(x.main_url) && urls.Any(url => x.main_url.Contains(url)))
				.Select(x => int.Parse(x.idsite))
				.ToList();
		}

		#endregion

	}

}
