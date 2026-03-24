namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: ContactLangEmailTemplateHandler

	/// <summary>
	/// Language search handler for the contact.
	/// </summary>
	public class ContactLangEmailTemplateHandler : EmailTemplateCommLangHandlerBase
	{

		#region Fields: Private

		private readonly ContactCommLanguage _commLang;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="ContactLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="caseId">Case record indentifier.</param>
		/// <param name="userConnection">User connection.</param>
		public ContactLangEmailTemplateHandler(Guid caseId, UserConnection userConnection)
			: base(caseId, userConnection) {
			_commLang = new ContactCommLanguage(UserConnection);
		}

		/// <summary>
		/// Initialize new instance of <see cref="ContactLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="userConnection"User connection.></param>
		public ContactLangEmailTemplateHandler(Entity entity, UserConnection userConnection)
			: base(entity, userConnection) {
			_commLang = new ContactCommLanguage(UserConnection);
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
			var contactId = Entity.GetTypedColumnValue<Guid>("ContactId");
			Guid contactLanguageId = _commLang.Get(contactId);
			if (contactLanguageId != default(Guid) && IsTemplateValid(templateId, contactLanguageId, templateLoader)) {
				return contactLanguageId;
			}
			return Successor != null ? Successor.Handle(templateId, templateLoader) : Guid.Empty;
		}

		#endregion

	}

	#endregion

}
