namespace CrtDigitalAdsApp.Models.Requests
{
	using System.Runtime.Serialization;
	using Newtonsoft.Json;

	/// <summary>
	/// Request model, which contains properties for requesting ad campaigns infos
	/// </summary>
	[DataContract]
	public class GetAdCampaignsInfosRequest : BaseDigitalAdsRequest {

		/// <summary>
		/// Ad account identifier
		/// </summary>
		[DataMember(Name = "adAccountId")]
		[JsonProperty("adAccountId")]
		public string AdAccountId { get; set; }
	}
}