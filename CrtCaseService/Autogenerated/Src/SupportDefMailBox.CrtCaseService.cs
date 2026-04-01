namespace Terrasoft.Configuration
{
	using Terrasoft.Core;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: SupportDefMailBox

	/// <summary>
	/// Provides mail box from system settings
	/// </summary>
	public class SupportDefMailBox : ISupportDefMailBox
	{
		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="SupportDefMailBox"/>
		/// </summary>
		/// <param name="userConnection">User connection</param>
		public SupportDefMailBox(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Method: Public
		
		/// <summary>
		/// Get support mailbox from system setting.
		/// </summary>
		/// <returns>Support mailbox.</returns>
		public string Get() {
			return SystemSettings.GetValue<string>(_userConnection, "SupportServiceEmail", string.Empty);
		}

		#endregion
		
	}

	#endregion

}
