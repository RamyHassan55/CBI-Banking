namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Core;
	using Core.Entities;
	using Core.Factories;
	using Mail.Sender;
	using System.Text;
	using Creatio.FeatureToggling;

	#region Class: BaseEmailWithMacrosManager

	/// <summary>
	/// Base e-mail sender which allows to control the flow of activity creating, filling and macroses processing.
	/// </summary>
	/// <typeparam name="TMacrosProcessor">Type of macros processor.</typeparam>
	public abstract class CrtBaseEmailWithMacrosManager<TMacrosProcessor>
		where TMacrosProcessor : Utils.MacrosHelperV2, new()
	{

		#region Constants: Private

		/// <summary>
		/// Email template entity schema name.
		/// </summary>
		private const string _emailTemplateSchemaName = "EmailTemplate";

		#endregion

		#region Fields: Private

		/// <summary>
		/// Cached templates.
		/// </summary>
		private readonly Dictionary<Guid, Entity> _cachedTemplates = new Dictionary<Guid, Entity>();

		#endregion

		#region Properties: Public

		/// <summary>
		/// The map of replacements.
		/// It may be used to provide the backward macros compatibility or to replace any part of the e-mail text data.
		/// </summary>
		public Dictionary<string, string> ReplacementMap {
			protected get;
			set;
		}

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
			private set;
		}

		private MLangContentFactory _contentFactory;
		/// <summary>
		/// Multilanguage content factory.
		/// </summary>
		public MLangContentFactory MLangContentFactory {
			get => _contentFactory ?? (_contentFactory = new MLangContentFactory(UserConnection));
			set => _contentFactory = value;
		}

		private CaseTimeZoneMacrosConverter _converter;
		/// <summary>
		/// Case time zone converter.
		/// </summary>
		public CaseTimeZoneMacrosConverter TimeZoneConverter {
			get => _converter ?? (_converter = new CaseTimeZoneMacrosConverter(UserConnection, "Case"));
		}

		#endregion

		#region Constructors: Protected

		/// <summary>
		/// Initializes a new instance of the <see cref="CrtBaseEmailWithMacrosManager"/> class.
		/// </summary>
		protected CrtBaseEmailWithMacrosManager() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CrtBaseEmailWithMacrosManager"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		protected CrtBaseEmailWithMacrosManager(UserConnection userConnection)
			: this() {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private string GetAdditionalHeaders() {
			var emailHeaders = GetPropertiesList();
			var headerProperties = new StringBuilder();
			foreach (var emailHeader in emailHeaders) {
				headerProperties.Append(emailHeader.Name);
				headerProperties.Append("=");
				headerProperties.Append(emailHeader.Value);
				headerProperties.Append("\n");
			}
			return headerProperties.ToString();
		}

		private bool IsNeedChangeMacrosCulture() {
			return Features.GetIsEnabled("UseMacrosAdditionalParameters")
				&& Features.GetIsEnabled("EmailMessageMultiLanguageV2");
		}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		private bool IsHideAutoEmailNotificationsEnabled() {
			return Features.GetIsEnabled("HideAutoEmailNotifications");
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets e-mail template entity schema name.
		/// </summary>
		/// <returns>E-mail template entity schema name.</returns>
		protected abstract string GetEmailTemplateSchemaName();

		/// <summary>
		/// Applies replacements to the given string <paramref name="source"/> by the <see cref="ReplacementMap"/>.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <returns>Source with replacements.</returns>
		protected string ApplyReplacements(string source) {
			string result = source;
			foreach (var oldMacros in ReplacementMap.Keys) {
				var newMacros = ReplacementMap[oldMacros];
				result = result.Replace(oldMacros, newMacros);
			}
			return result;
		}

		/// <summary>
		/// Gets template entity by given template record identifier from the <see cref="GetEmailTemplateSchemaName"/>.
		/// </summary>
		/// <remarks>Uses caching.</remarks>
		/// <param name="id">Template record identifier.</param>
		/// <returns>Template entity.</returns>
		protected Entity GetTemplate(Guid id) {
			Entity template;
			if (_cachedTemplates.ContainsKey(id)) {
				template = _cachedTemplates[id];
			} else {
				var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, GetEmailTemplateSchemaName());
				esq.AddColumn("Subject");
				esq.AddColumn("Body");
				esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", id));
				template = esq.GetEntity(UserConnection, id);
				_cachedTemplates.Add(id, template);
			}
			return template;
		}

		/// <summary>
		/// Gets template entity by given template record identifier and entity record identifier.
		/// </summary>
		/// <param name="templateId">Template record identifier.</param>
		/// <param name="recordId">Entity record identifier.</param>
		/// <param name="schemaName">Tag name to get an iterator.</param>
		/// <returns>Template entity.</returns>
		protected Entity GetTemplateByRecordId(Guid templateId, Guid recordId, string schemaName) {
			var contentKit = MLangContentFactory.GetContentKit(schemaName, _emailTemplateSchemaName);
			Entity template = contentKit.GetContent(templateId, recordId);
			return template;
		}

		private TMacrosProcessor _macrosProcessor;
		/// <summary>
		/// Macros processor.
		/// </summary>
		/// <remarks>Uses lazy loading.</remarks>
		protected TMacrosProcessor MacrosProcessor {
			get {
				return _macrosProcessor ??
						(_macrosProcessor = new TMacrosProcessor { UserConnection = UserConnection });
			}
		}

		/// <summary>
		/// Creates additional headers for activity.
		/// </summary>
		protected virtual List<EmailMessageHeader> GetPropertiesList() {
			return new List<EmailMessageHeader> { new EmailMessageHeader { Name = "Auto-Submitted",
				Value = "auto-replied" } };
		}

		/// <summary>
		/// Creates an activity and fills it with default values.
		/// </summary>
		/// <returns>Activity entity.</returns>
		protected virtual Entity CreateActivityEntity() {
			var activitySchema = UserConnection.EntitySchemaManager.GetInstanceByName("Activity");
			var activity = activitySchema.CreateEntity(UserConnection);
			activity.SetDefColumnValues();
			activity.SetColumnValue("Id" ,Guid.NewGuid());
			activity.SetColumnValue("OwnerId", UserConnection.CurrentUser.ContactId);
			activity.SetColumnValue("TypeId", ActivityConsts.EmailTypeUId);
			activity.SetColumnValue("MessageTypeId", ActivityConsts.OutgoingEmailTypeId);
			DateTime now = UserConnection.CurrentUser.GetCurrentDateTime();
			activity.SetColumnValue("StartDate", now);
			activity.SetColumnValue("DueDate", now);
			activity.SetColumnValue("ActivityCategoryId", ActivityConsts.EmailActivityCategoryId);
			activity.SetColumnValue("IsHtmlBody", true);
			activity.SetColumnValue("EmailSendStatusId", ActivityConsts.InProgressEmailStatusId);
			string headerProperties = GetAdditionalHeaders();
			if (IsHideAutoEmailNotificationsEnabled()) {
				activity.SetColumnValue("IsAutoSubmitted",
					headerProperties.IndexOf("Auto-Submitted", 0, StringComparison.CurrentCultureIgnoreCase) != -1);
			}
			activity.SetColumnValue("HeaderProperties", headerProperties);
			return activity;
		}

		/// <summary>
		/// Gets template body and processes macroses after replacements is done.
		/// </summary>
		/// <param name="schemaName">Entity schema name.</param>
		/// <param name="recordId">Entity record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		/// <returns>Template body.</returns>
		protected virtual string GetTemplateBody(string schemaName, Guid recordId, Guid tplId) {
			Entity template = null;
			string body = string.Empty;
			if (Features.GetIsEnabled("EmailMessageMultiLanguageV2")) {
				template = GetTemplateByRecordId(tplId, recordId, schemaName);
				body = TimeZoneConverter.ReplaceDateTimeMacroses(recordId,
					template.GetTypedColumnValue<string>("Body"),
					template.GetTypedColumnValue<string>("LanguageCode"));
			} else {
				template = GetTemplate(tplId);
				body = TimeZoneConverter.ReplaceDateTimeMacroses(recordId, template.GetTypedColumnValue<string>("Body"));
			}
			body = ApplyReplacements(body);
			if (IsNeedChangeMacrosCulture() && MacrosProcessor is GlobalMacrosHelper) {
				return (MacrosProcessor as GlobalMacrosHelper).GetTextTemplate(body, schemaName, recordId,
					new MacrosExtendedProperties { LanguageId = template.GetTypedColumnValue<Guid>("LanguageId") });
			}
			return MacrosProcessor.GetTextTemplate(body, schemaName, recordId);
		}

		/// <summary>
		/// Gets template subject and processes macroses after replacements is done.
		/// </summary>
		/// <param name="schemaName">Entity schema name.</param>
		/// <param name="recordId">Entity record identifier.</param>
		/// <param name="tplId">Template record identifier.</param>
		/// <returns>Template subject.</returns>
		protected virtual string GetTemplateSubject(string schemaName, Guid recordId, Guid tplId) {
			var template = Features.GetIsEnabled("EmailMessageMultiLanguageV2")
				? GetTemplateByRecordId(tplId, recordId, schemaName)
				: GetTemplate(tplId);
			string title = ApplyReplacements(template.GetTypedColumnValue<string>("Subject"));
			if (IsNeedChangeMacrosCulture() && MacrosProcessor is GlobalMacrosHelper) {
				return (MacrosProcessor as GlobalMacrosHelper).GetPlainTextTemplate(title, schemaName, recordId,
					new MacrosExtendedProperties { LanguageId = template.GetTypedColumnValue<Guid>("LanguageId") });
			}
			return MacrosProcessor.GetPlainTextTemplate(title, schemaName, recordId);
		}

		/// <summary>
		/// Instantiates activity e-mail sender then sends e-mail.
		/// </summary>
		/// <param name="id">Sending activity record identifier.</param>
		/// <remarks>
		/// It uses no custom logic, so may be excluded from the code coverage of this class.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		protected virtual void SendActivity(Guid id) {
			var userConnectionArg = new ConstructorArgument("userConnection", UserConnection);
			var emailClientFactory = ClassFactory.Get<EmailClientFactory>(userConnectionArg);
			var sender = new ActivityEmailSenderBase(emailClientFactory, UserConnection);
			sender.Send(id);
		}

		#endregion

	}

	#endregion

} 
