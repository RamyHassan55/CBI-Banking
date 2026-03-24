namespace Terrasoft.Configuration 
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: EmailTemplateCommLangHandler

	/// <summary>
	/// An abstract e-mail template communication language receiver.
	/// </summary>
	public abstract class EmailTemplateCommLangHandlerBase : EmailTemplateLanguageHandler<Guid, Guid>
	{

		#region Constructors: Protected

		/// <summary>
		/// Initialize new instance of <see cref="EmailTemplateCommLangHandler" />.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="userConnection">User connection.</param>
		protected EmailTemplateCommLangHandlerBase(Guid caseId, UserConnection userConnection) {
			UserConnection = userConnection;
			EntitySchema caseSchema = UserConnection.EntitySchemaManager.FindInstanceByName("Case");
			Entity = caseSchema.CreateEntity(UserConnection);
			Entity.FetchFromDB(caseId);
		}

		/// <summary>
		/// Initialize new instance of <see cref="EmailTemplateCommLangHandler" />.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="userConnection">User connection.</param>
		protected EmailTemplateCommLangHandlerBase(Entity entity, UserConnection userConnection) {
			UserConnection = userConnection;
			Entity = entity;
		}

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Case entity.
		/// </summary>
		protected Entity Entity {
			get;
			set;
		}

		/// <summary>
		/// User connection.
		/// </summary>
		protected UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Check is template with specified language exist.
		/// </summary>
		/// <param name="templateId">Email template identifier.</param>
		/// <param name="languageId">Communication language identifier.</param>
		/// <param name="templateLoader">Loader for email templates.</param>
		/// <returns>Is template exist.</returns>
		protected bool IsTemplateValid(Guid templateId, Guid languageId, ITemplateLoader templateLoader) {
			if (templateLoader == null) {
				templateLoader = GetEmailTemplateStore();
			}
			Entity template = templateLoader.GetTemplate(templateId, languageId);
			return template != null 
				&& !string.IsNullOrEmpty(template.GetTypedColumnValue<string>("Subject"))
				&& !string.IsNullOrEmpty(template.GetTypedColumnValue<string>("Body"));
		}

		/// <summary>
		/// Get email template store for searching email template.
		/// </summary>
		/// <returns>Email template store.</returns>
		protected virtual ITemplateLoader GetEmailTemplateStore() {
			return new EmailTemplateStore(UserConnection);
		}

		#endregion

	}

	#endregion

} 
