namespace CrtGoogleAnalytics.API.Models
{
	using Newtonsoft.Json;

	#region Class: GAPropertyQuota

	public class GAPropertyQuota
	{
		[JsonProperty("tokensPerDay")]
		public GAQuotaState TokensPerDay { get; set; }

		[JsonProperty("tokensPerHour")]
		public GAQuotaState TokensPerHour { get; set; }

		[JsonProperty("concurrentRequests")]
		public GAQuotaState ConcurrentRequestsQauntity { get; set; }

		[JsonProperty("serverErrorsPerProjectPerHour")]
		public GAQuotaState ServerErrorsPerProjectPerHour { get; set; }

		[JsonProperty("potentiallyThresholdedRequestsPerHour")]
		public GAQuotaState PotentiallyThresholdedRequestsPerHour { get; set; }

		[JsonProperty("tokensPerProjectPerHour")]
		public GAQuotaState TokensPerProjectPerHour { get; set; }
	}

	#endregion

	#region Class: GAQuotaState

	public class GAQuotaState
	{
		[JsonProperty("consumed")]
		public int Consumed { get; set; }

		[JsonProperty("remaining")]
		public int Remaining { get; set; }
	}

	#endregion

}
