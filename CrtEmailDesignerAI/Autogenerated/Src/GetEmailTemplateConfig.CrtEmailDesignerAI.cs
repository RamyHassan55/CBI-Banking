namespace Terrasoft.Core.Process
{

	using CrtContentDesigner.Interfaces;
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
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: GetEmailTemplateConfigMethodsWrapper

	/// <exclude/>
	public class GetEmailTemplateConfigMethodsWrapper : ProcessModel
	{

		public GetEmailTemplateConfigMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ValidateEmailTemplateLengthTaskExecute", ValidateEmailTemplateLengthTaskExecute);
		}

		#region Methods: Private

		private bool ValidateEmailTemplateLengthTaskExecute(ProcessExecutingContext context) {
			var templateJson = Get<string>("TemplateJson");
			var lengthLimit = Get<int>("TemplateLengthLimit");
			if (string.IsNullOrWhiteSpace(templateJson) || templateJson.Length > lengthLimit) {
				var messageProvider = ClassFactory.Get<IInfoMessageProvider>("ContentDesignerMessageProvider");
				var message = messageProvider.GetMessage(UserConnection, "EmailConfigLengthLimitError");
				throw new Exception(message);
			}
			return true;
		}

		#endregion

	}

	#endregion

}

