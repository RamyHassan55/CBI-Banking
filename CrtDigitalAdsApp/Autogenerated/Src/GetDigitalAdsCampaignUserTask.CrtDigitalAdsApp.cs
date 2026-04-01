namespace Terrasoft.Core.Process.Configuration
{
	using System;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using CrtDigitalAdsApp.Providers;
	using System.Collections.Generic;
	using System.Linq;
	using global::Common.Logging;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using CrtDigitalAdsApp.Repositories;
	using CrtCloudIntegration.Utilities;
	using CrtDigitalAdsApp;
	using CrtDigitalAdsApp.Enums;


	#region Class: GetDigitalAdsCampaignUserTask

	public partial class GetDigitalAdsCampaignUserTask
	{

		#region Constants: Private

		private const string Application = "digital_ads";

		#endregion

		#region Fields: Private

		private readonly Guid _connectedAdAccountStatusId = Guid.Parse("9F00F7C6-749B-442A-A794-02B17E81D9D4");

		private readonly Dictionary<string, Guid> _adCampaignStatusMapping = new Dictionary<string, Guid> {
			{ "active", Guid.Parse("6867077E-AD54-41D6-9900-D2F805C8226C") },
			{ "enabled", Guid.Parse("6867077E-AD54-41D6-9900-D2F805C8226C") },
			{ "archived", Guid.Parse("0B0E4526-212A-4C07-AD21-9FC407B8B543") },
			{ "disapproved", Guid.Parse("F58EA56E-90BB-4FBA-9FC6-D35D91894C6D") },
			{ "paused", Guid.Parse("BBCF5189-4664-4B6B-A66C-2B3954733D82") },
			{ "removed", Guid.Parse("F37CC6A4-699C-4307-800A-238582B4CF92") },
			{ "unknown", Guid.Parse("BB6F6DC4-0704-482C-81CD-18B741EB5AEF") }
		};

		private IDigitalAdsAdAccountsProvider _digitalAdsAdAccountsProvider;
		private ILog _logger;
		private AdPlatformRepository _adPlatformRepository;

		#endregion

		#region Properties: Private

		private AdPlatformRepository AdPlatformRepository =>
			_adPlatformRepository ?? (_adPlatformRepository = new AdPlatformRepository());

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
					LogManager.GetLogger("GetDigitalAdsCampaignUserTask");
			}
		}

		#endregion

		#region Methods: Private

		private static void SetColumnValues(Entity existedAdCampaign, AdCampaignInfoResponse adCampaign,
			Guid adCampaignStatusId) {
			existedAdCampaign.SetColumnValue("AdCampaignId", adCampaign.Id);
			existedAdCampaign.SetColumnValue("CampaignName", adCampaign.Name);
			existedAdCampaign.SetColumnValue("PlatformId", adCampaign.PlatformId);
			existedAdCampaign.SetColumnValue("StatusId", adCampaignStatusId);
			existedAdCampaign.SetColumnValue("AdAccountId", adCampaign.AdAccountId);
		}

		private EntityCollection GetAdAccounts() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AdAccount");
			esq.AddColumn("Id");
			esq.AddColumn("AccountId");
			esq.AddColumn("ConnectionStatus");
			esq.AddColumn("AdPlatform");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ConnectionStatus",
				_connectedAdAccountStatusId));
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			var adAccountEntities = esq.GetEntityCollection(UserConnection);
			return adAccountEntities;
		}

		private ResponseResult<List<AdCampaignInfoResponse>> GetAdCampaigns(EntityCollection adAccounts) {
			var adCampaignsResult = new List<AdCampaignInfoResponse>();
			foreach (var adAccount in adAccounts) {
				var platformId = adAccount.GetTypedColumnValue<Guid>("AdPlatformId");
				var adCampaignsInfosRequest = new GetAdCampaignsInfosRequest {
					PlatformName = AdPlatformRepository.GetPlatformName(platformId, UserConnection),
					Application = Application,
					AdAccountId = adAccount.GetTypedColumnValue<string>("AccountId")
				};
				var adCampaignsInfosResult = DigitalAdsAdAccountsProvider.GetCampaignsInfos(adCampaignsInfosRequest).Result;
				if (adCampaignsInfosResult.Success) {
					var adCampaignsInfos = adCampaignsInfosResult.Response;
					foreach (var adCampaignsInfo in adCampaignsInfos) {
						adCampaignsInfo.PlatformId = adAccount.GetTypedColumnValue<Guid>("AdPlatformId");
						adCampaignsInfo.AdAccountId = adAccount.GetTypedColumnValue<Guid>("Id");
					}
					adCampaignsResult.AddRange(adCampaignsInfos);
				}
				else {
					return new ResponseResult<List<AdCampaignInfoResponse>> { Success = false, ErrorReason = adCampaignsInfosResult.ErrorReason };
				}
			}
			return new ResponseResult<List<AdCampaignInfoResponse>> { Success = true, Response = adCampaignsResult };
		}

		private Guid GetAdCampaignStatusId(AdCampaignInfoResponse adCampaign) {
			var campaignStatus = adCampaign.Status.ToLower();
			var statusId = _adCampaignStatusMapping[campaignStatus];
			return statusId;
		}

		private Entity GetExistedAdCampaign(AdCampaignInfoResponse adCampaignInfoResponse) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AdCampaign");
			esq.AddColumn("Id");
			esq.AddColumn("AdCampaignId");
			esq.AddColumn("Platform");
			esq.AddColumn("AdAccount");
			esq.AddColumn("Status");
			esq.AddColumn("CampaignName");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AdCampaignId",
				adCampaignInfoResponse.Id));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Platform",
				adCampaignInfoResponse.PlatformId));
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			var adCampaignsEntities = esq.GetEntityCollection(UserConnection);
			if (adCampaignsEntities.Count > 1) {
				throw new DublicateDataException("AdCampaign");
			}
			return adCampaignsEntities.FirstOrDefault();
		}

		private void SaveOrUpdateAdCampaigns(List<AdCampaignInfoResponse> adCampaigns) {
			foreach (var adCampaign in adCampaigns) {
				var adCampaignEntity = GetExistedAdCampaign(adCampaign);
				var adCampaignStatusId = GetAdCampaignStatusId(adCampaign);
				if (adCampaignEntity == null) {
					var adCampaignSchema = UserConnection.EntitySchemaManager.GetInstanceByName("AdCampaign");
					adCampaignEntity = adCampaignSchema.CreateEntity(UserConnection);
					adCampaignEntity.SetDefColumnValues();
				}
				SetColumnValues(adCampaignEntity, adCampaign, adCampaignStatusId);
				adCampaignEntity.Save();
			}
		}

		#endregion

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			try {
				var notifier = ClassFactory.Get<IUINotifier>(CrtDigitalAdsAppConsts.UINotifierSenderName);
				notifier.Notify("import.started");
				var adAccounts = GetAdAccounts();
				var adCampaignsResult = GetAdCampaigns(adAccounts);
				if (adCampaignsResult.Success) {
					SaveOrUpdateAdCampaigns(adCampaignsResult.Response);
					AreCampaignsReceived = true;
				}
				else {
					AreCampaignsReceived = false;
					ErrorCode = Enum.GetName(typeof(ErrorReason), adCampaignsResult.ErrorReason);
				}
			} catch (Exception exception) {
				Logger.Warn($"Failed to process campaign infos. Exception: {exception}");
				AreCampaignsReceived = false;
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