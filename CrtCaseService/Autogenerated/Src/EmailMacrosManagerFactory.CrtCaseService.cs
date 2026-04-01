namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core;

	#region Class : EmailMacrosManagerFactory

	/// <summary>
	/// Manage instances of email managers for delayed notification.
	/// </summary>
	public class EmailMacrosManagerFactory
	{
		
		#region Fields: Private

		private Dictionary<string, EmailWithMacrosManagerBase> _managerMap;

		#endregion

		#region Properties: Public

		/// <summary>
		/// E-mail sender which allows to control the flow of activity creating, filling and macroses processing.
		/// Instance of the <see cref="DefaultMacrosManager"/> class.
		/// </summary>
		protected EmailWithMacrosManagerBase DefaultMacrosManager {	get; set; }

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection { get; }

		#endregion

		#region Constructors: Protected

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailMacrosManagerFactory"/> class.
		/// </summary>
		protected EmailMacrosManagerFactory() {
			_managerMap = new Dictionary<string, EmailWithMacrosManagerBase>();
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailMacrosManagerFactory"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public EmailMacrosManagerFactory(UserConnection userConnection) 
				: this() {
			UserConnection = userConnection;
			DefaultMacrosManager = new EmailWithMacrosManagerBase(UserConnection);
		}

		public EmailMacrosManagerFactory(UserConnection userConnection, EmailWithMacrosManagerBase defaultManager) 
				: this() {
			UserConnection = userConnection;
			DefaultMacrosManager = defaultManager;
		}

		#endregion

		#region Methods: Private

		private bool IsEmailWithMacrosManagerType(string managerName) {
			var workspaceTypeProvider = ClassFactory.Get<IWorkspaceTypeProvider>();
			var type = workspaceTypeProvider.GetType(managerName);
			return type != null && type.IsSubclassOf(typeof(EmailWithMacrosManagerBase));
		}

		private EmailWithMacrosManagerBase CreateManagerInstace(string managerName) {
			return ClassFactory.ForceGet<EmailWithMacrosManagerBase>(managerName, 
				new ConstructorArgument("userConnection", UserConnection));
		}

		private EmailWithMacrosManagerBase MapManager(string managerName) {
			EmailWithMacrosManagerBase manager = DefaultMacrosManager;
			if (IsEmailWithMacrosManagerType(managerName)) {
				manager = CreateManagerInstace(managerName);
			}
			_managerMap.Add(managerName, manager);
			return manager;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Get instance of EmailManager from manager storage or create new instance.
		/// </summary>
		/// <param name="managerName">Class name.</param>
		protected EmailWithMacrosManagerBase GetManagerInstance(string managerName) {
			return _managerMap.ContainsKey(managerName) ? _managerMap[managerName] : MapManager(managerName);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get instance of EmailManager.
		/// </summary>
		/// <param name="managerName">Class name.</param>
		public EmailWithMacrosManagerBase GetByTypeName(string managerName) {
			return GetManagerInstance(managerName);
		}

		#endregion
	}

	#endregion
}
