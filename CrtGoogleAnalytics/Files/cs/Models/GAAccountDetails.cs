namespace CrtGoogleAnalytics.API.Models
{
	using System.Collections.Generic;

	#region Class: GAAccountDetails

	public class GAAccountDetails
	{
		public string Id { get; set; }
		public IEnumerable<GAProperty> Properties { get; set; }
	}

	#endregion

	#region Class: GAProperty

	public class GAProperty
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<GAWebDataStream> DataStreams { get; set; }
	}

	#endregion

	#region Class: GAWebDataStream

	public class GAWebDataStream
	{
		public string Name { get; set; }
		public string MeasurementId { get; set; }
		public string DefaultUri { get; set; }
		public string StreamId { get; set; }
	}

	#endregion

}
