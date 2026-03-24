namespace Terrasoft.Configuration
{
	using global::Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Creatio.FeatureToggling;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Configuration.CaseService;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: SendEmailToCaseOnStatusChange

	/// <summary>
	/// Class represents process of send Email notification to Case Contact.
	/// </summary>
	public class SendEmailToCaseOnStatusChange
	{

		#region Constants: Private

		private const string StatusIsResolvedColumnName = "IsResolved";
		private const string StatusIsFinalColumnName = "IsFinal";
		private const string OwnerIdColumnName = "OwnerId";
		private const string ContactIdColumnName = "ContactId";
		private const string StatusIdColumnName = "StatusId";
		private const string CategoryColumnName = "CategoryId";
		private readonly Guid _closureStatusId = Guid.Parse("3E7F420C-F46B-1410-FC9A-0050BA5D6C38");
		private static readonly ILog _log = LogManager.GetLogger("EmailCases");
		private readonly Guid _cancelStatusId = Guid.Parse("6E5F4218-F46B-1410-FE9A-0050BA5D6C38");

		#endregion

		#region Properties: Protected

		protected Dictionary<string, string> ColumnAliases;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Instance of <see cref="UserConnection"/> type.
		/// </summary>
		public UserConnection UserConnection {
			get; private set;
		}

		/// <summary>
		/// Case identifier.
		/// </summary>
		public Guid CaseId {
			get; private set;
		}

		private DelayedNotificationWorker _delayedNotificationWorker;

		/// <summary>
		/// Worker for delayed notifications.
		/// instance of the <see cref="DelayedNotificationWorker"/> class
		/// </summary>
		public DelayedNotificationWorker DelayedNotificationWorker {
			get => _delayedNotificationWorker ?? new DelayedNotificationWorker(UserConnection);
			set => _delayedNotificationWorker = value;
		}

		private CaseBroker _caseBroker;

		/// <summary>
		/// Broker for working with case entity.
		/// instance of the <see cref="CaseBroker"/> class
		/// </summary>
		public CaseBroker CaseBroker {
			get => _caseBroker ?? new CaseBroker();
			set => _caseBroker = value;
		}

		private EmailWithMacrosManagerBase _emailWithMacrosManager;

		/// <summary>
		/// E-mail sender which allows to control the flow of activity creating, filling and macroses processing.
		/// Instance of the <see cref="EmailWithMacrosManager"/> class.
		/// </summary>
		public EmailWithMacrosManagerBase EmailWithMacrosManager {
			get => _emailWithMacrosManager ?? new EmailWithMacrosManagerBase(UserConnection);
			set => _emailWithMacrosManager = value;
		}

		private ExtendedEmailWithMacrosManagerBase _extendedEmailWithMacrosManager;

		/// <summary>
		/// E-mail sender which allows to control the flow of activity creating, filling and macroses processing.
		/// instance of the <see cref="EmailWithMacrosManager"/> class
		/// </summary>
		public ExtendedEmailWithMacrosManagerBase ExtendedEmailWithMacrosManager {
			get =>  _extendedEmailWithMacrosManager ?? new ExtendedEmailWithMacrosManagerBase(UserConnection);
			set => _extendedEmailWithMacrosManager = value;
		}

		private CaseEmailTemplateExtractor _emailTemplateExtractor;

		/// <summary>
		/// Converter for EmailTemplate macroses.
		/// instance of the <see cref="CaseEmailTemplateExtractor"/> class
		/// </summary>
		public CaseEmailTemplateExtractor EmailTemplateExtractor {
			get => _emailTemplateExtractor ?? new CaseEmailTemplateExtractor(UserConnection, CaseId);
			set => _emailTemplateExtractor = value;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="SendEmailToCaseOnStatusChange"/> type.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="caseId">Case record identifier.</param>
		public SendEmailToCaseOnStatusChange(UserConnection userConnection, Guid caseId) {
			UserConnection = userConnection;
			CaseId = caseId;
			ColumnAliases = new Dictionary<string, string>();
		}

		#endregion

		#region Methods: Private

		private Entity LoadCase(Guid caseId) {
			var caseEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			caseEsq.PrimaryQueryColumn.IsAlwaysSelect = true;
			ColumnAliases[StatusIdColumnName] = caseEsq.AddColumn("Status").Name;
			ColumnAliases[CategoryColumnName] = caseEsq.AddColumn("Category").Name;
			ColumnAliases[StatusIsResolvedColumnName] = caseEsq.AddColumn("Status."
				+ StatusIsResolvedColumnName).Name;
			ColumnAliases[StatusIsFinalColumnName] = caseEsq.AddColumn("Status."
				+ StatusIsFinalColumnName).Name;
			ColumnAliases[ContactIdColumnName] = caseEsq.AddColumn("Contact").Name;
			ColumnAliases[OwnerIdColumnName] = caseEsq.AddColumn("Owner").Name;
			var primaryKeyFilter = caseEsq.CreateFilterWithParameters(FilterComparisonType.Equal,
				caseEsq.RootSchema.GetPrimaryColumnName(), caseId);
			caseEsq.Filters.Add(primaryKeyFilter);
			var caseCollection = caseEsq.GetEntityCollection(UserConnection);
			return caseCollection.FirstOrDefault();
		}

		private EmailWithMacrosManagerBase GetMacrosManager(bool IsQuoteOriginalEmail) {
			return IsQuoteOriginalEmail ? ExtendedEmailWithMacrosManager : EmailWithMacrosManager;
		}

		private bool ClosingCondition(Entity caseEntity) {
			return caseEntity.GetTypedColumnValue<Guid>(ContactIdColumnName)
					== caseEntity.GetTypedColumnValue<Guid>(OwnerIdColumnName);
		}

		private EntityCollection GetTemplateCollection(Entity caseEntity) {
			var templateEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "CaseNotificationRule");
			templateEsq.AddColumn("EmailTemplate");
			templateEsq.AddColumn("RuleUsage");
			templateEsq.AddColumn("Delay");
			templateEsq.AddColumn("IsQuoteOriginalEmail");
			templateEsq.Filters.Add(templateEsq.CreateFilterWithParameters(FilterComparisonType.NotEqual,
				"RuleUsage", CaseConsts.DisableSendUsageType));
			var statusId = caseEntity.GetTypedColumnValue<Guid>(StatusIdColumnName);
			var categoryId = caseEntity.GetTypedColumnValue<Guid>(CategoryColumnName);
			templateEsq.Filters.Add(templateEsq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"CaseStatus", statusId));
			templateEsq.Filters.Add(templateEsq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"CaseCategory", categoryId));
			var templateCollection = templateEsq.GetEntityCollection(UserConnection);
			return templateCollection;
		}

		private void SendImmidiateNotification(EntityCollection templateCollection) {
			var immediateNotification = templateCollection.Where(rule =>
				rule.GetTypedColumnValue<Guid>("RuleUsageId") == CaseConsts.ImmediateSendUsageType);
			EmailWithMacrosManagerBase macrosManager;
			foreach (var notification in immediateNotification) {
				var tplId = notification.GetTypedColumnValue<Guid>("EmailTemplateId");
				try {
					macrosManager = GetMacrosManager(notification.GetTypedColumnValue<bool>("IsQuoteOriginalEmail"));
					macrosManager.SendEmail(CaseId, tplId);
				} catch (Exception ex) {
					_log.ErrorFormat("An error occured while sending email with CaseId: {0}," +
						" templateId: {1}, Message: {2}, CallStack: {3}",
						CaseId, tplId, ex.Message, ex.StackTrace);
				}
			}
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets current state of EmailMessageMultiLanguageV2 feature.
		/// </summary>
		/// <returns>true, if feature is enabled.</returns>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		protected bool IsMultiLanguageV2Enabled() {
			return Features.GetIsEnabled("EmailMessageMultiLanguageV2");
		}

		/// <summary>
		/// Gets current state of EmailMessageMultiLanguage feature.
		/// </summary>
		/// <returns>true, if feature is enabled.</returns>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		protected bool IsMultiLanguageEnabled() {
			return Features.GetIsEnabled("EmailMessageMultiLanguage");
		}

		/// <summary>
		/// Gets current state of DelayedNotification feature.
		/// </summary>
		/// <returns>true, if feature is enabled.</returns>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		protected bool IsDelayedNotificationEnabled() {
			return Features.GetIsEnabled("DelayedNotification");
		}

		/// <summary>
		/// Clear delayed notifications and close case if it is need to be closed.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		protected virtual bool ClearDelayedNotification(Entity caseEntity) {
			if (!caseEntity.GetTypedColumnValue<bool>(ColumnAliases[StatusIsResolvedColumnName])) {
				DelayedNotificationWorker.ClearSendDateForResolutionTemplate(CaseId);
			} else {
				var isClosed = CaseBroker.CloseOnCondition(caseEntity, ClosingCondition, true);
				if (isClosed) {
					return true;
				}
			}
			if (IsDelayedNotificationEnabled() &&
				caseEntity.GetTypedColumnValue<bool>(ColumnAliases[StatusIsFinalColumnName]) &&
				caseEntity.GetTypedColumnValue<Guid>(StatusIdColumnName) != _cancelStatusId) {
				DelayedNotificationWorker.SendAllDelayedEmails(CaseId);
			} else if (IsDelayedNotificationEnabled()) {
				DelayedNotificationWorker.DeleteAllDelayedEmails(CaseId);
			}
			return false;
		}

		/// <summary>
		/// Send notification to Case Contact.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		protected virtual void NotificationProcess(Entity caseEntity) {
			var emailTemplateCollection = GetTemplateCollection(caseEntity);
			if (!IsMultiLanguageV2Enabled() && IsMultiLanguageEnabled()) {
				emailTemplateCollection = EmailTemplateExtractor
					.GetMultilangTplListFromTplList(emailTemplateCollection);
			}
			if (IsDelayedNotificationEnabled()) {
				DelayedNotificationWorker.CreateDelayedNotification(emailTemplateCollection, CaseId);
			}
			SendImmidiateNotification(emailTemplateCollection);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Run execution notification Case process.
		/// </summary>
		public virtual void Run() {
			Entity caseEntity = LoadCase(CaseId);
			if (caseEntity == null) {
				return;
			}
			var caseClosed = ClearDelayedNotification(caseEntity);
			if (caseClosed) {
				return;
			}
			NotificationProcess(caseEntity);
		}

		#endregion

	}

	#endregion
}

