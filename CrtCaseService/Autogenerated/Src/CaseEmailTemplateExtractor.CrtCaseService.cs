namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: CaseEmailTemplateExtractor

	/// <summary>
	/// Utility class for reciving multilanguage email templates.
	/// </summary>
	public class CaseEmailTemplateExtractor
	{
		#region Properties: Public

		/// <summary>
		/// Instance of <see cref="UserConnection"/> type.
		/// </summary>
		public UserConnection UserConnection {
			get; set;
		}

		/// <summary>
		/// Case identifier.
		/// </summary>
		public Guid CaseId {
			get; private set;
		}

		private EmailTemplateStore _emailTemplateStore;

		/// <summary>
		/// Provides ability to get multilanguage email template.
		/// instance of the <see cref="EmailTemplateStore"/> class
		/// </summary>
		public EmailTemplateStore EmailTemplateStore {
			get {
				return _emailTemplateStore ?? new EmailTemplateStore(UserConnection);
			}
			set {
				_emailTemplateStore = value;
			}
		}

		private EmailTemplateLanguageHelper _emailTemplateLanguageHelper;

		/// <summary>
		/// Helper for searching communication language by case entity.
		/// instance of the <see cref="EmailTemplateLanguageHelper"/> class
		/// </summary>
		public EmailTemplateLanguageHelper EmailTemplateLanguageHelper {
			get {
				return _emailTemplateLanguageHelper ?? new EmailTemplateLanguageHelper(CaseId, UserConnection);
			}
			set {
				_emailTemplateLanguageHelper = value;
			}
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize a new instance of the <see cref="CaseEmailTemplateExtractor"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="caseId">Case identifier.</param>
		public CaseEmailTemplateExtractor(UserConnection userConnection, Guid caseId) {
			UserConnection = userConnection;
			CaseId = caseId;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Convert EmailTemplates to Multilanguage templates.
		/// </summary>
		/// <param name="templateCollection">Email templates.</param>
		///  <returns>Multilanguage email templates.</returns>
		public virtual EntityCollection GetMultilangTplListFromTplList(
			EntityCollection templateCollection) {
			var resultCollection = new EntityCollection(UserConnection, templateCollection.Schema.Name);
			foreach (var emailTemplate in templateCollection) {
				var emailTemplateId = emailTemplate.GetTypedColumnValue<Guid>("EmailTemplateId");
				var languageId = EmailTemplateLanguageHelper.GetLanguageId(emailTemplateId);
				var templateEntity = EmailTemplateStore.GetTemplate(emailTemplateId, languageId);
				var emailTemplateClone = (Entity)emailTemplate.Clone();
				emailTemplateClone.SetColumnValue("EmailTemplateId", templateEntity.PrimaryColumnValue);
				resultCollection.Add(emailTemplateClone);
			}
			return resultCollection;
		}

		#endregion

	}

	#endregion
}
