namespace CrtGoogleAnalytics.API.Parameters
{
	using System;

	public class DataRequestParameters
	{
		public string AccountId { get; set; }
		public string PropertyId { get; set; }
		public string StreamId { get; set; }
		public DateTime FromDate { get; set; }
		public int PeriodDays { get; set; }
		public int Offset { get; set; }
		public int Limit { get; set; }
	}
}
