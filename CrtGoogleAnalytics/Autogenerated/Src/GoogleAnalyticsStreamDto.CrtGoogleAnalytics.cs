namespace CrtGoogleAnalytics.Dto
{
	using System;

	#region Class: GoogleAnalyticsStreamDto

	/// <summary>
	/// Class to represent data transfer object for GA stream.
	/// </summary>
	public class GoogleAnalyticsStreamDto
	{

		#region Properties: Public

		/// <summary>
		/// Web analytics stream id.
		/// </summary>
		public Guid StreamId { get; set; }

		/// <summary>
		/// Google analytics stream id.
		/// </summary>
		public string GAStreamId { get; set; }

		/// <summary>
		/// Google analytics measurement id.
		/// </summary>
		public string MeasurementId { get; set; }

		/// <summary>
		/// Google analytics property id.
		/// </summary>
		public string PropertyId { get; set; }

		/// <summary>
		/// Google analytics account id.
		/// </summary>
		public string AccountId { get; set; }

		#endregion

	}

	#endregion

}

