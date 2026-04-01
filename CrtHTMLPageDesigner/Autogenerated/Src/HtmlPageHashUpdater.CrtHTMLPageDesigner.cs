namespace CrtHTMLPageDesigner
{
	using System.Collections.Generic;
	using System.Security.Cryptography;
	using System.Text;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: HtmlPageHashUpdater

	[DefaultBinding(typeof(IHtmlPageHashUpdater))]
	public class HtmlPageHashUpdater : IHtmlPageHashUpdater
	{

		#region Properties: Protected

		protected string Separator { get; set; } = "||";

		#endregion

		#region Methods: Private

		private string CalculateHash(string source) {
			byte[] bytes = Encoding.UTF8.GetBytes(source);
			using (SHA256 sha256 = SHA256.Create()) {
				byte[] hashBytes = sha256.ComputeHash(bytes);
				var sb = new StringBuilder();
				foreach (byte b in hashBytes) {
					sb.Append(b.ToString("x2"));
				}
				return sb.ToString();
			}
		}

		#endregion

		#region Methods: Protected

		protected virtual string BuildSourceString(Entity page) {
			var columns = new List<string> {
				"PageHtml", "ContentSlug"
			};
			var sb = new StringBuilder();
			foreach (var column in columns) {
				var value = page.GetTypedColumnValue<string>(column);
				sb.Append(value);
				sb.Append(Separator);
			}
			return sb.ToString();
		}

		#endregion

		#region Methods: Public

		public string UpdateHash(Entity page) {
			string source = BuildSourceString(page);
			var hash = CalculateHash(source);
			page.SetColumnValue("Hash", hash);
			return hash;
		}

		#endregion

	}

	#endregion

}
