
namespace CrtGoogleAnalytics.API.Dto
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using Newtonsoft.Json;

	#region Class: GADataRequest

	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	internal class GADataRequest
	{

		#region Properties: Public

		[JsonProperty("dimensions")]
		public Dimension[] Dimensions { get; set; }

		[JsonProperty("metrics")]
		public Dimension[] Metrics { get; set; }

		[JsonProperty("dimensionFilter")]
		public GADimensionFilter DimensionFilter { get; set; }

		[JsonProperty("dateRanges")]
		public DateRange[] DateRanges { get; set; }

		[JsonProperty("orderBys")]
		public OrderBy[] OrderBys { get; set; }

		[DefaultValue(false)]
		[JsonProperty("returnPropertyQuota", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public bool ReturnPropertyQuota { get; set; }

		[DefaultValue(0)]
		[JsonProperty("limit", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int Limit { get; set; }

		[DefaultValue(0)]
		[JsonProperty("offset", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int Offset { get; set; }

		#endregion

		#region Methods: Public

		public IEnumerable<string> GetUsedDimensions() {
			return DimensionFilter == default
				? Dimensions.Select(d => d.Name)
				: Dimensions.Select(d => d.Name).Union(DimensionFilter.GetFilterFields());
		}

		#endregion

	}

	#endregion

	#region Class: Dimension

	internal class Dimension
	{

		#region Properties: Public

		[JsonProperty("name")]
		public string Name { get; set; }

		#endregion

	}

	#endregion

	#region Class: DateRange

	internal class DateRange
	{

		#region Properties: Public

		[JsonProperty("startDate")]
		public string StartDate { get; set; }

		[JsonProperty("endDate")]
		public string EndDate { get; set; }

		#endregion

	}

	#endregion

	#region Class: OrderBy

	internal class OrderBy
	{

		#region Properties: Public

		[JsonProperty("dimension")]
		public OrderDimension Dimension { get; set; }

		[JsonProperty("desc")]
		public bool Desc { get; set; }

		#endregion

	}

	#endregion

	#region Class: OrderDimension

	internal class OrderDimension
	{

		#region Properties: Public

		[JsonProperty("dimensionName")]
		public string DimensionName { get; set; }

		[JsonProperty("orderType")]
		public string OrderType { get; set; }

		#endregion

	}

	#endregion

}
