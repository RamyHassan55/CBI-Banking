namespace CrtGoogleAnalytics.API.Dto
{
	using System.Collections.Generic;
	using CrtGoogleAnalytics.API.Models;
	using Newtonsoft.Json;

	#region Class: GADataResponse

	internal class GADataResponse
	{
		[JsonProperty("dimensionHeaders")]
		public List<DimensionHeader> DimensionHeaders { get; set; }

		[JsonProperty("metricHeaders")]
		public List<DimensionHeader> MetricHeaders { get; set; }

		[JsonProperty("rows")]
		public List<DataRow> Rows { get; set; }

		[JsonProperty("rowCount")]
		public int RowCount { get; set; }

		[JsonProperty("propertyQuota")]
		public GAPropertyQuota PropertyQuota { get; set; }
	}

	#endregion

	#region Class: DimensionHeader

	internal class DimensionHeader
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}

	#endregion

	#region Class: DimensionValue

	internal class DimensionValue
	{
		[JsonProperty("value")]
		public string Value { get; set; }
	}

	#endregion

	#region Class: DataRow

	internal class DataRow
	{
		[JsonProperty("dimensionValues")]
		public List<DimensionValue> DimensionValues { get; set; }

		[JsonProperty("metricValues")]
		public List<DimensionValue> MetricValues { get; set; }
	}

	#endregion

}
