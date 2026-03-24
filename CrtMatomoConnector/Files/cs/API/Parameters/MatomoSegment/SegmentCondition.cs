namespace MatomoConnector.API
{

	/// <summary>
	/// Matomo segment conditions. 
	/// https://developer.matomo.org/api-reference/reporting-api-segmentation.
	/// Note that if you combine OR and AND operators, the OR operator will take precedence.
	/// For example, the following query: 
	/// &segment=referrerType==search;referrerKeyword==Piwik,referrerKeyword==analytics
	/// will select "Visitors from Search engines AND (Keyword is Matomo OR Keyword is analytics)"
	/// </summary>
	public enum SegmentCondition {
		EMPTY = 0,
		AND = 1,
		OR = 2
	}

}
