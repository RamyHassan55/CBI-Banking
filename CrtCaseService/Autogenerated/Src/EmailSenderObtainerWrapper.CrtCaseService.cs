namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Creatio.FeatureToggling;
	using Terrasoft.Core;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: EmailSenderObtainerWrapper

	/// <summary>
	/// Wrapper for <see cref="EmailSenderObtainer"/>.
	/// </summary>
	public class EmailSenderObtainerWrapper
	{

		#region Properties: Private

		private EmailSenderObtainer _obtainer;
		private EmailSenderObtainer Obtainer {
			get {
				return _obtainer ?? (_obtainer = CreateObtainer());
			}
		}

		/// <summary>
		/// Indicates whether mailbox collection should be filtered by case category.
		/// </summary>
		private bool UseCategory =>
			SystemSettings.GetValue(UserConnection, "DefineCaseCategoryFromMailBox", false)
			&& Features.GetIsEnabled("CategoryFromMailBox");

		/// <summary>
		/// Indicates whether multilanguage is enabled.
		/// </summary>
		private bool IsMultilanguageEnabled =>
			Features.GetIsEnabled("EmailMessageMultiLanguage") ||
			Features.GetIsEnabled("EmailMessageMultiLanguageV2");

		#endregion

		#region Properties: Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Constructors: Public

		public EmailSenderObtainerWrapper(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		public EmailSenderObtainerWrapper(UserConnection userConnection, EmailSenderObtainer obtainer)
			: this(userConnection) {
			_obtainer = obtainer;
		}

		#endregion

		#region Methods: Private

		private EmailSenderObtainer CreateObtainer() {
			var mailBoxRepository = new SupportMailBoxRepository(UserConnection);
			var contactCommLang = new ContactCommLanguage(UserConnection);
			return new EmailSenderObtainer(mailBoxRepository, contactCommLang) {
				UseCategory = UseCategory,
				IsMultilanguageEnabled = IsMultilanguageEnabled
			};
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets support mailbox.
		/// </summary>
		/// <param name="recipients">Recipients, separated by semicolon.</param>
		/// <param name="contactId">Contact entity identifier.</param>
		/// <param name="categoryId">Category entity identifier.</param>
		/// <returns>Support mailbox.</returns>
		public string GetSupportMailBox(string recipients, Guid contactId, Guid categoryId = default(Guid)) {
			string[] parsedEmails = recipients.ParseEmailAddress().ToArray();
			return Obtainer.GetMailSender(parsedEmails, contactId, categoryId);
		}

		/// <summary>
		/// Gets support mailbox.
		/// </summary>
		/// <param name="recipients">List of recipients strings.</param>
		/// <param name="contactId">Contact entity identifier.</param>
		/// <param name="categoryId">Category entity identifier.</param>
		/// <returns>Support mailbox.</returns>
		public string GetSupportMailBox(List<string> recipients, Guid contactId, Guid categoryId = default(Guid)) {
			List<string> parsedEmails = new List<string>();
			recipients.ForEach(recipientString => parsedEmails.AddRange(recipientString.ParseEmailAddress()));
			return Obtainer.GetMailSender(parsedEmails.ToArray(), contactId, categoryId);
		}

		#endregion

	}

	#endregion

}
