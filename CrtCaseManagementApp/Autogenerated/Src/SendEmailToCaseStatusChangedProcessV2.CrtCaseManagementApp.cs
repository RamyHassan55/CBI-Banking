namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: SendEmailToCaseStatusChangedProcessV2MethodsWrapper

	/// <exclude/>
	public class SendEmailToCaseStatusChangedProcessV2MethodsWrapper : ProcessModel
	{

		public SendEmailToCaseStatusChangedProcessV2MethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var IsClassFeatureEnable = Terrasoft.Configuration.FeatureUtilities.GetIsFeatureEnabled(UserConnection, "SendEmailToCaseOnStatusChangeClass");
			Guid CaseRecordId = Guid.Empty;
			if (Get<Guid>("StartSignal1.RecordId") != Guid.Empty) {
				CaseRecordId = Get<Guid>("StartSignal1.RecordId");
			}
			if (Get<Guid>("StartSignal2.RecordId") != Guid.Empty) {
				CaseRecordId = Get<Guid>("StartSignal2.RecordId");
			}
			if(IsClassFeatureEnable) {
				var classExecutor = new Terrasoft.Configuration.SendEmailToCaseOnStatusChange(UserConnection, CaseRecordId);
				classExecutor.Run();
			}
			return true;
		}

		#endregion

	}

	#endregion

}

