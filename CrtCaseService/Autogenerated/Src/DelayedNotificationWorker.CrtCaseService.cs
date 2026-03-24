namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;

	#region Class: DelayedNotificationWorker

	/// <summary>
	/// Utility class for work with delayed notification.
	/// </summary>
	public class DelayedNotificationWorker
	{
		#region Properties: Public

		/// <summary>
		/// Instance of <see cref="UserConnection"/> type.
		/// </summary>
		public UserConnection UserConnection {
			get; private set;
		}

		#endregion

		#region Constructors: Public

		public DelayedNotificationWorker() {
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="DelayedNotificationWorker"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public DelayedNotificationWorker(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private Query ApplyMultiLanguageTemplateFilter(Query updateQuery) {
			updateQuery = updateQuery.Or("DelayedNotification", "EmailTemplateId")
				.In(new Select(UserConnection)
						.Column("Id")
						.From("EmailtemplateLang")
						.Where("EmailTemplateId")
							.In(Column.Parameters(CaseServiceConsts.ResolutionNotificationTplId,
					CaseServiceConsts.EstimationRequestTplId)));
			return updateQuery;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Update delayed notifications, set SendDate.
		/// </summary>
		/// <param name="notification">Entity of DelayedNotification.</param>
		/// <param name="caseId">Case identifier.</param>
		///  <returns>count of updated records.</returns>
		protected virtual int UpdateDelayedNotificationIfItExist(Entity notification, Guid caseId) {
			var currentDate = DateTime.UtcNow;
			var update = new Update(UserConnection, "DelayedNotification")
				.Set("SendDate", Column.Parameter(currentDate.AddMinutes(
					notification.GetTypedColumnValue<int>("Delay"))))
				.Where("CaseId").IsEqual(Column.Parameter(caseId))
				.And("EmailTemplateId").IsEqual(new QueryParameter(notification.GetTypedColumnValue<Guid>("EmailTemplateId")))
				.And("SendDate").IsNull();
			return update.Execute();
		}

		/// <summary>
		/// Get Email manager name for DelayedNotifying.
		/// </summary>
		/// <param name="notification">Entity of DelayedNotification.</param>
		protected virtual string GetAssembblyQualifiedManagerName(Entity notification) {
			return notification.GetTypedColumnValue<bool>("IsQuoteOriginalEmail") ?
				typeof(ExtendedEmailWithMacrosManagerBase).AssemblyQualifiedName : String.Empty;
		}

		/// <summary>
		/// Create new delayed notification.
		/// </summary>
		/// <param name="notification">Entity of DelayedNotification.</param>
		/// <param name="caseId">Case identifier.</param>
		protected virtual void CreateNewNotification(Entity notification, Guid caseId) {
			var currentDate = DateTime.UtcNow;
			string managerName = GetAssembblyQualifiedManagerName(notification);
			var insert = new Insert(UserConnection)
			.Into("DelayedNotification")
				.Set("CaseId", new QueryParameter(caseId))
				.Set("Delay", new QueryParameter(notification.GetTypedColumnValue<int>("Delay")))
				.Set("AssemblyQualifiedManagerName", new QueryParameter(managerName))
				.Set("EmailTemplateId", new QueryParameter(notification.GetTypedColumnValue<Guid>("EmailTemplateId")))
				.Set("SendDate", Column.Parameter(currentDate.AddMinutes(
			notification.GetTypedColumnValue<int>("Delay")))) as Insert;
			insert.Execute();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Update delayed notifications, set SendDate to null for some templates.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		public virtual void ClearSendDateForResolutionTemplate(Guid caseId) {
			var update = new Update(UserConnection, "DelayedNotification")
			.Set("SendDate", Column.Const(null))
			.Where("CaseId").IsEqual(Column.Parameter(caseId))
			.And()
			.OpenBlock("DelayedNotification", "EmailTemplateId")
				.In(Column.Parameters(CaseServiceConsts.ResolutionNotificationTplId,
					CaseServiceConsts.EstimationRequestTplId));
			if (Features.GetIsEnabled("EmailMessageMultiLanguage") && !Features.GetIsEnabled("EmailMessageMultiLanguageV2")) {
				update = ApplyMultiLanguageTemplateFilter(update);
			}
			update.CloseBlock();
			update.Execute();
		}

		/// <summary>
		/// Update delayed notifications, set SendDate.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		///  <returns>count of updated records.</returns>
		public virtual int SendAllDelayedEmails(Guid caseId) {
			var delayedEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "DelayedNotification");
			delayedEsq.AddColumn("SendDate");
			var delayColumnName = delayedEsq.AddColumn("Delay").Name;
			var idColumnName = delayedEsq.AddColumn("Id").Name;
			var filters = new[] {
				delayedEsq.CreateFilterWithParameters(FilterComparisonType.Equal, "Case", caseId),
				delayedEsq.CreateFilter(FilterComparisonType.IsNull, "SendDate")
			};
			delayedEsq.Filters.AddRange(filters);
			var delayedEmailCollection = delayedEsq.GetEntityCollection(UserConnection);
			var currentDate = DateTime.UtcNow;
			foreach (var delayedEmail in delayedEmailCollection) {
				var update = new Update(UserConnection, "DelayedNotification")
					.Set("SendDate", Column.Parameter(currentDate.AddMinutes(
							delayedEmail.GetTypedColumnValue<int>(delayColumnName))))
					.Where("Id").IsEqual(new QueryParameter(delayedEmail.GetTypedColumnValue<Guid>(idColumnName)));
				update.Execute();
			}
			return delayedEmailCollection.Count;
		}

		/// <summary>
		/// Create new notifications, or update exist.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		/// <param name="templateCollection">Collection of templates.</param>
		public virtual void CreateDelayedNotification(EntityCollection templateCollection, Guid caseId) {
			var delayedNotification = templateCollection.Where(rule =>
				rule.GetTypedColumnValue<Guid>("RuleUsageId") == CaseConsts.DelaySendUsageType);
			foreach (var notification in delayedNotification) {
				var countExistNotification = UpdateDelayedNotificationIfItExist(notification, caseId);
				if (countExistNotification == 0) {
					CreateNewNotification(notification, caseId);
				}
			}
		}

		/// <summary>
		/// Delete all delayed notifications for case, where send date is null.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		/// <remarks>Except feedback request notification and resolution nofication email templates.</remarks>
		public void DeleteAllDelayedEmails(Guid caseId) {
			var delete = new Delete(UserConnection)
				.From("DelayedNotification")
				.Where("CaseId").IsEqual(Column.Parameter(caseId))
				.And("SendDate").Not().IsNull() as Delete;
			delete.Execute();
		}

		#endregion
	}

	#endregion
}
