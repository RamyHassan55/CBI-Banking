namespace Terrasoft.Core.Process.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtDigitalAdsApp.Enums;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using CrtDigitalAdsApp.Providers;
	using CrtDigitalAdsApp.Repositories;
	using global::Common.Logging;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;

	#region Class: GetCampaignsInsightsUserTask

	/// <exclude/>
	public partial class GetCampaignsInsightsUserTask
	{

		#region Constants: Private

		private const string Application = "digital_ads";

		#endregion

		#region Fields: Private

		private IDigitalAdsAdAccountsProvider _digitalAdsAdAccountsProvider;
		private ILog _logger;
		private IAdPlatformRepository _adPlatformRepository;

		#endregion

		#region Properties: Private

		private IDigitalAdsAdAccountsProvider DigitalAdsAdAccountsProvider {
			get {
				if (_digitalAdsAdAccountsProvider != null) {
					return _digitalAdsAdAccountsProvider;
				}
				return _digitalAdsAdAccountsProvider =
					ClassFactory.Get<IDigitalAdsAdAccountsProvider>(new ConstructorArgument("userConnection",
						UserConnection));
			}
		}

		private ILog Logger {
			get {
				if (_logger != null) {
					return _logger;
				}
				return _logger =
					LogManager.GetLogger("GetCampaignsInsightsUserTask");
			}
		}

		private IAdPlatformRepository PlatformProvider =>
			_adPlatformRepository ?? (_adPlatformRepository = ClassFactory.Get<IAdPlatformRepository>());

		#endregion

		#region Methods: Private

		private EntityCollection GetAllCampaigns() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AdCampaign");
			esq.AddColumn("AdAccountCurrency");
			esq.AddColumn("AdCampaignId");
			esq.AddColumn("AllConversions");
			esq.AddColumn("AmountSpent");
			esq.AddColumn("Conversions");
			esq.AddColumn("CPC");
			esq.AddColumn("CPM");
			esq.AddColumn("CTR");
			esq.AddColumn("CampaignName");
			esq.AddColumn("Clicks");
			esq.AddColumn("CreatedDate");
			esq.AddColumn("EndDate");
			esq.AddColumn("Frequency");
			esq.AddColumn("AdAccount");
			esq.AddColumn("Id");
			esq.AddColumn("Impressions");
			esq.AddColumn("ModifiedBy");
			esq.AddColumn("ModifiedOn");
			esq.AddColumn("Platform");
			esq.AddColumn("PrimaryAmountSpent");
			esq.AddColumn("Reach");
			esq.AddColumn("StartDate");
			esq.AddColumn("Status");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			var adCampaignsEntities = esq.GetEntityCollection(UserConnection);
			return adCampaignsEntities;
		}

		private ResponseResult<AdCampaignInsightsResponse> GetCampaignsFromService(Entity existedCampaign) {
			try {
				var platformName =
					PlatformProvider.GetPlatformName(existedCampaign.GetTypedColumnValue<Guid>("PlatformId"),
						UserConnection);
				var adCampaignsInsightRequest = new GetAdCampaignsInsightsRequest {
					AdCampaignId = existedCampaign.GetTypedColumnValue<string>("AdCampaignId"),
					PlatformName = platformName,
					Application = Application,
					AdAccountId = GetPlatformAdAccountId(existedCampaign.GetTypedColumnValue<Guid>("AdAccountId"))
				};
				var adCampaignsInsightResult =
					DigitalAdsAdAccountsProvider.GetCampaignsInsight(adCampaignsInsightRequest).Result;
				if (adCampaignsInsightResult.Success) {
					return new ResponseResult<AdCampaignInsightsResponse> { Success = true, Response = adCampaignsInsightResult.Response };
				}
				return new ResponseResult<AdCampaignInsightsResponse> { Success = false, ErrorReason = adCampaignsInsightResult.ErrorReason };

			} catch (Exception exception) {
				Logger.Warn($"Failed to get the campaign insight from the cloud service. Exception: {exception}");
				return new ResponseResult<AdCampaignInsightsResponse> { Success = false };
			}
		}

		private string GetPlatformAdAccountId(Guid adAccountId) {
			var esqResult = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AdAccount") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("AccountId");
			var filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", adAccountId);
			esqResult.Filters.Add(filterById);
			var entities = esqResult.GetEntityCollection(UserConnection);
			return entities.First().GetTypedColumnValue<string>("AccountId");
		}

		private void UpdateCampaigns(Entity existedCampaign, AdCampaignInsightsResponse campaignFromService) {
			var campaignId = existedCampaign.GetTypedColumnValue<string>("AdCampaignId");
			if (!string.Equals(campaignFromService.CampaignId, campaignId,
					StringComparison.InvariantCultureIgnoreCase)) {
				return;
			}
			existedCampaign.SetColumnValue("AllConversions", campaignFromService.AllConversions);
			existedCampaign.SetColumnValue("Conversions", campaignFromService.Conversions);
			existedCampaign.SetColumnValue("AmountSpent", campaignFromService.Spend);
			existedCampaign.SetColumnValue("CPC", campaignFromService.Cpc);
			existedCampaign.SetColumnValue("CPM", campaignFromService.Cpm);
			existedCampaign.SetColumnValue("CTR", campaignFromService.Ctr);
			existedCampaign.SetColumnValue("Clicks", campaignFromService.Clicks);
			existedCampaign.SetColumnValue("CreatedDate", campaignFromService.CreatedDate);
			existedCampaign.SetColumnValue("AdAccountCurrency", campaignFromService.Currency);
			existedCampaign.SetColumnValue("EndDate", campaignFromService.EndDate);
			existedCampaign.SetColumnValue("Frequency", campaignFromService.Frequency);
			existedCampaign.SetColumnValue("Impressions", campaignFromService.Impressions);
			existedCampaign.SetColumnValue("ModifiedById", UserConnection.CurrentUser.ContactId);
			existedCampaign.SetColumnValue("ModifiedOn", DateTime.UtcNow);
			existedCampaign.SetColumnValue("PrimaryAmountSpent", campaignFromService.Spend);
			existedCampaign.SetColumnValue("Reach", campaignFromService.Reach);
			existedCampaign.SetColumnValue("StartDate", campaignFromService.StartDate);
			existedCampaign.Save();
		}

		#endregion

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var existedCampaigns = GetAllCampaigns();
			AreCampaignInsightsReceived = true;
			foreach (var campaign in existedCampaigns) {
				try {
					var campaignsFromServiceResult = GetCampaignsFromService(campaign);
					if (campaignsFromServiceResult.Success) {
						UpdateCampaigns(campaign, campaignsFromServiceResult.Response);
					}
					else {
						AreCampaignInsightsReceived = false;
						if (string.IsNullOrEmpty(ErrorCode)) {
							ErrorCode = Enum.GetName(typeof(ErrorReason), campaignsFromServiceResult.ErrorReason);
						}
					}
				} catch (Exception exception) {
					AreCampaignInsightsReceived = false;
					Logger.Warn($"Failed to process campaign insights. Exception: {exception}");
				}
			}
			return true;
		}

		#endregion

		#region Methods: Public

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion
}