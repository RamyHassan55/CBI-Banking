namespace CrtDigitalAdsApp.Providers.Responses
{
	using CrtDigitalAdsApp.Enums;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	/// <summary>
	/// Represents error, which occurred while sending request to ad platform
	/// </summary>
	public class AdPlatformErrorResponse
	{

		#region Properties: Public

		[JsonProperty("detail")]
		public string Detail { get; set; }

		[JsonProperty("reason")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ErrorReason Reason { get; set; }

		[JsonProperty("status")]
		public int Status { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		#endregion

	}
}