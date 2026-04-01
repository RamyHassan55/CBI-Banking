namespace Creatio.InstallScripts
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	#region Class: ChangeSubmittedFormSectionNameInstallScript

	internal class ChangeSubmittedFormSectionNameInstallScript : IInstallScriptExecutor 
	{
		private static readonly string _newSubmittedFormName = "Submissions";
		private static readonly string _oldSubmittedFormName = "Submitted forms";
		private static readonly string _submittedFormSectionCode = "FormSubmit";

		private UserConnection _userConnection;

		private void InternalExecute(UserConnection userConnection) {
			_userConnection = userConnection;
			var update = new Update(userConnection, "SysModule")
				.Set("Caption", Column.Parameter(_newSubmittedFormName))
				.Where("Code").IsEqual(Column.Parameter(_submittedFormSectionCode))
				.And("Caption").IsEqual(Column.Parameter(_oldSubmittedFormName));
			update.Execute();
		}

		/// <summary>Executes package install script.</summary>
		/// <param name="userConnection">User connection.</param>
		public void Execute(UserConnection userConnection) {
			InternalExecute(userConnection);
		}
	}

	#endregion
}

