namespace CrtGoogleAnalytics.API.Dto
{
	using Newtonsoft.Json;
	using System.Collections.Generic;
	using System.Linq;

	#region Class: GADimensionFilter

	internal class GADimensionFilter : DataFilter
	{

		#region Constructors: Public

		public GADimensionFilter() { }

		public GADimensionFilter(DataFilter filter) {
			Filter = filter.Filter;
			NotExpression = filter.NotExpression;
		}

		#endregion

		#region Properties: Public

		[JsonProperty("andGroup", NullValueHandling = NullValueHandling.Ignore)]
		public FilterGroup AndGroup { get; set; }

		[JsonProperty("orGroup", NullValueHandling = NullValueHandling.Ignore)]
		public FilterGroup OrGroup { get; set; }

		#endregion

		#region Methods: Public

		public IEnumerable<string> GetFilterFields() {
			var filterField = GetFilterField();
			if (filterField != null) {
				return new List<string> { filterField };
			}
			var filterFields = AndGroup?.GetFilterFields();
			return filterFields != null && filterFields.Any() ? filterFields : OrGroup?.GetFilterFields();
		}

		#endregion

	}

	#endregion

	#region Class: DataFilter

	internal class DataFilter
	{

		#region Properties: Public

		[JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
		public Filter Filter { get; set; }

		[JsonProperty("notExpression", NullValueHandling = NullValueHandling.Ignore)]
		public DataFilter NotExpression { get; set; }

		#endregion

		#region Methods: Public

		public string GetFilterField() {
			return Filter?.FieldName ?? NotExpression?.Filter.FieldName;
		}

		#endregion

	}

	#endregion

	#region Class: FilterGroup

	internal class FilterGroup
	{

		#region Properties: Public

		[JsonProperty("expressions")]
		public DataFilter[] Expressions { get; set; }

		#endregion

		#region Methods: Public

		public virtual IEnumerable<string> GetFilterFields() {
			return Expressions.Select(x => x.GetFilterField()).Distinct();
		}

		#endregion

	}

	#endregion

	#region Class: Filter

	internal class Filter
	{

		#region Properties: Public

		[JsonProperty("fieldName")]
		public string FieldName { get; set; }

		[JsonProperty("stringFilter", NullValueHandling = NullValueHandling.Ignore)]
		public StringFilter StringFilter { get; set; }

		[JsonProperty("inListFilter", NullValueHandling = NullValueHandling.Ignore)]
		public InListFilter InListFilter { get; set; }

		#endregion

	}

	#endregion

	#region Class: StringFilter

	internal class StringFilter
	{

		#region Properties: Public

		[JsonProperty("matchType")]
		public string MatchType { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		#endregion

	}

	#endregion

	#region Class: InListFilter

	internal class InListFilter
	{
		#region Properties: Public

		[JsonProperty("values")]
		public string[] Values { get; set; }

		#endregion

	}

	#endregion

}
