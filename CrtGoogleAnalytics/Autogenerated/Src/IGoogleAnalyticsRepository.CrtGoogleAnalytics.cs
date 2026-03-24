namespace CrtGoogleAnalytics
{
	using System.Collections.Generic;
	using System;
	using CrtGoogleAnalytics.API.Models;

	internal interface IGoogleAnalyticsRepository
	{
		/// <summary>
		/// Saves visits into the database.
		/// </summary>
		GoogleAnalyticsRepositoryResult SaveSessions(IEnumerable<GAWebSessionModel> sessions, Guid visitorId);

		/// <summary>
		/// Saves visits into the database for multiple visitors.
		/// </summary>
		GoogleAnalyticsRepositoryResult SaveSessions(IEnumerable<GAWebSessionModel> sessions,
			IDictionary<string, Guid[]> visitorsMapping);

		/// <summary>
		/// Saves actions into the database.
		/// </summary>
		GoogleAnalyticsRepositoryResult SaveActions(IEnumerable<GAWebActionModel> actions);

		/// <summary>
		/// Returns last session date by google analytics visitor id.
		/// </summary>
		DateTime GetLastSessionDateByVisitorId(string clientId);
	}
}
