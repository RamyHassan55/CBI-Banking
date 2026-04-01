using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CrtContentDesigner.Contracts;
using Terrasoft.Common;
using Terrasoft.Configuration;
using Terrasoft.Configuration.DynamicContent;
using Terrasoft.Configuration.DynamicContent.DataContract;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Entity = Terrasoft.Core.Entities.Entity;

#region Class: ${Name}

/// <summary>
/// Builder for email designer replicas.
/// </summary>
public class EmailDesignerReplicaBuilder
{

	#region Fields: Private

	private readonly UserConnection _userConnection;

	#endregion

	private static readonly Regex _startRegex =
		new Regex(@"BfConditionId=(.*?);BfGroupId=(.*?)", RegexOptions.Compiled);

	private static readonly Regex _endRegex = new Regex(@"END:BfConditionId=.*?;BfGroupId=.*?", RegexOptions.Compiled);

	private static readonly Regex _rowIdRegex = new Regex(@"row row-(\d+)", RegexOptions.Compiled);

	#region Constructors: Public

	/// <summary>
	/// Initializes a new instance of the <see cref="EmailDesignerReplicaBuilder"/> class.
	/// </summary>
	/// <param name="userConnection">The user connection.</param>
	public EmailDesignerReplicaBuilder(UserConnection userConnection) {
		_userConnection = userConnection;
	}

	#endregion

	#region Methods: Private

	private static IEnumerable<DCAttributeContract> GenerateDcAttributes(List<ConditionBlock> conditionBlocks) {
		var result = new List<DCAttributeContract>();
		var index = 1;
		var existedConditions = new List<string>();
		foreach (ConditionBlock block in conditionBlocks) {
			if (existedConditions.Contains(block.ConditionId)) {
				continue;
			}
			result.Add(new DCAttributeContract {
				TypeId = DCConstants.DCAttributeFilterTypeId,
				Index = index++,
				Caption = block.ConditionName,
				Value = block.ConditionRule
			});
			existedConditions.Add(block.ConditionId);
		}
		return result;
	}

	private static IEnumerable<DCTemplateBlockContract> GenerateDcBlocks(List<ConditionBlock> conditionBlocks) {
		var result = new List<DCTemplateBlockContract>();
		var conditionIdToAttributeIndex = new Dictionary<string, int>();
		var currentAttributeIndex = 1;
		var indexDegree = 0;
		foreach (ConditionBlock conditionBlock in conditionBlocks) {
			if (!conditionIdToAttributeIndex.TryGetValue(conditionBlock.ConditionId, out int attrIndex)) {
				attrIndex = currentAttributeIndex++;
				conditionIdToAttributeIndex[conditionBlock.ConditionId] = attrIndex;
			}
			var index = (int)Math.Pow(2, indexDegree++);
			result.Add(new DCTemplateBlockContract {
				Index = index,
				AttributeIndexes = new[] { attrIndex },
			});
			conditionBlock.Index = index;
		}
		return result;
	}

	private static IEnumerable<DCTemplateGroupContract> GenerateDcGroups(List<ConditionBlock> conditionBlocks) {
		IEnumerable<string> uniqueGroupIds = conditionBlocks.Select(conditionBlock => conditionBlock.GroupId ?? "null")
			.Distinct();
		List<DCTemplateGroupContract> groups = uniqueGroupIds.Select((groupId, index) => new DCTemplateGroupContract {
			Index = index
		}).ToList();
		return groups;
	}

	private static IEnumerable<DCReplicaContract> GenerateDcReplicas(ContentRows contentRows) {
		var result = new List<DCReplicaContract>();
		Dictionary<string, List<ConditionBlock>> blocksGroupedByCondition = contentRows.ConditionBlocks
			.GroupBy(conditionBlock => conditionBlock.ConditionId)
			.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
		List<List<ConditionBlock>> allCombinations =
			GetAllBlockCombinations(blocksGroupedByCondition, contentRows.Rows);
		foreach (List<ConditionBlock> conditionBlock in allCombinations) {
			string caption = string.Join(" + ",
				conditionBlock.Where(cb => cb.ConditionName.IsNotNullOrEmpty()).OrderBy(cb => cb.RowId)
					.Select(cb => cb.ConditionName).Distinct());
			string content = string.Join("", conditionBlock.OrderBy(cb => cb.RowId).Select(cb => cb.InnerHtml));
			int[] blockIndexes = conditionBlock.Select(cb => cb.Index).ToArray();
			int mask = conditionBlock.Sum(cb => cb.Index);
			var replica = new DCReplicaContract {
				Caption = caption,
				Content = content,
				BlockIndexes = blockIndexes,
				Mask = mask
			};
			result.Add(replica);
		}
		return result;
	}

	private static List<List<ConditionBlock>> GetAllBlockCombinations(
		Dictionary<string, List<ConditionBlock>> blocksGroupedByCondition, List<BfRow> defaultContentRows) {
		List<List<ConditionBlock>> allGroups = blocksGroupedByCondition.Values.ToList();
		var allCombinations = new List<List<ConditionBlock>> {
			defaultContentRows.Select(defaultContentRow => new ConditionBlock {
				RowId = defaultContentRow.RowId,
				InnerHtml = defaultContentRow.InnerHtml,
				Index = defaultContentRow.Index
			}).ToList()
		};
		foreach (List<ConditionBlock> group in allGroups) {
			var newCombinations = new List<List<ConditionBlock>>();
			foreach (List<ConditionBlock> existing in allCombinations) {
				newCombinations.Add(existing);
				var withGroup = new List<ConditionBlock>(existing);
				withGroup.AddRange(group);
				newCombinations.Add(withGroup);
			}
			allCombinations = newCombinations;
		}
		return allCombinations.Where(c => c.Any(x => x.ConditionName.IsNotNullOrEmpty())).ToList();
	}

	private void BuildReplicas(string htmlContent, DCTemplateContract dcTemplateContract) {
		ContentRows contentRows = ExtractConditionBlocks(htmlContent, dcTemplateContract.RecordId);
		if (contentRows.ConditionBlocks.Count == 0) {
			dcTemplateContract.Replicas = new[] {
				new DCReplicaContract {
					Content = htmlContent,
					Caption = "Static",
					Mask = 1
				}
			};
			return;
		}
		dcTemplateContract.Attributes = GenerateDcAttributes(contentRows.ConditionBlocks);
		dcTemplateContract.Blocks = GenerateDcBlocks(contentRows.ConditionBlocks);
		dcTemplateContract.Groups = GenerateDcGroups(contentRows.ConditionBlocks);
		dcTemplateContract.Replicas = GenerateDcReplicas(contentRows);
	}

	private ContentRows ExtractConditionBlocks(string html, Guid recordId) {
		var result = new ContentRows();
		EntityCollection conditions = GetConditions(recordId);
		if (conditions.IsNullOrEmpty()) {
			return result;
		}
		IBrowsingContext context = BrowsingContext.New(Configuration.Default);
		IDocument document = context.OpenAsync(req => req.Content(html)).Result;
		List<INode> allNodes = document.Body.Descendents().ToList();
		var isCollecting = false;
		string conditionId = null, groupId = null;
		INode collectedNode = null;
		foreach (INode node in allNodes) {
			if (node.NodeType == NodeType.Comment) {
				string text = node.TextContent;
				if (_endRegex.IsMatch(text) && isCollecting) {
					var conditionBlock = GetConditionBlock(conditions, conditionId, groupId, collectedNode);
					isCollecting = false;
					conditionId = groupId = null;
					if (conditionBlock == null) {
						continue;
					}
					result.ConditionBlocks.Add(conditionBlock);
					continue;
				}
				Match startMatch = _startRegex.Match(text);
				if (startMatch.Success) {
					conditionId = startMatch.Groups[1].Value;
					groupId = startMatch.Groups[2].Value;
					isCollecting = true;
					continue;
				}
			}
			if (!(node is IElement element) || element.TagName != "TABLE" ||
				!element.ClassList.ToString().Contains("row row")) {
				continue;
			}
			if (isCollecting) {
				collectedNode = node;
			} else {
				result.Rows.Add(GetBfRow(element));
			}
		}
		return result;
	}

	private BfRow GetBfRow(IElement element) {
		Match rowIdMatch = _rowIdRegex.Match(element.ClassName);
		int rowId = rowIdMatch.Success ? int.Parse(rowIdMatch.Groups[1].Value) : 0;
		return new BfRow {
			RowId = rowId,
			InnerHtml = element.OuterHtml
		};
	}

	private ConditionBlock GetConditionBlock(EntityCollection conditions, string conditionId, string groupId,
		INode collectedNode) {
		if (!(collectedNode is IHtmlTableElement element)) {
			return null;
		}
		Match rowIdMatch = _rowIdRegex.Match(element.OuterHtml);
		int rowId = rowIdMatch.Success ? int.Parse(rowIdMatch.Groups[1].Value) : 0;
		Guid conditionGuidId = Guid.TryParse(conditionId, out Guid conditionGuid) ? conditionGuid : Guid.Empty;
		Entity condition =
			conditions.FirstOrDefault(conditionEntity => conditionEntity.PrimaryColumnValue == conditionGuidId);
		if (condition == null) {
			return null;
		}
		string conditionName = condition?.GetTypedColumnValue<string>("ConditionName") ?? string.Empty;
		byte[] bytes = condition?.GetBytesValue("ConditionRule");
		string conditionRule = bytes.IsNotNullOrEmpty() ? Encoding.UTF8.GetString(bytes) : string.Empty;
		return new ConditionBlock {
			RowId = rowId,
			ConditionId = conditionId,
			GroupId = groupId,
			ConditionName = conditionName,
			ConditionRule = conditionRule,
			InnerHtml = element.OuterHtml
		};
	}

	private EntityCollection GetConditions(Guid recordId) {
		var displayConditionQuery = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BfDisplayCondition") {
			PrimaryQueryColumn = {
				IsAlwaysSelect = true
			}
		};
		displayConditionQuery.Filters.Add(
			displayConditionQuery.CreateFilterWithParameters(FilterComparisonType.Equal, "BulkEmail", recordId));
		displayConditionQuery.AddAllSchemaColumns();
		displayConditionQuery.AddColumn("ConditionName");
		displayConditionQuery.AddColumn("ConditionRule");
		return displayConditionQuery.GetEntityCollection(_userConnection);
	}

	#endregion

	#region Methods: Public

	/// <summary>
	/// Builds a dynamic content template contract from the provided save request and HTML content.
	/// </summary>
	public DCTemplateContract BuildDcTemplateContract(SaveTemplateRequest saveRequest, string htmlContent) {
		var template = new DCTemplateContract {
			RecordId = Guid.Parse(saveRequest.RecordId),
			LanguageId = Guid.Parse(saveRequest.LanguageId)
		};
		BuildReplicas(htmlContent, template);
		return template;
	}

	#endregion

}

#endregion

