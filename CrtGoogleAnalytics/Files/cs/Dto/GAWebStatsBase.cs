namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GAWebStatsBase

	[JsonObject]
	internal class GAWebStatsBase
	{
		[JsonProperty("streamId")]
		public string StreamId { get; set; }

		[JsonProperty("hostName")]
		public string Host { get; set; }

		[JsonProperty("pagePath")]
		public string PagePath { get; set; }

		[JsonProperty("pageTitle")]
		public string PageTitle { get; set; }

		[JsonProperty("date")]
		public string Date { get; set; }
	}

	#endregion

}
