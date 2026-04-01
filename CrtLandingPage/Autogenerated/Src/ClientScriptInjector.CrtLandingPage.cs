namespace CrtLandingPage
{
	using System;
	using System.Linq;
	using System.Text.RegularExpressions;
	using CrtLandingPage.Dto;
	using CrtLandingPage.Parameters;
	using Terrasoft.Configuration.LandingiIntegration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using CoreConfig = Terrasoft.Core.Configuration;

	[DefaultBinding(typeof(IClientScriptInjector))]
	internal class ClientScriptInjector : IClientScriptInjector
	{

		private readonly UserConnection _userConnection;
		private readonly string _clientScriptCdnUrl = "https://webtracking-v01.creatio.com/JS/";

		/// <summary>
		/// Initializes a new instance of the <see cref="ClientScriptInjector"/> class.
		/// </summary>
		/// <param name="userConnection"></param>
		public ClientScriptInjector(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		private string WebhookServiceURL =>
			CoreConfig.SysSettings.GetValue(_userConnection, "WebhookURL").ToString();


		/// <summary>
		/// The webhook api key provider.
		/// </summary>
		private IWebhookApiKeyProvider _apiKeyProvider;
		public IWebhookApiKeyProvider ApiKeyProvider {
			get => _apiKeyProvider ?? (_apiKeyProvider = new ApiKeyProvider(_userConnection));
			set => _apiKeyProvider = value;
		}

		private string AppendWebhookScript(Guid landingId, string pageHtml) {
			var scriptRegex = new Regex(
				@"\s*<script src="".*crt-landing-page.js""></script>\s*<script>\s*crtLanding[\s\S]+?</script>",
				RegexOptions.Compiled);
			pageHtml = scriptRegex.Replace(pageHtml, "");
			var apiKeyResponse = ApiKeyProvider.GetOrCreateWebhookApiKey("Webhook Account Service Identity", "Web Form");
			if (!apiKeyResponse.Success) {
				throw new Exception("Webhook service settings are not configured.");
			}
			var serviceUrl = $"{WebhookServiceURL}{apiKeyResponse.ApiKey}";
			var script = $"\t<script src=\"{_clientScriptCdnUrl}crt-landing-page.js\"></script>\r\n" +
				"\t<script>\r\n" +
				$"\t\tcrtLanding.webhookServiceUrl = '{serviceUrl}';\r\n" +
				$"\t\tcrtLanding.setParameter('LandingPageId', '{landingId}');\r\n" +
				"\t</script>";
			return pageHtml.Replace("</head>", $"{script}\r\n</head>");
		}

		private string Append(LandingClientScript script, string pageHtml) {
			if (string.IsNullOrWhiteSpace(script.Content)) {
				return pageHtml;
			}
			script.Content = $"\t<!--- {script.Marker} --->\r\n{script.Content}" +
				$"\r\n\t<!--- End of {script.Marker} --->";
			switch (script.Position) {
				case ClientScriptPosition.HEAD_BEGIN:
					pageHtml = pageHtml.Replace("<head>", $"<head>\r\n{script.Content}");
					break;
				case ClientScriptPosition.HEAD_END:
					pageHtml = pageHtml.Replace("</head>", $"{script.Content}\r\n</head>");
					break;
				case ClientScriptPosition.BODY_BEGIN:
					pageHtml = pageHtml.Replace("<body>", $"<body>\r\n{script.Content}");
					break;
				case ClientScriptPosition.BODY_END:
					pageHtml = pageHtml.Replace("</body>", $"{script.Content}\r\n</body>");
					break;
			}
			return pageHtml;
		}

		/// <summary>
		/// Injects client scripts in html markup.
		/// </summary>
		/// <param name="landingId">The identifier of landing page.</param>
		/// <param name="pageHtml">Html markup of the landing page.</param>
		/// <returns>The result html markup of the landing page.</returns>
		public string Inject(Guid landingId, string pageHtml) {
			pageHtml = AppendWebhookScript(landingId, pageHtml);
			var scriptProviders = ClassFactory.GetAll<IClientScriptProvider>(
				new ConstructorArgument("userConnection", _userConnection));
			var scripts = scriptProviders.Select(p => p.GetScripts(landingId)).SelectMany(s => s);
			if (scripts.Where(x => string.IsNullOrWhiteSpace(x.Marker)).Any()) {
				throw new Exception("Some client script has no name.");
			}
			foreach (var script in scripts) {
				pageHtml = Append(script, pageHtml);
			}
			return pageHtml;
		}
	}
}

