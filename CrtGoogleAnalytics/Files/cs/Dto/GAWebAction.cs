namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GAWebAction

	[JsonObject]
	internal class GAWebAction
	{
		[JsonProperty("customEvent:crt_session_id")]
		public string SessionId { get; set; }

		[JsonProperty("dateHourMinute")]
		public string DateTime { get; set; }

		[JsonProperty("eventName")]
		public string EventName { get; set; }

		[JsonProperty("fullPageUrl")]
		public string FullPageUrl { get; set; }

		[JsonProperty("pageTitle")]
		public string PageTitle { get; set; }

		[MetricProperty]
		[JsonProperty("eventCount")]
		public string EventCount { get; set; }
	}

	#endregion

}
