namespace CrtGoogleAnalytics.API.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CrtGoogleAnalytics.API.Models;
	using CrtGoogleAnalytics.API.Parameters;
	using Terrasoft.Configuration;

	/// <summary>
	/// Declares implementation of Google Analytics Data service.
	/// </summary>
	public interface IGADataService
	{
		/// <summary>
		/// Returns all Google analytics accounts by user identifier.
		/// <param name="platformUserId">Google profile identifier</param>
		/// </summary>
		IEnumerable<IWebAnalyticsAccount> GetAccountList(string platformUserId);

		/// <summary>
		/// Returns Google analytics account details.
		/// <param name="accountId">Google analytics account identifier</param>
		/// </summary>
		GAAccountDetails GetAccountDetails(string accountId);

		/// <summary>
		/// Returns web sessions by visitor identifier.
		/// <param name="userId">Google analytics user/visitor identifier</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		GADataResponse<GAWebSessionModel> GetSessionsByUserId(string userId, DataRequestParameters parameters);

		/// <summary>
		/// Returns web actions by visitor identifier.
		/// <param name="userId">Google analytics user/visitor identifier</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		GADataResponse<GAWebActionModel> GetActionsByUserId(string userId, DataRequestParameters parameters);

		/// <summary>
		/// Returns web sessions for visitors' segment.
		/// <param name="users">Google analytics users' segment</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		GADataResponse<GAWebSessionModel> GetSessionsForUsers(IEnumerable<string> users, DataRequestParameters parameters);

		/// <summary>
		/// Returns web actions for visitors' segment.
		/// <param name="userId">Google analytics users' segment</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		GADataResponse<GAWebActionModel> GetActionsForUsers(IEnumerable<string> users, DataRequestParameters parameters);
	}

	/// <summary>
	/// Declares implementation of Google Analytics users Data service.
	/// </summary>
	public interface IGAUserDataService
	{
		/// <summary>
		/// Returns all tracked by GA user sessions with userIds.
		/// </summary>
		IEnumerable<GAUserSessionModel> GetAvailableUserSessions(DataRequestParameters parameters);
	}

	public interface IGAStatsDataService
	{
		/// <summary>
		/// Returns GA active users statistics for streams specified.
		/// </summary>
		/// <param name="streams">Streams info to get data.</param>
		/// <param name="parameters">Request parameters.</param>
		/// <returns>Response of type <see cref="GADataResponse<GAWebUserStatsModel>"/></returns>
		GADataResponse<GAWebUserStatsModel> GetUserStats(IEnumerable<StreamWebSite> streams,
				DataRequestParameters parameters);

		/// <summary>
		/// Returns GA site events statistics for streams specified.
		/// </summary>
		/// <param name="streams">Streams info to get data.</param>
		/// <param name="parameters">Request parameters.</param>
		/// <returns>Response of type <see cref="GADataResponse<GAWebEventStatsModel>"/></returns>
		GADataResponse<GAWebEventStatsModel> GetEventStats(IEnumerable<StreamWebSite> streams,
				DataRequestParameters parameters);

		/// <summary>
		/// Returns GA user sessions by channels statistics for streams specified.
		/// </summary>
		/// <param name="streams">Streams info to get data.</param>
		/// <param name="parameters">Request parameters.</param>
		/// <returns>Response of type <see cref="GADataResponse<GAWebSessionStatsModel>"/></returns>
		GADataResponse<GAWebSessionStatsModel> GetSessionStats(IEnumerable<StreamWebSite> streams,
				DataRequestParameters parameters);
	}
}
