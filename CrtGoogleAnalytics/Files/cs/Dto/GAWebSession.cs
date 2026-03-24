namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	[JsonObject]
	internal class GAWebSession
	{
		[JsonProperty("customEvent:crt_session_id")]
		public string SessionId { get; set; }

		[JsonProperty("countryId")]
		public string CountryCode { get; set; }

		[JsonProperty("cityId")]
		public string CityId { get; set; }

		[JsonProperty("languageCode")]
		public string LanguageCode { get; set; }

		[JsonProperty("pageReferrer")]
		public string PageReferrer { get; set; }

		[JsonProperty("sessionSourceMedium")]
		public string SourceMedium { get; set; }

		[JsonProperty("sessionCampaignName")]
		public string Campaign { get; set; }
	}
}
