namespace Terrasoft.Configuration
{
	using Common;
	using Core;
	using Core.Configuration;
	using Core.Scheduler;
	using Creatio.FeatureToggling;
	using Web.Common;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class : DelayedNotificationManagement

	/// <summary>
	/// A class for delayed notification management.
	/// </summary>
	public class DelayedNotificationManagement : AppEventListenerBase
	{

		#region Constants : Private

		/// <summary>
		/// Daily execution period.
		/// </summary>
		private const int DefaultCleanerExecutionPeriod = 24 * 60;
		/// <summary>
		/// Execute every five minutes.
		/// </summary>
		private const int DefaultNotifierExecutionPeriod = 5;
		private const string JobGroupName = "DelayedNotificationGroup";

		#endregion

		#region Fields : Private

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
		/// Schedules a minutely job.
		/// </summary>
		/// <typeparam name="TJob">Job type.</typeparam>
		/// <param name="periodInMinutes">Period in minutes.</param>
		protected virtual void ScheduleJob<TJob>(int periodInMinutes)
			where TJob : IJobExecutor {
			SysUserInfo currentUser = UserConnection.CurrentUser;
			string className = typeof(TJob).AssemblyQualifiedName;
			if (!AppScheduler.DoesJobExist(className, JobGroupName)) {
				AppScheduler.ScheduleMinutelyJob<TJob>(JobGroupName, UserConnection.Workspace.Name,
					currentUser.Name, periodInMinutes, null, true);
			}
		}

		/// <summary>
		/// Removes job.
		/// </summary>
		/// <typeparam name="TJob">Job type.</typeparam>
		protected void RemoveJob<TJob>()
			where TJob : IJobExecutor {
			string className = typeof(TJob).AssemblyQualifiedName;
			AppScheduler.RemoveJob(className, JobGroupName);
		}

		/// <summary>
		/// Sets up delayed notification jobs.
		/// </summary>
		protected virtual void SetupDelayedNotificationJobs() {
			if (Features.GetIsEnabled("DelayedNotification")) {
				int notifierPeriod = SystemSettings.GetValue(UserConnection,
					"DelayedNotifyingExecutionPeriod", DefaultNotifierExecutionPeriod);
				ScheduleJob<DelayedNotifying>(notifierPeriod);
				int cleanerPeriod = SystemSettings.GetValue(UserConnection,
					"DelayedNotificationCleaningExecutionPeriod", DefaultCleanerExecutionPeriod);
				ScheduleJob<DelayedNotificationCleaning>(cleanerPeriod);
			} else {
				RemoveJob<DelayedNotifying>();
				RemoveJob<DelayedNotificationCleaning>();
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
			SetupDelayedNotificationJobs();
		}

		#endregion

	}

	#endregion

}
