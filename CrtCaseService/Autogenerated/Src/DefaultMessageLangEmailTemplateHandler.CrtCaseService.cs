namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: DefaultMessageLangEmailTemplateHandler

	/// <summary>
	/// Default language search handler.
	/// </summary>
	public class DefaultMessageLangEmailTemplateHandler : EmailTemplateCommLangHandlerBase
	{

		#region Constants: Protected

		private const string DefaultMessageLanguageCode = "DefaultMessageLanguage";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="DefaultMessageLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="caseId">Case record indentifier.</param>
		/// <param name="userConnection">User connection.</param>
		public DefaultMessageLangEmailTemplateHandler(Guid caseId, UserConnection userConnection)
			: base(caseId, userConnection) {
		}

		/// <summary>
		/// Initialize new instance of <see cref="DefaultMessageLangEmailTemplateHandler" />.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="userConnection">User connection.></param>
		public DefaultMessageLangEmailTemplateHandler(Entity entity, UserConnection userConnection)
			: base(entity, userConnection) {
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
			Guid defaultLanguageMessageId = SystemSettings.GetValue(UserConnection, DefaultMessageLanguageCode,
				default(Guid));
			if (defaultLanguageMessageId != default(Guid) && IsTemplateValid(templateId, defaultLanguageMessageId, templateLoader)) {
				return defaultLanguageMessageId;
			}
			return Successor != null ? Successor.Handle(templateId, templateLoader) : Guid.Empty;
		}

		#endregion

	}

	#endregion

}
