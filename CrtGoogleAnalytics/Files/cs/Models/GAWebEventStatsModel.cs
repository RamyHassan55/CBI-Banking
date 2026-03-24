namespace CrtGoogleAnalytics.API.Models
{

	#region Class: GAWebEventStatsModel

	/// <summary>
	/// Class to represent web event GA statistics model.
	/// </summary>
	public class GAWebEventStatsModel : GAWebStatsModelBase
	{

		#region Properties: Public

		/// <summary>
		/// Metric for the web event count.
		/// </summary>
		public int EventCount { get; set; }

		/// <summary>
		/// Name of the tracked web event.
		/// </summary>
		public string EventName { get; set; }

		#endregion

	}

	#endregion

}
