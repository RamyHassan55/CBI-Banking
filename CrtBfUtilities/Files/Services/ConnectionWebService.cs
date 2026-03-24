namespace Creatio.ContentDesigner.Services
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Common.Logging;
	using Terrasoft.Core.Factories;
	using Terrasoft.Web.Common;

	#region Class: ConnectionWebService

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class ContentDesignerConnectionService : BaseService
	{

		#region Fields: Private

		private ILog _logger;
		private IConnectionService _connectionService;

		#endregion

		#region Properties: Internal

		/// <summary>
		/// The logger
		/// </summary>
		internal ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(nameof(ContentDesignerConnectionService)));
			set => _logger = value;
		}

		/// <summary>
		/// Connection Service
		/// </summary>
		internal IConnectionService ConnectionService {
			get =>
				_connectionService ??
				(_connectionService = ClassFactory.Get<IConnectionService>(
					new ConstructorArgument("userConnection", UserConnection)));
			set => _connectionService = value;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Checks if the system settings are configured
		/// </summary>
		/// <returns>Bool value</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public bool SettingsConfigured() {
			try {
				return ConnectionService.CheckSysSettingsConfiguration();
			} catch (Exception exception) {
				Logger.Error(exception.Message, exception);
				return false;
			}
		}

		#endregion

	}

	#endregion

}
