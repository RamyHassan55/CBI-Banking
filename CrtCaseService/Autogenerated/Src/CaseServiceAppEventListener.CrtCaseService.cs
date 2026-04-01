namespace Terrasoft.Configuration
{
	using Common;
	using Core;
	using Core.Configuration;
	using SystemSettings = Core.Configuration.SysSettings;
	using Core.Scheduler;
	using Web.Common;

	#region Class : CaseServiceAppEventListener

	/// <summary>
	///  A class that setup`s service features on app start.
	/// </summary>
	public class CaseServiceAppEventListener : AppEventListenerBase
	{

		#region Constants : Private

		/// <summary>
		/// Daily execution period.
		/// </summary>
		private const int CloserExecutionPeriod = 24 * 60;
		private const string CloserJobGroupName = "CaseCloserGroup";

		#endregion

		#region Fields : Private

		protected UserConnection UserConnection	{
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
		protected UserConnection GetUserConnection(AppEventContext context)	{
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
		protected virtual void ScheduleJob<TJob>(int periodInMinutes) where TJob : IJobExecutor	{
			SysUserInfo currentUser = UserConnection.CurrentUser;
			string className = typeof(TJob).AssemblyQualifiedName;
			if (!AppScheduler.DoesJobExist(className, CloserJobGroupName)) {
				AppScheduler.ScheduleMinutelyJob<TJob>(CloserJobGroupName, UserConnection.Workspace.Name,
					currentUser.Name, periodInMinutes, null, true);
			}
		}

		/// <summary>
		/// Sets up jobs.
		/// </summary>
		protected virtual void SetupJobs() {
			bool isAutoClose = SystemSettings.GetValue(UserConnection, "CloseResolvedCases", true);
			if (isAutoClose) {
				ScheduleJob<CaseCloserJob>(CloserExecutionPeriod);
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
			SetupJobs();
		}

		#endregion

	}

	#endregion

}
