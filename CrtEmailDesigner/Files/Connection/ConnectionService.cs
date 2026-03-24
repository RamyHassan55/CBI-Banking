namespace CrtEmailDesigner.Connection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	[DefaultBinding(typeof(IConnectionService))]
	[Obsolete("Class will be removed in upcoming releases")]
	internal class ConnectionService : IConnectionService
	{
		#region Fields: Private

		private readonly UserConnection _userConnection;
		private readonly List<string> _requiredSysSettings = new List<string>()
		{
			"IdentityServerUrl",
			"IdentityServerClientId",
			"IdentityServerClientSecret",
			"SocialAccountServiceUrl"
		};

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="ConnectionService"/>
		/// </summary>
		/// <param name="userConnection">User connection instance</param>
		public ConnectionService(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IConnectionService.CheckSysSettingsConfiguration"/>
		public bool CheckSysSettingsConfiguration() {
			return !_requiredSysSettings.
				Any(setting => string.IsNullOrEmpty((string)SystemSettings.GetValue(_userConnection, setting)));
		}

		#endregion
	}
}
