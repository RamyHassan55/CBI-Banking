namespace CrtDigitalAdsApp.Providers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using Terrasoft.Configuration;
	using static DigitalAdsAdAccountsProvider;

	/// <summary>
	/// Provides logic for retrieving ad accounts from ad platforms
	/// </summary>
	public interface IDigitalAdsAdAccountsProvider
	{

		#region Methods: Public

		/// <summary>
		/// Gets ad account from particular ad platform
		/// </summary>
		/// <param name="getAdAccountsRequest"></param>
		/// <returns></returns>
		Task<List<AdAccountResponse>> GetAdAccounts(GetAdAccountsRequest getAdAccountsRequest);

		/// <summary>
		/// Gets ad campaigns daily insight from social platform
		/// </summary>
		/// <param name="campaignsDailyInsightRequest"></param>
		/// <returns>Collection of ad campaigns daily insights from
		/// the particular social platform of particular ad account</returns>
		Task<ResponseResult<IEnumerable<AdCampaignDailyInsightsResponse>>> GetCampaignsDailyInsights(
			GetAdCampaignsDailyInsightsRequest campaignsDailyInsightRequest);

		/// <summary>
		/// Gets ad campaigns from social platform
		/// </summary>
		/// <param name="getAdCampaignsInfosRequest"> Request model, which contains properties
		/// for requesting ad campaigns infos</param>
		/// <returns>Collection of ad campaigns infos from
		/// the particular social platform of particular ad account</returns>
		Task<ResponseResult<List<AdCampaignInfoResponse>>> GetCampaignsInfos(GetAdCampaignsInfosRequest getAdCampaignsInfosRequest);

		/// <summary>
		/// Gets ad campaigns insight from social platform
		/// </summary>
		/// <param name="getAdCampaignsInsightsRequest"> Request model, which contains properties
		/// for requesting ad campaigns insight</param>
		/// <returns>Collection of ad campaigns insight from
		/// the particular social platform of particular ad account</returns>
		Task<ResponseResult<AdCampaignInsightsResponse>> GetCampaignsInsight(
			GetAdCampaignsInsightsRequest getAdCampaignsInsightsRequest);

		#endregion

	}
}