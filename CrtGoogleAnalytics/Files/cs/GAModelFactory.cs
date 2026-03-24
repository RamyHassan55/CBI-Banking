namespace CrtGoogleAnalytics.API
{
	using Common.Logging;
	using CrtGoogleAnalytics.API.Dto;
	using CrtGoogleAnalytics.API.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common.Json;

	internal class GAModelFactory
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		private static ILog _logger;
		public static ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger("GoogleAnalytics"));
		}

		#endregion

		#region Methods: Private

		private static void LogDebug(string eventName, string description = "") {
			Logger.Debug($"{nameof(GAModelFactory)} {eventName}: {description}.");
		}

		#endregion

		#region Methods: Public

		public static IEnumerable<T> GetWebEventModels<T>(GADataResponse analyticsData, Func<string, T> deserializeModel)
				where T: GAItemModelBase {
			if (analyticsData?.Rows == null) {
				yield break;
			}
			var dimensionsCount = analyticsData.DimensionHeaders.Count;
			var metricsCount = analyticsData.MetricHeaders?.Count ?? 0;
			var count = 0;
			foreach (var row in analyticsData.Rows) {
				var rowData = new Dictionary<string, object>();
				for (var i = 0; i < dimensionsCount; i++) {
					rowData[analyticsData.DimensionHeaders.ElementAt(i).Name] = row.DimensionValues[i].Value;
				}
				for (var i = 0; i < metricsCount; i++) {
					rowData[analyticsData.MetricHeaders.ElementAt(i).Name] = row.MetricValues[i].Value;
				}
				var rowStr = Json.Serialize(rowData, true);
				var model = deserializeModel(rowStr);
				if (model == null) {
					continue;
				}
				count++;
				yield return model;
			}
			LogDebug($"GetWebEventModels {nameof(T)}", $"Total rows: {analyticsData.RowCount}, Result rows: {count}.");
		}

		public static IEnumerable<GAProperty> CreateAnalyticsProperties(IEnumerable<GAAccountProperty> properties) {
			foreach (var property in properties) {
				yield return new GAProperty() {
					Id = property.Id,
					Name = property.Name
				};
			}
		}

		public static IEnumerable<GAWebDataStream> CreateWebDataStreams(IEnumerable<GADataStream> streams) {
			foreach (var stream in streams) {
				if (stream.StreamType != "WEB_DATA_STREAM") {
					continue;
				}
				yield return new GAWebDataStream() {
					Name = stream.Name,
					MeasurementId = stream.StreamData.MeasurementId,
					DefaultUri = stream.StreamData.DefaultUri,
					StreamId = stream.Id
				};
			}
		}

		#endregion

	}
}
