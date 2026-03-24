namespace Terrasoft.Core.Process.Configuration
{
	using System;
	using System.Collections.Generic;
	using CrtContentDesigner.Contracts;
	using CrtContentDesigner.Models;
	using CrtContentDesigner.Utilities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;

	#region Class: SaveContentRequestUserTask

	/// <exclude/>
	public partial class SaveContentRequestUserTask
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var handler = ClassFactory.Get<ITemplateRequestHandler>(new ConstructorArgument("userConnection", UserConnection));
			var handlerCodes = DesignerType == "Email" ? new List<string> { "Email", "BulkEmail" } : new List<string> { "LandingPage" };
			var request = new SaveTemplateRequest {
				RecordId = RecordId.ToString(),
				HandlerCodes = handlerCodes,
				Content = new TemplateModel {
					Id = ContentId.ToString(),
					PageJson = ContentJson,
					PageHtml = ContentHtml
				}
			};
			if (LanguageId != default) {
				request.LanguageId = LanguageId.ToString();
			}
			var response = handler.SaveTemplate(request);
			if (!response.Success) {
				var errorMessage = DesignerType == "Email" ? EmailSavingError : PageSavingError;
				throw new Exception(errorMessage);
			}
			return true;
		}

		#endregion

		#region Methods: Public

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion

}

