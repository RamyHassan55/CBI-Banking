namespace MatomoConnector.API
{
	using System.Collections.Generic;

	/// <summary>
	/// Matomo simple segment like &segment=countryCode==US or &segment=countryCode==US,userId==someUserId.
	/// https://developer.matomo.org/api-reference/reporting-api-segmentation.
	/// </summary>
	public class MatomoSegment {

		#region Constants: Public

		/// <summary>
		/// String representation of segment operators.
		/// </summary>
		public readonly Dictionary<SegmentOperator, string> SegmentOperators = new Dictionary<SegmentOperator, string> {
			[SegmentOperator.EQUALS] = "==",
			[SegmentOperator.NOT_EQUALS] = "!=",
			[SegmentOperator.LESS_OR_EQUALS] = "<=",
			[SegmentOperator.LESS] = "<",
			[SegmentOperator.GREATER_OR_EQUALS] = ">=",
			[SegmentOperator.GREATER] = "<",
			[SegmentOperator.CONTAINS] = "@=",
			[SegmentOperator.NOT_CONTAIN] = "!@",
			[SegmentOperator.STARTS_WITH] = "=^",
			[SegmentOperator.ENDS_WITH] = "=$"
		};

		/// <summary>
		/// String representation of segment conditions.
		/// </summary>
		public readonly Dictionary<SegmentCondition, string> SegmentConditions = new Dictionary<SegmentCondition, string> {
			[SegmentCondition.EMPTY] = null,
			[SegmentCondition.AND] = ";",
			[SegmentCondition.OR] = ","
		};

		#endregion

		#region Constructors: Public

		public MatomoSegment(string column, SegmentOperator segmentOperator, string value) {
			Column = column;
			Value = value;
			SegmentOperator = segmentOperator;
		}

		public MatomoSegment(string column, SegmentOperator segmentOperator, string value,
				SegmentCondition condition, MatomoSegment nextSegment) : this(column, segmentOperator, value) {
			Condition = condition;
			NextSegment = nextSegment;
		}

		#endregion

		#region Properties: Public

		public string Column { get; set; }
		public string Value { get; set; }
		public SegmentOperator SegmentOperator { get; set; }
		public SegmentCondition Condition { get; set; } = SegmentCondition.EMPTY;
		public MatomoSegment NextSegment { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Helps to build a chain of segments and conditions.
		/// </summary>
		public MatomoSegment AddCondition(SegmentCondition condition, MatomoSegment nextSegment) {
			Condition = condition;
			NextSegment = nextSegment;
			return nextSegment;
		}

		public override string ToString() {
			if (string.IsNullOrWhiteSpace(Column)) {
				return string.Empty;
			}
			var segmentString = $"{Column}{SegmentOperators[SegmentOperator]}{Value}";
			if (NextSegment == default || Condition == SegmentCondition.EMPTY) {
				return segmentString;
			}
			var nextSegmentString = NextSegment.ToString();
			return string.IsNullOrWhiteSpace(nextSegmentString)
				? segmentString
				: $"{segmentString}{SegmentConditions[Condition]}{nextSegmentString}";
		}

		#endregion

	}
}
