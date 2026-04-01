namespace Terrasoft.Core.Process.Configuration
{
	using CrtLandingPageAnalytics;
	using Terrasoft.Configuration;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;

	#region Class: StartGoogleAnalyticsStatsSyncUserTask

	/// <exclude/>
	public partial class StartGoogleAnalyticsStatsSyncUserTask
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var message = new GoogleAnalyticsSyncStatsMessage();
			var queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", UserConnection)
			);
			queueManager.Enqueue(new TouchQueueMessage[] { message });
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

