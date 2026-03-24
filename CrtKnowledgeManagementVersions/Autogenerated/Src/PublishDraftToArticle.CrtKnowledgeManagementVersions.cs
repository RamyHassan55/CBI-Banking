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

	#region Class: PublishDraftToArticleMethodsWrapper

	/// <exclude/>
	public class PublishDraftToArticleMethodsWrapper : ProcessModel
	{

		public PublishDraftToArticleMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var uc = Get<UserConnection>("UserConnection");
			var articleId = Get<Guid>("Article");
			var draftId = Get<Guid>("Draft");
			
			if (articleId == Guid.Empty) {
			    throw new Exception("ArticleId is empty.");
			}
			
			var excluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
			    "Id",
			    "CreatedOn",
			    "CreatedBy",
			    "ModifiedOn",
			    "ModifiedBy",
			    "ProcessListeners",
			    "Code",
			    "Version",
			    "Status",
			    "ParentArticle"
			};
			
			var esqDraft = new EntitySchemaQuery(uc.EntitySchemaManager, "KnowledgeBase") {
			    UseAdminRights = false
			};
			esqDraft.AddAllSchemaColumns();
			var source = esqDraft.GetEntity(uc, draftId);
			if (source == null) {
			    throw new Exception($"KnowledgeBase source {draftId} not found.");
			}
			
			var esqArticle = new EntitySchemaQuery(uc.EntitySchemaManager, "KnowledgeBase") {
			    UseAdminRights = false
			};
			esqArticle.AddAllSchemaColumns();
			var target = esqArticle.GetEntity(uc, articleId);
			if (!target.FetchFromDB(articleId)) {
			    throw new Exception($"KnowledgeBase target {articleId} not found.");
			}
			
			foreach (var column in source.Schema.Columns) {
			    if (column.IsSystem || column.IsVirtual) {
			        continue;
			    }
			    if (excluded.Contains(column.Name)) {
			        continue;
			    }
			    var valueName = column.ColumnValueName;
			    var value = source.GetColumnValue(valueName);
			    if (value == null) {
			        continue;
			    }
			    target.SetColumnValue(valueName, value);
			}
			target.Save(false);
			return true;
		}

		#endregion

	}

	#endregion

}

