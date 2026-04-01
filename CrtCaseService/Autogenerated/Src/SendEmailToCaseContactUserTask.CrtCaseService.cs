namespace Terrasoft.Core.Process.Configuration
{
	using Terrasoft.Configuration;
	using Terrasoft.Core.Process;

	#region Class: SendEmailToCaseContactUserTask

	/// <exclude/>
	public partial class SendEmailToCaseContactUserTask
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var macrosManager = new EmailWithMacrosManagerBase(UserConnection);
			macrosManager.SendEmail(CaseId, EmailTemplateId);
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

