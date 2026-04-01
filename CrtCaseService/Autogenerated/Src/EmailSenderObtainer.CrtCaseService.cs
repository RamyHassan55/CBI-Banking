namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;

	#region Class: EmailSenderObtainer

	/// <summary>
	/// Obtains sender from support mailboxes.
	/// </summary>
	public class EmailSenderObtainer
	{

		#region Fields: Private

		private readonly ISupportMailBoxRepository _supportMailBoxRepository;
		private readonly IContactCommLanguage _contactCommLanguage;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Indicates whether mailbox collection should be filtered by case category.
		/// </summary>
		public bool UseCategory {
			get;
			set;
		}

		/// <summary>
		/// Indicates whether multilanguage is enabled.
		/// </summary>
		public bool IsMultilanguageEnabled {
			get;
			set;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="EmailSenderObtainer"/>.
		/// </summary>
		/// <param name="supportMailBoxRepository">Support mailboxes obtainer.</param>
		/// <param name="supportDefMailBox">Obtainer for mailbox from system setting.</param>
		/// <param name="contactCommLanguage">Obtainer for mailbox related with contact language.</param>
		public EmailSenderObtainer(ISupportMailBoxRepository supportMailBoxRepository,
			IContactCommLanguage contactCommLanguage) {
			_supportMailBoxRepository = supportMailBoxRepository;
			_contactCommLanguage = contactCommLanguage;
		}

		#endregion

		#region Methods: Private

		private Guid GetContactLanguage(Guid contactId) {
			var contactLangId = _contactCommLanguage.Get(contactId);
			return contactLangId.IsNotEmpty()
				? contactLangId
				: _contactCommLanguage.GetDefault();
		}

		private string GetMailboxFromContactLanguage(IEnumerable<Entity> mailboxes, Guid contactId, Guid categoryId) {
			if (IsMultilanguageEnabled) {
				string mailboxFromContactLanguage = GetMailboxByContactLanguage(mailboxes, contactId);
				if (!string.IsNullOrEmpty(mailboxFromContactLanguage)) {
					return mailboxFromContactLanguage;
				}
			}
			return null;
		}

		private string GetSupportMailBox(IEnumerable<Entity> mailboxes) {
			var mailbox = mailboxes.FirstOrDefault();
			return mailbox?.GetTypedColumnValue<string>("MailboxName");
		}

		/// <summary>
		/// Gets support mailbox related to contact language.
		/// </summary>
		/// <param name="mailboxes">Support mailboxes.</param>
		/// <param name="contactId">Contact entity identifier.</param>
		/// <returns>Support mailbox.</returns>
		private string GetMailboxByContactLanguage(IEnumerable<Entity> mailboxes, Guid contactId) {
			Guid langId = GetContactLanguage(contactId);
			if (langId != default(Guid)) {
				IEnumerable<Entity> langMailboxes
					= mailboxes.Where(mbox => mbox.GetTypedColumnValue<Guid>("MessageLanguageId") == langId);
				if (langMailboxes.Any()) {
					var mbox = langMailboxes.First();
					var mboxName = mbox.GetTypedColumnValue<string>("MailboxName");
					return mboxName;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets mailbox if support mailbox is single in recipients.
		/// </summary>
		/// <param name="recipients">Recipients.</param>
		/// <param name="mailboxes">Support mailboxes.</param>
		/// <returns>Support mailbox, if single.</returns>
		private string GetSingleMailbox(string[] recipients, IEnumerable<Entity> mailboxes) {
			IEnumerable<string> mboxNames = mailboxes.Select(mbox => mbox.GetTypedColumnValue<string>("MailboxName"));
			IEnumerable<string> common = mboxNames.Intersect(recipients, StringComparer.OrdinalIgnoreCase);
			return common.Count() == 1 ? common.First() : null;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Obtains sender from support mailbox.
		/// </summary>
		/// <param name="recipients">Recipients.</param>
		/// <param name="contactId">Contact entity identifier.</param>
		/// <param name="categoryId">Category entity identifier.</param>
		/// <returns>Support mailbox.</returns>
		public virtual string GetMailSender(string[] recipients, Guid contactId, Guid categoryId) {
			IEnumerable<Entity> mailboxes = _supportMailBoxRepository.GetMailboxes();
			string singleMailbox = GetSingleMailbox(recipients, mailboxes);
			if (singleMailbox != null) {
				return singleMailbox;
			}
			if (UseCategory && categoryId != Guid.Empty ) {
				mailboxes = mailboxes.Where(mbox => mbox.GetTypedColumnValue<Guid>("CategoryId") == categoryId);
			}
			string mailboxSenderName = GetMailboxFromContactLanguage(mailboxes, contactId, categoryId);
			if (!string.IsNullOrEmpty(mailboxSenderName)) {
				return mailboxSenderName;
			}
			string defMailbox = _supportMailBoxRepository.GetMailboxFromSystemSetting();
			if (!string.IsNullOrEmpty(defMailbox)) {
				return UseCategory && categoryId != Guid.Empty ?
					GetSingleMailbox(new string[] { defMailbox }, mailboxes) ??
					mailboxes.FirstOrDefault()?.GetTypedColumnValue<string>("MailboxName") ?? defMailbox
					: defMailbox;
			}
			return GetSupportMailBox(mailboxes);
		}

		#endregion

	}

	#endregion

}
