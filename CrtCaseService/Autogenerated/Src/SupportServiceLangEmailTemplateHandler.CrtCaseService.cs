namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using SystemSettings = Core.Configuration.SysSettings;

	#region SupportServiceLangEmailTemplateHandler

	/// <summary>
	/// Language search handler for service support system settings.
	/// </summary>
	public class SupportServiceLangEmailTemplateHandler : EmailTemplateCommLangHandlerBase
	{

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="SupportServiceLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="caseId">Case record indentifier.</param>
		/// <param name="userConnection">User connection.</param>
		public SupportServiceLangEmailTemplateHandler(Guid caseId, UserConnection userConnection)
			: base(caseId, userConnection) {
		}

		/// <summary>
		/// Initialize new instance of <see cref="SupportServiceLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="userConnection">User connection.</param>
		public SupportServiceLangEmailTemplateHandler(Entity entity, UserConnection userConnection)
			: base(entity, userConnection) {
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Get language identifier from mailbox by system settings for incident registration.
		/// </summary>
		/// <param name="supportServiceEmail">System setting for incident registration value.</param>
		/// <returns>Language identifier.</returns>
		private Guid GetCommunicationLanguageId(string supportServiceEmail) {
			if (string.IsNullOrEmpty(supportServiceEmail)) {
				return Guid.Empty;
			}
			EntitySchema mailboxSyncSettingsSchema =
				UserConnection.EntitySchemaManager.FindInstanceByName("MailboxSyncSettings");
			Entity entity = mailboxSyncSettingsSchema.CreateEntity(UserConnection);
			return entity.FetchFromDB("SenderEmailAddress", supportServiceEmail)
				? entity.GetTypedColumnValue<Guid>("MessageLanguageId")
				: Guid.Empty;
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
			string supportServiceEmail = SystemSettings.GetValue(UserConnection, "SupportServiceEmail", string.Empty);
			Guid languageId = GetCommunicationLanguageId(supportServiceEmail);
			if (languageId != default(Guid) && IsTemplateValid(templateId, languageId, templateLoader)) {
				return languageId;
			}
			return Successor != null ? Successor.Handle(templateId, templateLoader) : Guid.Empty;
		}

		#endregion

	}

	#endregion

}
