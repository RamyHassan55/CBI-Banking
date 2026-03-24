namespace Creatio.ContentDesigner
{
	using System;
	using System.Net.Http;
	using System.Collections.Generic;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core;
	using Terrasoft.Common.Threading;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	/// <summary>
	/// Provides logic to work with BeeFree services
	/// </summary>
	[DefaultBinding(typeof(IBfContentServicesAPIClient))]
	internal class BfContentServicesAPIClient : BaseBfServiceProvider, IBfContentServicesAPIClient
	{

		#region Class: ServiceGroup

		private class ServiceGroup
		{
			private static readonly Dictionary<ContentDesignerType, string> _contentBuilderToServiceGroupMapping =
				new Dictionary<ContentDesignerType, string> {
					{ ContentDesignerType.Email, Email },
					{ ContentDesignerType.Page, Page }
				};

			public const string Email = "message";
			public const string Page = "page";
			public const string Conversion = "conversion";

			public static string GetByDesignerType(ContentDesignerType type) {
				_contentBuilderToServiceGroupMapping.TryGetValue(type, out var group);
				return group;
			}
		}

		#endregion

		#region Constants: Private

		private const string ContentServicesUrlSysSettingCode = "BeefreeContentServicesUrl";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="BfContentServicesAPIClient"/>
		/// </summary>
		/// <param name="userConnection">User connection instance</param>
		public BfContentServicesAPIClient(UserConnection userConnection) {
			UserConnection = userConnection;
			var baseServiceUrl = "https://api.getbee.io/v1/";
			if (SystemSettings.TryGetValue(userConnection, ContentServicesUrlSysSettingCode, out object settingValue)) {
				baseServiceUrl = settingValue.ToString();
			}
			HttpClient = new HttpClient {
				BaseAddress = new Uri(baseServiceUrl)
			};
		}

		#endregion

		#region Properties: Protected

		protected override string LoggerName => nameof(BfContentServicesAPIClient);

		#endregion

		#region Properties: Internal

		/// <summary>
		/// Provides logic to work with designer tokens
		/// </summary>
		private IContentDesignerTokenProvider _contentDesignerTokenService;
		internal IContentDesignerTokenProvider ContentDesignerTokenProvider {
			get => _contentDesignerTokenService ?? (_contentDesignerTokenService =
				ClassFactory.Get<IContentDesignerTokenProvider>(
					new ConstructorArgument("userConnection", UserConnection)));
			set => _contentDesignerTokenService = value;
		}

		#endregion

		#region Methods: Private

		private string GetAccessToken(ContentDesignerType designerType) {
			var designerCode = ContentDesignerCode.GetByDesignerType(designerType);
			var tokenResponse = AsyncPump.Run(() =>
					ContentDesignerTokenProvider.GetToken(designerCode, string.Empty));
			return tokenResponse.AccessToken;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IBfContentServicesAPIClient.TransformConfigToHTML"/>
		public string TransformConfigToHTML(string contentConfig, ContentDesignerType designerType) {
			var collection = ServiceGroup.GetByDesignerType(designerType);
			var accessToken = GetAccessToken(designerType);
			var requestMessage = GetHttpRequestMessage(HttpMethod.Post, $"{collection}/html", accessToken,
				contentConfig);
			var response = AsyncPump.Run(() => SendRequest<string>(requestMessage));
			if (!response.Success) {
				return string.Empty;
			}
			return response.Content;
		}

		/// <inheritdoc cref="IBfContentServicesAPIClient.ConvertSimpleToFullJson"/>
		public string ConvertSimpleToFullJson(string contentConfig, ContentDesignerType designerType) {
			var collection = ServiceGroup.Conversion;
			var accessToken = GetAccessToken(designerType);
			var requestMessage = GetHttpRequestMessage(HttpMethod.Post, $"{collection}/simple-to-full-json",
				accessToken,
				contentConfig);
			var response = AsyncPump.Run(() => SendRequest<string>(requestMessage));
			if (!response.Success) {
				return string.Empty;
			}
			return response.Content;
		}

		#endregion

	}
}
