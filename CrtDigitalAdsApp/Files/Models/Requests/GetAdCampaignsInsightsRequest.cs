namespace CrtDigitalAdsApp.Models.Requests
{
	using System.Runtime.Serialization;
	using Newtonsoft.Json;

	/// <summary>
	/// Request model, which contains properties for requesting ad campaigns insights.
	/// </summary>
	public class GetAdCampaignsInsightsRequest : BaseDigitalAdsRequest
	{

		#region Properties: Public

		/// <summary>
		/// Ad campaign identifier.
		/// </summary>
		[DataMember(Name = "adCampaignId")]
		[JsonProperty("adCampaignId")]
		public string AdCampaignId { get; set; }

		/// <summary>
		/// Ad account identifier
		/// </summary>
		[JsonProperty("adAccountId")]
		[DataMember(Name = "adAccountId")]
        public string AdAccountId { get; set; }

        #endregion

    }
}