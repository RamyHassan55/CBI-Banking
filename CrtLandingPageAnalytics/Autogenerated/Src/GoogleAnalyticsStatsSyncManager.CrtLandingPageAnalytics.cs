namespace CrtLandingPageAnalytics
{
	using Common.Logging;
	using CrtGoogleAnalytics;
	using CrtGoogleAnalytics.API.Interfaces;
	using CrtGoogleAnalytics.API.Parameters;
	using CrtGoogleAnalytics.Messages;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.Models;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsStatsSyncManager

	/// <summary>
	/// Class to represent Google Analytics aggregated data synchronization manager.
	/// </summary>
	public class GoogleAnalyticsStatsSyncManager
	{

		#region Fields: Pivate

		private readonly UserConnection _userConnection;
		private readonly int _syncStatsByStreamPeriodDays = 30;
		private readonly string _defaultStatsName = "AnalyticsUserStats";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsStatsSyncManager"/>
		/// </summary>
		public GoogleAnalyticsStatsSyncManager(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Properties: Private

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

		#region Properties: Public

		/// <summary>
		/// Instance of the <see cref="GoogleAnalyticsStatsInfoProvider"/>.
		/// </summary>
		private GoogleAnalyticsStatsInfoProvider _statsInfoProvider;
		internal GoogleAnalyticsStatsInfoProvider StatsInfoProvider {
			get => _statsInfoProvider ?? (_statsInfoProvider = ClassFactory.Get<GoogleAnalyticsStatsInfoProvider>(
				new ConstructorArgument("userConnection", _userConnection)));
			set => _statsInfoProvider = value;
		}

		/// <summary>
		/// Instance of the <see cref="HtmlWebPageRepository"/>.
		/// </summary>
		private HtmlWebPageRepository _webPageRepository;
		internal HtmlWebPageRepository WebPageRepository {
			get => _webPageRepository ?? (_webPageRepository = ClassFactory.Get<HtmlWebPageRepository>(
				new ConstructorArgument("userConnection", _userConnection)));
			set => _webPageRepository = value;
		}

		/// <summary>
		/// Instance of the <see cref="IGoogleAnalyticsAggregatedDataRepository"/>.
		/// </summary>
		private IGoogleAnalyticsAggregatedDataRepository _aggregatedDataRepository;
		internal IGoogleAnalyticsAggregatedDataRepository AggregatedDataRepository {
			get => _aggregatedDataRepository
				?? (_aggregatedDataRepository = ClassFactory.Get<IGoogleAnalyticsAggregatedDataRepository>());
			set => _aggregatedDataRepository = value;
		}

		private TouchQueueManager _queueManager;
		public TouchQueueManager QueueManager {
			get => _queueManager ?? (_queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", _userConnection)
			));
		}

		/// <summary>
		/// Batch size to split stream stats data for separate sync messages.
		/// </summary>
		private int _syncStreamStatsBatchSize = int.MinValue;
		public int SyncStreamStatsBatchSize {
			get {
				if (_syncStreamStatsBatchSize < 0) {
					_syncStreamStatsBatchSize = Terrasoft.Core.Configuration.SysSettings.GetValue(_userConnection,
						"GoogleAnalyticsSyncStreamStatsBatchSize", 10);
				}
				return _syncStreamStatsBatchSize;
			}
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		private ILog _logger;
		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger("GoogleAnalytics"));
			set => _logger = value;
		}

		/// <summary>
		/// Instance of <see cref="IGADataService"/> to get analytics data.
		/// </summary>
		private IGAStatsDataService _analyticsDataService;
		private IGAStatsDataService AnalyticsDataService {
			get => _analyticsDataService ?? (_analyticsDataService =
				ClassFactory.Get<IGAStatsDataService>(
					new ConstructorArgument("userConnection", _userConnection)));
			set => _analyticsDataService = value;
		}

		#endregion

		#region Methods: Private

		private IEnumerable<TouchQueueMessage> GetMessagesForStats(
			IEnumerable<IGrouping<DateTime, GoogleAnalyticsLandingStreamDto>> groups
		) {
			var result = new List<TouchQueueMessage>();
			foreach (var group in groups) {
				var date = group.Key;
				if (date == null || date == DateTime.MinValue) {
					date = DateTime.UtcNow.Date.AddDays(-_syncStatsByStreamPeriodDays);
					group.ForEach(g => g.Date = date.Date);
				}
				var messages = GetBatchedMessages(group.ToList());
				result.AddRange(messages);
			}
			return result;
		}

		private IEnumerable<GoogleAnalyticsTouchQueueMessage> GetBatchedMessages(
			IEnumerable<GoogleAnalyticsLandingStreamDto> streams
		) {
			var messages = new List<GoogleAnalyticsTouchQueueMessage>();
			var chunks = streams.SplitOnChunks(chunkSize: SyncStreamStatsBatchSize);
			chunks.ForEach(chunk => {
				var message = new GoogleAnalyticsSyncStatsBatchMessage(chunk);
				messages.Add(message);
			});
			return messages;
		}

		private void LogDebug(string eventName, string description = "") {
			Logger.Debug($"{nameof(GoogleAnalyticsDataSyncManager)} {eventName}: {description}.");
		}

		private void LogResult(string eventName, DateTime startDate, int amount, string description = "") {
			EventLogger.Info(eventName, startDate, GoogleAnalyticsConsts.GoogleAnalyticsPlatformId, string.Empty,
				amount, description);
		}

		private void LogError(string eventName, DateTime startDate, Exception exception, string description = "") {
			Logger.Error($"{nameof(GoogleAnalyticsDataSyncManager)}.{eventName} " +
					"Exception: ", exception);
			EventLogger.Error(eventName, startDate, exception, GoogleAnalyticsConsts.GoogleAnalyticsPlatformId,
				string.Empty, description);
		}

		private int CalculatePeriodDays(DateTime date) {
			if (date.Date > DateTime.UtcNow.Date) {
				throw new ArgumentException($"Date to sync {date.Date} is greater than today ({DateTime.UtcNow.Date})");
			}
			var periodDays = (DateTime.UtcNow.Date - date.Date).Days;
			if (periodDays < 0) {
				throw new Exception($"Period (days) to syncis invalid for {date.Date} (today {DateTime.UtcNow.Date})");
			}
			return periodDays;
		}

		private void ActualizeStats(IEnumerable<GoogleAnalyticsLandingStreamDto> streams) {
			var landings = streams.Select(s => s.LandingPageId);
			var webPages = WebPageRepository.GetWebPages(landings);
			var startDate = streams.Min(s => s.Date);
			var period = CalculatePeriodDays(startDate);
			var groupedStreams = streams.GroupBy(s => s.PropertyId);
			foreach (var streamGroup in groupedStreams) {
				var streamList = streamGroup.Select(s => s.MeasurementId);
				LogDebug("ActualizeStats", streamList.GetItemsString());
				var parameters = new DataRequestParameters() {
					AccountId = streamGroup.FirstOrDefault().AccountId,
					PropertyId = streamGroup.Key,
					PeriodDays = period
				};
				var aggregatedData = new AnalyticsAggregatedData {
					UserStats = GetUserStats(streamGroup.ToList(), parameters, webPages),
					SessionStats = GetSessionStats(streamGroup.ToList(), parameters, webPages),
					EventStats = GetEventStats(streamGroup.ToList(), parameters, webPages)
				};
				var result = AggregatedDataRepository.SaveAnalytics(aggregatedData);
				if (result.ErrorsCount > 0) {
					LogError("Error on stats save", DateTime.UtcNow, null, result.LastErrorMessage);
					return;
				}
				LogResult("Saved stats", DateTime.UtcNow, result.AffectedRows);
			}
		}

		private IEnumerable<AggregatedUserDataModel> GetUserStats(
			IEnumerable<GoogleAnalyticsLandingStreamDto> streams,
			DataRequestParameters parameters, IEnumerable<WebPageModel> webPages
		) {
			var streamSites = streams.Select(s => new StreamWebSite {
				StreamId = s.GAStreamId,
				HostUrl = s.LandingPageUrl,
			});
			var data = AnalyticsDataService.GetUserStats(streamSites, parameters);
			var responses = new List<AggregatedUserDataModel>();
			foreach (var stats in data.Events) {
				var sourceStream = streams.FirstOrDefault(s => s.GAStreamId == stats.StreamId);
				var webPage = webPages.FirstOrDefault(wp => wp.Url == stats.PageUrl && wp.PageTitle == stats.PageTitle);
				if (sourceStream == null || webPage == null) {
					continue;
				}
				responses.Add(new AggregatedUserDataModel() {
					PageUrl = stats.PageUrl,
					Country = stats.CountryCode,
					Date = stats.Date,
					Count = stats.ActiveUsers,
					DeviceCategory = stats.DeviceCategory,
					StreamId = sourceStream.StreamId,
					WebPageId = webPage.Id
				});
			}
			return responses;
		}

		private IEnumerable<AggregatedSessionDataModel> GetSessionStats(
			IEnumerable<GoogleAnalyticsLandingStreamDto> streams,
			DataRequestParameters parameters, IEnumerable<WebPageModel> webPages
		) {
			var streamSites = streams.Select(s => new StreamWebSite {
				StreamId = s.GAStreamId,
				HostUrl = s.LandingPageUrl,
			});
			var data = AnalyticsDataService.GetSessionStats(streamSites, parameters);
			var responses = new List<AggregatedSessionDataModel>();
			foreach (var stats in data.Events) {
				var sourceStream = streams.FirstOrDefault(s => s.GAStreamId == stats.StreamId);
				var webPage = webPages.FirstOrDefault(wp => wp.Url == stats.PageUrl && wp.PageTitle == stats.PageTitle);
				if (sourceStream == null || webPage == null) {
					continue;
				}
				responses.Add(new AggregatedSessionDataModel() {
					PageUrl = stats.PageUrl,
					Medium = stats.Medium,
					Date = stats.Date,
					Count = stats.SessionCount,
					Source = stats.Source,
					StreamId = sourceStream.StreamId,
					WebPageId = webPage.Id
				});
			}
			return responses;
		}

		private IEnumerable<AggregatedEventDataModel> GetEventStats(
			IEnumerable<GoogleAnalyticsLandingStreamDto> streams,
			DataRequestParameters parameters, IEnumerable<WebPageModel> webPages
		) {
			var streamSites = streams.Select(s => new StreamWebSite {
				StreamId = s.GAStreamId,
				HostUrl = s.LandingPageUrl,
			});
			var data = AnalyticsDataService.GetEventStats(streamSites, parameters);
			var responses = new List<AggregatedEventDataModel>();
			foreach (var stats in data.Events) {
				var sourceStream = streams.FirstOrDefault(s => s.GAStreamId == stats.StreamId);
				var webPage = webPages.FirstOrDefault(wp => wp.Url == stats.PageUrl && wp.PageTitle == stats.PageTitle);
				if (sourceStream == null || webPage == null) {
					continue;
				}
				responses.Add(new AggregatedEventDataModel() {
					PageUrl = stats.PageUrl,
					Event = stats.EventName,
					Date = stats.Date,
					Count = stats.EventCount,
					StreamId = sourceStream.StreamId,
					WebPageId = webPage.Id
				});
			}
			return responses;
		}

		private void ActualizeStatsForStream(GoogleAnalyticsLandingStreamDto stream) {
			ActualizeStats(new GoogleAnalyticsLandingStreamDto[] { stream });
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Enqueues messages to sync GA data for all connected landing pages.
		/// </summary>
		public void Synchronize() {
			LogDebug("Synchronize started");
			try {
				var streamLastStats = StatsInfoProvider.GetLastAnalyticsStats(_defaultStatsName);
				var groupedStats = streamLastStats.GroupBy(x => x.Date.Date);
				var messages = GetMessagesForStats(groupedStats);
				QueueManager.Enqueue(messages);
			} catch (Exception ex) {
				LogError("SynchronizeStats", DateTime.UtcNow, ex);
				throw;
			}
		}

		/// <summary>
		/// Synchronize data from GA for streams' batch specified.
		/// </summary>
		/// <param name="streams">Batch of the stream models to process.</param>
		public void Synchronize(IEnumerable<GoogleAnalyticsLandingStreamDto> streams) {
			ActualizeStats(streams);
		}

		/// <summary>
		/// Synchronize data from GA for specified landing page.
		/// </summary>
		/// <param name="streamId">Identifier of web analytics stream.</param>
		/// <param name="landingPageId">Identifier of the landing page</param>
		public void SynchronizeForLanding(Guid streamId, Guid landingPageId) {
			var streamData = StatsInfoProvider.GetLastAnalyticsStats(_defaultStatsName, landingPageId, streamId);
			if (streamData.Date == null || streamData.Date == DateTime.MinValue) {
				streamData.Date = DateTime.UtcNow.Date.AddDays(-_syncStatsByStreamPeriodDays);
			}
			LogDebug("Synchronize for Landing started", landingPageId.ToString());
			ActualizeStatsForStream(streamData);
		}

		#endregion

	}

	#endregion

}
