namespace CrtGoogleAnalytics.API.Models
{

	#region Class: GAWebUserStatsModel

	/// <summary>
	/// Class to represent active user GA statistics model.
	/// </summary>
	public class GAWebUserStatsModel : GAWebStatsModelBase
	{

		#region Properties: Public

		/// <summary>
		/// Metric value for active users.
		/// </summary>
		public int ActiveUsers { get; set; }

		/// <summary>
		/// ISO 3166-1 alpha-2 country code.
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// Device category name.
		/// </summary>
		public string DeviceCategory { get; set; }

		#endregion

	}

	#endregion

}
