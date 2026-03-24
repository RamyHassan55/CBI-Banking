namespace CrtDigitalAdsApp.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtDigitalAdsApp.Models.Responses;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Represent the interface for the saving ad accounts.
	/// </summary>
	public interface IAdAccountRepository
	{

		#region Methods: Public

		/// <summary>
		/// Gets the ad accounts.
		/// </summary>
		/// <param name="platformIdFilter">Platform identifier</param>
		/// <returns></returns>
		EntityCollection GetAdAccounts(Guid platformIdFilter = default);

		/// <summary>
		/// Saves the ad accounts.
		/// </summary>
		/// <param name="adAccounts">The ad accounts.</param>
		/// <param name="platform">The platform.</param>
		/// <returns>True when at least one account added.</returns>
		bool SaveAdAccounts(IEnumerable<AdAccountResponse> adAccounts, string platform);

		#endregion

	}

	/// <summary>
	/// Implements the interface for the saving ad accounts.
	/// </summary>
	/// <seealso cref="IAdAccountRepository" />
	[DefaultBinding(typeof(IAdAccountRepository))]
	public class AdAccountRepository : IAdAccountRepository
	{

		#region Fields: Private

		private readonly Guid _connectedConnectionStatusId = Guid.Parse("9f00f7c6-749b-442a-a794-02b17e81d9d4");
		private readonly Guid _disconnectedConnectionStatusId = Guid.Parse("5508ccb5-8642-4eb2-9951-abc3a4230953");
		private readonly UserConnection _userConnection;
		private readonly ILog _logger;
		private readonly AdPlatformRepository _adPlatformRepository = new AdPlatformRepository();

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AdAccountRepository"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="logger">The logger.</param>
		public AdAccountRepository(UserConnection userConnection, ILog logger) {
			_userConnection = userConnection;
			_logger = logger;
		}

		#endregion

		#region Methods: Private

		private void UpdateAdAccountsValues(List<AdAccountResponse> adAccounts, string platformUserId) {
			var existingAdAccounts = GetExistingAdAccounts(adAccounts);
			foreach (var adAccount in existingAdAccounts) {
				var matchingAccount = adAccounts.FirstOrDefault(a => a.PlatformAdAccountId == adAccount.GetTypedColumnValue<string>("AccountId"));
				if (matchingAccount == null) {
					continue;
				}
				adAccount.SetColumnValue("PlatformUserId", platformUserId);
				adAccount.SetColumnValue("Name", matchingAccount.Name);
				adAccount.Save();
			}
		}

		private EntityCollection GetExistingAccount(AdAccountResponse adAccount) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdAccount") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("AccountId");
			esqResult.AddColumn("Name");
			var filterByName =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Name", adAccount.Name);
			esqResult.Filters.Add(filterByName);
			var filterByAdAccountId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "AccountId",
					adAccount.PlatformAdAccountId);
			esqResult.Filters.Add(filterByAdAccountId);
			var entities = esqResult.GetEntityCollection(_userConnection);
			return entities;
		}

		private EntityCollection GetExistingAdAccounts(List<AdAccountResponse> adAccounts) {
			var accountIds = adAccounts.Select(x => x.PlatformAdAccountId);
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdAccount") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("AccountId");
			esqResult.AddColumn("PlatformUserId");
			esqResult.AddColumn("Name");
			var filterByAdAccountId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "AccountId", accountIds);
			esqResult.Filters.Add(filterByAdAccountId);
			var entities = esqResult.GetEntityCollection(_userConnection);
			return entities;
		}

		private string GetPlatformUserId(IEnumerable<AdAccountResponse> adAccounts) {
			var platformUserIdFromCache = _userConnection.ApplicationCache["PlatformUserId"] as string;
			var platformUserId = string.IsNullOrWhiteSpace(platformUserIdFromCache) ?
				adAccounts?.FirstOrDefault()?.PlatformUserId : platformUserIdFromCache;
			if (string.IsNullOrWhiteSpace(platformUserId)) {
				_logger.Error($"{nameof(SaveAdAccounts)}. PlatformUserId is null");
				throw new ArgumentNullOrEmptyException(nameof(platformUserId));
			}
			return platformUserId;
		}

		private bool SaveNewAccount(Guid platformId, AdAccountResponse adAccount) {
			var adAccountEntity = _userConnection.EntitySchemaManager.GetInstanceByName("AdAccount")
				.CreateEntity(_userConnection);
			adAccountEntity.SetDefColumnValues();
			adAccountEntity.SetColumnValue("AccountId", adAccount.PlatformAdAccountId);
			adAccountEntity.SetColumnValue("Name", adAccount.Name);
			adAccountEntity.SetColumnValue("AdAccountUrl", adAccount.AdAccountUrl);
			adAccountEntity.SetColumnValue("AdPlatformId", platformId);
			adAccountEntity.SetColumnValue("ConnectionStatusId", _connectedConnectionStatusId);
			adAccountEntity.SetColumnValue("PlatformUserId", adAccount.PlatformUserId);
			var accountAdded = adAccountEntity.Save();
			return accountAdded;
		}

		private bool SetNewConnectionStatuses(string[] connectedAdAccountsIds, Guid platform, string platformUserId) {
			var atLeastOneReconnected = false;
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdAccount") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("ConnectionStatus");
			esqResult.AddColumn("Id");
			var filterByPlatformId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "AdPlatform", platform);
			var filterByPlatformUserId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "PlatformUserId", platformUserId);
			esqResult.Filters.Add(filterByPlatformId);
			esqResult.Filters.Add(filterByPlatformUserId);
			var entities = esqResult.GetEntityCollection(_userConnection);
			entities.ForEach(entity => {
				var connectionStatusId =
					connectedAdAccountsIds.Any(connectedAdAccountId =>
						entity.PrimaryColumnValue.ToString().Equals(connectedAdAccountId))
						? _connectedConnectionStatusId : _disconnectedConnectionStatusId;
				entity.SetColumnValue("ConnectionStatusId", connectionStatusId);
				entity.Save();
				atLeastOneReconnected = true;
			});

			return atLeastOneReconnected;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets ad accounts from database.
		/// </summary>
		/// <param name="platformIdFilter">Ad platform identifier</param>
		/// <returns>Collection of ad accounts entities</returns>
		public EntityCollection GetAdAccounts(Guid platformIdFilter = default) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "AdAccount") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("AccountId");
			esqResult.AddColumn("Name");
			esqResult.AddColumn("ConnectionStatus");
			esqResult.AddColumn("PlatformUserId");
			if (platformIdFilter != default) {
				var filterByPlatformId =
					esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "AdPlatform", platformIdFilter);
				esqResult.Filters.Add(filterByPlatformId);
			}
			var entities = esqResult.GetEntityCollection(_userConnection);
			return entities;
		}

		/// <summary>
		/// Saves the ad accounts.
		/// </summary>
		/// <param name="adAccounts">The ad accounts.</param>
		/// <param name="platform">The platform.</param>
		/// <returns>
		/// True when at least one account added.
		/// </returns>
		public bool SaveAdAccounts(IEnumerable<AdAccountResponse> adAccounts, string platform) {
			var newAccountAdded = false;
			bool atLeastOneReconnected;
			try {
				var platformUserId = GetPlatformUserId(adAccounts);
				if (adAccounts.IsNotEmpty()) {
					UpdateAdAccountsValues(adAccounts.ToList(), platformUserId);
				}
				var connectedAccountsIdentifiers = adAccounts.Select(x => x.Id.ToString()).ToArray();
				var platformId = _adPlatformRepository.GetPlatformId(platform, _userConnection);
				atLeastOneReconnected =
					SetNewConnectionStatuses(connectedAccountsIdentifiers, platformId, platformUserId);
				foreach (var adAccount in adAccounts) {
					var entities = GetExistingAccount(adAccount);
					if (entities.Count != 0) {
						continue;
					}
					var accountAdded = SaveNewAccount(platformId, adAccount);
					if (newAccountAdded == false && accountAdded) {
						newAccountAdded = true;
					}
				}
			} catch (ArgumentNullOrEmptyException) {
				throw;
			} catch (Exception e) {
				_logger.Error(e.Message);
				throw;
			}
			return newAccountAdded || atLeastOneReconnected;
		}

		#endregion

	}
}