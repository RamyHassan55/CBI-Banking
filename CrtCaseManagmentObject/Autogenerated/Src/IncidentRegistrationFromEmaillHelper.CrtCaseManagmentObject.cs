namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Common;
	using Core;
	using Core.DB;
	using Core.Entities;
	using Core.Factories;
	using Terrasoft.Configuration.CaseService;
	using HttpUtility = System.Web.HttpUtility;
	using SystemSettings = Core.Configuration.SysSettings;
	using global::Common.Logging;

	#region Class: IncidentRegistrationFromEmailHelper

	/// <summary>
	/// Incident registration for incoming e-mail messages.
	/// </summary>
	public class IncidentRegistrationFromEmailHelper : RegistrationFromEmail
	{
		#region Class: Private

		private class ParticipantInfoProvider
		{
			private ParticipantInfo _participantInfo;

			private readonly UserConnection _userConnection;

			public ParticipantInfoProvider(UserConnection userConnection) {
				_userConnection = userConnection;
			}

			public ParticipantInfo Get(Guid activityId) {
				if (_participantInfo == null) {
					_participantInfo = GetContactIdFromParticipant(activityId);

				}
				return _participantInfo;
			}

			private ParticipantInfo GetContactIdFromParticipant(Guid activityId) {
				Guid contactId = Guid.Empty;
				Guid accountId = Guid.Empty;
				Select selectQuery =
						new Select(_userConnection).Column("c", "Id")
							.Column("c", "AccountId")
							.From("ActivityParticipantRole")
							.As("apr")
							.InnerJoin("ActivityParticipant")
							.As("ap")
							.On("apr", "Id")
							.IsEqual("ap", "RoleId")
							.InnerJoin("Activity")
							.As("a")
							.On("ap", "ActivityId")
							.IsEqual("a", "Id")
							.InnerJoin("Contact")
							.As("c")
							.On("ap", "ParticipantId")
							.IsEqual("c", "Id")
							.Where("apr", "Code")
							.IsEqual(Column.Const("From"))
							.And("a", "Id")
							.IsEqual(Column.Const(activityId)) as Select;
				using (DBExecutor dbExec = _userConnection.EnsureDBConnection()) {
					using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
						while (reader.Read()) {
							contactId = reader.GetColumnValue<Guid>("Id");
							accountId = reader.GetColumnValue<Guid>("AccountId");
						}
					}
				}
				return new ParticipantInfo(contactId, accountId);
			}
		}

		/// <summary>
		/// Represent information for case account related with contact
		/// </summary>
		private class ParticipantInfo
		{
			#region Properties: Public

			/// <summary>
			/// Contact identifier
			/// </summary>
			public Guid ContactId {
				get;
				private set;
			}

			/// <summary>
			/// Account identifier for case entities
			/// </summary>
			public Guid AccountId {
				get;
				private set;
			}

			#endregion

			#region Constructors: Public

			/// <summary>
			/// Initialize new instance of <see cref="ParticipantInfo"/>
			/// </summary>
			/// <param name="contactId">Contact identifier</param>
			/// <param name="accountId">Account identifier</param>
			public ParticipantInfo(Guid contactId, Guid accountId) {
				ContactId = contactId;
				AccountId = accountId;
			}

			#endregion
		}

		#endregion

		#region Class: Protected

		/// <summary>
		/// Represents an e-mail message.
		/// </summary>
		protected class Email
		{

			#region Properties: Public

			/// <summary>
			/// Chooses correct closure code depends on its origin.
			/// </summary>
			/// <returns>Closure code id if found.</returns>
			public Guid? ClosureCode {
				get {
					if (IsBlacklisted) {
						return CaseConsts.ReceivedFromBlacklistedEmail;
					}
					if (!AreHeaderPropertiesOk) {
						return CaseConsts.AutomaticEmailReply;
					}
					return null;
				}
			}

			/// <summary>
			/// Header properties validation success.
			/// </summary>
			public bool AreHeaderPropertiesOk {
				get;
				private set;
			}

			/// <summary>
			/// Is in blacklist.
			/// </summary>
			public bool IsBlacklisted {
				get;
				private set;
			}

			/// <summary>
			/// Is junk mail.
			/// </summary>
			public bool IsJunk {
				get {
					return IsBlacklisted || !AreHeaderPropertiesOk;
				}
			}

			/// <summary>
			/// E-mail header properties.
			/// </summary>
			public string HeaderProperties {
				get;
				private set;
			}

			/// <summary>
			/// E-mail body.
			/// </summary>
			public string Body {
				get;
				private set;
			}

			/// <summary>
			/// Email title.
			/// </summary>
			public string Title {
				get;
				private set;
			}

			/// <summary>
			/// Account identifier.
			/// </summary>
			public Guid AccountId {
				get;
				private set;
			}

			/// <summary>
			/// Contact identifier.
			/// </summary>
			public Guid ContactId {
				get;
				private set;
			}

			/// <summary>
			/// Activity identifier.
			/// </summary>
			public Guid ActivityId {
				get;
				private set;
			}

			/// <summary>
			/// Email Recipients.
			/// </summary>
			public string Recipients {
				get;
				private set;
			}

			#endregion

			#region Constructors: Public

			/// <summary>
			/// Initializes a new instance of the <see cref="Email" />.
			/// </summary>
			/// <param name="registration">Registration instance.</param>
			/// <param name="email">Activity entity.</param>
			public Email(RegistrationFromEmail registration, Entity email) {
				HeaderProperties = email.GetTypedColumnValue<string>("HeaderProperties");
				Body = email.GetTypedColumnValue<string>("Body");
				Title = email.GetTypedColumnValue<string>("Title");
				AccountId = email.GetTypedColumnValue<Guid>("AccountId");
				ContactId = email.GetTypedColumnValue<Guid>("ContactId");
				ActivityId = email.PrimaryColumnValue;
				Recipients = string.Join(";", email.GetTypedColumnValue<string>("Recepient"),
						email.GetTypedColumnValue<string>("CopyRecepient"));
				IsBlacklisted = registration.CheckIsBlacklisted(email);
				AreHeaderPropertiesOk = registration.CheckAllHeaderProperties(HeaderProperties);
			}

			#endregion
		}

		#endregion

		#region Constants: Private

		/// <summary>
		/// Maximum length of the message body default.
		/// </summary>
		private const int MaxDefaultEmailBodyLength = 500;

		/// <summary>
		/// Maximum length of the message body in MB.
		/// </summary>
		private const int MaxDefaultEmailBodySize = 10;

		#endregion

		#region Fields: Private

		private ParticipantInfoProvider _participantInfoProvider;

		private static readonly ILog _log = LogManager.GetLogger("IncidentRegistration");

		private ISupMailBoxLangProvider _supMailBoxLangProvider;

		#endregion

		#region Fields: Protected

		protected bool CreateCasesFromJunkEmails;
		protected Guid JunkCaseDefaultStatus;
		protected Dictionary<string, Guid> EmailLanguageAliases;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref = "IncidentRegistrationFromEmailHelper" />, 
		/// without a specified user connection.
		/// </summary>
		public IncidentRegistrationFromEmailHelper() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref = "IncidentRegistrationFromEmailHelper" />, 
		/// using the specified user connection.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public IncidentRegistrationFromEmailHelper(UserConnection userConnection)
			: base(userConnection) {
			JunkCaseDefaultStatus = SystemSettings.GetValue(UserConnection,
				"JunkCaseDefaultStatus", CaseConsts.CaseStatusNewId);
			CreateCasesFromJunkEmails = SystemSettings.GetValue(UserConnection,
				"CreateCasesFromJunkEmails", false);
			ClassFactory.TryGet<ISupMailBoxLangProvider>("SupMailBoxLang", out _supMailBoxLangProvider,
				new ConstructorArgument("userConnection", UserConnection));
			EmailLanguageAliases = new Dictionary<string, Guid>();
		}

		public IncidentRegistrationFromEmailHelper(UserConnection userConnection, EntityCollection casesByEmail)
			: this(userConnection) {
			CasesByEmail = casesByEmail;
		}

		#endregion

		#region Properties: Private

		private ParticipantInfoProvider PInfoProvider {
			get {
				if (_participantInfoProvider == null) {
					_participantInfoProvider = new ParticipantInfoProvider(UserConnection);
				}
				return _participantInfoProvider;
			}
		}

		#endregion

		#region Properties: Public

		public EntityCollection CasesByEmail {
			get;
			protected set;
		}

		/// <summary>
		/// Case number fetched from email.
		/// </summary>
		public string MatchedCaseNumber {
			get;
			private set;
		}

		public Guid ContactParticipantId {
			get;
			private set;
		}

		public Guid CaseAccountId {
			get;
			private set;
		}

		/// <summary>
		/// Category provider for mail box.
		/// </summary>
		public ICategoryProviderWrapper CategoryProviderWrapper {
			get;
			set;
		}

		public Guid EmailSyncSession {
			get;
			set;
		}

		/// <summary>
		/// Instance of <see cref="CaseBroker"/> type.
		/// </summary>
		private CaseBroker _caseBroker;
		public CaseBroker CaseBroker {
			get => _caseBroker ?? new CaseBroker();
			set => _caseBroker = value;
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Gets the case of message headers containing a number of case.
		/// </summary>
		/// <param name="email">Message</param>
		/// <returns>Case entity.</returns>
		private Entity GetCase(Email email) {
			MatchedCaseNumber = GetCaseNumber(email);
			if (string.IsNullOrEmpty(MatchedCaseNumber)) {
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case number was NOT matched for ActivityId: {1}, EmailTitle: {2}", EmailSyncSession, email.ActivityId, email.Title);
				return null;
			}
			if (CasesByEmail.IsNullOrEmpty()) {
				CasesByEmail = GetCasesCollection(MatchedCaseNumber);
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case number was matched for ActivityId: {1}, EmailTitle: {2}. Try get mathed collection", EmailSyncSession, email.ActivityId, email.Title);
			}
			return CasesByEmail.FirstOrDefault();
		}

		/// <summary>
		/// Get case number.
		/// </summary>
		/// <param name="email">Email entity.</param>
		/// <returns>Return case number.</returns>
		private string GetCaseNumber(Email email) {
			string caseCodeMask = SystemSettings.GetValue<string>(UserConnection, CaseConsts.CaseCodeMaskCode, null);
			bool needParseBody = SystemSettings.GetValue<bool>(UserConnection, "ConnectEmailsByCaseNumberInBody", false)
				&& !IsEmailBodyOversized(email);
			string caseNumber = GetCaseNumberByRegex(email, GetMatchMaskRegex(caseCodeMask), needParseBody);
			if (string.IsNullOrEmpty(caseNumber)) {
				string alternateRegexp = SystemSettings.GetValue<string>(UserConnection, "AltCaseBindingRegex", default(string));
				caseNumber = GetCaseNumberByRegex(email, alternateRegexp, needParseBody);
			}
			return caseNumber;
		}

		/// <summary>
		/// Get case number from email by regular expression.
		/// </summary>
		/// <param name="email">Email entity.</param>
		/// <param name="pattern">Regular expression to search case.</param>
		/// <param name="needParseBody">Need to search in body flag.</param>
		/// <returns>Return case number.</returns>
		private string GetCaseNumberByRegex(Email email, string pattern, bool needParseBody) {
			if (string.IsNullOrWhiteSpace(pattern)) {
				return null;
			}
			ParsePattern(pattern, out Regex searchPattern, out string replacePattern);
			TryGetCaseNumber(email.Title, searchPattern, replacePattern, out string caseNumber);
			if (caseNumber.IsNullOrWhiteSpace() && needParseBody) {
				TryGetCaseNumber(email.Body, searchPattern, replacePattern, out caseNumber);
			}
			return caseNumber.IsNullOrEmpty() ? null : caseNumber;
		}

		private bool IsEmailBodyOversized(Email email) {
			return System.Text.Encoding.Unicode.GetByteCount(email.Body) / 1048576f > MaxDefaultEmailBodySize;
		}

		/// <summary>
		/// Splits alternate regex pattern into different search and replace parts.
		/// </summary>
		/// <param name="pattern">Full alternate regex search pattern.</param>
		/// <param name="searchRegex">Found search pattern.</param>
		/// <param name="replacePattern">Found replace pattern if exists or empty string else.</param>
		private void ParsePattern(string pattern, out Regex searchRegex, out string replacePattern) {
			string[] parts = new Regex(@"(?<=[^\\](\\{2})*)\/").Split(pattern);
			searchRegex = new Regex(parts[0], RegexOptions.IgnoreCase | RegexOptions.Compiled);
			replacePattern = parts.Length == 2 ? parts[1] : "";
		}

		/// <summary>
		/// Tries to find case number in text by regular expression.
		/// </summary>
		/// <param name="source">Source text.</param>
		/// <param name="searchRegex">Regular expression to find.</param>
		/// <param name="replacePattern">Pattern to replace parts by regular expression result.</param>
		/// <param name="caseNumber">Found case number.</param>
		private void TryGetCaseNumber(string source, Regex searchRegex, string replacePattern, out string caseNumber) {
			var caseMatch = searchRegex.Match(source);
			if (!caseMatch.Success) {
				caseNumber = "";
				return;
			}
			if (replacePattern.IsNullOrWhiteSpace()) {
				caseNumber = caseMatch.Value;
			} else {
				caseNumber = replacePattern;
				for (int i = 0; i < caseMatch.Groups.Count; i++) {
					caseNumber = caseNumber.Replace("\\" + i, caseMatch.Groups[i].Value);
				}
			}
		}

		/// <summary>
		/// Get cases collection by filter.
		/// </summary>
		/// <param name="caseNumber">Case number.</param>
		/// <returns>Collection of cases.</returns>
		private EntityCollection GetCasesCollection(string caseNumber) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			esq.AddAllSchemaColumns();
			esq.AddColumn("Status.IsFinal");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Number", caseNumber));
			CasesByEmail = esq.GetEntityCollection(UserConnection);
			return CasesByEmail;
		}

		/// <summary>
		/// Defines the object of activity (emailEntity) by Id.
		/// </summary>
		/// <param name="recordId">Object activity (emailEntity) Id.</param>
		/// <returns>Object activity (emailEntity).</returns>
		private Entity GetCreatedEmail(Guid recordId) {
			var email = UserConnection.EntitySchemaManager.GetInstanceByName("Activity").CreateEntity(UserConnection);
			email.FetchFromDB(recordId);
			return FillEmailContacts(email);
		}

		/// <summary>
		/// Fills the author field, contacts counterparty activities (emailEntity).
		/// </summary>
		/// <param name="email">Object activity (emailEntity).</param>
		/// <returns>Object activity (emailEntity).</returns>
		private Entity FillEmailContacts(Entity email) {
			var participantInfo = PInfoProvider.Get(email.PrimaryColumnValue);
			ContactParticipantId = participantInfo.ContactId;
			CaseAccountId = participantInfo.AccountId;
			var sender = email.GetTypedColumnValue<string>("Sender");
			var emailAddress = Regex.Match(sender, CaseConsts.EmailAddressPattern).Value;
			var emailObject = new Email(this, email);
			bool needCreateNewContacts = SystemSettings.GetValue(UserConnection,
				"CreateNewContactsForUnknownEmailAddresses", true);
			if (ContactParticipantId.Equals(Guid.Empty)) {
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Sender contact was not found from ActivityParticipants. ActivityId: {1}, EmailTitle {2}",
					EmailSyncSession, email.GetTypedColumnValue<Guid>("Id"), email.GetTypedColumnValue<string>("Title"));
				List<Guid> foundContacts = BaseContactUtilities.FindContactsByEmail(emailAddress, UserConnection);
				if (foundContacts.Any()) {
					ContactParticipantId = foundContacts.FirstOrDefault();
					CaseAccountId = GetContactAccount(ContactParticipantId);
					CreateNewParticipant(emailObject.ActivityId, ContactParticipantId);
					return email;
				}
				if (needCreateNewContacts && (!emailObject.IsJunk || CreateCasesFromJunkEmails)) {
					MatchSenderEmailAndEmailLanguage(emailAddress, emailObject.Recipients);
					ContactParticipantId = CreateNewContactByEmail(emailAddress);
					CreateNewParticipant(emailObject.ActivityId, ContactParticipantId);
					if (emailObject.ContactId.Equals(Guid.Empty)) {
						email.SetColumnValue("ContactId", ContactParticipantId);
						email.SetColumnValue("AuthorId", ContactParticipantId);
					}
				}
			}
			return email;
		}

		/// <summary>
		/// Get language id for contact wich will be created and match it with contacts email.
		/// </summary>
		/// <param name="email">Contacts email.</param>
		/// <param name="emailRecipients">Emails recipients.</param>
		private void MatchSenderEmailAndEmailLanguage(string email, string emailRecipients) {
			List<string> recepients = EmailUtils.ParseEmailAddress(emailRecipients);
			if (_supMailBoxLangProvider != null) {
				var languageId = _supMailBoxLangProvider.GetLanguage(recepients);
				if (languageId != Guid.Empty) {
					EmailLanguageAliases.Add(email, languageId);
				}
			}
		}

		/// <summary>
		/// Returns the value of localized strings.
		/// </summary>
		/// <param name="name">Name of localized strings.</param>
		/// <returns>The value of localized strings.</returns>
		private string GetLocalizableStringValue(string name) {
			return new LocalizableString(UserConnection.Workspace.ResourceStorage,
				"IncidentRegistrationFromEmaillHelper", "LocalizableStrings." + name + ".Value").ToString();
		}

		/// <summary>
		/// Creates a new case by emailEntity message.
		/// </summary>
		/// <param name="email">The object of activity (emailEntity).</param>
		/// <returns>The object of activity (emailEntity).</returns>
		private Entity CreateNewCaseByEmail(Email email) {
			var caseEntity = UserConnection.EntitySchemaManager.GetInstanceByName("Case").CreateEntity(UserConnection);
			var filteredEmailBody = ClearHtmlText(email.Body);
			int maxEmailBodyLength = GetMaxEmailBodyLength();
			if (filteredEmailBody.Length > maxEmailBodyLength) {
				if (UserConnection.GetIsFeatureEnabled("SafeEmailBodyCutting")) {
					filteredEmailBody = SafeCutEmailBody(filteredEmailBody, maxEmailBodyLength);
				} else {
					filteredEmailBody = filteredEmailBody.Substring(0, maxEmailBodyLength);
				}
			}
			caseEntity.SetDefColumnValues();
			caseEntity.SetColumnValue("RegisteredOn", UserConnection.CurrentUser.GetCurrentDateTime());
			caseEntity.SetColumnValue("Subject", email.Title);
			caseEntity.SetColumnValue("Symptoms", filteredEmailBody);
			caseEntity.SetColumnValue("OriginId", CaseConsts.CaseOriginDefaultEmailId);
			caseEntity.SetColumnValue("ParentActivityId", email.ActivityId);
			SetDefaultCategory(caseEntity, email);
			Guid statusId;
			if (email.IsJunk && CreateCasesFromJunkEmails) {
				statusId = JunkCaseDefaultStatus;
				caseEntity.SetColumnValue("ClosureCodeId", email.ClosureCode);
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case was registered, but closed by junk filter. ActivityId: {1}, EmailTitle {2}", EmailSyncSession, email.ActivityId, email.Title);
			} else {
				statusId = CaseConsts.CaseStatusNewId;
			}
			caseEntity.SetColumnValue("StatusId", statusId);
			var participantInfo = PInfoProvider.Get(email.ActivityId);
			if (ContactParticipantId.Equals(Guid.Empty)) {
				ContactParticipantId = participantInfo.ContactId;
			}
			if (!ContactParticipantId.Equals(Guid.Empty)) {
				if (CaseAccountId.Equals(Guid.Empty) && !participantInfo.AccountId.Equals(Guid.Empty)) {
					CaseAccountId = participantInfo.AccountId;
				}
				caseEntity.SetColumnValue("ContactId", ContactParticipantId);
				if (!CaseAccountId.Equals(Guid.Empty)) {
					caseEntity.SetColumnValue("AccountId", CaseAccountId);
				}
			}
			caseEntity = GetCaseWithServicePact(caseEntity, ContactParticipantId, CaseAccountId);
			caseEntity.Save(false);
			return caseEntity;
		}
		
		/// <summary>
		/// Method to safe cut email body. If last symbol is indivisible (e.g. emoji), it will be removed at all
		/// </summary>
		/// <param name="content">String content.</param>
		/// <param name="maxLength">Maximum content string length.</param>
		/// <returns>String with required length.</returns>
		private string SafeCutEmailBody(string content, int maxLength) {
			string result = UnicodeSafeSubstring(content, 0, maxLength);
			if (result.Length > maxLength) {
				result = result.Substring(0, result.Length - 2);
			}
			return result;
		}
        
		private string UnicodeSafeSubstring(string str, int startIndex, int length) {
			var sb = new StringBuilder(length);
			var enumerator = StringInfo.GetTextElementEnumerator(str, startIndex);
			while (enumerator.MoveNext()) {
				if (startIndex >= length) {
					break;
				}
				string grapheme = enumerator.GetTextElement();
				startIndex += grapheme.Length;
				if (sb.Length == 0) {
					if (!char.IsLowSurrogate(grapheme[0])) {
						UnicodeCategory cat = char.GetUnicodeCategory(grapheme, 0);
						if (cat == UnicodeCategory.NonSpacingMark || cat == UnicodeCategory.SpacingCombiningMark || cat == UnicodeCategory.EnclosingMark) {
							continue;
						}
					}
				}
				sb.Append(grapheme);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Get activity recipients.
		/// </summary>
		/// <param name="activityId">Activity id.</param>
		/// <returns>Activity recipients, joined in string.</returns>
		private string GetRecipients(Guid activityId) {
			string recipients = string.Empty;
			Select selectQuery =
					new Select(UserConnection)
								.Column("a", "Recepient")
								.Column("a", "CopyRecepient")
								.Column("a", "BlindCopyRecepient")
							.From("Activity").As("a")
							.Where("a", "Id").IsEqual(Column.Const(activityId)) as Select;
			using (DBExecutor dbExec = UserConnection.EnsureDBConnection()) {
				using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
					while (reader.Read()) {
						string recipient = reader.GetColumnValue<string>("Recepient");
						recipients = JoinRecipients(recipients, recipient);
						recipient = reader.GetColumnValue<string>("CopyRecepient");
						recipients = JoinRecipients(recipients, recipient);
						recipient = reader.GetColumnValue<string>("BlindCopyRecepient");
						recipients = JoinRecipients(recipients, recipient);
						_log.InfoFormat(@"Activity recipients {0}", recipients);
					}
				}
			}
			return recipients;
		}

		/// <summary>
		/// Joins recipients into string.
		/// </summary>
		/// <param name="recipients">Recipients string.</param>
		/// <param name="recipient">Recipient, which should be joined.</param>
		/// <returns>Joined recipients string.</returns>
		private string JoinRecipients(string recipients, string recipient) {
			if (!string.IsNullOrEmpty(recipient)) {
				recipients = string.Join(";", recipients, recipient);
			}
			return recipients;
		}

		/// <summary>
		/// Gets the regular expression from a string mask.
		/// </summary>
		/// <param name="incidentMask">Case number mask.</param>
		/// <returns>Regular expression of case number.</returns>
		private string GetMatchMaskRegex(string incidentMask) {
			var regex = new Regex(@"\{[^{}]+\}");
			var text = regex.Match(incidentMask);
			var pattern = incidentMask.Replace(text.Value, @"\d+");
			return pattern;
		}

		/// <summary>
		/// It returns the maximum length of the message body for case.
		/// </summary>
		/// <returns>Maximum length of the message body.</returns>
		private int GetMaxEmailBodyLength() {
			return SystemSettings.GetValue<int>(UserConnection,
				CaseConsts.EmailBodyForCaseMaxLengthCode, MaxDefaultEmailBodyLength);
		}

		/// <summary>
		/// Set category for case.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <param name="email">The object of activity (emailEntity).</param>
		private void SetDefaultCategory(Entity caseEntity, Email email) {
			bool getCategoryFromEmail = SystemSettings.GetValue(UserConnection,
				"DefineCaseCategoryFromMailBox", false);
			Guid defaultCaseCategory = Guid.Empty;
			if (getCategoryFromEmail && UserConnection.GetIsFeatureEnabled("CategoryFromMailBox")) {
				if (CategoryProviderWrapper != null) {
					string recipientsAsString = GetRecipients(email.ActivityId);
					defaultCaseCategory = CategoryProviderWrapper.GetCategoryFromSupportMailBox(recipientsAsString);
				}
			}
			if (defaultCaseCategory.IsEmpty()) {
				defaultCaseCategory = SystemSettings.GetValue(UserConnection,
				CaseConsts.DefaultCaseCategory, Guid.Empty);
			}
			if (defaultCaseCategory.IsNotEmpty()) {
				caseEntity.SetColumnValue("CategoryId", defaultCaseCategory);
			}
			_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case will be created with CategoryId: {1} for ActivityId: {2}, EmailTitle: {3}", EmailSyncSession, defaultCaseCategory, email.ActivityId, email.Title);
		}

		private bool IsNeedCreateNewCase(Entity caseEntity) {
			if (caseEntity == null || (caseEntity.GetTypedColumnValue<bool>("Status_IsFinal")
					&& IsParentCaseFinal(caseEntity.GetTypedColumnValue<Guid>("ParentCaseId")))) {
				return true;
			}
			return false;
		}

		private bool IsParentCaseFinal(Guid caseId) {
			return CaseBroker.IsCaseFinal(caseId, UserConnection);
		}

		private void SetCaseRights(Entity emailEntity, Entity caseEntity) {
			var copyEntityRightsParams = new SysEntityRightsHelper.CopyEntityRightsParams(emailEntity.SchemaName,
				emailEntity.PrimaryColumnValue, caseEntity.SchemaName, caseEntity.PrimaryColumnValue);
			SysEntityRightsHelper.CopyEntityAdministrateByRecordsRights(UserConnection, copyEntityRightsParams);
		}

		private Guid GetContactAccount(Guid contactId) {
			var accountId = Guid.Empty;
			var contactEntitySchema = UserConnection.EntitySchemaManager.GetInstanceByName("Contact");
			Entity contactEntity = contactEntitySchema.CreateEntity(UserConnection);
			var contactEntitySchemaPrimaryColumnName = contactEntitySchema.GetPrimaryColumnName();
			var contactEntitySchemaPrimaryColumn = contactEntitySchema.Columns.GetByName(contactEntitySchemaPrimaryColumnName);
			var columnsToFetch = new EntitySchemaColumn[] {
				contactEntitySchema.Columns.GetByName("Account")
			};
			if (contactEntity.FetchFromDB(contactEntitySchemaPrimaryColumn, contactId, columnsToFetch)) {
				accountId = contactEntity.GetTypedColumnValue<Guid>("AccountId");
			}
			return accountId;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Clears the text sender HTML-markup.
		/// </summary>
		/// <param name="inputString">Text for cleaning.</param>
		/// <returns>Text after cleaning.</returns>
		protected virtual string ClearHtmlText(string inputString) {
			var htmlWithoutImages = Regex.Replace(inputString, @"(<img\/?[^>]+>)", string.Empty, RegexOptions.IgnoreCase);
			var noCss = Regex.Replace(htmlWithoutImages, @"/\*.+?\*/", string.Empty, RegexOptions.Singleline);
			var noFormatting = Regex.Replace(noCss, @"/<!--[\s\S]*?-->/g", string.Empty, RegexOptions.IgnoreCase);
			var noStyle = Regex.Replace(noFormatting, "<style.*?</style>", string.Empty, RegexOptions.Singleline);
			var noScript = Regex.Replace(noStyle, "<script.*?</script>", string.Empty, RegexOptions.Singleline);
			var noHTML = noScript.Replace("\r\n</span>", " </span>");
			noHTML = noHTML.Replace("</span>\r\n", " </span>");
			noHTML = Regex.Replace(noHTML, @"<div>|<li>", "\r\n");
			noHTML = Regex.Replace(noHTML, @"\r\n{2,}", "\r\n");
			noHTML = Regex.Replace(noHTML, @"<[^>]+>| ", string.Empty);
			noHTML = Regex.Replace(noHTML, "<.*?>", string.Empty);
			noHTML = HttpUtility.HtmlDecode(noHTML);
			noHTML = Regex.Replace(noHTML, @"^\s+$[\r\n]*", "\r\n", RegexOptions.Multiline);
			noHTML = Regex.Replace(noHTML, @"<base[^>]*>", string.Empty, RegexOptions.IgnoreCase);
			if (noHTML.StartsWith("\r\n")) {
				noHTML = noHTML.Substring(2, noHTML.Length - 2);
			}
			return noHTML;
		}

		/// <summary>
		/// Registers a new case for an activity or appends it to an existing case.
		/// </summary>
		/// <param name="emailEntity">Activity entity.</param>
		/// <returns>Identifier of created Case.</returns>
		protected Guid InternalRegisterCase(Entity emailEntity) {
			var email = new Email(this, emailEntity);
			var caseId = default(Guid);
			if (email.IsJunk && !CreateCasesFromJunkEmails) {
				_log.InfoFormat(@"Activity marked as junk Email, skip Case registration for ActivityId: {0}, EmailTitle: {1}", email.ActivityId, email.Title);
				return caseId;
			}
			Entity caseEntity = GetCase(email);
			if (IsNeedCreateNewCase(caseEntity)) {
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Create new case by ActivityId: {1}, EmailTitle: {2}", EmailSyncSession, email.ActivityId, email.Title);
				caseEntity = CreateNewCaseByEmail(email);
				SetCaseRights(emailEntity, caseEntity);
				emailEntity.SetColumnValue("ServiceProcessed", true);
				caseId = caseEntity.GetTypedColumnValue<Guid>("Id");
			} else if (caseEntity.GetTypedColumnValue<bool>("Status_IsFinal") == false) {
				caseId = caseEntity.GetTypedColumnValue<Guid>("Id");
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case state is not final. Add caseId {1} to activity with id {2}", EmailSyncSession, caseId, email.ActivityId);
			} else {
				caseId = caseEntity.GetTypedColumnValue<Guid>("ParentCaseId");
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Case state is final. Add parentCaseId {1} to activity with id {2}", EmailSyncSession, caseId, email.ActivityId);
			}
			emailEntity.SetColumnValue("CaseId", caseId);
			emailEntity.SetColumnValue("IsNeedProcess", false);
			_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Update Activity. Set CaseId: {1} to activity with ActivityId: {2}, EmailTitle: {3}", EmailSyncSession, caseId, email.ActivityId, email.Title);
			emailEntity.Save();
			return caseId;
		}

		/// <summary>
		/// Creates new activity participant 
		/// </summary>
		/// <param name="activityId">Id of activity</param>
		/// <param name="contactParticipantId">Id of contact</param>
		protected void CreateNewParticipant(Guid activityId, Guid contactParticipantId) {
			EntitySchema activityParticipantSchema = UserConnection.EntitySchemaManager
				.GetInstanceByName("ActivityParticipant");
			Entity activityParticipant = activityParticipantSchema.CreateEntity(UserConnection);
			activityParticipant.SetDefColumnValues();
			activityParticipant.SetColumnValue("ActivityId", activityId);
			activityParticipant.SetColumnValue("ParticipantId", contactParticipantId);
			activityParticipant.SetColumnValue("RoleId", ActivityConsts.ActivityParticipantRoleFrom);
			activityParticipant.SetColumnValue("ReadMark", false);
			activityParticipant.SetColumnValue("InviteParticipant", true);
			activityParticipant.SetColumnValue("InviteResponseId", ActivityConsts.ParticipantResponseInDoubtId);
			activityParticipant.Save();
		}

		/// <summary>
		/// Gets the handling chosen by the counterparty contact and service contract.
		/// </summary>
		/// <param name="caseEntity">Object Case.</param>
		/// <param name="contactId">contact Id.</param>
		/// <param name="accountId">account Id.</param>
		/// <returns>Object Case.</returns>
		protected virtual Entity GetCaseWithServicePact(Entity caseEntity, Guid contactId, Guid accountId) {
			return caseEntity;
		}

		/// <summary>
		/// Creates a new contact, indicating e-mail.
		/// </summary>
		/// <param name="email">Contact email.</param>
		/// <returns>Created contact identifier.</returns>
		protected virtual Guid CreateNewContactByEmail(string email) {
			EntitySchema contactSchema = UserConnection.EntitySchemaManager.GetInstanceByName("Contact");
			Entity contact = contactSchema.CreateEntity(UserConnection);
			var id = Guid.NewGuid();
			var name = string.Format("{0} ({1})", GetLocalizableStringValue("NewContactNameLocalizableString"), email);
			contact.SetDefColumnValues();
			if (EmailLanguageAliases.ContainsKey(email)) {
				var languageId = EmailLanguageAliases[email];
				contact.SetColumnValue("LanguageId", languageId);
			}
			contact.SetColumnValue("Id", id);
			contact.SetColumnValue("Name", name);
			contact.SetColumnValue("Email", email);
			contact.Save(false);
			return id;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Creates a new case by emailEntity message.
		/// </summary>
		/// <param name="emailEntity">The object of activity (emailEntity).</param>
		/// <returns>The object of activity (emailEntity).</returns>
		public Entity CreateNewIncidentByEmail(Entity emailEntity) {
			var email = new Email(this, emailEntity);
			var caseEntity = CreateNewCaseByEmail(email);
			var caseId = caseEntity.GetTypedColumnValue<Guid>("Id");
			SetCaseRights(emailEntity, caseEntity);
			emailEntity.SetColumnValue("ServiceProcessed", true);
			emailEntity.SetColumnValue("CaseId", caseId);
			emailEntity.SetColumnValue("IsNeedProcess", false);
			_log.InfoFormat(@"[EmailSyncSessionId:{0}]| Update Activity. Set CaseId: {1} to activity with ActivityId: {2}, EmailTitle: {3}", EmailSyncSession, caseId, email.ActivityId, email.Title);
			emailEntity.Save();
			return caseEntity;
		}

		/// <summary>
		/// Registers a new case for an emailEntity or an emailEntity to an existing ties case.
		/// </summary>
		/// <param name="emailId">activity Id.</param>
		[Obsolete("Use RegisterIncident instead.")]
		public virtual void RegisterCase(Guid emailId) {
			Entity emailEntity = GetCreatedEmail(emailId);
			RegisterCase(emailEntity);
		}

		/// <summary>
		/// Registers a new case for an activity or appends it to an existing case.
		/// </summary>
		/// <param name="emailEntity">Activity entity.</param>
		public virtual void RegisterCase(Entity emailEntity) {
			InternalRegisterCase(emailEntity);
		}

		/// <summary>
		/// Registers a new case for an activity or appends it to an existing case.
		/// </summary>
		/// <param name="emailId">Activity identifier.</param>
		public virtual void RegisterIncident(Guid emailId) {
			GetRegisterIncidentId(emailId);
		}

		/// <summary>
		/// Registers a new case for an activity or appends it to an existing case.
		/// </summary>
		/// <param name="emailId">Activity identifier.</param>
		/// <returns>Identifier of registered Case.</returns>
		public virtual Guid GetRegisterIncidentId(Guid emailId) {
			Guid createdCaseId = default(Guid);
			Stopwatch watch = Stopwatch.StartNew();
			try {
				Entity emailEntity = GetCreatedEmail(emailId);
				createdCaseId = InternalRegisterCase(emailEntity);
				watch.Stop();
				_log.DebugFormat(@"[EmailSyncSessionId:{0}]|Time for case registration {1} ActivityId {2}", EmailSyncSession, watch.Elapsed, emailId);
			} catch (Exception ex) {
				_log.ErrorFormat(@"[EmailSyncSessionId:{0}]| Message: {1}, CallStack: {2}, InnerException: {3}", EmailSyncSession, ex.Message, ex.StackTrace, ex.InnerException);
			} finally {
				if (watch.IsRunning) {
					watch.Stop();
				}
			}
			_log.DebugFormat(@"[EmailSyncSessionId:{0}]|Case has been registered with id: {1}, ActivityId: {2}", EmailSyncSession, createdCaseId, emailId);
			return createdCaseId;
		}

		#endregion

	}

	#endregion

}

