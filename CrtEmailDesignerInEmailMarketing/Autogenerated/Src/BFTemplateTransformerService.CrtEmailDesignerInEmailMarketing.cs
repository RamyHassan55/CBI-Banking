namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	#region Class: BFTemplateTransformerService

	/// <summary>
	/// Service for transforming templates in bulk email.
	/// </summary>
	public static class BFTemplateTransformerService
	{

		#region Constants: Private

		private const string BlockPattern = "\"displayCondition\"\\s*:\\s*\\{([^{}]*)\\}";
		private const string DescPattern = "(\"description\"\\s*:\\s*\")([^\"]*)(\")";
		private const string LabelPattern = "(\"label\"\\s*:\\s*\")([^\"]*)(\")";

		#endregion

		#region Methods: Private

		private static void ReplaceGuidInComment(JObject obj, string propertyName, Guid oldGuid, Guid newGuid) {
			var original = obj[propertyName]?.ToString();
			if (!string.IsNullOrEmpty(original)) {
				string replaced = original.Replace(oldGuid.ToString().ToLowerInvariant(),
					newGuid.ToString().ToLowerInvariant());
				obj[propertyName] = replaced;
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Replaces all labels and descriptions in the JSON template by condition ID.
		/// </summary>
		public static string ReplaceAllLabelsAndDescriptionsByConditionId(string json, string conditionId,
			string newLabel, string newDescription) {
			if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(conditionId)) {
				return json;
			}
			return Regex.Replace(json, BlockPattern, Rewriter, RegexOptions.CultureInvariant | RegexOptions.Singleline);
			string Rewriter(Match m) {
				string inner = m.Groups[1].Value;
				var idPattern = $"\"conditionId\"\\s*:\\s*\"{conditionId}\"";
				if (!Regex.IsMatch(inner, idPattern, RegexOptions.CultureInvariant)) {
					return m.Value;
				}
				inner = Regex.IsMatch(inner, LabelPattern, RegexOptions.CultureInvariant)
					? Regex.Replace(inner, LabelPattern, match => 
						match.Groups[1].Value + newLabel + match.Groups[3].Value, RegexOptions.CultureInvariant)
					: $"\"label\":\"{newLabel}\",{(inner.Length > 0 ? inner.TrimStart() : string.Empty)}";
				inner = Regex.IsMatch(inner, DescPattern, RegexOptions.CultureInvariant)
					? Regex.Replace(inner, DescPattern, match => match.Groups[1].Value + 
						newDescription + match.Groups[3].Value, RegexOptions.CultureInvariant)
					: $"\"description\":\"{newDescription}\",{(inner.Length > 0 ? inner.TrimStart() : string.Empty)}";
				string prefix = m.Value.Substring(0, m.Groups[1].Index - m.Index);
				string suffix = m.Value.Substring(m.Groups[1].Index - m.Index + m.Groups[1].Length);
				return prefix + inner + suffix;
			}
		}

		/// <summary>
		/// Replaces condition rules in HTML template.
		/// </summary>
		public static string ReplaceConditionRulesInHtmlTemplate(string htmlTemplate,
			Dictionary<Guid, Guid> oldNewIdConditionMap) {
			foreach (KeyValuePair<Guid, Guid> kvp in oldNewIdConditionMap) {
				string oldId = kvp.Key.ToString().ToLowerInvariant();
				string newId = kvp.Value.ToString().ToLowerInvariant();
				htmlTemplate = Regex.Replace(htmlTemplate,
					@"(<!--(?:END:)?" + "BfConditionId=)" + Regex.Escape(oldId) + @"(;BfGroupId=.*?-->)",
					match => match.Groups[1].Value + newId + match.Groups[2].Value, RegexOptions.IgnoreCase);
			}
			return htmlTemplate;
		}

		/// <summary>
		/// Replaces condition rules in JSON template.
		/// </summary>
		public static string ReplaceConditionRulesInJSONTemplate(string jsonTemplate,
			Dictionary<Guid, Guid> oldNewIdConditionMap) {
			JObject jObject = JObject.Parse(jsonTemplate);
			JToken rows = jObject["page"]?["rows"];
			var rowsArray = rows as JArray;
			if (rowsArray == null) {
				return jsonTemplate;
			}
			foreach (JToken row in rowsArray) {
				JToken displayCondition = row["container"]?["displayCondition"];
				if (displayCondition is JObject conditionObject) {
					var oldConditionIdStr = conditionObject["conditionId"]?.ToString();
					if (Guid.TryParse(oldConditionIdStr, out Guid oldGuid) &&
						oldNewIdConditionMap.TryGetValue(oldGuid, out Guid newGuid)) {
						conditionObject["conditionId"] = newGuid.ToString().ToLowerInvariant();
						ReplaceGuidInComment(conditionObject, "before", oldGuid, newGuid);
						ReplaceGuidInComment(conditionObject, "after", oldGuid, newGuid);
					}
				}
			}
			return jObject.ToString(Formatting.None);
		}

		#endregion

	}

	#endregion

}

