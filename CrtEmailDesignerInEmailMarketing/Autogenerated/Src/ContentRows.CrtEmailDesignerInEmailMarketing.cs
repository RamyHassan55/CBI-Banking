using System.Collections.Generic;

/// <summary>
/// Represents a collection of content rows and condition blocks extracted from an email template.
/// </summary>
public class ContentRows
{

	#region Properties: Public

	/// <summary>
	/// Gets or sets the list of rows in the content.
	/// </summary>
	public List<BfRow> Rows { get; set; } = new List<BfRow>();

	/// <summary>
	/// Gets or sets the list of condition blocks in the content.
	/// </summary>
	public List<ConditionBlock> ConditionBlocks { get; set; } = new List<ConditionBlock>();

	#endregion

}

