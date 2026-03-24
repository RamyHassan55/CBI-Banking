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

	#region Class: Copilot_GetCaseKnowledgeArticlesMethodsWrapper

	/// <exclude/>
	public class Copilot_GetCaseKnowledgeArticlesMethodsWrapper : ProcessModel
	{

		public Copilot_GetCaseKnowledgeArticlesMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var readKnowledgeArticles = Get<CompositeObjectList<CompositeObject>>("ReadArticles.ResultCompositeObjectList");
			Set("KnowledgeArticles", readKnowledgeArticles);
			return true;
		}

		#endregion

	}

	#endregion

}

