namespace Terrasoft.Configuration
{
	using Common;
	using Core;
	using Core.Scheduler;
	using Web.Common;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;


	#region Class : CaseManagementAppEventListener

	/// <summary>
	/// Application starts event listener for case management.
	/// </summary>
	public class CaseManagementAppEventListener : AppEventListenerBase
	{

		private const int SatisfactionTaskPeriodMinute = 5;

		#region Fields : Protected

		protected UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Gets user connection from application event context.
		/// </summary>
		/// <param name="context">Application event context.</param>
		/// <returns>User connection.</returns>
		protected UserConnection GetUserConnection(AppEventContext context) {
			var appConnection = context.Application["AppConnection"] as AppConnection;
			if (appConnection == null) {
				throw new ArgumentNullOrEmptyException("AppConnection");
			}
			return appConnection.SystemUserConnection;
		}


		/// <summary>
		/// Schedules a minutely process job.
		/// </summary>
		/// <param name="jobName">Job name.</param>
		/// <param name="processName">Process name.</param>
		/// <param name="periodInMinutes">Job process period in minutes.</param>
		protected virtual void ScheduleProcessJob(string jobName, string processName, int periodInMinutes) {
			var jobGroupName = jobName + "Group";
			if (periodInMinutes > 0) {
				AppScheduler.ScheduleMinutelyProcessJob(jobName, jobGroupName, processName,
				UserConnection.Workspace.Name, UserConnection.CurrentUser.Name, periodInMinutes, null, true);
			}
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Handles application start.
		/// </summary>
		/// <param name="context">Application event context.</param>
		public override void OnAppStart(AppEventContext context) {
			base.OnAppStart(context);
			UserConnection = GetUserConnection(context);
			ScheduleProcessJob("SatisfactionUpdateProcessJob", "SatisfactionUpdateProcess", SatisfactionTaskPeriodMinute);
			ScheduleProcessJob("CaseOverduesSettingJob", "CaseOverduesSettingProcess", SystemSettings.GetValue(UserConnection, "CaseOverduesCheckTerm", 0));
		}

		#endregion

	}

	#endregion
}
