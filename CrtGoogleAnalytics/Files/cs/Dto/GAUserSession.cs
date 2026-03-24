namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;

	/// <summary>
	/// Class to represent user specified session from GA.
	/// </summary>
	[JsonObject]
	public class GAUserSession
	{
		[JsonProperty("streamId")]
		public string StreamId { get; set; }

		[JsonProperty("customEvent:crt_user_id")]
		public string UserId { get; set; }

		[JsonProperty("customEvent:crt_session_id")]
		public string SessionId { get; set; }
	}
}
