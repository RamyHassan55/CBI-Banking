namespace CrtDigitalAdsApp.Utilities
{
	using System;
	using System.Linq;
	using CrtDigitalAdsApp.Models.Responses;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: DailyInsightRepository

	internal class DailyInsightRepository : IDailyInsightRepository
	{

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Internal

		internal DailyInsightRepository(UserConnection userConnection) {
			_userConnection = userConnection ?? ClassFactory.Get<UserConnection>();
		}

		#endregion

		#region Methods: Public

		public EntityCollection GetAdCampaigns() {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdCampaign");
			esq.AddColumn("Id");
			esq.AddColumn("AdCampaignId");
			esq.AddColumn("Platform");
			esq.AddColumn("AdAccount");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			EntityCollection adCampaignsEntities = esq.GetEntityCollection(_userConnection);
			return adCampaignsEntities;
		}

		public DateTime GetLastSyncDate(Guid campaignId) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdCampaignDailyInsights");
			esq.AddColumn("AdCampaign");
			EntitySchemaQueryColumn col = esq.AddColumn("DataRangeDate");
			col.OrderPosition = 0;
			col.OrderDirection = OrderDirection.Descending;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AdCampaign", campaignId));
			esq.RowCount = 1;
			Entity entity = esq.GetEntityCollection(_userConnection).FirstOrDefault();
			if (entity == null) {
				return default;
			}
			var date = entity.GetTypedColumnValue<DateTime>("DataRangeDate");
			return date;
		}

		public void InsertDailyInsight(AdCampaignDailyInsightsResponse dailyInsight, Guid adCampaignId) {
			EntitySchema insightSchema =
				_userConnection.EntitySchemaManager.GetInstanceByName("AdCampaignDailyInsights");
			Entity insightEntity = insightSchema.CreateEntity(_userConnection);
			insightEntity.SetDefColumnValues();
			SavesDailyInsightData(dailyInsight, adCampaignId, insightEntity);
		}

		public void SavesDailyInsightData(AdCampaignDailyInsightsResponse dailyInsight, Guid adCampaignId,
			Entity insightEntity) {
			insightEntity.SetColumnValue("AdCampaignId", adCampaignId);
			insightEntity.SetColumnValue("AllConversions", dailyInsight.AllConversions);
			insightEntity.SetColumnValue("Conversions", dailyInsight.Conversions);
			insightEntity.SetColumnValue("DataRangeDate", dailyInsight.Date);
			insightEntity.SetColumnValue("Reach", dailyInsight.Reach);
			insightEntity.SetColumnValue("Impressions", dailyInsight.Impressions);
			insightEntity.SetColumnValue("Frequency", dailyInsight.Frequency);
			insightEntity.SetColumnValue("CTR", dailyInsight.Ctr);
			insightEntity.SetColumnValue("CPM", dailyInsight.Cpm);
			insightEntity.SetColumnValue("CPC", dailyInsight.Cpc);
			insightEntity.SetColumnValue("Clicks", dailyInsight.Clicks);
			insightEntity.SetColumnValue("AmountSpent", dailyInsight.Spend);
			insightEntity.SetColumnValue("PrimaryAmountSpent", dailyInsight.Spend);
			insightEntity.Save();
		}

		public bool TryGetIsDailyInsight(Guid adCampaignId,
			AdCampaignDailyInsightsResponse adCampaignDailyInsightsResponse, out Entity entity) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdCampaignDailyInsights");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			esq.AddColumn("AdCampaign");
			esq.AddColumn("AllConversions");
			esq.AddColumn("DataRangeDate");
			esq.AddColumn("Reach");
			esq.AddColumn("Impressions");
			esq.AddColumn("Frequency");
			esq.AddColumn("Conversions");
			esq.AddColumn("CTR");
			esq.AddColumn("CPM");
			esq.AddColumn("CPC");
			esq.AddColumn("Clicks");
			esq.AddColumn("AmountSpent");
			esq.AddColumn("PrimaryAmountSpent");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AdCampaign", adCampaignId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DataRangeDate",
				adCampaignDailyInsightsResponse.Date));
			EntityCollection entities = esq.GetEntityCollection(_userConnection);
			entity = entities.FirstOrDefault();
			return entity != null;
		}

		#endregion

	}

	#endregion

}
