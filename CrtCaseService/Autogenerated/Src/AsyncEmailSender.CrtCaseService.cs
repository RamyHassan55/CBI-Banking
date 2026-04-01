namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Scheduler;

	#region Class : AsyncEmailSender

	/// <summary>
	/// A class for asynchronous email sending.
	/// </summary>
	public class AsyncEmailSender
	{

		#region Properties: Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
		}

		#endregion

		#region Constuctors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncEmailSender"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public AsyncEmailSender(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Schedules immediate job for sending email.
		/// </summary>
		/// <param name="activityId"></param>
		public virtual void SendAsync(Guid activityId) {
			SysUserInfo currentUser = UserConnection.CurrentUser;
			string jobGroup = string.Concat("AsyncEmailSender", "_", Guid.NewGuid());
			var parameters = new Dictionary<string, object> {
				{"activityId", activityId}
			};
			AppScheduler.ScheduleImmediateJob<AsyncEmailSenderExecutor>(jobGroup, UserConnection.Workspace.Name,
				currentUser.Name, parameters);
		}

		#endregion

	}

	#endregion

}
