namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GAWebEventStats

	[JsonObject]
	internal class GAWebEventStats : GAWebStatsBase
	{
		[JsonProperty("eventName")]
		public string EventName { get; set; }

		[MetricProperty]
		[JsonProperty("eventCount")]
		public string EventCount { get; set; }
	}

	#endregion

}
