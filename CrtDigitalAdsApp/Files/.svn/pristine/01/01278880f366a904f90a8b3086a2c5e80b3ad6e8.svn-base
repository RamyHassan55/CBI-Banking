namespace CrtDigitalAdsApp.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Common.Logging;
	using CrtCloudIntegration.Utilities;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using CrtDigitalAdsApp.Repositories;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Web.Common;

	/// <summary>
	/// Provides logic for operations with ad accounts.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class AdAccountsService : BaseService
	{

		#region Fields: Private

		private static readonly ILog Logger = LogManager.GetLogger("AdAccountsService");
		private readonly UserConnection _userConnection;
		private readonly IUINotifier _uiNotifier;
		private readonly IAdAccountRepository _adAccountRepository;
		private readonly Guid _connectedConnectionStatusId = Guid.Parse("9f00f7c6-749b-442a-a794-02b17e81d9d4");
		private readonly IAdPlatformRepository _adPlatformRepository;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AdAccountsService"/>.
		/// </summary>
		public AdAccountsService() {
			_userConnection = ClassFactory.Get<UserConnection>();
			_uiNotifier = ClassFactory.Get<IUINotifier>(CrtDigitalAdsAppConsts.UINotifierSenderName);
			_adAccountRepository = ClassFactory.Get<IAdAccountRepository>(
				new ConstructorArgument("userConnection", _userConnection), new ConstructorArgument("logger", Logger));
			_adPlatformRepository = ClassFactory.Get<IAdPlatformRepository>();
		}

		#endregion

		#region Methods: Private

		private void RunSyncProcess() {
			_userConnection.ProcessEngine.ProcessExecutor.Execute("SynchronizeAdCampaignData");
		}

		private bool SaveAdAccounts(IEnumerable<AdAccountResponse> adAccounts, string platform) {
			var isNewAccountAdded = _adAccountRepository.SaveAdAccounts(adAccounts, platform);
			if (isNewAccountAdded) {
				_uiNotifier.Notify($"refresh.adAccount.{platform}",
					"Account added successfully");
			}
			return isNewAccountAdded;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets connected ad accounts.
		/// </summary>
		/// <param name="getAdAccountsFromCreatioRequest">Model of request, which contains properties
		/// for retrieving ad accounts from Creatio.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public string[] GetAdAccountsIdentifiers(GetAdAccountsFromCreatioRequest getAdAccountsFromCreatioRequest) {
			var platformIdentifier =
				_adPlatformRepository.GetPlatformId(getAdAccountsFromCreatioRequest.PlatformName, UserConnection);
			var adAccountsIdentifiersFromDb = _adAccountRepository
				.GetAdAccounts(platformIdentifier)
				.Where(x => x.GetTypedColumnValue<Guid>("ConnectionStatusId") == _connectedConnectionStatusId)
				.Select(x => x.GetTypedColumnValue<Guid>("Id").ToString()).ToArray();
			return adAccountsIdentifiersFromDb;
		}

		/// <summary>
		/// Saves the ad accounts.
		/// </summary>
		/// <param name="saveAdAccountsRequest">Model of request, which contains properties for saving ad accounts</param>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public void SaveAdAccounts(SaveAdAccountsRequest saveAdAccountsRequest) {
			try {
				var adAccounts = UserConnection.ApplicationCache["VirtualAdAccounts"] as List<AdAccountResponse>;
				adAccounts = adAccounts?.Where(item =>
					saveAdAccountsRequest.VirtualAdAccountsIdentifiers.Contains(item.Id.ToString())).ToList();
				if (SaveAdAccounts(adAccounts, saveAdAccountsRequest.PlatformName)) {
					RunSyncProcess();
					UserConnection.ApplicationCache["VirtualAdAccounts"] = new List<AdAccountResponse>();
				}
			} catch (Exception e) {
				Logger.Error("AdAccountService_SaveAdAccounts. Error while saving ad accounts", e);
			}

		}

		#endregion

	}
}