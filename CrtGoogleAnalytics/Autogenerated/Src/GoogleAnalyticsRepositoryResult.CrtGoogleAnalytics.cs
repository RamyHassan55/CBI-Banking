namespace CrtGoogleAnalytics
{
	internal class GoogleAnalyticsRepositoryResult
	{

		#region Constructors: Public

		public GoogleAnalyticsRepositoryResult(int affectedRows = 0, int totalRows = 0, int errorsCount = 0,
				string lastErrorMessage = "") {
			AffectedRows = affectedRows;
			TotalRows = totalRows;
			ErrorsCount = errorsCount;
			LastErrorMessage = lastErrorMessage ?? "";
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Total rows affected by the repository.
		/// </summary>
		public int AffectedRows { get; set; }

		/// <summary>
		/// Total number of processed rows by the reepository.
		/// </summary>
		public int TotalRows { get; set; }

		/// <summary>
		/// Total number of errors during operation.
		/// </summary>
		public int ErrorsCount { get; set; }

		/// <summary>
		/// Contains the message of the last exception or the empty string.
		/// </summary>
		public string LastErrorMessage { get; set; }

		#endregion

	}
}
