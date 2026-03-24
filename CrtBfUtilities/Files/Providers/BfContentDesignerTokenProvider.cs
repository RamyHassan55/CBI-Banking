namespace Creatio.ContentDesigner
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.OAuthIntegration;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	/// <summary>
	/// Provides logic to work with BF tokens
	/// </summary>
	[DefaultBinding(typeof(IContentDesignerTokenProvider))]
	internal class BfContentDesignerTokenProvider : BaseBfServiceProvider, IContentDesignerTokenProvider
	{

		#region Constants: Private

		private const string GetBeefreeTokenPath = "api/beefreetoken/access/";
		private const string ContentDesignerUIdSysSettingCode = "ContentDesignerUId";
		private const string EmailBuilderInstanceSysSettingCode = "EmailBuilderInstance";
		private const string LandingPageBuilderInstanceSysSettingCode = "LandingPageBuilderInstance";
		private const string DefaultInstanceName = "Production";
		private const string Scope = "beefree_api";
		private const string SocialAccountUrlSysSettingCode = "SocialAccountServiceUrl";

		#endregion

		#region Fields: Private

		private readonly Dictionary<string, string> _contentBuilderToInstanceNameMapping =
			new Dictionary<string, string> {
				{ ContentDesignerCode.Email, EmailBuilderInstanceSysSettingCode },
				{ ContentDesignerCode.Page, LandingPageBuilderInstanceSysSettingCode }
			};

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="BfContentDesignerTokenProvider"/>
		/// </summary>
		/// <param name="userConnection">User connection instance</param>
		public BfContentDesignerTokenProvider(UserConnection userConnection) {
			var socialAccountServiceUrl = SystemSettings.GetValue(userConnection, SocialAccountUrlSysSettingCode);
			var baseAddress = new Uri(socialAccountServiceUrl.ToString());
			HttpClient = new HttpClient {
				BaseAddress = baseAddress
			};
		}

		#endregion

		#region Properties: Protected

		protected override string LoggerName => nameof(BfContentDesignerTokenProvider);

		#endregion

		#region Methods: Private

		private string GetAccessToken() {
			var identityServiceWrapper = GlobalAppSettings.FeatureUseSeparateSettingsForOAuth20
				? ClassFactory.Get<IIdentityServiceWrapper>("ExternalAccess")
				: ClassFactory.Get<IIdentityServiceWrapper>();
			var token = identityServiceWrapper.GetAccessToken(Scope);
			return token;
		}

		private string GetContentBuilderInstance(string designerCode) {
			if (_contentBuilderToInstanceNameMapping.TryGetValue(designerCode, out var sysSettingCode)) {
				var instanceName = SystemSettings.GetValue(UserConnection.Current, sysSettingCode, string.Empty);
				return !string.IsNullOrEmpty(instanceName) ? instanceName : DefaultInstanceName;
			}
			throw new ArgumentException($"Invalid designer code: {designerCode}", nameof(designerCode));
		}

		private string GetUid() {
			var uid = SystemSettings.GetValue(UserConnection.Current, ContentDesignerUIdSysSettingCode, string.Empty);
			var emailDesignerUId = SystemSettings.GetValue(UserConnection.Current, "EmailDesignerUId", string.Empty);
			if (emailDesignerUId.IsNotNullOrWhiteSpace() && uid.IsNullOrEmpty()) {
				uid = emailDesignerUId;
				SystemSettings.SetValue(UserConnection.Current, ContentDesignerUIdSysSettingCode, uid);
			}
			if (!string.IsNullOrWhiteSpace(uid)) {
				return uid;
			}
			var newUid = Guid.NewGuid().ToString();
			SystemSettings.SetDefValue(UserConnection.Current, ContentDesignerUIdSysSettingCode, newUid);
			return newUid;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IContentDesignerTokenProvider.GetToken"/>
		public async Task<GetDesignerTokenResponse> GetToken(string designerCode, string appVersion) {
			var bfTokenRequestBody = new GetBfTokenRequest {
				UId = GetUid(),
				InstanceName = GetContentBuilderInstance(designerCode),
				AppVersion = appVersion
			};
			var requestMessage = GetHttpRequestMessage(HttpMethod.Post, $"{GetBeefreeTokenPath}{designerCode}",
				GetAccessToken(), bfTokenRequestBody);
			var response = await SendRequest<GetDesignerTokenResponse>(requestMessage);
			var bfTokenResponse = response.Content;
			if (response.Success) {
				bfTokenResponse.IsSuccess = response.Success;
			} else {
				bfTokenResponse = new GetDesignerTokenResponse {
					Error = response.Error
				};
			}
			return bfTokenResponse;
		}

		#endregion

	}
}
