namespace CrtGoogleAnalytics.API.Dto
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	internal class GAPropertiesResponse
	{
		[JsonProperty("properties")]
		public List<GAAccountProperty> Properties { get; set; }
	}
}
