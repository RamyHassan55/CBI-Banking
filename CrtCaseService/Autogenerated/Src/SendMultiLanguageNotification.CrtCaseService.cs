namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Common;
	using Core;
	using Core.Entities;
	using Core.Scheduler;
	using Creatio.FeatureToggling;

	#region Class : SendMultiLanguageNotification

	/// <summary>
	/// Represent a multilanguage notifier job.
	/// </summary>
	public class SendMultiLanguageNotification : IJobExecutor
	{
		#region Class: EmailData

		/// <summary>
		/// Email data container.
		/// </summary>
		public class EmailData
		{
			#region Properties: Public

			/// <summary>
			/// Case identifier.
			/// </summary>
			public Guid CaseId {
				get;
				private set;
			}
			/// <summary>
			/// Email template identifier.
			/// </summary>
			public Guid TplId {
				get;
				private set;
			}
			/// <summary>
			/// Recipient e-mail.
			/// </summary>
			public string Recipient {
				get;
				private set;
			}

			#endregion

			#region Methods: Private

			/// <summary>
			/// Check job parameters. Throws <see cref="KeyNotFoundException"/> if parameter
			/// isn't valid.
			/// </summary>
			/// <param name="parameters">Job parameters.</param>
			private void CheckParameters(IDictionary<string, object> parameters) {
				if (!parameters.ContainsKey("CaseRecordId")) {
					throw new KeyNotFoundException("The CaseRecordId key was not found in parameters.");
				}
				if (!parameters.ContainsKey("Recipient")) {
					throw new KeyNotFoundException("The Recipient key was not found in parameters.");
				}
				if (!parameters.ContainsKey("EmailTemplateId")) {
					throw new KeyNotFoundException("The Recipient key was not found in parameters.");
				}
			}

			/// <summary>
			/// Parse input parameters.
			/// </summary>
			/// <param name="parameters">Input parameters.</param>
			private void TryParse(IDictionary<string, object> parameters) {
				Guid caseRecordId = parameters.GetValue("CaseRecordId", Guid.Empty);
				if (caseRecordId == Guid.Empty) {
					throw new ArgumentEmptyException("CaseRecordId");
				}
				string recipient = parameters.GetValue("Recipient", string.Empty);
				if (recipient.IsNullOrEmpty()) {
					throw new ArgumentEmptyException("Recipient");
				}
				Guid emailTemplateId = parameters.GetValue("EmailTemplateId", Guid.Empty);
				if (emailTemplateId == Guid.Empty) {
					throw new ArgumentEmptyException("EmailTemplateId");
				}
				SetProperties(caseRecordId, recipient, emailTemplateId);
			}

			/// <summary>
			/// Set class data properties.
			/// </summary>
			/// <param name="caseRecordId">Case identifier.</param>
			/// <param name="recipient">Recipient email.</param>
			/// <param name="emailTemplateId">Email template identifier.</param>
			private void SetProperties(Guid caseRecordId, string recipient, Guid emailTemplateId) {
				CaseId = caseRecordId;
				Recipient = recipient;
				TplId = emailTemplateId;
			}

			#endregion

			#region Methods: Public

			/// <summary>
			/// Parse input parameters.
			/// </summary>
			/// <param name="parameters">Input parameters.</param>
			public void ParseParameters(IDictionary<string, object> parameters) {
				CheckParameters(parameters);
				TryParse(parameters);
			}

			#endregion

		}

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Email sender.
		/// </summary>
		protected EmailWithMacrosManagerBase EmailWithMacrosManager {
			get;
			set;
		}

		#endregion

		#region Properties : Public

		/// <summary>
		/// Helper for searching communication language by case entity.
		/// </summary>
		public IEmailTemplateLanguageHelper EmailTemplateLanguageHelper {
			get;
			set;
		}

		/// <summary>
		/// Loader for email templates.
		/// </summary>
		public ITemplateLoader TemplateLoader {
			get;
			set;
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Send email to recipient.
		/// </summary>
		/// <param name="emailData">Email data container.</param>
		/// <param name="multilangTemplateId">Multilanguage template.</param>
		protected virtual void SendEmail(EmailData emailData, Guid multilangTemplateId) {
			EmailWithMacrosManager.SendEmailTo(emailData.CaseId, multilangTemplateId, emailData.Recipient);
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Notifies contacts via multilanguage templates.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void Execute(UserConnection userConnection, IDictionary<string, object> parameters) {
			if (EmailWithMacrosManager == null) {
				EmailWithMacrosManager = new EmailWithMacrosManagerBase(userConnection);
			}
			EmailData emailData = new EmailData();
			emailData.ParseParameters(parameters);
			if (Features.GetIsEnabled("EmailMessageMultiLanguageV2")) {
				SendEmail(emailData, emailData.TplId);
			} else {
				if (TemplateLoader == null) {
					TemplateLoader = new EmailTemplateStore(userConnection);
				}
				if (EmailTemplateLanguageHelper == null) {
					EmailTemplateLanguageHelper = new EmailTemplateLanguageHelper(emailData.CaseId, userConnection);
				}
				Guid languageId = EmailTemplateLanguageHelper.GetLanguageId(emailData.TplId, TemplateLoader);
				Entity templateEntity = TemplateLoader.GetTemplate(emailData.TplId, languageId);
				SendEmail(emailData, templateEntity.PrimaryColumnValue);
			}
		}

		#endregion

	}

	#endregion

}
