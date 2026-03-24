namespace CrtGoogleAnalytics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtGoogleAnalytics.API.Interfaces;
	using CrtGoogleAnalytics.API.Models;
	using CrtGoogleAnalytics.API.Parameters;
	using CrtGoogleAnalytics.Dto;
	using CrtGoogleAnalytics.Messages;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Factories;
	using CoreSysSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: GoogleAnalyticsDataSyncManager

	/// <summary>
	/// Class to represent Google Analytics data synchronization manager.
	/// </summary>
	public class GoogleAnalyticsDataSyncManager
	{

		#region Class: VisitorInfo

		private class VisitorInfo
		{
			/// <summary>
			/// Google client_id.
			/// </summary>
			public string UserId { get; set; }

			/// <summary>
			/// Creatio web analytics visitor id.
			/// </summary>
			public Guid WebAnalyticsVisitorId { get; set; }

			/// <summary>
			/// Code to filter GA data by client_id.
			/// </summary>
			public string VisitorCode => $"{UserId}:";
		}

		#endregion

		#region Fields: Private

		private readonly UserConnection _userConnection;

		private static readonly Guid _trackingId = GoogleAnalyticsConsts.GoogleAnalyticsPlatformId;

		private readonly int _defaultBatchSize = 500;
		private int _syncByUserPeriodDays = 90;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsDataSyncManager"/>.
		/// </summary>
		/// <param name="userConnection"></param>
		public GoogleAnalyticsDataSyncManager(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Properties: Private

		/// <summary>
		/// Instance of <see cref="IGoogleAnalyticsRepository"/> to save analytics data.
		/// </summary>
		private IGoogleAnalyticsRepository _analyticsRepository;
		private IGoogleAnalyticsRepository AnalyticsRepository {
			get => _analyticsRepository ?? (_analyticsRepository =
				ClassFactory.Get<IGoogleAnalyticsRepository>(
					new ConstructorArgument("userConnection", _userConnection)));
			set => _analyticsRepository = value;
		}

		/// <summary>
		/// Instance of <see cref="IGADataService"/> to get analytics data.
		/// </summary>
		private IGADataService _analyticsDataService;
		private IGADataService AnalyticsDataService {
			get => _analyticsDataService ?? (_analyticsDataService =
				ClassFactory.Get<IGADataService>(
					new ConstructorArgument("userConnection", _userConnection)));
			set => _analyticsDataService = value;
		}

		/// <summary>
		/// Instance of <see cref="IGAUserDataService"/> to get user analytics data.
		/// </summary>
		private IGAUserDataService _usersDataService;
		private IGAUserDataService UsersDataService {
			get => _usersDataService ?? (_usersDataService =
				ClassFactory.Get<IGAUserDataService>(
					new ConstructorArgument("userConnection", _userConnection)));
			set => _usersDataService = value;
		}

		/// <summary>
		/// Instance of <see cref="GoogleAnalyticsAccountRepositoty"/> to get analytics account data.
		/// </summary>
		private GoogleAnalyticsAccountRepositoty _analyticsAccountRepositoty;
		private GoogleAnalyticsAccountRepositoty AnalyticsAccountRepositoty {
			get => _analyticsAccountRepositoty ?? (_analyticsAccountRepositoty =
				ClassFactory.Get<GoogleAnalyticsAccountRepositoty>(
					new ConstructorArgument("userConnection", _userConnection),
					new ConstructorArgument("logger", Logger),
					new ConstructorArgument("analitycsPlatformId", GoogleAnalyticsConsts.GoogleAnalyticsPlatformId)));
			set => _analyticsAccountRepositoty = value;
		}

		/// <summary>
		/// Instance of the <see cref="WebAnalyticsVisitorRepository"/> to save new visitors.
		/// </summary>
		private WebAnalyticsVisitorRepository _visitorRepository;
		private WebAnalyticsVisitorRepository VisitorRepository {
			get => _visitorRepository ?? (_visitorRepository = ClassFactory.Get<WebAnalyticsVisitorRepository>(
				new ConstructorArgument("userConnection", _userConnection),
				new ConstructorArgument("logger", Logger),
				new ConstructorArgument("analitycsPlatformId", GoogleAnalyticsConsts.GoogleAnalyticsPlatformId)
			));
			set => _visitorRepository = value;
		}

		/// <summary>
		/// Instance of <see cref="ITouchEventLogger"/> to log touch processing events.
		/// </summary>
		private ITouchEventLogger _eventLogger;
		private ITouchEventLogger EventLogger {
			get => _eventLogger ?? (_eventLogger =
				ClassFactory.Get<ITouchEventLogger>(new ConstructorArgument("userConnection", _userConnection)));
			set => _eventLogger = value;
		}

		#endregion

		#region Properties: Protected

		private int _batchSize;
		protected int BatchSize {
			get => _batchSize > 0 ? _batchSize : _defaultBatchSize;
			set => _batchSize = value;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		private ILog _logger;
		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger("GoogleAnalytics"));
			set => _logger = value;
		}

		/// <summary>
		/// Batch size to split visitors data for separate sync messages.
		/// </summary>
		private int _syncVisitorsBatchSize = int.MinValue;
		public int SyncVisitorsBatchSize {
			get {
				if (_syncVisitorsBatchSize < 0) {
					_syncVisitorsBatchSize = CoreSysSettings.GetValue(_userConnection,
						"GoogleAnalyticsSyncVisitorsBatchSize", 100);
				}
				return _syncVisitorsBatchSize;
			}
		}

		private TouchQueueManager _queueManager;
		public TouchQueueManager QueueManager {
			get => _queueManager ?? (_queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", _userConnection)
			));
		}

		#endregion

		#region Methods: Private

		private void LogResult(string eventName, DateTime startDate, int amount, string description = "") {
			EventLogger.Info(eventName, startDate, _trackingId, string.Empty, amount, description);
		}

		private void LogError(string eventName, DateTime startDate, Exception exception, string description = "") {
			Logger.Error($"{nameof(GoogleAnalyticsDataSyncManager)}.{eventName} " +
					"Exception: ", exception);
			EventLogger.Error(eventName, startDate, exception, _trackingId, string.Empty, description);
		}

		private GoogleAnalyticsRepositoryResult SyncWebSessionsForUsers(IEnumerable<VisitorInfo> visitors,
				DataRequestParameters parameters) {
			var users = visitors.Select(x => x.UserId).Distinct();
			var visitorMapping = visitors
				.GroupBy(v => v.VisitorCode)
				.ToDictionary(x => x.Key,
					y => y.Select(v => v.WebAnalyticsVisitorId).ToArray());
			var data = AnalyticsDataService.GetSessionsForUsers(users, parameters);
			return AnalyticsRepository.SaveSessions(data?.Events, visitorMapping);
		}

		private GoogleAnalyticsRepositoryResult SyncWebActionsForUsers(IEnumerable<VisitorInfo> visitors,
				DataRequestParameters parameters) {
			var users = visitors.Select(x => x.UserId).Distinct();
			var data = AnalyticsDataService.GetActionsForUsers(users, parameters);
			return AnalyticsRepository.SaveActions(data.Events);
		}

		private GoogleAnalyticsRepositoryResult SyncBatchedData(Func<IEnumerable<VisitorInfo>, DataRequestParameters,
				GoogleAnalyticsRepositoryResult> func, IEnumerable<VisitorInfo> visitors, DataRequestParameters parameters) {
			int rowsCount = 0;
			var repositoryResult = new GoogleAnalyticsRepositoryResult();
			parameters.Limit = BatchSize;
			do {
				parameters.Offset = repositoryResult.TotalRows;
				var result = func(visitors, parameters);
				rowsCount = result.TotalRows;
				repositoryResult.TotalRows += rowsCount;
				repositoryResult.AffectedRows += result.AffectedRows;
				repositoryResult.ErrorsCount += result.ErrorsCount;
				if (repositoryResult.ErrorsCount > 0) {
					repositoryResult.LastErrorMessage = result.LastErrorMessage;
				}
			} while (rowsCount == BatchSize);
			return repositoryResult;
		}

		private void SyncPropertyDataForVisitors(IEnumerable<VisitorInfo> visitors, DataRequestParameters parameters) {
			var startDate = DateTime.UtcNow;
			int amount = 0;
			var description = $"Sync data by visitors (UserId:{visitors.Select(x => x.UserId).GetItemsString()}). " +
				$"GA PropertyId:{parameters.PropertyId};";
			try {
				var sessionsResult = SyncBatchedData(SyncWebSessionsForUsers, visitors, parameters);
				description += $" Sessions imported: {sessionsResult.AffectedRows}" +
					$" of {sessionsResult.TotalRows};";
				if (sessionsResult.ErrorsCount > 0) {
					description += $" {sessionsResult.ErrorsCount} errors were occured." +
						$" Last error message: {sessionsResult.LastErrorMessage}.";
				}
				var actionsResult = sessionsResult.TotalRows != 0
					? SyncBatchedData(SyncWebActionsForUsers, visitors, parameters)
					: new GoogleAnalyticsRepositoryResult();
				description += $" Actions imported: {actionsResult.AffectedRows} of {actionsResult.TotalRows};";
				if (actionsResult.ErrorsCount > 0) {
					description += $" {actionsResult.ErrorsCount} errors were occured." +
						$" Last error message: {actionsResult.LastErrorMessage}.";
				}
				amount = sessionsResult.AffectedRows + actionsResult.AffectedRows;
			} catch (Exception exception) {
				LogError("SynchronizePropertyDataByUser", startDate, exception, description);
				return;
			}
			LogResult("SynchronizePropertyDataByUser", startDate, amount, description);
		}

		private void SyncPropertyDataByVisitorModels(IEnumerable<GAVisitorModel> visitorModels, DataRequestParameters parameters) {
			var visitors = visitorModels.Select(x => new VisitorInfo {
				UserId = x.VisitorId,
				WebAnalyticsVisitorId = x.WebAnalyticsVisitorId
			});
			SyncPropertyDataForVisitors(visitors, parameters);
		}

		private Select GetVisitorsToSyncSelect() {
			var select = new Select(_userConnection)
					.Column("WAV", "VisitorId")
					.Column("WAV", "WebAnalyticsStreamId").As("StreamId")
					.Column(Func.Max("WAV", "Id")).As("WebAnalyticsVisitorId")
					.Column(Func.Max("WAS", "GAPropertyId")).As("GAPropertyId")
					.Column(Func.Max("WAA", "AccountId")).As("AccountId")
					.Column(Func.Max("T", "LastActionDateTime")).As("ActionDate")
				.From("WebAnalyticsVisitor").As("WAV")
				.InnerJoin("WebAnalyticsStream").As("WAS")
					.On("WAS", "Id").IsEqual("WAV", "WebAnalyticsStreamId")
				.InnerJoin("WebAnalyticsAccount").As("WAA")
					.On("WAA", "Id").IsEqual("WAS", "WebAnalyticsAccountId")
				.LeftOuterJoin("Touch").As("T")
					.On("T", "WebAnalyticsVisitorId").IsEqual("WAV", "Id")
				.Where("WAA", "WebAnalyticsPlatformId")
					.IsEqual(Column.Parameter(GoogleAnalyticsConsts.GoogleAnalyticsPlatformId))
					.And().OpenBlock("T", "LastActionDateTime").IsNull()
						.Or("T", "LastActionDateTime").IsGreaterOrEqual(Column.Parameter(DateTime.UtcNow.Date.AddDays(-90)))
					.CloseBlock()
				.GroupBy("WAV", "VisitorId")
				.GroupBy("WAV", "WebAnalyticsStreamId") as Select;
			select.SpecifyNoLockHints();
			return select;
		}

		private IEnumerable<GAVisitorModel> GetVisitorsToSync() {
			var visitors = new List<GAVisitorModel>();
			var visitorsSelect = GetVisitorsToSyncSelect();
			visitorsSelect.ExecuteReader(reader => {
				var defaultDate = DateTime.UtcNow.AddDays(-_syncByUserPeriodDays);
				var lastActionDate = reader.GetColumnValue<DateTime>("ActionDate");
				visitors.Add(new GAVisitorModel {
					WebAnalyticsVisitorId = reader.GetColumnValue<Guid>("WebAnalyticsVisitorId"),
					VisitorId = reader.GetColumnValue<string>("VisitorId"),
					StreamId = reader.GetColumnValue<Guid>("StreamId"),
					LastActionDateTime = lastActionDate.Equals(DateTime.MinValue) ? defaultDate : lastActionDate,
					AccountId = reader.GetColumnValue<string>("AccountId"),
					GAPropertyId = reader.GetColumnValue<string>("GAPropertyId")
				});
			});
			return visitors;
		}

		private IEnumerable<GoogleAnalyticsTouchQueueMessage> GetMessagesForVisitors(
				IEnumerable<IGrouping<DateTime, GAVisitorModel>> groupedExistedVisitors) {
			var messages = new List<GoogleAnalyticsTouchQueueMessage>();
			foreach (var group in groupedExistedVisitors) {
				var startDate = group.Key;
				var visitors = group.ToList();
				var batches = GetBatchedMessages(visitors, startDate);
				messages.AddRange(batches);
			}
			return messages;
		}

		private IEnumerable<GoogleAnalyticsTouchQueueMessage> GetBatchedMessages(
				IEnumerable<GAVisitorModel> visitors, DateTime date) {
			var messages = new List<GoogleAnalyticsTouchQueueMessage>();
			var chunks = visitors.SplitOnChunks(chunkSize: SyncVisitorsBatchSize);
			chunks.ForEach(chunk => {
				var message = new GoogleAnalyticsSyncDataBatchMessage(chunk, date);
				messages.Add(message);
			});
			return messages;
		}

		private IEnumerable<GAUserSessionModel> GetUserSessionsForStream(GoogleAnalyticsStreamDto stream) {
			var parameters = new DataRequestParameters() {
				AccountId = stream.AccountId,
				PropertyId = stream.PropertyId,
				StreamId = stream.GAStreamId,
				PeriodDays = 90
			};
			var userSessions = UsersDataService.GetAvailableUserSessions(parameters).ToList();
			return userSessions
				.Where(us => us.StreamId == stream.GAStreamId
					&& us.ContactId.IsNotEmpty());
		}

		private IEnumerable<GAUserSessionModel> FilterForExistedContacts(
				IEnumerable<GAUserSessionModel> userSessions) {
			if (!userSessions.Any()) {
				return Enumerable.Empty<GAUserSessionModel>();
			}
			var contactSessions = new List<GAUserSessionModel>();
			var idsToCheck = userSessions.Select(us => us.ContactId).Distinct();
			var batchSize = 500;
			var recordsCount = idsToCheck.Count();
			for (var i = 0; i <= (recordsCount - 1) / batchSize; i++) {
				var select = new Select(_userConnection).Distinct()
					.Column("Id")
					.From("Contact")
					.Where("Id").In(Column.Parameters(idsToCheck.Skip(i * batchSize).Take(batchSize))) as Select;
				select.SpecifyNoLockHints();
				select.ExecuteReader(r => {
					var id = r.GetColumnValue<Guid>("Id");
					contactSessions.AddRange(userSessions.Where(x => x.ContactId == id));
				});
			}
			return contactSessions;
		}

		private IEnumerable<GAUserSessionModel> FilterForExistedVisitors(
				IEnumerable<GAUserSessionModel> contactSessions, Guid streamId) {
			if (!contactSessions.Any()) {
				return Enumerable.Empty<GAUserSessionModel>();
			}
			var existedVisitors = new List<string>();
			var visitorIds = contactSessions.Select(x => x.ClientId);
			var batchSize = 500;
			var recordsCount = visitorIds.Count();
			for (var i = 0; i <= (recordsCount - 1) / batchSize; i++) {
				var batch = visitorIds.Skip(i * batchSize).Take(batchSize);
				var select = new Select(_userConnection).Distinct()
					.Column("VisitorId")
					.From("WebAnalyticsVisitor")
					.Where("VisitorId").In(Column.Parameters(batch))
						.And("WebAnalyticsStreamId").IsEqual(Column.Parameter(streamId)) as Select;
				select.SpecifyNoLockHints();
				select.ExecuteReader(r => {
					var id = r.GetColumnValue<string>("VisitorId");
					existedVisitors.Add(id);
				});
			}
			return contactSessions.Where(cs => !existedVisitors.Contains(cs.ClientId));
		}

		private IEnumerable<GAUserSessionModel> GetNewVisitorSessions(GoogleAnalyticsStreamDto stream) {
			try {
				var userSessions = GetUserSessionsForStream(stream);
				var contactSessions = FilterForExistedContacts(userSessions);
				return FilterForExistedVisitors(contactSessions, stream.StreamId);
			} catch (Exception ex) {
				LogError($"GetNewVisitorSessions for stream {stream.GAStreamId}", DateTime.UtcNow, ex);
				return Enumerable.Empty<GAUserSessionModel>();
			}
		}

		private IEnumerable<WebAnalyticsVisitorModel> GetNewVisitorsToSync() {
			var streams = AnalyticsAccountRepositoty.GetAllStreamData();
			var data = new List<WebAnalyticsVisitorModel>();
			foreach (var stream in streams) {
				var newVisitorSessions = GetNewVisitorSessions(stream);
				if (!newVisitorSessions.Any()) {
					continue;
				}
				var newVisitors = newVisitorSessions.DistinctBy(vs => (vs.ContactId, vs.ClientId));
				var newVisitorModels = newVisitors
					.Where(vs => !data.Any(x => x.VisitorId == vs.ClientId
						&& x.ContactId == vs.ContactId
						&& x.StreamId == stream.StreamId))
					.Select(vs => new WebAnalyticsVisitorModel {
						ContactId = vs.ContactId,
						StreamId = stream.StreamId,
						VisitorId = vs.ClientId
					});
				data.AddRange(newVisitorModels);
			}
			return data;
		}

		private void CreateNewVisitors(IEnumerable<WebAnalyticsVisitorModel> models) {
			foreach (var model in models) {
				if (VisitorRepository.FindVisitor(model).IsEmpty()) {
					VisitorRepository.Save(model);
				}
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Processes the incoming webhook for visitor specified.
		/// </summary>
		/// <param name="visitorId">Creatio unique identifier of the visitor.</param>
		/// <param name="clientId">Google analytics unique client id.</param>
		/// <param name="streamDto"><see cref="GoogleAnalyticsStreamDto"/>.</param>
		public void SynchronizeByWebhook(Guid visitorId, string clientId, GoogleAnalyticsStreamDto streamDto) {
			if (string.IsNullOrWhiteSpace(clientId) || visitorId.IsEmpty()) {
				return;
			}
			try {
				var visitor = new VisitorInfo() {
					UserId = clientId,
					WebAnalyticsVisitorId = visitorId
				};
				var parameters = new DataRequestParameters() {
					AccountId = streamDto.AccountId,
					PropertyId = streamDto.PropertyId,
					PeriodDays = _syncByUserPeriodDays
				};
				SyncPropertyDataForVisitors(new VisitorInfo[] { visitor }, parameters);
			} catch (Exception exception) {
				LogError("SynchronizeByWebhook", DateTime.UtcNow, exception);
				throw;
			}
		}

		/// <summary>
		/// Searches new visitors by filled userId in the GA.
		/// </summary>
		public virtual void ActualizeVisitorsByUserId() {
			try {
				var recordsToSync = GetNewVisitorsToSync();
				CreateNewVisitors(recordsToSync);
			} catch (Exception ex) {
				LogError("ActualizeVisitorsByUserId", DateTime.UtcNow, ex);
			}
		}

		/// <summary>
		/// Enqueues messages to sync GA data for all existing visitors.
		/// </summary>
		public virtual void Synchronize() {
			ActualizeVisitorsByUserId();
			using (var executor = _userConnection.EnsureDBConnection()) {
				executor.StartTransaction();
				try {
					var visitorsToSync = GetVisitorsToSync();
					var groupedExistedVisitors = visitorsToSync
						.GroupBy(x => x.LastActionDateTime.Date);
					var messages = GetMessagesForVisitors(groupedExistedVisitors);
					QueueManager.Enqueue(messages);
					executor.CommitTransaction();
				} catch (Exception ex) {
					executor.RollbackTransaction();
					LogError("Synchronize", DateTime.UtcNow, ex);
					throw;
				}
			}
		}

		/// <summary>
		/// Synchronize data from GA for visitors' batch specified.
		/// </summary>
		/// <param name="visitors">Batch of the visitor models to process.</param>
		/// <param name="startDate">Start date to retreive data from GA</param>
		public virtual void Synchronize(IEnumerable<GAVisitorModel> visitorModels, DateTime startDate) {
			visitorModels.CheckArgumentNull("visitorModels");
			if (!visitorModels.Any()) {
				return;
			}
			if (startDate == DateTime.MinValue) {
				startDate = DateTime.UtcNow.AddDays(-_syncByUserPeriodDays);
			}
			var groupedVisitors = visitorModels.GroupBy(x => x.GAPropertyId);
			groupedVisitors.ForEach(x => {
				try {
					var accountId = x.FirstOrDefault()?.AccountId;
					var parameters = new DataRequestParameters() {
						AccountId = accountId,
						PropertyId = x.Key,
						PeriodDays = Convert.ToInt32((DateTime.UtcNow.Date - startDate.Date).TotalDays) + 1
					};
					SyncPropertyDataByVisitorModels(visitorModels, parameters);
				} catch (Exception exception) {
					LogError("Synchronize for visitors' batch", DateTime.UtcNow, exception);
				}
			});

		}

		#endregion

	}

	#endregion

}

