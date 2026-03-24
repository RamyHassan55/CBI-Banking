namespace Terrasoft.Configuration
{
	using System;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Configuration.Packages.Case;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: CaseServiceActivityEventsHandler

	[DefaultBinding(typeof(ICaseServiceActivityEventsHandler))]
	internal class CaseServiceActivityEventsHandler : ICaseServiceActivityEventsHandler
	{

		#region Methods: Private

		private static void NotifyListeners(Entity activityEntity, UserConnection userConnection) {
			var notifier = new EmailMessageNotifier(activityEntity, userConnection);
			var manager = new MessageHistoryManager(userConnection, notifier);
			manager.Notify();
		}

		#endregion

		#region Methods: Public

		public void HandleActivityInserted(Entity activityEntity, UserConnection userConnection,
				Action notificationAction = null) {
			if (activityEntity.GetTypedColumnValue<Guid>("TypeId") == ActivityConsts.EmailTypeUId) {
				if (notificationAction != null) {
					notificationAction();
				} else {
					var notifier = new EmailMessageNotifier(activityEntity, userConnection);
					var manager = new MessageHistoryManager(userConnection, notifier);
					manager.Notify();
				}
			}
		}

		public void HandleActivityUpdated(Entity activityEntity, UserConnection userConnection,
			Action notificationAction = null) {
			var caseId = activityEntity.GetTypedColumnValue<Guid>("CaseId");
			if (caseId.IsNotEmpty() && Features.GetIsEnabled("CanUpdateHistoryMessage")) {
				if (notificationAction != null) {
					notificationAction();
				} else {
					NotifyListeners(activityEntity, userConnection);
				}
			}
		}

		public void HandleActivityUpdating(Entity activityEntity, UserConnection userConnection,
				Func<bool> getIsCategoryFilterMethod = null,
				Func<Guid, bool> isRequiredFieldsValidMethod = null,
				Action<Guid, Guid> setCategoryAction = null,
				Func<string, string, string> joinRecipientsMethod = null,
				Func<Guid, string> getRecipientsMethod = null) {
			var caseId = activityEntity.GetTypedColumnValue<Guid>("CaseId");
			Guid activityId = activityEntity.PrimaryColumnValue;
			if (caseId.IsEmpty()) {
				return;
			}
			bool useCategoryFilter = getIsCategoryFilterMethod?.Invoke() ??
				CaseServiceActivityEventHandlingUtilities.UseCategoryFilter(userConnection);
			if (useCategoryFilter){
				var caseOldId = activityEntity.GetTypedOldColumnValue<Guid>("CaseId");
				if (caseOldId.IsEmpty()) {
					bool isRequiredFieldsValid = isRequiredFieldsValidMethod?.Invoke(caseId) ??
						CaseServiceActivityEventHandlingUtilities.IsRequiredFieldsValid(userConnection, caseId);
					if (isRequiredFieldsValid) {
						if (setCategoryAction != null) {
							setCategoryAction(caseId, activityId);
						} else {
							CaseServiceActivityEventHandlingUtilities.SetCaseCategory(userConnection, caseId,
								activityId, joinRecipientsMethod, getRecipientsMethod);
						}
					}
				}
			}
			if (activityEntity.GetTypedColumnValue<bool>("IsNotPublished")) {
				activityEntity.SetColumnValue("IsNotPublished", false);
				var copyUtilityDetail = new CaseEntityFileCopier(userConnection);
				copyUtilityDetail.CopyAll("Activity", activityId, caseId);
			}
		}

		#endregion

	}

	#endregion

}

