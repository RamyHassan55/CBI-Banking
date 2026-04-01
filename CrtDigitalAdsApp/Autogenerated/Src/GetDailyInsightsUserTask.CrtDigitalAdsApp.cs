namespace Terrasoft.Core.Process.Configuration
{
	using CrtDigitalAdsApp.Enums;
	using CrtDigitalAdsApp.Utilities;
	using System;
	using System.Linq;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;

	#region Class: GetDailyInsights

	/// <exclude/>
	public partial class GetDailyInsightsUserTask
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var actualizer = ClassFactory.Get<DailyInsightActualizer>();
			var actualizationResult = actualizer.Actualize();
			AreDailyTasksReceived = true;
			var failedResult = actualizationResult.FirstOrDefault(t => !t.Success);
			if (failedResult != null) {
				AreDailyTasksReceived = false;
				ErrorCode = failedResult.ErrorCode;
			}
			return true;
		}

		#endregion

		#region Methods: Public

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
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