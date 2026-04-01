 namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.DB;
	using global::Common.Logging;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: EmailWithMacrosManager

	/// <summary>
	/// E-mail sender which allows to control the flow of activity creating, filling and macros processing.
	/// As a macros processor it uses <see cref="InvokableMacrosHelper"/>.
	/// It also automates e-mail sender obtaining if it is not specified.
	/// </summary>
	/// <remarks>It works only with Case entities.</remarks>
	public class EmailWithMacrosManagerBase : CrtBaseEmailWithMacrosManager<InvokableMacrosHelper>
	{

		#region Struct: CaseData

		/// <summary>
		/// Case data container.
		/// </summary>
		protected struct CaseData
		{
			/// <summary>
			/// Contact identifier.
			/// </summary>
			public Guid ContactId;
			/// <summary>
			/// Category identifier.
			/// </summary>
			public Guid CategoryId;
			/// <summary>
			/// Contact e-mail.
			/// </summary>
			public string ContactEmail;
			/// <summary>
			/// Activity recipient.
			/// </summary>
			public string Recipient;
			/// <summary>
			/// Parent activity copy recipient.
			/// </summary>
			public string CC;
			/// <summary>
			/// Parent activity blind copy recipient.
			/// </summary>
			public string BCC;
			/// <summary>
			/// Parent activity title.
			/// </summary>
			public string Title;
			/// <summary>
			/// Parent activity InReplyTo property.
			/// </summary>
			public string ParentActivityInReplyTo;
			/// <summary>
			/// Parent activity id.
			/// </summary>
			public Guid ParentActivityId;
			/// <summary>
			/// Parent activity recipient.
			/// </summary>
			public string ParentActivityRecipient;
			/// <summary>
			/// Parent activity sender.
			/// </summary>
			public string ParentActivitySender;

			/// <summary>
			/// Gives a row with recipient, copy recipient and blind copy recipient separated with semicolons.
			/// </summary>
			public string Recipients => string.Join(";", ParentActivityRecipient, CC, BCC);

			public void PickUpContactEmail(string contactEmail, UserConnection userConnection) {
				ContactEmail = contactEmail;
				if (ContactEmail.IsNullOrEmpty()) {
					ContactEmail = ContactUtilities.GetLastAddContactEmail(userConnection, ContactId);
				}
				bool notifyOnlyContact = SystemSettings.GetValue(userConnection, "AutoNotifyOnlyContact", false);
				if (ContactEmail.IsNullOrEmpty() && !notifyOnlyContact) {
					ContactEmail = ParentActivitySender;
				}
			}

			public void PickUpRecipient(string recipient, UserConnection userConnection) {
				Recipient = ParentActivitySender.IsNullOrEmpty()
					? ContactEmail
					: ParentActivitySender;
				if (!Features.GetIsEnabled("SendOnlyToContactEmail")) {
					Recipient = string.Join(";", Recipient, recipient);
				}
				Recipient = Recipient.Trim(';');
			}
		}

		#endregion

		#region Constants: Protected

		/// <summary>
		/// E-mail subject replying prefix.
		/// </summary>
		protected const string ReplyPrefix = "RE: ";
		/// <summary>
		/// Case entity schema name.
		/// </summary>
		/// <remarks>We work only with cases.</remarks>
		protected const string SchemaName = "Case";

		#endregion

		#region Properties: Protected

		private static ILog _log;
		/// <summary>
		/// Case notification log.
		/// </summary>
		protected static ILog Log => _log ?? (_log = LogManager.GetLogger("CaseNotification"));

		#endregion

		#region Properties: Public

		/// <summary>
		/// E-mail sender obtain wrapper.
		/// </summary>
		public EmailSenderObtainerWrapper EmailSenderObtainerWrapper {
			get;
			set;
		}

		/// <summary>
		/// Filter for incident registration mailboxes.
		/// </summary>
		public IncindentRegistrationMailboxFilter IncindentRegistrationMailboxFilter {
			get;
			set;
		}

		/// <summary>
		/// Asynchronous email sender manager.
		/// </summary>
		public AsyncEmailSender AsyncEmailSender {
			get;
			set;
		}

		#endregion

		#region Constructors: Public

		[ExcludeFromCodeCoverage]
		[Obsolete("7.10.2 | There's no way to work without user connection. Left only for the backward compatibility.")]
		protected EmailWithMacrosManagerBase() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailWithMacrosManager"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public EmailWithMacrosManagerBase(UserConnection userConnection)
			: base(userConnection) {
			InitializeReplacementMap();
			IncindentRegistrationMailboxFilter = new IncindentRegistrationMailboxFilter(userConnection);
			EmailSenderObtainerWrapper = new EmailSenderObtainerWrapper(userConnection);
			AsyncEmailSender = new AsyncEmailSender(userConnection);
		}

		#endregion

		#region Methods: Private

		private void InitializeReplacementMap() {
			ReplacementMap = new Dictionary<string, string> {
				{ "#EstimatePic#", "[#@Invoke.EstimateLinksGenerator#]" },
				{ "[#Symptoms#]",  "[#@Invoke.SymptomsGenerator#]" }
			};
		}

		private string AddInReplyToHeaderProperty(string inReplyToValue, string existProperties) {
			return existProperties + "In-Reply-To" + "=" + inReplyToValue + "\n";
		}

		private string ApplyPrefix(string title) {
			return title.IndexOf(ReplyPrefix, StringComparison.OrdinalIgnoreCase) != 0
				? ReplyPrefix + title
				: title;
		}

		private string GetInReplyToProperty(Guid activityId) {
			var resultSelect = new Select(UserConnection).Top(1)
				.Column("MessageId")
				.From("EmailMessageData").Where()
				.OpenBlock("MessageId").IsNotEqual(Column.Const(string.Empty))
				.And("MessageId").Not().IsNull()
				.And("ActivityId").IsEqual(new QueryParameter(activityId))
				.CloseBlock() as Select;
			return resultSelect.ExecuteScalar<string>();
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets current e-mail templates entity schema name.
		/// </summary>
		/// <returns>E-mail templates entity schema name.</returns>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		protected override string GetEmailTemplateSchemaName() {
			return Features.GetIsDisabled("EmailMessageMultiLanguage")
				? "EmailTemplate"
				: "EmailTemplateLang";
		}

		/// <summary>
		/// Sets activity title as a subject from e-mail template if it is not specified yet.
		/// Otherwise, adds a replying prefix (<see cref="ReplyingPrefix" />)
		/// to an <paramref name="activity"/>'s title.
		/// </summary>
		/// <param name="activity">Activity entity.</param>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		protected void FillTitle(Entity activity, Guid caseId, Guid tplId) {
			var title = activity.GetTypedColumnValue<string>("Title");
			activity.SetColumnValue("Title", title.IsNullOrEmpty()
				? GetTemplateSubject(SchemaName, caseId, tplId)
				: ApplyPrefix(title));
		}

		/// <summary>
		/// Binds <paramref name="activity"/> to the case with given record identifier.
		/// </summary>
		/// <param name="activity">Activity entity.</param>
		/// <param name="caseId">Case record identifier.</param>
		protected void BindToCase(Entity activity, Guid caseId) {
			activity.SetColumnValue("CaseId", caseId);
		}

		/// <summary>
		/// Gets e-mail sender by given case data.
		/// It uses <see cref="EmailSenderObtainerWrapper"/>.
		/// </summary>
		/// <param name="data">Case data.</param>
		/// <returns>E-mail sender.</returns>
		protected string GetSender(CaseData data) {
			return EmailSenderObtainerWrapper.GetSupportMailBox(data.Recipients, data.ContactId, data.CategoryId);
		}

		/// <summary>
		/// Gets case data by given case record identifier.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <returns>Case data.</returns>
		protected virtual CaseData GetCaseData(Guid caseId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, SchemaName);
			string contactIdColumnName = esq.AddColumn("Contact.Id").Name;
			string categoryIdColumnName = esq.AddColumn("Category.Id").Name;
			string contactEmailColumnName = esq.AddColumn("Contact.Email").Name;
			string titleColumnName = esq.AddColumn("ParentActivity.Title").Name;
			string parentActivityIdColumnName = esq.AddColumn("ParentActivity.Id").Name;
			string parentActivitySenderColumnName = esq.AddColumn("ParentActivity.Sender").Name;
			string recipientColumnName = esq.AddColumn("ParentActivity.Recepient").Name;
			string ccColumnName = esq.AddColumn("ParentActivity.CopyRecepient").Name;
			string bccColumnName = esq.AddColumn("ParentActivity.BlindCopyRecepient").Name;
			var @case = esq.GetEntity(UserConnection, caseId);
			var data = new CaseData();
			if (@case == null) {
				return data;
			}
			data.ContactId = @case.GetTypedColumnValue<Guid>(contactIdColumnName);
			data.CategoryId = @case.GetTypedColumnValue<Guid>(categoryIdColumnName);
			data.Title = @case.GetTypedColumnValue<string>(titleColumnName);
			data.ParentActivitySender = @case
				.GetTypedColumnValue<string>(parentActivitySenderColumnName)
				.ExtractEmailAddressIf();
			data.PickUpContactEmail(@case.GetTypedColumnValue<string>(contactEmailColumnName), UserConnection);
			data.PickUpRecipient(IncindentRegistrationMailboxFilter
				.GetRecipientWithoutIncindent(@case.GetTypedColumnValue<string>(recipientColumnName)), UserConnection);
			if (!Features.GetIsEnabled("CasesOnlyRespondToSender")) {
				data.CC = @case.GetTypedColumnValue<string>(ccColumnName);
				data.BCC = @case.GetTypedColumnValue<string>(bccColumnName);
			}
			if (SystemSettings.GetValue(UserConnection, "AutoNotifyOnlyContact", false)) {
				data.Recipient = data.ContactEmail;
				data.CC = data.BCC = default(string);
			}
			data.ParentActivityId = @case.GetTypedColumnValue<Guid>(parentActivityIdColumnName);
			data.ParentActivityRecipient = @case.GetTypedColumnValue<string>(recipientColumnName);
			data.ParentActivityInReplyTo = data.ParentActivityId.IsNotEmpty()
				? GetInReplyToProperty(data.ParentActivityId)
				: string.Empty;
			return data;
		}

		/// <summary>
		/// Fills an <paramref name="activity"/> with given <paramref name="data"/>.
		/// </summary>
		/// <param name="activity">Activity entity.</param>
		/// <param name="data">Case data.</param>
		protected virtual void FillEntityWithCaseData(Entity activity, CaseData data) {
			activity.SetColumnValue("Title", data.Title);
			activity.SetColumnValue("Recepient", data.Recipient);
			activity.SetColumnValue("CopyRecepient", data.CC);
			activity.SetColumnValue("BlindCopyRecepient", data.BCC);
			if (data.ParentActivityInReplyTo.IsNotNullOrEmpty()) {
				var headerProperties = activity.GetTypedColumnValue<string>("HeaderProperties");
				activity.SetColumnValue("HeaderProperties", AddInReplyToHeaderProperty(data.ParentActivityInReplyTo, headerProperties));
			}
		}

		/// <summary>
		/// Processes and sets body to the <paramref name="activity"/> from the template
		/// which will be taken by template identifier (<paramref name="tplId"/>)
		/// from e-mail template table (<seealso cref="GetEmailTemplateSchemaName"/>).
		/// </summary>
		/// <param name="activity">Activity entity.</param>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		protected virtual void PreProcess(Entity activity, Guid caseId, Guid tplId) {
			activity.SetColumnValue("Body", GetTemplateBody(SchemaName, caseId, tplId));
			BindToCase(activity, caseId);
		}

		protected override void SendActivity(Guid id) {
			if (Features.GetIsEnabled("UseAsyncEmailSender")) {
				AsyncEmailSender.SendAsync(id);
			} else {
				base.SendActivity(id);
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Builds an e-mail activity then sends it.
		/// The sender and recipients will be obtained automatically.
		/// Use this method to reply to all the participants of the parent (root) e-mail activity.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		public virtual void SendEmail(Guid caseId, Guid tplId) {
			var activity = CreateActivityEntity();
			PreProcess(activity, caseId, tplId);
			CaseData data = GetCaseData(caseId);
			activity.SetColumnValue("Sender", GetSender(data));
			FillEntityWithCaseData(activity, data);
			FillTitle(activity, caseId, tplId);
			activity.Save();
			SendActivity(activity.GetTypedColumnValue<Guid>("Id"));
		}

		/// <summary>
		/// Builds an e-mail activity then sends it.
		/// Recipients will be obtained automatically.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		/// <param name="sender">Sender e-mail.</param>
		public virtual void SendEmailFrom(Guid caseId, Guid tplId, string sender) {
			Entity activity = CreateActivityEntity();
			PreProcess(activity, caseId, tplId);
			activity.SetColumnValue("Sender", sender);
			CaseData data = GetCaseData(caseId);
			FillEntityWithCaseData(activity, data);
			FillTitle(activity, caseId, tplId);
			activity.Save();
			SendActivity(activity.GetTypedColumnValue<Guid>("Id"));
		}

		/// <summary>
		/// Builds an e-mail activity then sends it to <paramref name="recipient"/>.
		/// The sender will be obtained automatically.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		/// <param name="recipient">Recipient e-mail.</param>
		public virtual void SendEmailTo(Guid caseId, Guid tplId, string recipient) {
			Entity activity = CreateActivityEntity();
			PreProcess(activity, caseId, tplId);
			CaseData data = GetCaseData(caseId);
			activity.SetColumnValue("Sender", GetSender(data));
			activity.SetColumnValue("Recepient", recipient);
			FillTitle(activity, caseId, tplId);
			activity.Save();
			SendActivity(activity.GetTypedColumnValue<Guid>("Id"));
		}

		/// <summary>
		/// Builds an e-mail activity then sends it from <paramref name="sender"/> to <paramref name="recipient"/>.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		/// <param name="sender">Sender e-mail.</param>
		/// <param name="recipient">Recipient e-mail.</param>
		public virtual void SendEmailFromTo(Guid caseId, Guid tplId, string sender, string recipient) {
			Entity activity = CreateActivityEntity();
			PreProcess(activity, caseId, tplId);
			activity.SetColumnValue("Sender", sender);
			activity.SetColumnValue("Recepient", recipient);
			FillTitle(activity, caseId, tplId);
			activity.Save();
			SendActivity(activity.GetTypedColumnValue<Guid>("Id"));
		}

		#endregion

	}

	#endregion

}
