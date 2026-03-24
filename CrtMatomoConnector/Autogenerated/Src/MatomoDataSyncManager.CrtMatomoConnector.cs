namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Factories;
	using CoreSysSettings = Core.Configuration.SysSettings;
	using global::Common.Logging;

	#region Class: MatomoDataSyncManager

	/// <summary>
	/// Class to synchronize matomo data for existed visitors.
	/// </summary>
	public class MatomoDataSyncManager
	{

		#region Constants: Private

		private const string VisitorIdSegmentName = "visitorId";
		private const string UserIdSegmentName = "userId";

		#endregion

		#region Fields: Private

		private static readonly ILog _logger = LogManager.GetLogger("MatomoConnector");

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoDataSyncManager"/>.
		/// </summary>
		/// <param name="userConnection"></param>
		public MatomoDataSyncManager(UserConnection userConnection) => UserConnection = userConnection;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Instance of <see cref="UserConnection"/>.
		/// </summary>
		public UserConnection UserConnection { get; set; }

		/// <summary>
		/// Batch size to process visitors data for single sync message.
		/// </summary>
		private int _syncDataBatchSize = int.MinValue;
		public int SyncDataBatchSize {
			get {
				if (_syncDataBatchSize < 0) {
					var value = CoreSysSettings.GetValue(UserConnection, "MatomoSyncDataBatchSize", 100);
					_syncDataBatchSize = Math.Max(value, 0);
				}
				return _syncDataBatchSize;
			}
		}

		/// <summary>
		/// Period in days to select contacts for Matomo data sync by userId.
		/// </summary>
		private int _syncByUserPeriod = int.MinValue;
		public int SyncByUserPeriod {
			get {
				if (_syncByUserPeriod < 0) {
					var value = CoreSysSettings.GetValue(UserConnection, "MatomoSyncByUserPeriodInDays", 7);
					_syncByUserPeriod = Math.Max(value, 0);
				}
				return _syncByUserPeriod;
			}
		}

		/// <summary>
		/// Instance of <see cref="TouchQueueManager"/> to enqueue matomo data sync tasks.
		/// </summary>
		private TouchQueueManager _queueManager;
		public TouchQueueManager QueueManager {
			get => _queueManager ?? (_queueManager = new TouchQueueManager(UserConnection));
			set => _queueManager = value;
		}

		#endregion

		#region Methods: Private

		private Select GetVisitorsToSyncSelect() {
			var select = new Select(UserConnection)
				.Column(nameof(Touch.MatomoVisitorId))
				.Column(Func.Max(nameof(Touch.ContactId))).As(nameof(Touch.ContactId))
				.Column(Func.Max(nameof(Touch.LastActionDateTime))).As(nameof(Touch.LastActionDateTime))
				.Column(Func.Max(nameof(Touch.MatomoSiteId))).As(nameof(Touch.MatomoSiteId))
				.From(nameof(Touch))
				.Where(nameof(Touch.MatomoVisitorId)).IsNotEqual(Column.Parameter(string.Empty))
				.GroupBy(nameof(Touch.MatomoVisitorId)) as Select;
			select.SpecifyNoLockHints();
			return select;
		}

		private IEnumerable<MatomoTouchModel> GetVisitorsToSync() {
			var result = new Dictionary<string, DateTime>();
			var select = GetVisitorsToSyncSelect();
			var defaultDate = DateTime.UtcNow.AddYears(-1);
			return select.ExecuteEnumerable(reader => {
				var lastActionDate = reader.GetColumnValue<DateTime>(nameof(Touch.LastActionDateTime));
				var visitorId = reader.GetColumnValue<string>(nameof(Touch.MatomoVisitorId));
				var contactId = reader.GetColumnValue<Guid>(nameof(Touch.ContactId));
				var siteId = reader.GetColumnValue<int>(nameof(Touch.MatomoSiteId));
				return new MatomoTouchModel {
					VisitorId = visitorId,
					LastActionDateTime = lastActionDate.Equals(DateTime.MinValue) ? defaultDate : lastActionDate,
					ContactId = contactId,
					MatomoSiteId = siteId
				};
			});
		}

		private IEnumerable<string> GetUrlHosts(string externalURL) =>
			externalURL
				.Split(new char[] { ';', '*', ' ', ',', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(url => {
					if (!url.StartsWith("http://") && !url.StartsWith("https://")) {
						url = "http://" + url;
					}
					var uri = new Uri(url);
					return uri.Host;
				});

		private IEnumerable<string> GetSites() {
			var select = new Select(UserConnection)
				.Column(nameof(GeneratedWebForm.ExternalURL))
				.From(nameof(GeneratedWebForm));
			select.SpecifyNoLockHints();
			var siteUrls = new List<string>();
			select.ExecuteReader(reader => {
				var externalURL = reader.GetColumnValue<string>(nameof(GeneratedWebForm.ExternalURL));
				var hosts = GetUrlHosts(externalURL);
				siteUrls.AddRangeIfNotExists(hosts);
			});
			return siteUrls;
		}
		
		private IEnumerable<int> GetSiteIdsFromTouch() {
			var select = new Select(UserConnection)
				.Column(nameof(Touch.MatomoSiteId))
				.Distinct()
				.From(nameof(Touch))
				.Where(nameof(Touch.MatomoSiteId)).Not().IsNull() as Select;
			select.SpecifyNoLockHints();
			var siteIds = new List<int>();
			select.ExecuteReader(reader => {
				var matomoSiteId = reader.GetColumnValue<int>(nameof(Touch.MatomoSiteId));
				siteIds.Add(matomoSiteId);
			});
			return siteIds;
		}

		private IEnumerable<int> GetSiteIds(List<MatomoTouchModel> visitors = null) {
			var facade = ClassFactory.Get<MatomoServiceFacade>(new ConstructorArgument("userConnection",
				UserConnection));
			var siteUrls = GetSites();
			var urlSiteIds = facade.GetSiteIds(siteUrls).ToList();
			var touchSiteIds = visitors == null 
				? GetSiteIdsFromTouch()
				: visitors.Select(x => x.MatomoSiteId).Distinct();
			return urlSiteIds.Union(touchSiteIds);
		}

		private DateTime GetLastVisitDate(string visitorId) {
			var select = new Select(UserConnection)
				.Column(Func.Max("LastActionDateTime"))
				.From(nameof(Touch))
				.Where("MatomoVisitorId").IsEqual(Column.Parameter(visitorId)) as Select;
			select.SpecifyNoLockHints();
			var lastTouchActionDate = select.ExecuteScalar<DateTime>();
			var lastYear = DateTime.UtcNow.AddYears(-1);
			return lastYear > lastTouchActionDate ? lastYear : lastTouchActionDate;
		}

		private DateTime GetStartDateForRequest() {
			return DateTime.UtcNow.AddDays(-SyncByUserPeriod);
		}

		private IContactsToSyncRequest GetContactsToSyncRequest() {
			try {
				return ClassFactory.Get<IContactsToSyncRequest>();
			} catch (Exception) {
				return null;
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Synchronizes matomo data with specified Creatio contacts' touch points.
		/// </summary>
		public virtual int Synchronize(IEnumerable<TouchModel> visitors, IEnumerable<int> siteIds, DateTime startDate) {
			var facade = ClassFactory.Get<MatomoServiceFacade>(new ConstructorArgument("userConnection",
				UserConnection));
			var rowsCount = 0;
			try {
				var repository = ClassFactory.Get<TouchPointRepository>(new ConstructorArgument("userConnection",
					UserConnection));
				var visitorIds = visitors.Select(x => x.VisitorId);
				var details = facade.GetLastVisitDetails(siteIds.ToList(), startDate, VisitorIdSegmentName, visitorIds);
				var contactVisits = visitors.GroupBy(x => x.ContactId);
				foreach (var contact in contactVisits) {
					var contactId = contact.Key;
					var contactVisitorIds = contact.Select(y => y.VisitorId);
					var visitDetails = details.VisitDetails.Where(x => contactVisitorIds.Contains(x.visitorId));
					rowsCount += repository.SaveVisitDetails(contactId, visitDetails);
				}
			} catch (Exception ex) {
				_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.Synchronize batch. Exception. ", ex);
				throw;
			}
			return rowsCount;
		}

		/// <summary>
		/// Enqueues messages to sync matomo data for all existing touches.
		/// </summary>
		public virtual int Synchronize() {
			var messagesCount = 0;
			using (var executor = UserConnection.EnsureDBConnection()) {
				executor.StartTransaction();
				try {
					var visitorsToSync = GetVisitorsToSync().ToList();
					var groupedVisitors = visitorsToSync.GroupBy(x => x.LastActionDateTime);
					var siteIds = GetSiteIds(visitorsToSync).ToList();
					var messages = new List<MatomoTouchQueueMessage>();
					foreach (var group in groupedVisitors) {
						var startDate = group.Key;
						var visitors = group.ToList();
						var chunks = visitors.SplitOnChunks(chunkSize: SyncDataBatchSize);
						chunks.ForEach(chunk => {
							var message = new MatomoSyncVisitorsMessage(chunk, startDate, siteIds);
							messages.Add(message);
						});
					}
					messagesCount += QueueManager.Enqueue(messages);
					executor.CommitTransaction();
				} catch (Exception ex) {
					executor.RollbackTransaction();
					messagesCount = 0;
					_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.Synchronize. Exception. ", ex);
					throw;
				}
			}
			return messagesCount;
		}

		/// <summary>
		/// Synchronizes matomo data with Creatio for specified form submit contact.
		/// </summary>
		public virtual int SynchronizeByFormSubmit(Guid contactId, string visitorId, string userId, string externalURL) {
			var facade = ClassFactory.Get<MatomoServiceFacade>(new ConstructorArgument("userConnection",
				UserConnection));
			var rowsCount = 0;
			try {
				var urlHosts = GetUrlHosts(externalURL);
				var siteIds = facade.GetSiteIds(urlHosts).ToList();
				var startDate = GetLastVisitDate(visitorId);
				var segmentValues = new[] { visitorId };
				var details = facade.GetLastVisitDetails(siteIds, startDate, VisitorIdSegmentName, segmentValues);
				var repository = ClassFactory.Get<TouchPointRepository>(
					new ConstructorArgument("userConnection", UserConnection));
				rowsCount = repository.SaveVisitDetails(contactId, userId, details.VisitDetails);
			} catch (Exception ex) {
				_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.SynchronizeByFormSubmit. "
					+ "Exception. ", ex);
				throw;
			}
			return rowsCount;
		}
		
		/// <summary>
		/// Synchronizes matomo data with Creatio for specified contact from webhook.
		/// </summary>
		public virtual int SynchronizeByWebhook(Guid contactId, string visitorId, string landingPageUrl) {
			var facade = ClassFactory.Get<MatomoServiceFacade>(new ConstructorArgument("userConnection",
				UserConnection));
			var rowsCount = 0;
			try {
				var userId = string.Empty;
				var urlHosts = GetUrlHosts(landingPageUrl);
				var siteIds = facade.GetSiteIds(urlHosts).ToList();
				var startDate = GetLastVisitDate(visitorId);
				var segmentValues = new[] { visitorId };
				var details = facade.GetLastVisitDetails(siteIds, startDate, VisitorIdSegmentName, segmentValues);
				var repository = ClassFactory.Get<TouchPointRepository>(
					new ConstructorArgument("userConnection", UserConnection));
				rowsCount = repository.SaveVisitDetails(contactId, userId, details.VisitDetails);
			} catch (Exception ex) {
				_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.SynchronizeByFormSubmit. "
					+ "Exception. ", ex);
				throw;
			}
			return rowsCount;
		}

		/// <summary>
		/// Synchronizes matomo data with specified userIds.
		/// </summary>
		public virtual int SynchronizeByUserIds(IEnumerable<Guid> contactIds, IEnumerable<int> siteIds, DateTime startDate) {
			var facade = ClassFactory.Get<MatomoServiceFacade>(new ConstructorArgument("userConnection",
				UserConnection));
			var rowsCount = 0;
			try {
				var repository = ClassFactory.Get<TouchPointRepository>(new ConstructorArgument("userConnection",
					UserConnection));
				var userIds = contactIds.Select(x => x.ToString());
				var details = facade.GetLastVisitDetails(siteIds.ToList(), startDate, UserIdSegmentName, userIds);
				foreach (var contactId in contactIds) {
					var visitDetails = details.VisitDetails
						.Where(x => x.userId.ToLower() == contactId.ToString().ToLower());
					rowsCount += repository.SaveVisitDetails(contactId, visitDetails);
				}
			} catch (Exception ex) {
				_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.Synchronize batch. Exception. ", ex);
				throw;
			}
			return rowsCount;
		}

		/// <summary>
		/// Enqueues messages to sync matomo data for requested contacts.
		/// </summary>
		public virtual int SynchronizeForContactsRequest() {
			var messagesCount = 0;
			var request = GetContactsToSyncRequest();
			if (request == null) {
				return messagesCount;
			}
			using (var executor = UserConnection.EnsureDBConnection()) {
				executor.StartTransaction();
				try {
					var startDate = GetStartDateForRequest();
					var siteIds = GetSiteIds().ToList();
					var contactIds = request.GetContactsToSync(UserConnection, startDate);
					var chunks = contactIds.SplitOnChunks(chunkSize: SyncDataBatchSize);
					var messages = new List<MatomoTouchQueueMessage>();
					chunks.ForEach(chunk => {
						var message = new MatomoContactsToSyncMessage(chunk, startDate, siteIds);
						messages.Add(message);
					});
					messagesCount += QueueManager.Enqueue(messages);
					executor.CommitTransaction();
				} catch (Exception ex) {
					executor.RollbackTransaction();
					messagesCount = 0;
					_logger.Error($"MatomoConnector.{nameof(MatomoDataSyncManager)}.SynchronizeForContactsRequest. Exception. ", ex);
					throw;
				}
			}
			return messagesCount;
		}

		#endregion

	}

	#endregion

}

