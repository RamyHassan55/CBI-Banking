namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;
	using System.Collections.Generic;

	internal class GADataStreamsResponse
	{
		[JsonProperty("dataStreams")]
		public List<GADataStream> DataStreams { get; set; }
	}
}
