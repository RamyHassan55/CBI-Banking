namespace CrtGoogleAnalytics
{
	using Terrasoft.Configuration;

	internal interface IGoogleAnalyticsAggregatedDataRepository
	{
		/// <summary>
		/// Saves aggregated analytics data into the DB.
		/// </summary>
		/// <param name="data">Collection of aggregated page statistics.</param>
		/// <returns>Save operation result.</returns>
		WebAnalyticsDataSaveResult SaveAnalytics(AnalyticsAggregatedData data);
	}
}

