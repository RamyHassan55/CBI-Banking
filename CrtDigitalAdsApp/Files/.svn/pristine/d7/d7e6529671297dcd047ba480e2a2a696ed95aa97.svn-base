namespace CrtDigitalAdsApp.Utilities
{
	using System;
	using CrtDigitalAdsApp.Models.Responses;
	using Terrasoft.Core.Entities;

	/// <summary>
	/// Represent the interface for operating with daily insight data.
	/// </summary>
	public interface IDailyInsightRepository
	{

		#region Methods: Public

		/// <summary>
		/// Gets the ad campaigns.
		/// </summary>
		/// <returns></returns>
		EntityCollection GetAdCampaigns();

		/// <summary>
		/// Gets the last synchronize date.
		/// </summary>
		/// <param name="campaignId">The campaign identifier.</param>
		/// <returns></returns>
		DateTime GetLastSyncDate(Guid campaignId);

		/// <summary>
		/// Inserts the daily insight.
		/// </summary>
		/// <param name="dailyInsight">The daily insight.</param>
		/// <param name="adCampaignId">The ad campaign identifier.</param>
		void InsertDailyInsight(AdCampaignDailyInsightsResponse dailyInsight, Guid adCampaignId);

		/// <summary>
		/// Saves the daily insight data.
		/// </summary>
		/// <param name="dailyInsight">The daily insight.</param>
		/// <param name="adCampaignId">The ad campaign identifier.</param>
		/// <param name="insightEntity">The insight entity.</param>
		void SavesDailyInsightData(AdCampaignDailyInsightsResponse dailyInsight, Guid adCampaignId,
			Entity insightEntity);

		/// <summary>
		/// Tries the get is daily insight.
		/// </summary>
		/// <param name="adCampaignId">The ad campaign identifier.</param>
		/// <param name="adCampaignDailyInsightsResponse">The ad campaign daily insights response.</param>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		bool TryGetIsDailyInsight(Guid adCampaignId,
			AdCampaignDailyInsightsResponse adCampaignDailyInsightsResponse, out Entity entity);

		#endregion

	}
}