namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration.CaseService;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: RunReopenCaseAndNotifyAssigneeBySocialMessageMethodsWrapper

	/// <exclude/>
	public class RunReopenCaseAndNotifyAssigneeBySocialMessageMethodsWrapper : ProcessModel
	{

		public RunReopenCaseAndNotifyAssigneeBySocialMessageMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
			AddScriptTaskMethod("SetIsExternalUserExecute", SetIsExternalUserExecute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var classExecutor =	ClassFactory.Get<ReopenCaseAndNotifyAssignee>(new ConstructorArgument("userConnection", UserConnection.AppConnection.SystemUserConnection));
			classExecutor.CaseId = Get<Guid>("SocialMessageCaseId");
			classExecutor.Run();
			return true;
		}

		private bool SetIsExternalUserExecute(ProcessExecutingContext context) {
			Set("IsExternalUser", UserConnection.CurrentUser.ConnectionType == UserType.SSP);
			return true;
		}

		#endregion

	}

	#endregion

}

