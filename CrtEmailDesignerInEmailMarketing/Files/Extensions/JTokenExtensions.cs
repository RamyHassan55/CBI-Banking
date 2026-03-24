namespace CrtEmailDesignerInEmailMarketing.Extensions
{
	using System.Collections.Generic;
	using Newtonsoft.Json.Linq;
	using System.Linq;

	/// <summary>
	/// Extension methods for <see cref="JToken"/>.
	/// </summary>
	public static class JTokenExtensions
	{

		#region Constants: Private

		private const string PathToDescriptors = "$..descriptor";

		#endregion

		#region Methods: Private

		private static bool ContainsUnsubscribe(JToken token, List<string> macroses) {
			switch (token.Type) {
				case JTokenType.String:
					return macroses.Any(substring => ((string)token).Contains(substring));
				case JTokenType.Array:
					return token.Children().Any(x => ContainsUnsubscribe(x, macroses));
				case JTokenType.Object:
					return token.Children<JProperty>().Any(property => ContainsUnsubscribe(property.Value, macroses));
				default:
					return false;
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Finds unsubscribe link in BfTemplate JSON.
		/// </summary>
		/// <param name="templateJson">BfTemplate JSON</param>
		/// <param name="macroses">Unsubscribe macroses</param>
		/// <returns>True if unsubscribe link was found, false otherwise</returns>
		public static bool FindUnsubscribeLinkInBfTemplateJson(this JToken templateJson, List<string> macroses) {
			var descriptors = templateJson
				.SelectTokens(PathToDescriptors)
				.OfType<JObject>();
			return descriptors.Any(descriptor => descriptor
				.Properties()
				.Any(p => ContainsUnsubscribe(p.Value, macroses)));
		}

		#endregion

	}
}