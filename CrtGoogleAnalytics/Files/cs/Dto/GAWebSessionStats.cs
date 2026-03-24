namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GAWebSessionStats

	[JsonObject]
	internal class GAWebSessionStats : GAWebStatsBase
	{
		[JsonProperty("sessionMedium")]
		public string Medium { get; set; }

		[JsonProperty("sessionSource")]
		public string Source { get; set; }

		[MetricProperty]
		[JsonProperty("sessions")]
		public string SessionCount { get; set; }
	}

	#endregion

}
