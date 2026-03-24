namespace MatomoConnector.API
{

	/// <summary>
	/// Matomo segment operators.
	/// https://developer.matomo.org/api-reference/reporting-api-segmentation.
	/// </summary>
	public enum SegmentOperator {
		EQUALS = 0,
		NOT_EQUALS = 1,
		LESS_OR_EQUALS = 2,
		LESS = 3,
		GREATER_OR_EQUALS = 4,
		GREATER = 5,
		CONTAINS = 6,
		NOT_CONTAIN = 7,
		STARTS_WITH = 8,
		ENDS_WITH = 9
	}

}
