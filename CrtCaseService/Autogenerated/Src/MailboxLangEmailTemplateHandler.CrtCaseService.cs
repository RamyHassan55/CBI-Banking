namespace Terrasoft.Configuration
{
	using System;
	using System.Text.RegularExpressions;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: MailboxLangEmailTemplateHandler

	/// <summary>
	/// Language search handler in the support mailbox.
	/// </summary>
	public class MailboxLangEmailTemplateHandler : EmailTemplateCommLangHandlerBase
	{

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="MailboxLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="caseId">Case record indentifier.</param>
		/// <param name="userConnection">User connection.</param>
		public MailboxLangEmailTemplateHandler(Guid caseId, UserConnection userConnection)
			: base(caseId, userConnection) {
		}

		/// <summary>
		/// Initialize new instance of <see cref="MailboxLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="userConnection">User connection.</param>
		public MailboxLangEmailTemplateHandler(Entity entity, UserConnection userConnection)
			: base(entity, userConnection) {
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Get mailbox language identifier.
		/// </summary>
		/// <param name="templateId">Email template identifier.</param>
		/// <param name="templateLoader">Email template store.</param>
		/// <returns>Mailbox language identifier</returns>
		private Guid GetLanguageId(Guid templateId, ITemplateLoader templateLoader) {
			var recipients = GetRecipientMailboxes();
			if (recipients.Length > 0) {
				string languageIdColumnName;
				var mailboxes = GetMailboxCollection(recipients, out languageIdColumnName);
				foreach (Entity mailbox in mailboxes) {
					var mailboxLanguageId = mailbox.GetTypedColumnValue<Guid>(languageIdColumnName);
					if (mailboxLanguageId != default(Guid) && IsTemplateValid(templateId, mailboxLanguageId, templateLoader)) {
						return mailboxLanguageId;
					}
				}
			}
			return Guid.Empty;
		}

		/// <summary>
		/// Get recipients mailboxes.
		/// </summary>
		/// <returns>All recipients mailboxes.</returns>
		private string[] GetRecipientMailboxes() {
			var parentActivityId = Entity.GetTypedColumnValue<Guid>("ParentActivityId");
			if (parentActivityId != default(Guid)) {
				string allRecipients = GetRecipients(parentActivityId);
				return ActivityUtils.ParseEmailAddress(allRecipients).ToArray();
			}
			return new string[0];
		}

		/// <summary>
		/// Get all email recipients.
		/// </summary>
		/// <param name="activityId">Activity identifier.</param>
		/// <returns>All recipients.</returns>
		private string GetRecipients(Guid activityId) {
			EntitySchema activitySchema = UserConnection.EntitySchemaManager.FindInstanceByName("Activity");
			Entity entity = activitySchema.CreateEntity(UserConnection);
			if (!entity.FetchFromDB(activityId)) {
				return null;
			}
			string recipient = entity.GetTypedColumnValue<string>("Recepient");
			string copyRecepient = entity.GetTypedColumnValue<string>("CopyRecepient");
			string blindCopyRecepient = entity.GetTypedColumnValue<string>("BlindCopyRecepient");
			return string.Join(";", recipient, copyRecepient, blindCopyRecepient);
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Get emails for incident registration.
		/// </summary>
		/// <param name="recipients">All email recipients.</param>
		/// <param name="languageIdColumnName">Name of the column with language identifier.</param>
		/// <returns>Mailboxes collection.</returns>
		protected virtual EntityCollection GetMailboxCollection(object[] recipients, out string languageIdColumnName) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "MailboxForIncidentRegistration");
			languageIdColumnName = esq.AddColumn("MailboxSyncSettings.MessageLanguage.Id").Name;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"MailboxSyncSettings.SenderEmailAddress", recipients));
			return esq.GetEntityCollection(UserConnection);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Entry point for language searching.
		/// </summary>
		/// <param name="templateId">Email template identifier.</param>
		/// <param name="templateLoader">Email template store.</param>
		/// <returns>Language identifier for email template.</returns>
		public override Guid Handle(Guid templateId, ITemplateLoader templateLoader) {
			Guid languageId = GetLanguageId(templateId, templateLoader);
			if (languageId != default(Guid)) {
				return languageId;
			}
			return Successor != null ? Successor.Handle(templateId, templateLoader) : Guid.Empty;
		}

		#endregion

	}

	#endregion

}
