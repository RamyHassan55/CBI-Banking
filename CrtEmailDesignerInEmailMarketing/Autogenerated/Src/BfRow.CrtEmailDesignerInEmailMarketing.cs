/// <summary>
/// Represents a row in the email designer.
/// </summary>
public class BfRow
{

	#region Properties: Public

	/// <summary>
	/// Gets or sets the index of the row.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Gets or sets the inner HTML content of the row.
	/// </summary>
	public string InnerHtml { get; set; }

	/// <summary>
	/// Gets or sets the row identifier.
	/// </summary>
	public int RowId { get; set; }

	#endregion

}

/// <summary>
/// Represents a condition block in the email designer.
/// </summary>
public class ConditionBlock : BfRow
{

	#region Properties: Public

	/// <summary>
	/// Gets or sets the identifier of the condition.
	/// </summary>
	public string ConditionId { get; set; }

	/// <summary>
	/// Gets or sets the group identifier of the condition.
	/// </summary>
	public string GroupId { get; set; }

	/// <summary>
	/// Gets or sets the inner HTML content of the condition block.
	/// </summary>
	public string ConditionName { get; set; }

	/// <summary>
	/// Gets or sets the rule associated with the condition.
	/// </summary>
	public string ConditionRule { get; set; }

	#endregion

}

