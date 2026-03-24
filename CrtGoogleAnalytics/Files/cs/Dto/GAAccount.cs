namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;
	using Terrasoft.Configuration;

	#region Class: GAAccount

	internal class GAAccount : IWebAnalyticsAccount
	{
		[JsonProperty("name")]
		public string Id { get; set; }

		[JsonProperty("displayName")]
		public string Name { get; set; }
	}

	#endregion

	#region Class: GAAccountProperty

	internal class GAAccountProperty : GAAccount
	{
	}

	#endregion

}
