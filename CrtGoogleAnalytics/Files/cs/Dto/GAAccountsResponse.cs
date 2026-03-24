namespace CrtGoogleAnalytics.API.Dto
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	internal class GAAccountsResponse
	{
		[JsonProperty("accounts")]
		public List<GAAccount> Accounts { get; set; }
	}
}
