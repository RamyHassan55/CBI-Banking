namespace CrtGoogleAnalytics.API
{
	using System;
	using CrtGoogleAnalytics.API.Dto;
	using CrtGoogleAnalytics.API.Models;
	using CrtGoogleAnalytics.API.Parameters;
	using Terrasoft.Common;
	using Terrasoft.Common.Json;

	internal class GAModelDeserializer
	{

		#region Methods: Private

		private static int GetEventCount(string eventCountStr) {
			if (int.TryParse(eventCountStr, out var result)) {
				return result;
			}
			return 1;
		}

		private static int GetStatsMetricCount(string countStr) {
			if (int.TryParse(countStr, out var result)) {
				return result;
			}
			return 0;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Delegate that returns deserialized session model or null.
		/// </summary>
		public static Func<string, GAWebSessionModel> ToSessionModel = (string rowStr) => {
			var sessionDto = Json.Deserialize<GAWebSession>(rowStr);
			var model = new GAWebSessionModel {
				SessionStr = sessionDto.SessionId,
				CityId = sessionDto.CityId,
				CountryCode = sessionDto.CountryCode,
				LanguageCode = sessionDto.LanguageCode,
				PageReferrer = sessionDto.PageReferrer,
				SourceMedium = sessionDto.SourceMedium,
				Campaign = sessionDto.Campaign
			};
			if (string.IsNullOrWhiteSpace(model.SessionId)
				|| string.IsNullOrWhiteSpace(model.ClientId)
				|| model.SessionStr == GADataConstants.NotSetValue) {
				return null;
			}
			return model;
		};

		/// <summary>
		/// Delegate that returns deserialized action model or null.
		/// </summary>
		public static Func<string, GAWebActionModel> ToActionModel = (string rowStr) => {
			var actionDto = Json.Deserialize<GAWebAction>(rowStr);
			var model = new GAWebActionModel {
				SessionStr = actionDto.SessionId,
				DateTimeStr = actionDto.DateTime,
				EventName = actionDto.EventName,
				FullPageUrl = actionDto.FullPageUrl,
				PageTitle = actionDto.PageTitle,
				EventsCount = GetEventCount(actionDto.EventCount),
			};
			if (string.IsNullOrWhiteSpace(model.SessionId)
				|| string.IsNullOrWhiteSpace(model.ClientId)
				|| string.IsNullOrWhiteSpace(model.EventName)
				|| string.IsNullOrWhiteSpace(model.DateTimeStr)
				|| string.IsNullOrWhiteSpace(model.FullPageUrl)
				|| model.SessionStr == GADataConstants.NotSetValue
				|| model.EventName == GADataConstants.NotSetValue
				|| model.DateTimeStr == GADataConstants.NotSetValue
				|| model.FullPageUrl == GADataConstants.NotSetValue) {
				return null;
			}
			return model;
		};

		/// <summary>
		/// Delegate that returns deserialized user stats model or null.
		/// </summary>
		public static Func<string, GAWebUserStatsModel> ToUserStatsModel = (string rowStr) => {
			var userStatsDto = Json.Deserialize<GAWebUserStats>(rowStr);
			var model = new GAWebUserStatsModel {
				DateStr = userStatsDto.Date,
				StreamId = userStatsDto.StreamId,
				Host = userStatsDto.Host,
				PagePath = userStatsDto.PagePath,
				PageTitle = userStatsDto.PageTitle,
				DeviceCategory = userStatsDto.DeviceCategory,
				CountryCode = userStatsDto.CountryCode,
				ActiveUsers = GetStatsMetricCount(userStatsDto.ActiveUsers)
			};
			if (string.IsNullOrWhiteSpace(model.StreamId)
				|| string.IsNullOrWhiteSpace(model.PageUrl)
				|| model.Date == DateTime.MinValue
				|| model.ActiveUsers == 0) {
				return null;
			}
			return model;
		};

		/// <summary>
		/// Delegate that returns deserialized session stats model or null.
		/// </summary>
		public static Func<string, GAWebSessionStatsModel> ToSessionStatsModel = (string rowStr) => {
			var sessionStatsDto = Json.Deserialize<GAWebSessionStats>(rowStr);
			var model = new GAWebSessionStatsModel {
				DateStr = sessionStatsDto.Date,
				StreamId = sessionStatsDto.StreamId,
				Host = sessionStatsDto.Host,
				PagePath = sessionStatsDto.PagePath,
				PageTitle = sessionStatsDto.PageTitle,
				Medium = sessionStatsDto.Medium,
				Source = sessionStatsDto.Source,
				SessionCount = GetStatsMetricCount(sessionStatsDto.SessionCount)
			};
			if (string.IsNullOrWhiteSpace(model.StreamId)
				|| string.IsNullOrWhiteSpace(model.PageUrl)
				|| model.Date == DateTime.MinValue
				|| model.SessionCount == 0) {
				return null;
			}
			return model;
		};

		/// <summary>
		/// Delegate that returns deserialized event stats model or null.
		/// </summary>
		public static Func<string, GAWebEventStatsModel> ToEventStatsModel = (string rowStr) => {
			var eventStatsDto = Json.Deserialize<GAWebEventStats>(rowStr);
			var model = new GAWebEventStatsModel {
				DateStr = eventStatsDto.Date,
				StreamId = eventStatsDto.StreamId,
				Host = eventStatsDto.Host,
				PagePath = eventStatsDto.PagePath,
				PageTitle = eventStatsDto.PageTitle,
				EventName = eventStatsDto.EventName,
				EventCount = GetStatsMetricCount(eventStatsDto.EventCount)
			};
			if (string.IsNullOrWhiteSpace(model.StreamId)
				|| string.IsNullOrWhiteSpace(model.PageUrl)
				|| string.IsNullOrWhiteSpace(model.EventName)
				|| model.Date == DateTime.MinValue
				|| model.EventCount == 0) {
				return null;
			}
			return model;
		};

		/// <summary>
		/// Delegate that returns deserialized user specified session model or null.
		/// </summary>
		public static Func<GAUserSession, GAUserSessionModel> ToUserSessionModel = (GAUserSession sessionDto) => {
			if (sessionDto.SessionId == GADataConstants.NotSetValue || sessionDto.UserId == GADataConstants.NotSetValue) {
				return null;
			}
			var model = new GAUserSessionModel {
				SessionStr = sessionDto.SessionId,
				UserId = sessionDto.UserId,
				StreamId = sessionDto.StreamId
			};
			if (string.IsNullOrWhiteSpace(model.SessionId)
				|| string.IsNullOrWhiteSpace(model.ClientId)
				|| string.IsNullOrWhiteSpace(model.StreamId)
				|| model.ContactId.IsEmpty()
				|| model.StreamId == GADataConstants.NotSetValue
				|| model.SessionStr == GADataConstants.NotSetValue) {
				return null;
			}
			return model;
		};

		#endregion

	}
}
