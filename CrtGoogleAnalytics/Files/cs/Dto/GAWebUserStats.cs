namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GAWebUserStats

	[JsonObject]
	internal class GAWebUserStats : GAWebStatsBase
	{
		[JsonProperty("countryId")]
		public string CountryCode { get; set; }

		[JsonProperty("deviceCategory")]
		public string DeviceCategory { get; set; }

		[MetricProperty]
		[JsonProperty("activeUsers")]
		public string ActiveUsers { get; set; }
	}

	#endregion

}
