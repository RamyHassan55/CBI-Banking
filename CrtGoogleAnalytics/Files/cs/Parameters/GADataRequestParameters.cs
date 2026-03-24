namespace CrtGoogleAnalytics.API.Parameters
{
	internal class GADataRequestParameters : DataRequestParameters
	{
		public string[] Dimensions { get; set; }
		public string[] Metrics { get; set; }
		public bool ReturnQuota { get; set; } = false;
		public EventFilterCondition EventFilter { get; set; } = EventFilterCondition.ALL;
		public string UserId { get; set; }
		public string ContactId { get; set; }
		public string[] Sessions { get; set; }
		public string[] Users { get; set; }
		public StreamWebSite[] StreamWebSites { get; set; }
	}
}
