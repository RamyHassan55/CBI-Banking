namespace Creatio.ContentDesigner.Services
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using Terrasoft.Web.Common;
	using System.ServiceModel.Web;
	using Common.Logging;
	using Terrasoft.Common.Threading;
	using Terrasoft.Core.Factories;

	#region Class: BfTemplateService

	/// <summary>
	/// Provides services for managing designer tokens.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class ContentDesignerTokenService : BaseService
	{

		#region Fields: Private

		private ILog _logger;
		private IContentDesignerTokenProvider _contentDesignerTokenService;

		#endregion

		#region Properties: Internal

		/// <summary>
		/// Provides logic to work with designer tokens
		/// </summary>
		internal IContentDesignerTokenProvider ContentDesignerTokenProvider {
			get =>
				_contentDesignerTokenService ?? (_contentDesignerTokenService =
					ClassFactory.Get<IContentDesignerTokenProvider>(new ConstructorArgument("userConnection",
						UserConnection)));
			set => _contentDesignerTokenService = value;
		}

		/// <summary>
		/// The logger
		/// </summary>
		internal ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(nameof(ContentDesignerTokenService)));
			set => _logger = value;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets the designer token
		/// </summary>
		/// <param name="request">The token request containing the designer code.</param>
		/// <returns>Access token for the designer</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public GetDesignerTokenResponse GetToken(DesignerTokenRequest request) {
			try {
				GetDesignerTokenResponse tokenResponse = AsyncPump.Run(() =>
					ContentDesignerTokenProvider.GetToken(request.DesignerCode, request.AppVersion));
				return tokenResponse;
			} catch (Exception exception) {
				Logger.Error(exception.Message, exception);
				return new GetDesignerTokenResponse {
					Error = exception.Message
				};
			}
		}

		#endregion

	}

	#endregion

}
