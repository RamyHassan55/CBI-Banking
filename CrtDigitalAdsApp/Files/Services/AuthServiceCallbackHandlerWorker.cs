namespace CrtDigitalAdsApp.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using Creatio.FeatureToggling;
	using CrtCloudIntegration.Services;
	using CrtCloudIntegration.Utilities;
	using CrtDigitalAdsApp.Utilities;
	using CrtCloudIntegration.Utilities.Errors;
	using CrtDigitalAdsApp.Models.Requests;
	using CrtDigitalAdsApp.Models.Responses;
	using CrtDigitalAdsApp.Providers;
	using CrtDigitalAdsApp.Repositories;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	[DefaultBinding(typeof(IAuthServiceCallbackHandlerWorker), Name = "digital_ads")]
	public class AuthServiceCallbackHandlerWorker : IAuthServiceCallbackHandlerWorker
	{

		#region Fields: Private

		private static readonly ILog Logger = LogManager.GetLogger("AuthServiceCallbackHandler");
		private readonly UserConnection _userConnection;
		private IDigitalAdsAdAccountsProvider _digitalAdsAdAccountsProvider;
		private IAdPlatformRepository _adPlatformRepository;
		private readonly IUINotifier _uiNotifier;
		private readonly IAdAccountRepository _adAccountRepository;
		private readonly string[] _connectedStatuses = { "ACTIVE", "ENABLED" };

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthServiceCallbackHandlerWorker"/> class.
		/// </summary>
		public AuthServiceCallbackHandlerWorker() {
			_userConnection = ClassFactory.Get<UserConnection>();
			_uiNotifier = ClassFactory.Get<IUINotifier>(CrtDigitalAdsAppConsts.UINotifierSenderName);
			_adAccountRepository = ClassFactory.Get<IAdAccountRepository>(
				new ConstructorArgument("userConnection", _userConnection), new ConstructorArgument("logger", Logger));
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets <see cref="IAdPlatformRepository"/>
		/// </summary>
		public IAdPlatformRepository AdPlatformRepository {
			get => _adPlatformRepository ?? ClassFactory.Get<IAdPlatformRepository>();
			set => _adPlatformRepository = value;
		}

		/// <summary>
		/// Gets or sets <see cref="IDigitalAdsAdAccountsProvider"/>
		/// </summary>
		public IDigitalAdsAdAccountsProvider DigitalAdsAdAccountsProvider {
			get =>
				_digitalAdsAdAccountsProvider ??
				(_digitalAdsAdAccountsProvider = new DigitalAdsAdAccountsProvider(_userConnection));
			set => _digitalAdsAdAccountsProvider = value;
		}

		#endregion

		#region Methods: Private

		private Func<AdAccountResponse, bool> IsAccountConnected() {
			return adAccount => _connectedStatuses.Any(platformStatus =>
				platformStatus.Equals(adAccount.AccountStatus, StringComparison.InvariantCultureIgnoreCase));
		}

		private void RunSyncProcess() {
			_userConnection.ProcessEngine.ProcessExecutor.Execute("SynchronizeAdCampaignData");
		}

		private void SaveAdAccounts(IEnumerable<AdAccountResponse> adAccounts, string platform) {
			var isAnyAdded = _adAccountRepository.SaveAdAccounts(adAccounts, platform);
			if (isAnyAdded) {
				_uiNotifier.Notify($"refresh.adAccount.{platform}",
					description:"Account added successfully");
			}
		}

		private void SetIdentifiersFromDb(Guid platformIdentifier, List<AdAccountResponse> adAccounts) {
			var allAdAccounts = _adAccountRepository.GetAdAccounts(platformIdentifier).ToList();
			foreach (var adAccount in adAccounts) {
				var foundEntity = allAdAccounts.FirstOrDefault(x =>
					x.GetTypedColumnValue<string>("AccountId") == adAccount.PlatformAdAccountId);
				adAccount.Id = foundEntity?.GetTypedColumnValue<Guid>("Id") ?? Guid.NewGuid();
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Handles callback request for the specified platform.
		/// </summary>
		/// <param name="platform">The platform.</param>
		/// <param name="platformUserId">The platform user identifier.</param>
		/// <param name="application">The application.</param>
		public void Handle(string platform, string platformUserId, string application) {
			List<AdAccountResponse> activeAdAccounts;
			try {
				if (string.IsNullOrWhiteSpace(platformUserId)) {
					Logger.Error($"{nameof(Handle)}: Platform user identifier is empty.");
					throw new ArgumentNullOrEmptyException(nameof(platformUserId));
				}
				var adAccounts = DigitalAdsAdAccountsProvider.GetAdAccounts(new GetAdAccountsRequest {
					PlatformName = platform,
					PlatformUserId = platformUserId,
					Application = application
				}).Result;
				activeAdAccounts = adAccounts.Where(IsAccountConnected()).ToList();
				activeAdAccounts.ForEach(x => x.PlatformUserId = platformUserId);
			} catch (Exception e) {
				Logger.Error("AuthServiceCallbackHandlerWorker Handle failed. Exception:", e);
				var error = new CouldNotGetAdAccountError();
				_uiNotifier.ReportError(error);
				return;
			}
			if (activeAdAccounts.IsEmpty()) {
				_uiNotifier.Notify("adaccounts.notfound");
				return;
			}
			if (Features.GetIsDisabled("SelectAdAccounts")) {
				SaveAdAccounts(activeAdAccounts, platform);
				RunSyncProcess();
			} else {
				var platformId = AdPlatformRepository.GetPlatformId(platform, _userConnection);
				SetIdentifiersFromDb(platformId, activeAdAccounts);
				UserConnection.Current.ApplicationCache["VirtualAdAccounts"] = activeAdAccounts;
				UserConnection.Current.ApplicationCache["PlatformUserId"] = platformUserId;
				_uiNotifier.Notify("select.adaccounts", platform, customSenderName: "CrtDigitalAdsApp.SelectAdAccounts");
			}
		}

		#endregion

	}
}