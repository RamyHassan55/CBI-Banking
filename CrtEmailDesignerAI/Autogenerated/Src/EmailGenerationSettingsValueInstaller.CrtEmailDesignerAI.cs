 namespace Creatio.InstallScripts
{
	using Terrasoft.Core;
	using CoreSysSettings = Terrasoft.Core.Configuration.SysSettings;


	#region Class: EmailGenerationSettingsValueInstaller

	internal class EmailGenerationSettingsValueInstaller : IInstallScriptExecutor
	{
		private const string _userQueryLengthCode = "CreatioAIMaxUserQueryLength";
		private const int _userQueryLengthValue = 50000;

		/// <summary>Executes package install script.</summary>
		/// <param name="userConnection">User connection.</param>
		public void Execute(UserConnection userConnection) {
			var value = (int)CoreSysSettings.GetValue(userConnection, _userQueryLengthCode);
			if (value < _userQueryLengthValue) {
				CoreSysSettings.SetDefValue(userConnection, _userQueryLengthCode, _userQueryLengthValue);
			}
		}
	}

	#endregion

}
