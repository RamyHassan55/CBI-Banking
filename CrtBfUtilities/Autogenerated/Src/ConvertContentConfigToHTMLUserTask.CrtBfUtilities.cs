namespace Terrasoft.Core.Process.Configuration
{
	using System;
	using Creatio.ContentDesigner;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;

	#region Class: ConvertContentConfigToHTMLUserTask

	/// <exclude/>
	public partial class ConvertContentConfigToHTMLUserTask
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var client = ClassFactory.Get<IBfContentServicesAPIClient>(
				new ConstructorArgument("userConnection", UserConnection));
			var designerType = DesignerType == "Email" ? ContentDesignerType.Email : ContentDesignerType.Page;
			if (!string.IsNullOrWhiteSpace(SimpleJson)) {
				ContentJson = client.ConvertSimpleToFullJson(SimpleJson, designerType);
			}
			if (string.IsNullOrWhiteSpace(ContentJson)) {
				var errorMessage = DesignerType == "Email" ? EmailJsonGenerationError : PageJsonGenerationError;
				throw new Exception(errorMessage);
			}
			ContentHtml = client.TransformConfigToHTML(ContentJson, designerType);
			if (string.IsNullOrWhiteSpace(ContentHtml)) {
				var errorMessage = DesignerType == "Email" ? EmailHtmlGenerationError : PageHtmlGenerationError;
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

