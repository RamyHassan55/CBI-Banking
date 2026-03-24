namespace Terrasoft.Configuration
{
	using System;
	using System.Data;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;

	#region Class: CaseServiceActivityEventHandlingUtilities

	public class CaseServiceActivityEventHandlingUtilities
	{

		#region Methods: Public

		public static bool UseCategoryFilter(UserConnection userConnection) {
			bool getCategoryFromEmail = SysSettings.GetValue(userConnection, "DefineCaseCategoryFromMailBox", false);
			return getCategoryFromEmail && Features.GetIsEnabled("CategoryFromMailBox");
		}
		
		public static bool IsRequiredFieldsValid(UserConnection userConnection, Guid caseId) {
			Select selectQuery = new Select(userConnection)
				.Column("c", "CategoryId")
				.Column("c", "ParentActivityId")
				.From("Case").As("c")
				.Where("c", "Id").IsEqual(Column.Const(caseId)) as Select;
			using (DBExecutor dbExec = userConnection.EnsureDBConnection()) {
				using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
					while (reader.Read()) {
						Guid categoryId = reader.GetColumnValue<Guid>("CategoryId");
						Guid parentActivityId = reader.GetColumnValue<Guid>("ParentActivityId");
						if (categoryId.IsEmpty() && parentActivityId.IsEmpty()) {
							return true;
						}
					}
				}
			}
			return false;
		}
		
		public static string JoinRecipients(string recipients, string recipient) {
			if (!string.IsNullOrEmpty(recipient)) {
				recipients = string.Join(";", recipients, recipient);
			}
			return recipients;
		}

		public static string GetRecipients(UserConnection userConnection, Guid activityId,
			Func<string, string, string> joinRecipientsMethod=null) {
			string recipients = string.Empty;
			Select selectQuery = new Select(userConnection)
				.Column("a", "Recepient")
				.Column("a", "CopyRecepient")
				.Column("a", "BlindCopyRecepient")
				.From("Activity").As("a")
				.Where("a", "Id").IsEqual(Column.Const(activityId)) as Select;
			using (DBExecutor dbExec = userConnection.EnsureDBConnection()) {
				using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
					while (reader.Read()) {
						string recipient = reader.GetColumnValue<string>("Recepient");
						recipients = joinRecipientsMethod?.Invoke(recipients, recipient)
							?? CaseServiceActivityEventHandlingUtilities.JoinRecipients(recipients, recipient);
						string copyRecipient = reader.GetColumnValue<string>("CopyRecepient");
						recipients = joinRecipientsMethod?.Invoke(recipients, copyRecipient)
							?? CaseServiceActivityEventHandlingUtilities.JoinRecipients(recipients, copyRecipient);
						string blindCopyRecipient = reader.GetColumnValue<string>("BlindCopyRecepient");
						recipients = joinRecipientsMethod?.Invoke(recipients, blindCopyRecipient)
							?? CaseServiceActivityEventHandlingUtilities.JoinRecipients(recipients, blindCopyRecipient);
					}
				}
			}
			return recipients;
		}
		
		public static void SetCaseCategory(UserConnection userConnection, Guid caseId, Guid activityId,
				Func<string, string, string> joinRecipientsMethod = null,
				Func<Guid, string> getRecipientsMethod = null) {
			Entity caseEntity = userConnection.EntitySchemaManager.GetInstanceByName("Case")
				.CreateEntity(userConnection);
			caseEntity.FetchFromDB(caseId);
			caseEntity.SetColumnValue("ParentActivityId", activityId);
			string recipients = getRecipientsMethod?.Invoke(activityId) ?? GetRecipients(userConnection, activityId);
			CategoryFromSupportMailBox supportMailBoxStore = new CategoryFromSupportMailBox(userConnection);
			CategoryProvider categoryProvider = new CategoryProvider(supportMailBoxStore);
			CategoryProviderWrapper categoryWrapper = new CategoryProviderWrapper(userConnection) {
				CategoryProvider = categoryProvider
			};
			Guid categoryId = categoryWrapper.GetCategoryFromSupportMailBox(recipients);
			if (categoryId.IsEmpty()) {
				categoryId = SysSettings.GetValue(userConnection, CaseConsts.DefaultCaseCategory, Guid.Empty);
			}
			if (categoryId.IsNotEmpty()) {
				caseEntity.SetColumnValue("CategoryId", categoryId);
			}
			caseEntity.Save(false);
		}

		#endregion

	}

	#endregion

}

