namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtGoogleAnalytics;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Factories;
	using global::Common.Logging;

	/// <summary>
	/// Repository for storing aggregated analytics data obtained from the GA.
	/// </summary>
	[DefaultBinding(typeof(IGoogleAnalyticsAggregatedDataRepository))]
	public class GoogleAnalyticsAggregatedDataRepository : BaseAnalyticsAggregatedDataRepository,
		IGoogleAnalyticsAggregatedDataRepository
	{

		#region Fields: Private

		private static readonly ILog _logger = LogManager.GetLogger("GoogleAnalytics");

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsAggregatedDataRepository"/>.
		/// </summary>
		/// <param name="userConnection"></param>
		public GoogleAnalyticsAggregatedDataRepository(UserConnection userConnection) {
			UserConnection = userConnection;
			CodeColumnName = "GACode";
		}

		#endregion

		#region Methods: Private

		private void ActualizeLookups(AnalyticsAggregatedData data) {
			LookupValues["Event"] = ActualizeLookupValues("TouchActionType", data.EventStats.Select(x => x.Event));
			LookupValues["DeviceCategory"] = ActualizeLookupValues("TouchDeviceCategory",
				data.UserStats.Select(x => x.DeviceCategory));
			LookupValues["Country"] = ActualizeCountries(data.UserStats.Select(x => x.Country), isUpperCase: true);
		}

		private Dictionary<Guid, AggregatedUserDataModel> GetExistingUserStats(DateTime date, Guid streamId) {
			var result = new Dictionary<Guid, AggregatedUserDataModel>();
			var select = new Select(UserConnection)
					.Column("Id")
					.Column("WebPageId")
					.Column("CountryCode")
					.Column("DeviceCategoryCode")
					.Column("Count")
				.From("AnalyticsUserStats")
				.Where("Date").IsEqual(Column.Parameter(date))
				.And("StreamId").IsEqual(Column.Parameter(streamId)) as Select;
			select.SpecifyNoLockHints();
			select.ExecuteReader(x => {
				var id = x.GetColumnValue<Guid>("Id");
				var userStats = new AggregatedUserDataModel {
					WebPageId = x.GetColumnValue<Guid>("WebPageId"),
					Country = x.GetColumnValue<string>("CountryCode"),
					DeviceCategory = x.GetColumnValue<string>("DeviceCategoryCode"),
					Count = x.GetColumnValue<int>("Count")
				};
				result.Add(id, userStats);
			});
			return result;
		}

		private Dictionary<Guid, AggregatedSessionDataModel> GetExistingSessionStats(DateTime date, Guid streamId) {
			var result = new Dictionary<Guid, AggregatedSessionDataModel>();
			var select = new Select(UserConnection)
					.Column("Id")
					.Column("WebPageId")
					.Column("MediumCode")
					.Column("SourceStr")
					.Column("Count")
				.From("AnalyticsSessionStats")
				.Where("Date").IsEqual(Column.Parameter(date))
				.And("StreamId").IsEqual(Column.Parameter(streamId)) as Select;
			select.SpecifyNoLockHints();
			select.ExecuteReader(x => {
				var id = x.GetColumnValue<Guid>("Id");
				var sessionStats = new AggregatedSessionDataModel {
					WebPageId = x.GetColumnValue<Guid>("WebPageId"),
					Medium = x.GetColumnValue<string>("MediumCode"),
					Source = x.GetColumnValue<string>("SourceStr"),
					Count = x.GetColumnValue<int>("Count")
				};
				result.Add(id, sessionStats);
			});
			return result;
		}

		private Dictionary<Guid, AggregatedEventDataModel> GetExistingEventStats(DateTime date, Guid streamId) {
			var result = new Dictionary<Guid, AggregatedEventDataModel>();
			var select = new Select(UserConnection)
					.Column("Id")
					.Column("WebPageId")
					.Column("EventCode")
					.Column("Count")
				.From("AnalyticsEventStats")
				.Where("Date").IsEqual(Column.Parameter(date))
				.And("StreamId").IsEqual(Column.Parameter(streamId)) as Select;
			select.SpecifyNoLockHints();
			select.ExecuteReader(x => {
				var id = x.GetColumnValue<Guid>("Id");
				var eventStats = new AggregatedEventDataModel {
					WebPageId = x.GetColumnValue<Guid>("WebPageId"),
					Event = x.GetColumnValue<string>("EventCode"),
					Count = x.GetColumnValue<int>("Count")
				};
				result.Add(id, eventStats);
			});
			return result;
		}

		private int InsertUserStats(AggregatedUserDataModel userStats) {
			var id = Guid.NewGuid();
			var insert = new Insert(UserConnection).Into("AnalyticsUserStats");
			insert.Values()
				.Set("Id", Column.Parameter(id))
				.Set("Date", Column.Parameter(userStats.Date))
				.Set("StreamId", Column.Parameter(userStats.StreamId))
				.Set("WebPageId", Column.Parameter(userStats.WebPageId))
				.Set("PageUrl", Column.Parameter(userStats.PageUrl))
				.Set("CountryCode", Column.Parameter(userStats.Country))
				.TrySetLookupValue("CountryId", LookupValues["Country"], userStats.Country, true)
				.Set("DeviceCategoryCode", Column.Parameter(userStats.DeviceCategory))
				.TrySetLookupValue("DeviceCategoryId", LookupValues["DeviceCategory"], userStats.DeviceCategory)
				.Set("Count", Column.Parameter(userStats.Count));
			return insert.Execute();
		}

		private int InsertSessionStats(AggregatedSessionDataModel sessionStats) {
			var id = Guid.NewGuid();
			var nullGuid = Column.Parameter(DBNull.Value, new GuidDataValueType(UserConnection.DataValueTypeManager));
			var parameters = new Dictionary<string, string> {
				{ "utm_medium", sessionStats.Medium },
				{ "utm_source", sessionStats.Source }
			};
			var sourceChannel = SourceHelper.ComputeMediumAndSource(parameters);
			var insert = new Insert(UserConnection).Into("AnalyticsSessionStats");
			insert.Values()
				.Set("Id", Column.Parameter(id))
				.Set("Date", Column.Parameter(sessionStats.Date))
				.Set("StreamId", Column.Parameter(sessionStats.StreamId))
				.Set("WebPageId", Column.Parameter(sessionStats.WebPageId))
				.Set("PageUrl", Column.Parameter(sessionStats.PageUrl))
				.Set("Count", Column.Parameter(sessionStats.Count));
			if (!string.IsNullOrWhiteSpace(sessionStats.Source)) {
				insert.Set("SourceStr", Column.Parameter(sessionStats.Source));
			} else {
				insert.Set("SourceStr", Column.Parameter("direct"));
			}
			if (sourceChannel.Source.IsNotEmpty()) {
				insert.Set("SourceId", Column.Parameter(sourceChannel.Source));
			} else {
				insert.Set("SourceId", nullGuid);
			}
			if (!string.IsNullOrWhiteSpace(sessionStats.Medium)) {
				insert.Set("MediumCode", Column.Parameter(sessionStats.Medium));
			} else {
				insert.Set("MediumCode", Column.Parameter("direct"));
			}
			if (sourceChannel.Medium.IsNotEmpty()) {
				insert.Set("MediumId", Column.Parameter(sourceChannel.Medium));
			} else if (sessionStats.Medium == "referral") {
				insert.Set("MediumId", Column.Parameter(LeadMediumConsts.LeadMediumReferrerTrafficId));
			} else {
				insert.Set("MediumId", Column.Parameter(LeadMediumConsts.LeadMediumDirectTrafficId));
			}
			return insert.Execute();
		}

		private int InsertEventStats(AggregatedEventDataModel eventStats) {
			var id = Guid.NewGuid();
			var insert = new Insert(UserConnection).Into("AnalyticsEventStats");
			insert.Values()
				.Set("Id", Column.Parameter(id))
				.Set("Date", Column.Parameter(eventStats.Date))
				.Set("StreamId", Column.Parameter(eventStats.StreamId))
				.Set("WebPageId", Column.Parameter(eventStats.WebPageId))
				.Set("PageUrl", Column.Parameter(eventStats.PageUrl))
				.Set("EventCode", Column.Parameter(eventStats.Event))
				.TrySetLookupValue("ActionTypeId", LookupValues["Event"], eventStats.Event)
				.Set("Count", Column.Parameter(eventStats.Count));
			return insert.Execute();
		}

		private int UpdateAnalyticsStats(string entityName, Guid statsId, int count) {
			var update = new Update(UserConnection, entityName).WithHints(new RowLockHint())
				.Set("Count", Column.Parameter(count))
				.Where("Id").IsEqual(Column.Parameter(statsId));
			return update.Execute();
		}

		private int SaveUserStats(IEnumerable<AggregatedUserDataModel> userStats, DateTime date, Guid streamId,
				DBExecutor executor) {
			var count = 0;
			var existingUserStats = GetExistingUserStats(date, streamId);
			foreach (var stats in userStats) {
				var existingStats = existingUserStats.FirstOrDefault(x => x.Value.WebPageId == stats.WebPageId
					&& x.Value.Country == stats.Country && x.Value.DeviceCategory == stats.DeviceCategory);
				if (existingStats.Key != default && existingStats.Value.Count == stats.Count) {
					continue;
				}
				if (existingStats.Key != default) {
					UpdateAnalyticsStats("AnalyticsUserStats", existingStats.Key, stats.Count);
				} else {
					InsertUserStats(stats);
				}
				count++;
			}
			return count;
		}

		private int SaveSessionStats(IEnumerable<AggregatedSessionDataModel> sessionStats, DateTime date, Guid streamId,
				DBExecutor executor) {
			var count = 0;
			var existingSessionStats = GetExistingSessionStats(date, streamId);
			foreach (var stats in sessionStats) {
				var existingStats = existingSessionStats.FirstOrDefault(x => x.Value.WebPageId == stats.WebPageId
					&& x.Value.Source == stats.Source && x.Value.Medium == stats.Medium);
				if (existingStats.Key != default && existingStats.Value.Count == stats.Count) {
					continue;
				}
				if (existingStats.Key != default) {
					UpdateAnalyticsStats("AnalyticsSessionStats", existingStats.Key, stats.Count);
				} else {
					InsertSessionStats(stats);
				}
				count++;
			}
			return count;
		}

		private int SaveEventStats(IEnumerable<AggregatedEventDataModel> eventStats, DateTime date, Guid streamId,
				DBExecutor executor) {
			var count = 0;
			var existingEventStats = GetExistingEventStats(date, streamId);
			foreach (var stats in eventStats) {
				var existingStats = existingEventStats.FirstOrDefault(x => x.Value.WebPageId == stats.WebPageId
					&& x.Value.Event == stats.Event);
				if (existingStats.Key != default && existingStats.Value.Count == stats.Count) {
					continue;
				}
				if (existingStats.Key != default) {
					UpdateAnalyticsStats("AnalyticsEventStats", existingStats.Key, stats.Count);
				} else {
					InsertEventStats(stats);
				}
				count++;
			}
			return count;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Saves aggregated analytics data into the DB.
		/// </summary>
		/// <param name="data">Collection of aggregated page statistics.</param>
		/// <returns>Save operation result.</returns>
		public WebAnalyticsDataSaveResult SaveAnalytics(AnalyticsAggregatedData data) {
			data.UserStats.CheckArgumentNull("UserStats");
			data.SessionStats.CheckArgumentNull("SessionStats");
			data.EventStats.CheckArgumentNull("EventStats");
			var affectedRows = 0;
			var errorsCount = 0;
			var lastErrorMessage = string.Empty;
			var totalRows = data.UserStats.Count() + data.SessionStats.Count() + data.EventStats.Count();
			if (data.UserStats.IsEmpty() || data.SessionStats.IsEmpty() || data.EventStats.IsEmpty()) {
				return new WebAnalyticsDataSaveResult(affectedRows);
			}
			ActualizeLookups(data);
			var streams = data.UserStats.Select(x => x.StreamId).Distinct();
			foreach (var streamId in streams) {
				var userStats = data.UserStats.Where(x => x.StreamId == streamId);
				var sessionStats = data.SessionStats.Where(x => x.StreamId == streamId);
				var eventStats = data.EventStats.Where(x => x.StreamId == streamId);
				var days = userStats.Select(x => x.Date).Distinct().OrderBy(a => a);
				foreach (var day in days) {
					using (var executor = UserConnection.EnsureDBConnection()) {
						executor.StartTransaction();
						try {
							affectedRows += SaveUserStats(userStats.Where(x => x.Date == day), day, streamId, executor);
							affectedRows += SaveSessionStats(sessionStats.Where(x => x.Date == day), day, streamId, executor);
							affectedRows += SaveEventStats(eventStats.Where(x => x.Date == day), day, streamId, executor);
							executor.CommitTransaction();
						} catch (Exception ex) {
							executor.RollbackTransaction();
							_logger.Error($"{nameof(GoogleAnalyticsAggregatedDataRepository)}" +
								".SaveAnalytics. Exception: ", ex);
							lastErrorMessage = ex.Message;
							errorsCount++;
						}
						if (errorsCount > 0) {
							return new WebAnalyticsDataSaveResult(affectedRows, totalRows, errorsCount, lastErrorMessage);
						}
					}
				}
			}
			return new WebAnalyticsDataSaveResult(affectedRows, totalRows, errorsCount, lastErrorMessage);
		}

		#endregion

	}
}

