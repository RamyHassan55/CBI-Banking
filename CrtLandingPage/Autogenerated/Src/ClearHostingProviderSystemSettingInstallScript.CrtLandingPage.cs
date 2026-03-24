namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
    using SysSettingsClass = Terrasoft.Core.Configuration.SysSettings;

	#region Class: ClearHostingProviderSystemSettingInstallScript

	/// <summary>
	/// Install script that clears the LandingPageHostingProviderFullClassName system setting
	/// if its value is equal to "LandingPageHostingProvider, CrtLandingPage".
	/// </summary>
	public class ClearHostingProviderSystemSettingInstallScript : IInstallScriptExecutor
	{

		#region Fields: Private
		
		private const string SystemSettingCode = "LandingPageHostingProviderFullClassName";
		private const string PreviousValue = "LandingPageHostingProvider, CrtLandingPage";

		#endregion

		#region Methods: Public

		/// <summary>
		/// Executes the installation script.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public void Execute(UserConnection userConnection) {
			if (userConnection == null) {
				throw new ArgumentNullException(nameof(userConnection));
			}
			string currentValue = SysSettingsClass.GetValue(userConnection, SystemSettingCode, string.Empty);
			if (currentValue == PreviousValue) {
                SysSettingsClass.SetDefValue(userConnection, SystemSettingCode, null);
			}
		}

		#endregion

	}

	#endregion

}

