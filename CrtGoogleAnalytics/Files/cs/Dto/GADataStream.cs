namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	#region Class: GADataStream

	internal class GADataStream : GAAccount
	{
		[JsonProperty("type")]
		public string StreamType { get; set; }

		[JsonProperty("webStreamData")]
		public WebStreamData StreamData { get; set; }
	}

	#endregion

	#region Class: WebStreamData

	internal class WebStreamData
	{
		[JsonProperty("measurementId")]
		public string MeasurementId { get; set; }

		[JsonProperty("defaultUri")]
		public string DefaultUri { get; set; }
	}

	#endregion

}
