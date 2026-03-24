namespace CrtEmailDesigner.Services
{
	using System;
	using System.ServiceModel.Activation;
	using System.ServiceModel;
	using Terrasoft.Web.Common;
	using System.ServiceModel.Web;
	using Common.Logging;
	using CrtEmailDesigner.Providers;
	using Terrasoft.Core.Factories;
	using CrtEmailDesigner.Models;
	using CrtEmailDesigner.Contracts;


	#region Class: BfTemplateService
	/// <summary>
	/// Provides services for managing Beefree tokens.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[Obsolete("Service will be removed in upcoming releases")]
	public class BfTokenService : BaseService
	{

		#region Fields: Private

		private IBfTokenProvider _bfTokenService;
		private ILog _logger;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Provides logic to work with Beefree tokens
		/// </summary>
		public IBfTokenProvider BfTokenProvider {
			get =>
				_bfTokenService ??
				(_bfTokenService = ClassFactory.Get<IBfTokenProvider>(
					new ConstructorArgument("userConnection", UserConnection)));
			set => _bfTokenService = value;
		}

		/// <summary>
		/// The logger
		/// </summary>
		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(nameof(BfTokenService)));
			set => _logger = value;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets the Beefree token
		/// </summary>
		/// <param name="request">The token request containing the designer code.</param>
		/// <returns>Access token for the Beefree</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public GetBfTokenResponse GetToken(BfTokenRequest request) {
			try {
				var getBfTokenResponse = BfTokenProvider.GetToken(request.DesignerCode).Result;
				return getBfTokenResponse;
			} catch (Exception exception) {
				Logger.Error(exception.Message, exception);
				return new GetBfTokenResponse { Error = exception.Message };
			}
		}

		#endregion

	}

	#endregion
}