using Common.Logging;
using CrtEmailDesigner.Connection;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;

namespace CrtEmailDesigner.Services
{
	#region Class: BfTemplateService

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[Obsolete("Service will be removed in upcoming releases")]
	public class ConnectionWebService : BaseService
	{
		#region Fields: Private

		private ILog _logger;
		private IConnectionService _connectionService;

		#endregion

		#region Properties: Public

		/// <summary>
		/// The logger
		/// </summary>
		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(nameof(ConnectionWebService)));
			set => _logger = value;
		}

		/// <summary>
		/// Connection Service
		/// </summary>
		public IConnectionService ConnectionService {
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
