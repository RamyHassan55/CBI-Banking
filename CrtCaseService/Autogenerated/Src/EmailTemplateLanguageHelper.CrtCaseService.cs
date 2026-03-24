namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;


	#region Class: EmailTemplateLanguageHelper

	/// <summary>
	/// Helper for searching communication language by case entity.
	/// </summary>
	public class EmailTemplateLanguageHelper: IEmailTemplateLanguageHelper
	{

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="EmailTemplateLanguageHelper" />.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="userConnection">User connection.</param>
		public EmailTemplateLanguageHelper(Guid caseId, UserConnection userConnection) {
			UserConnection = userConnection;
			EntitySchema caseSchema = UserConnection.EntitySchemaManager.FindInstanceByName("Case");
			CaseEntity = caseSchema.CreateEntity(UserConnection);
			CaseEntity.FetchFromDB(caseId);
		}

		#endregion


		#region Properties: Protected

		/// <summary>
		/// User connection.
		/// </summary>
		protected UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Case entity.
		/// </summary>
		public Entity CaseEntity {
			get;
			private set;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get default language for sending email template.
		/// </summary>
		/// <param name="templateId">Email template identifier.</param>
		/// <returns>Language identifier.</returns>
		public virtual Guid GetLanguageId(Guid templateId) {
			return GetLanguageId(templateId, null);
		}

		/// <summary>
		/// Get default language for sending email template.
		/// </summary>
		/// <param name="templateId">Email template id.</param>
		/// <param name="templateLoader">Email template store.</param>
		/// <returns>Return language id.</returns>
		public virtual Guid GetLanguageId(Guid templateId, ITemplateLoader templateLoader) {
			var contactHandler = new ContactLangEmailTemplateHandler(CaseEntity, UserConnection);
			var mailboxHandler = new MailboxLangEmailTemplateHandler(CaseEntity, UserConnection);
			var supportServiceHandler = new SupportServiceLangEmailTemplateHandler(CaseEntity, UserConnection);
			var defaultMessageHandler = new DefaultMessageLangEmailTemplateHandler(CaseEntity, UserConnection);
			contactHandler.Successor = mailboxHandler;
			mailboxHandler.Successor = supportServiceHandler;
			supportServiceHandler.Successor = defaultMessageHandler;
			return contactHandler.Handle(templateId, templateLoader);
		}

		#endregion

	}

	#endregion

}
