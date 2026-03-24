namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common;
	using Core;
	using Core.Entities;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class : DelayedNotifying

	/// <summary>
	/// Represents a delayed notification job.
	/// </summary>
	public class DelayedNotifying : IJobExecutor
	{

		#region Constants : Private

		private const string ErrorFormat = "{0}\n{1}";

		#endregion

		#region Fields : Private

		private string _caseIdColumnName;

		#endregion

		#region Constructors : Public

		public DelayedNotifying() {
		}

		#endregion

		#region Properties : Public

		/// <summary>
		/// Manage email senders.
		/// instance of the <see cref="EmailMacrosManagerFactory"/> class
		/// </summary>
		public EmailMacrosManagerFactory ManagerFactory { get; set;	}

		#endregion

		#region Methods : Private

		private void AddDelayedNotificationColumns(EntitySchemaQuery esq) {
			_caseIdColumnName = esq.AddColumn("Case.Id").Name;
			esq.AddColumn("EmailTemplateId");
			esq.AddColumn("ActualSendDate");
			esq.AddColumn("AssemblyQualifiedManagerName");
			esq.AddColumn("Error");
		}

		private void AddDelayedNotificationFilters(EntitySchemaQuery esq) {
			var filters = new[] {
				esq.CreateFilterWithParameters(FilterComparisonType.LessOrEqual, "SendDate", DateTime.UtcNow),
				esq.CreateFilter(FilterComparisonType.IsNull, "ActualSendDate"),
				esq.CreateFilter(FilterComparisonType.IsNotNull, "SendDate"),
				esq.CreateFilter(FilterComparisonType.IsNull, "Error")
			};
			esq.Filters.AddRange(filters);
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Get esq to notifications.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		protected virtual EntitySchemaQuery GetDelayedNotificationEsq(UserConnection userConnection) {
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "DelayedNotification");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			AddDelayedNotificationColumns(esq);
			AddDelayedNotificationFilters(esq);
			return esq;
		}

		/// <summary>
		/// Send notification.
		/// </summary>
		/// <param name="notification">Notificatin entity.</param>
		/// <param name="sender">Sender email.</param>
		protected virtual void SendNotification(Entity notification) {
			var caseId = notification.GetTypedColumnValue<Guid>(_caseIdColumnName);
			var templateId = notification.GetTypedColumnValue<Guid>("EmailTemplateId");
			var managerName = notification.GetTypedColumnValue<string>("AssemblyQualifiedManagerName");
			try {
				var manager = ManagerFactory.GetByTypeName(managerName);
				manager.SendEmail(caseId, templateId);
				notification.SetColumnValue("ActualSendDate", DateTime.UtcNow);
			} catch (Exception e) {
				var error = string.Format(ErrorFormat, e.Message, e.StackTrace);
				notification.SetColumnValue("Error", error);
			} finally {
				notification.Save(validateRequired: false);
			}
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Notifies contacts when it is time.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void Execute(UserConnection userConnection, IDictionary<string, object> parameters) {
			var esq = GetDelayedNotificationEsq(userConnection);
			EntityCollection notifications = esq.GetEntityCollection(userConnection);
			if (!notifications.Any()) {
				return;
			}
			ManagerFactory = ManagerFactory ?? new EmailMacrosManagerFactory(userConnection);
			foreach (Entity notification in notifications) {
				SendNotification(notification);
			}
		}

		#endregion

	}

	#endregion

}
