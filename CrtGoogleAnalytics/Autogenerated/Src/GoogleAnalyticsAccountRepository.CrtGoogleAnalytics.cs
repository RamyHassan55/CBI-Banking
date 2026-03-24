namespace CrtGoogleAnalytics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtGoogleAnalytics.Dto;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Entity = Terrasoft.Core.Entities.Entity;

	#region Class: GoogleAnalyticsAccountRepositoty

	public class GoogleAnalyticsAccountRepositoty : BaseWebAnalyticsAccountRepositoty
	{

		#region Constructors: Public

		public GoogleAnalyticsAccountRepositoty(UserConnection userConnection, ILog logger)
			: base(userConnection, logger, GoogleAnalyticsConsts.GoogleAnalyticsPlatformId) {
		}

		#endregion

		#region Methods: Private

		private EntitySchemaQuery GetAnalyticsAccountUserQuery(string platformUserId) {
			var esqResult = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AnalyticsAccountUser") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("Id");
			esqResult.AddColumn("PlatformUserId");
			esqResult.AddColumn("WebAnalyticsAccount");
			esqResult.AddColumn("WebAnalyticsAccount.AccountId");
			var filterByPlatformId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "PlatformUserId", platformUserId);
			esqResult.Filters.Add(filterByPlatformId);
			return esqResult;
		}

		private EntitySchemaQuery GetAnalyticsAccountUserQuery(string platformUserId, Guid accountId) {
			var esqResult = GetAnalyticsAccountUserQuery(platformUserId);
			var filterByAccountId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "WebAnalyticsAccount", accountId);
			esqResult.Filters.Add(filterByAccountId);
			return esqResult;
		}

		private EntityCollection GetExistingAccountsForUser(string platformUserId) {
			var esqResult = GetAccountsQuery();
			var filterByPlatformUserId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal,
					"[AnalyticsAccountUser:WebAnalyticsAccount:Id].PlatformUserId", platformUserId);
			esqResult.Filters.Add(filterByPlatformUserId);
			return esqResult.GetEntityCollection(UserConnection);
		}

		private EntitySchemaQuery GetBaseStreamQuery() {
			var esqResult = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WebAnalyticsStream") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("Id");
			esqResult.AddColumn("WebAnalyticsAccount.AccountId");
			esqResult.AddColumn("GAPropertyId");
			esqResult.AddColumn("StreamId");
			return esqResult;
		}

		private bool HasActiveAccountUsers(Guid webAnalyticsAccountId) {
			var select = new Select(UserConnection)
				.Column(Func.Count("Id"))
				.From("AnalyticsAccountUser")
				.Where("WebAnalyticsAccountId").IsEqual(Column.Parameter(webAnalyticsAccountId)) as Select;
			select.SpecifyNoLockHints();
			return select.ExecuteScalar<int>() > 0;
		}

		private void UpdateAccountState(Entity webAnalyticsAccount) {
			var webAnalyticsAccountId = webAnalyticsAccount.PrimaryColumnValue;
			var connectionStatusId = HasActiveAccountUsers(webAnalyticsAccountId)
				? ConnectedStatusId
				: DisconnectedStatusId;
			webAnalyticsAccount.SetColumnValue("ConnectionStatusId", connectionStatusId);
			webAnalyticsAccount.Save();
		}

		private void RemoveAccountUser(Entity webAnalyticsAccount, string platformUserId) {
			var accountUsersQuery = GetAnalyticsAccountUserQuery(platformUserId, webAnalyticsAccount.PrimaryColumnValue);
			var accountUsers = accountUsersQuery.GetEntityCollection(UserConnection);
			if (accountUsers.Any()) {
				for (var i = accountUsers.Count; i > 0; i--) {
					accountUsers[i - 1].Delete();
				}
				UpdateAccountState(webAnalyticsAccount);
			}
		}

		private void UpdateAccounts(IEnumerable<IWebAnalyticsAccount> accountsList, EntityCollection accounts,
				string platformUserId) {
			accounts.ForEach(entity => {
				var accountName = accountsList
					.Where(a => entity.GetTypedColumnValue<string>("AccountId").Equals(a.Id))
					.Select(a => a.Name).FirstOrDefault();
				if (string.IsNullOrEmpty(accountName)) {
					RemoveAccountUser(entity, platformUserId);
				}
			});
		}

		private void SaveAccountUser(Guid accountId, string platformUserId) {
			var accountUserEntity = UserConnection.EntitySchemaManager.GetInstanceByName("AnalyticsAccountUser")
				.CreateEntity(UserConnection);
			accountUserEntity.SetDefColumnValues();
			accountUserEntity.SetColumnValue("WebAnalyticsAccountId", accountId);
			accountUserEntity.SetColumnValue("PlatformUserId", platformUserId);
			accountUserEntity.Save();
		}

		private Guid SaveNewStream(WebAnalyticsStreamModel stream, Guid accountId, IDictionary<string, object> values) {
			var schema = UserConnection.EntitySchemaManager.GetInstanceByName("WebAnalyticsStream");
			var entity = schema.CreateEntity(UserConnection);
			entity.SetDefColumnValues();
			entity.SetColumnValue("StreamId", stream.StreamId);
			entity.SetColumnValue("MeasurementId", stream.Id);
			entity.SetColumnValue("Name", stream.Name);
			entity.SetColumnValue("SiteUrl", stream.DefaultUri);
			entity.SetColumnValue("WebAnalyticsAccountId", accountId);
			foreach (var item in values) {
				if (schema.Columns.Any(x => x.Name == item.Key)) {
					entity.SetColumnValue(item.Key, item.Value);
				}
			}
			entity.Save();
			return entity.PrimaryColumnValue;
		}

		#endregion

		#region Methods: Protected

		protected Guid SaveNewAccount(IWebAnalyticsAccount account, string platformUserId) {
			var accountEntity = UserConnection.EntitySchemaManager.GetInstanceByName("WebAnalyticsAccount")
				.CreateEntity(UserConnection);
			var isAccountExists = accountEntity.FetchFromDB("AccountId", account.Id);
			if (isAccountExists) {
				accountEntity.SetColumnValue("ConnectionStatusId", ConnectedStatusId);
				accountEntity.SetColumnValue("Name", account.Name);
				accountEntity.Save();
			}
			var accountId = isAccountExists
				? accountEntity.PrimaryColumnValue
				: base.SaveNewAccount(account);
			SaveAccountUser(accountId, platformUserId);
			return accountId;
		}

		protected EntitySchemaQuery GetStreamQuery(string measurementId) {
			var esqResult = GetBaseStreamQuery();
			var filterByPlatformId =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "MeasurementId", measurementId);
			esqResult.Filters.Add(filterByPlatformId);
			return esqResult;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Saves accounts and actualizes account users.
		/// </summary>
		/// <param name="accountsList">Account models to save.</param>
		/// <param name="platformUserId">Platform user id.</param>
		public virtual void SaveAccounts(IEnumerable<IWebAnalyticsAccount> accountsList, string platformUserId) {
			try {
				if (platformUserId == null) {
					throw new ArgumentNullException("PlatformUserId is null");
				}
				var connectedAccountsIdentifiers = accountsList.Select(x => x.Id.ToString()).ToArray();
				var accountCollection = GetExistingAccountsForUser(platformUserId);
				UpdateAccounts(accountsList, accountCollection, platformUserId);
				foreach (var account in accountsList) {
					if (IsExistingAccount(account.Id, accountCollection)) {
						var existingAccount = GetExistingAccount(account.Id, accountCollection);
						if (existingAccount.GetTypedColumnValue<string>("Name") != account.Name) {
							existingAccount.SetColumnValue("Name", account.Name);
							existingAccount.Save();
						}
						continue;
					}
					SaveNewAccount(account, platformUserId);
				}
			} catch (Exception e) {
				Logger.Error(e.Message);
				throw;
			}
		}

		/// <summary>
		/// Saves web analytic streams: updates existing, adds new.
		/// </summary>
		/// <param name="streams">Collection of the stream models to save.</param>
		/// <param name="accountId">Web analytics account id.</param>
		/// <param name="values">Extra column values to save.</param>
		/// <returns>Stream id mapping.</returns>
		public virtual IDictionary<string, Guid> SaveStreams(IEnumerable<WebAnalyticsStreamModel> streams, Guid accountId,
				IDictionary<string, object> values) {
			try {
				var connectedStreamIdentifiers = streams.Select(x => x.Id).ToArray();
				var streamCollection = GetExistingStreams(accountId);
				UpdateStreams(streams, streamCollection);
				var actualStreams = new Dictionary<string, Guid>();
				foreach (var stream in streams) {
					var existingStream = streamCollection?
						.FirstOrDefault(x => x.GetTypedColumnValue<string>("StreamId") == stream.StreamId);
					var id = existingStream?.GetTypedColumnValue<Guid>("Id");
					if (existingStream == null) {
						id = SaveNewStream(stream, accountId, values);
					}
					actualStreams.Add(stream.Id, id.Value);
				}
				return actualStreams;
			} catch (Exception e) {
				Logger.Error(e.Message);
				throw;
			}
		}

		/// <summary>
		/// Returns account id mapping for user specified.
		/// </summary>
		/// <param name="platformUserId">Platform user id.</param>
		/// <returns>Returns account id mapping.</returns>
		public virtual IDictionary<string, Guid> GetAccountsForUser(string platformUserId) {
			var collection = GetExistingAccountsForUser(platformUserId);
			var result = new Dictionary<string, Guid>();
			collection.ForEach(x => {
				var accountId = x.GetTypedColumnValue<string>("AccountId");
				if (!result.ContainsKey(accountId)) {
					result.Add(accountId, x.PrimaryColumnValue);
				}
			});
			return result;
		}

		/// <summary>
		/// Returns google analytics integrated data for measurement id specified.
		/// </summary>
		/// <param name="measurementId">Google analytics stream measurement id.</param>
		/// <returns><see cref="GoogleAnalyticsStreamDto"/></returns>
		public virtual GoogleAnalyticsStreamDto GetStreamData(string measurementId) {
			var esq = GetStreamQuery(measurementId);
			var streamEntity = esq.GetEntityCollection(UserConnection).FirstOrDefault();
			if (streamEntity == null) {
				Logger.Error($"Stream with measurementId = {measurementId} not found");
				return null;
			}
			return new GoogleAnalyticsStreamDto {
				StreamId = streamEntity.GetTypedColumnValue<Guid>("Id"),
				GAStreamId = streamEntity.GetTypedColumnValue<string>("StreamId"),
				AccountId = streamEntity.GetTypedColumnValue<string>("WebAnalyticsAccount_AccountId"),
				PropertyId = streamEntity.GetTypedColumnValue<string>("GAPropertyId")
			};
		}

		/// <summary>
		/// Returns google analytics integrated data for all existed streams.
		/// </summary>
		/// <returns><see cref="GoogleAnalyticsStreamDto"/></returns>
		public virtual IEnumerable<GoogleAnalyticsStreamDto> GetAllStreamData() {
			var esq = GetBaseStreamQuery();
			var filerByAccountStatus = esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"[WebAnalyticsAccount:Id:WebAnalyticsAccount].ConnectionStatus", ConnectedStatusId);
			esq.Filters.Add(filerByAccountStatus);
			return esq.GetEntityCollection(UserConnection)
				.Select(streamEntity => new GoogleAnalyticsStreamDto {
					StreamId = streamEntity.GetTypedColumnValue<Guid>("Id"),
					GAStreamId = streamEntity.GetTypedColumnValue<string>("StreamId"),
					AccountId = streamEntity.GetTypedColumnValue<string>("WebAnalyticsAccount_AccountId"),
					PropertyId = streamEntity.GetTypedColumnValue<string>("GAPropertyId")
				});
		}

		#endregion

	}

	#endregion

}

