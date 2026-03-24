namespace CrtGoogleAnalytics.API
{
	using Common.Logging;
	using CrtGoogleAnalytics.API.Dto;
	using CrtGoogleAnalytics.API.Modules;
	using CrtGoogleAnalytics.API.Parameters;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common.Json;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Factories;

	#region Class: GAAPIClient

	[DefaultBinding(typeof(IGAAPIClient))]
	internal class GAAPIClient : IGAAPIClient
	{

		#region Fields: Private

		private readonly int _defaultLimit = 10000;
		private List<BaseModule> Modules = new List<BaseModule>();

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes API client.
		/// </summary>
		public GAAPIClient() => InitModules();

		#endregion

		#region Properties: Protected

		private int _batchSize;
		protected int BatchSize {
			get => _batchSize > 0 ? _batchSize : _defaultLimit;
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
		}

		#endregion

		#region Methods: Private

		private void InitModules() {
			var userConnection = ClassFactory.Get<UserConnection>();
			var googleApiServiceUrls =
				SysSettings.GetValue(userConnection, "GoogleAnalyticsApiServiceUrls", string.Empty);
			if (string.IsNullOrWhiteSpace(googleApiServiceUrls?.ToString())) {
				throw new Exception("Google Analytics API service url is not set.");
			};
			var serviceUrls = Json.Deserialize<Dictionary<string, string>>(googleApiServiceUrls.ToString());
			serviceUrls.TryGetValue("AnalyticsAdminApi", out var adminApiUrl);
			serviceUrls.TryGetValue("AnalyticsDataApi", out var dataApiUrl);
			if (string.IsNullOrWhiteSpace(adminApiUrl) || string.IsNullOrWhiteSpace(dataApiUrl)) {
				throw new Exception("Google Analytics Data API service url is not set.");
			}
			Modules = new List<BaseModule> {
				new AdminModule(adminApiUrl),
				new AnalyticsModule(dataApiUrl),
				new AnalyticsStatsModule(dataApiUrl)
			};
		}

		private void LogDebug(string eventName, string description = "") {
			Logger.Debug($"{nameof(GAAPIClient)} {eventName}: {description}.");
		}

		/// <summary>
		/// Returns analytics data.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		private GADataResponse InternalGetAnalyticsData<T>(string accessToken, GADataRequestParameters parameters)
				where T : AnalyticsModule {
			var analyticsModule = Modules.Get<T>();
			var limit = parameters.Limit;
			parameters.Limit = limit > 0 ? Math.Min(limit, BatchSize) : BatchSize;
			LogDebug("GetAnalyticsData", $"Parameters: {Json.Serialize(parameters, true)}");
			var result = analyticsModule.GetData(accessToken, parameters);
			if (result?.Rows == null) {
				LogDebug("GetAnalyticsData", "No data returned.");
				return result;
			}
			GetAllDataRows<T>(accessToken, parameters, limit, ref result);
			return result;
		}

		private void GetAllDataRows<T>(string accessToken, GADataRequestParameters parameters, int limit,
				ref GADataResponse data) where T : AnalyticsModule {
			var analyticsModule = Modules.Get<T>();
			var offset = parameters.Offset;
			var totalCount = data.RowCount - offset;
			while (totalCount > data.Rows.Count) {
				if (limit > 0 && data.Rows.Count == limit) { break; }
				parameters.Offset = data.Rows.Count + offset;
				var rowsToLimit = limit - parameters.Offset;
				if (limit > 0 && rowsToLimit < BatchSize) {
					parameters.Limit = rowsToLimit;
				}
				var dataResponse = analyticsModule.GetData(accessToken, parameters);
				if (dataResponse.Rows == null) { break; }
				data.Rows.AddRange(dataResponse.Rows);
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns Google analytics accounts list.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// </summary>
		public IEnumerable<GAAccount> GetAccountList(string accessToken) {
			var adminModule = Modules.Get<AdminModule>();
			return adminModule.GetAccounts(accessToken);
		}

		/// <summary>
		/// Returns Google analytics properties by account identifier.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="accountId">Google analytics account identifier</param>
		/// </summary>
		public IEnumerable<GAAccountProperty> GetAccountProperties(string accessToken, string accountId) {
			var adminModule = Modules.Get<AdminModule>();
			return adminModule.GetProperties(accessToken, accountId);
		}

		/// <summary>
		/// Returns Google analytics data streams by property identifier.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="propertyId">Google analytics property identifier</param>
		/// </summary>
		public IEnumerable<GADataStream> GetDataStreams(string accessToken, string propertyId) {
			var adminModule = Modules.Get<AdminModule>();
			return adminModule.GetDataStreams(accessToken, propertyId);
		}

		/// <summary>
		/// Returns analytics data.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		public GADataResponse GetAnalyticsData(string accessToken, GADataRequestParameters parameters) {
			return InternalGetAnalyticsData<AnalyticsModule>(accessToken, parameters);
		}

		/// <summary>
		/// Returns google analytics statistics data.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		public GADataResponse GetStatsData(string accessToken, GADataRequestParameters parameters) {
			return InternalGetAnalyticsData<AnalyticsStatsModule>(accessToken, parameters);
		}

		/// <summary>
		/// Returns collection of the available user specified sessions from GA.
		/// </summary>
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="parameters">Request parameters</param>
		public IEnumerable<GAUserSession> GetUserSessions(string accessToken, GADataRequestParameters parameters) {
			var analyticsModule = Modules.Get<AnalyticsModule>();
			var result = analyticsModule.GetData(accessToken, parameters);
			var userIdIndex = result.DimensionHeaders
				.FindIndex(x => x.Name == "customEvent:crt_user_id");
			var sessionIdIndex = result.DimensionHeaders
				.FindIndex(x => x.Name == "customEvent:crt_session_id");
			var streamIdIndex = result.DimensionHeaders
				.FindIndex(x => x.Name == "streamId");
			if (result.RowCount == 0) {
				return Enumerable.Empty<GAUserSession>();
			}
			return result.Rows?
				.Select(x =>  new GAUserSession {
					UserId = x.DimensionValues[userIdIndex]?.Value,
					SessionId = x.DimensionValues[sessionIdIndex]?.Value,
					StreamId = x.DimensionValues[streamIdIndex]?.Value
				})
				.Where(us => !string.IsNullOrWhiteSpace(us.UserId)
					&& !string.IsNullOrWhiteSpace(us.SessionId)
					&& !string.IsNullOrWhiteSpace(us.StreamId));
		}

		#endregion

	}

	#endregion

}
