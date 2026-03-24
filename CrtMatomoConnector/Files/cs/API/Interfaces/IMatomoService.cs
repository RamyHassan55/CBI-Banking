namespace MatomoConnector.API
{
	using System.Collections.Generic;

	/// <summary>
	/// Defines implementation of Matomo service.
	/// </summary>
	public interface IMatomoService
	{
		/// <summary>
		/// Returns all users by site URL address.
		/// </summary>
		IEnumerable<UserDetails> GetUsersBySiteUrl(string siteUrl, MatomoPeriod period, MatomoDate date,
			MatomoSegment segment);

		/// <summary>
		/// Returns all visit details by <paramref name="siteUrl"/>, <paramref name="period"/>, <paramref name="date"/>,
		/// <paramref name="segmentName"/> and a list of <paramref name="segmentValues"/>.
		/// </summary>
		LastVisitDetails GetLastVisitDetails(string siteUrl, MatomoPeriod period, MatomoDate date, string segmentName,
			IEnumerable<string> segmentValues);

		/// <summary>
		/// Returns all visit details by list of <paramref name="siteIds"/>, <paramref name="period"/>,
		/// <paramref name="date"/>, <paramref name="segmentName"/> and a list of <paramref name="segmentValues"/>.
		/// </summary>
		LastVisitDetails GetLastVisitDetails(List<int> siteIds, MatomoPeriod period, MatomoDate date,
			string segmentName, IEnumerable<string> segmentValues);

		/// <summary>
		/// Returns all site details for current account.
		/// </summary>
		IEnumerable<SiteDetails> GetAllSites();
	}
}
