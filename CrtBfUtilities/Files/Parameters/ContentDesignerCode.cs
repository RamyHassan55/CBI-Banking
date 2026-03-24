using System.Collections.Generic;

namespace Creatio.ContentDesigner
{
	internal class ContentDesignerCode
	{
		private static readonly Dictionary<ContentDesignerType, string> _contentBuilderTypeToCodeMapping =
			new Dictionary<ContentDesignerType, string> {
				{ ContentDesignerType.Email, Email },
				{ ContentDesignerType.Page, Page }
			};

		public const string Email = "email_builder";
		public const string Page = "page_builder";

		public static string GetByDesignerType(ContentDesignerType type) {
			_contentBuilderTypeToCodeMapping.TryGetValue(type, out var code);
			return code;
		}
	}
}
