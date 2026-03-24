namespace Terrasoft.Configuration
{
	using System.Collections.Generic;
	using Core;
	using Core.DB;
	using Core.Scheduler;

	#region Class : DelayedNotificationCleaning

	/// <summary>
	/// Represents a cleaning job of already sent notifications.
	/// </summary>
	public class DelayedNotificationCleaning : IJobExecutor
	{

		#region Methods : Public

		/// <summary>
		/// Deletes already sent notifications.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void Execute(UserConnection userConnection, IDictionary<string, object> parameters) {
			var delete = new Delete(userConnection)
				.From("DelayedNotification")
				.Where("ActualSendDate").Not().IsNull() as Delete;
			delete.Execute();
		}		

		#endregion

	}

	#endregion

}
