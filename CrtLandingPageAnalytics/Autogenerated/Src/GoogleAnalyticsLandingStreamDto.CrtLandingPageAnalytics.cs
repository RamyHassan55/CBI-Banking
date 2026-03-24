namespace CrtLandingPageAnalytics
{
	using System;
	using CrtGoogleAnalytics.Dto;

	#region Class: GoogleAnalyticsLandingStreamDto


	/// <summary>
	/// Class to represent data transfer object for landing page and GA stream.
	/// </summary>
	public class GoogleAnalyticsLandingStreamDto : GoogleAnalyticsStreamDto
	{

		#region Properties: Public

		/// <summary>
		/// Identifier of the landing page.
		/// </summary>
		public Guid LandingPageId { get; set; }

		/// <summary>
		/// Url of the landing page.
		/// </summary>
		public string LandingPageUrl { get; set; }

		/// <summary>
		/// Start date to retreive data from GA.
		/// </summary>
		public DateTime Date { get; set; }

		#endregion

	}

	#endregion

}
