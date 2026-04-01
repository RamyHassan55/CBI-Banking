namespace Terrasoft.Configuration.CaseService
{
	using Creatio.FeatureToggling;
	using global::Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: ReopenCaseAndNotifyAssignee

	/// <summary>
	/// Class represents process of reopening case on conditions.
	/// </summary>
	public class ReopenCaseAndNotifyAssignee
	{

		#region Constants: Private

		private const string DefaultNotifySubject = "New email received regarding the case No.{0}";
		private const string OwnerIdColumnName = "OwnerId";
		private const string StatusIdColumnName = "StatusId";
		private const string NumberColumnName = "Number";
		private const string NotifySubjectLczName = "NotifySubject";

		#endregion

		#region Fields: Private

		private static readonly ILog _log = LogManager.GetLogger("CaseMessageListener");

		#endregion

		#region Properties: Protected

		protected Dictionary<string, string> ColumnAliases;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Instance of <see cref="UserConnection"/> type.
		/// </summary>
		public UserConnection UserConnection { get; private set; }

		private string _notifySubject;
		/// <summary>
		/// Notification subject.
		/// </summary>
		public string NotifySubject {
			get {
				if (string.IsNullOrEmpty(_notifySubject)) {
					_notifySubject = GetLocalizableStringValue(UserConnection, NotifySubjectLczName);
				}
				return _notifySubject;
			}
			set { _notifySubject = value; }
		}

		private CaseStatusStore _caseStatusStore;
		/// <summary>
		/// Instance of <see cref="CaseStatusStore"/> type.
		/// </summary>
		public CaseStatusStore CaseStatusStore {
			get => _caseStatusStore ?? (_caseStatusStore = new CaseStatusStore(UserConnection));
			set => _caseStatusStore = value;
		}

		private IncidentRegistrationFromEmailHelper _registrationHelper;
		/// <summary>
		/// Instance of <see cref="IncidentRegistrationFromEmailHelper"/> type.
		/// </summary>
		public IncidentRegistrationFromEmailHelper RegistrationHelper {
			get => _registrationHelper ?? InitRegistrationHelper();
			set => _registrationHelper = value;
		}

		private EmailSenderObtainerWrapper _senderObtainer;
		/// <summary>
		/// Instance of <see cref="EmailSenderObtainerWrapper"/> type.
		/// </summary>
		public EmailSenderObtainerWrapper SenderObtainer {
			get => _senderObtainer ?? (_senderObtainer = new EmailSenderObtainerWrapper(UserConnection));
			set => _senderObtainer = value;
		}

		/// <summary>
		/// Uniqueidentifier of activity record.
		/// </summary>
		public Guid ActivityId { get; set; }

		/// <summary>
		/// Uniqueidentifier of case record.
		/// </summary>
		public Guid CaseId { get; set; }

		/// <summary>
		/// Count of reopened cases.
		/// </summary>
		public int ReopenedCount { get; private set; }

		/// <summary>
		/// Instance of <see cref="CaseBroker"/> type.
		/// </summary>
		private CaseBroker _caseBroker;
		public CaseBroker CaseBroker {
			get { return _caseBroker ?? (_caseBroker = new CaseBroker()); }
			set { _caseBroker = value; }
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="ReopenCaseAndNotifyAssignee"/> type.
		/// </summary>
		public ReopenCaseAndNotifyAssignee(UserConnection userConnection) {
			UserConnection = userConnection;
			ColumnAliases = new Dictionary<string, string>();
		}

		#endregion

		#region Methods: Private

		private IncidentRegistrationFromEmailHelper InitRegistrationHelper() {
			_registrationHelper = ClassFactory.Get<IncidentRegistrationFromEmailHelper>(
					 new ConstructorArgument("userConnection", UserConnection));
			_registrationHelper.CategoryProviderWrapper = new CategoryProviderWrapper(UserConnection) {
				CategoryProvider = new CategoryProvider(new CategoryFromSupportMailBox(UserConnection))
			};
			return _registrationHelper;
		}

		private static string GetLocalizableStringValue(UserConnection userConnection, string lczName) {
			return new LocalizableString(userConnection.Workspace.ResourceStorage, "ReopenCaseAndNotifyAssignee",
				"LocalizableStrings." + lczName + ".Value").ToString();
		}

		private void NotifyOwner(Entity caseEntity) {
			Entity remindingEntity = UserConnection.EntitySchemaManager.GetInstanceByName("Reminding")
				.CreateEntity(UserConnection);
			var caseId = CaseId;
			var ownerId = caseEntity.GetTypedColumnValue<Guid>(OwnerIdColumnName);
			var number = caseEntity.GetTypedColumnValue<string>(ColumnAliases[NumberColumnName]);
			var condition = new Dictionary<string, object> {
					{
						"Author", UserConnection.CurrentUser.ContactId
					}, {
						"Contact", ownerId
					}, {
						"Source", RemindingConsts.RemindingSourceAuthorId
					}, {
						"Number", number
					}, {
						"ActivityId", ActivityId
					}
				};
			var str = new StringBuilder();
			foreach (object value in condition.Values) {
				str.Append(value);
			}
			string hash = FileUtilities.GetMD5Hash(Encoding.Unicode.GetBytes(str.ToString()));
			remindingEntity.SetDefColumnValues();
			remindingEntity.SetColumnValue("AuthorId", UserConnection.CurrentUser.ContactId);
			remindingEntity.SetColumnValue("ContactId", ownerId);
			remindingEntity.SetColumnValue("SourceId", RemindingConsts.RemindingSourceAuthorId);
			remindingEntity.SetColumnValue("RemindTime", UserConnection.CurrentUser.GetCurrentDateTime());
			var subjectCaption = string.Format(!string.IsNullOrEmpty(NotifySubject)
					? NotifySubject
					: DefaultNotifySubject,
					number
				);
			remindingEntity.SetColumnValue("SubjectCaption", subjectCaption);
			var caseSchema = UserConnection.EntitySchemaManager.GetInstanceByName("Case");
			remindingEntity.SetColumnValue("SysEntitySchemaId", caseSchema.UId);
			remindingEntity.SetColumnValue("SubjectId", caseId);
			remindingEntity.SetColumnValue("Hash", hash);
			remindingEntity.Save();
			_log.InfoFormat($"[CaseId: {CaseId}] Notification was sent to Contact with ContactId = {ownerId}");
		}

		private void UpdateActivityProcessed(Guid activityId) {
			var activityUpdate = new Update(UserConnection, "Activity")
				.Set("ServiceProcessed", Column.Parameter(true))
				.Where("Id").IsEqual(Column.Parameter(activityId)) as Update;
			activityUpdate.Execute();
		}

		private Entity LoadCase(Guid caseId, Dictionary<string, string> columnAlieses) {
			var caseEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			caseEsq.PrimaryQueryColumn.IsAlwaysSelect = true;
			caseEsq.AddColumn("Owner");
			caseEsq.AddColumn("Status");
			caseEsq.AddColumn("ParentActivity");
			columnAlieses[NumberColumnName] = caseEsq.AddColumn(NumberColumnName).Name;
			var primaryKeyFilter = caseEsq.CreateFilterWithParameters(FilterComparisonType.Equal,
				caseEsq.RootSchema.GetPrimaryColumnName(), caseId);
			caseEsq.Filters.Add(primaryKeyFilter);
			var caseCollection = caseEsq.GetEntityCollection(UserConnection);
			return caseCollection.FirstOrDefault();
		}

		private string GetSupportMailBoxFromActivity(Guid activityId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Activity");
			var recepientColumnName = esq.AddColumn("Recepient").Name;
			var copyRecepientColumnName = esq.AddColumn("CopyRecepient").Name;
			var caseContactColumnName = esq.AddColumn("Case.Contact.Id").Name;
			var caseCategoryNameColumnName = esq.AddColumn("Case.Category.Id").Name;
			var activty = esq.GetEntity(UserConnection, activityId);
			return SenderObtainer.GetSupportMailBox(
					new List<string> {
						activty.GetTypedColumnValue<string>(recepientColumnName),
						activty.GetTypedColumnValue<string>(copyRecepientColumnName)
					},
					activty.GetTypedColumnValue<Guid>(caseContactColumnName),
					activty.GetTypedColumnValue<Guid>(caseCategoryNameColumnName));		 
		}

		private Entity LoadActivity(Guid activityId) {
			Entity entity = UserConnection.EntitySchemaManager.GetInstanceByName("Activity")
				.CreateEntity(UserConnection);
			entity.FetchFromDB("Id", activityId);
			return entity;
		}

		private bool IsNeedToCreateNewCase(Entity caseEntity) {
			var parentActivityId = caseEntity.GetTypedColumnValue<Guid>("ParentActivityId");
			return Features.GetIsEnabled("CreateNewCaseInsteadOfReopenWhenRecipientChange") &&
				ActivityId != default &&
				parentActivityId != default &&
				IsRecipientChange(caseEntity);
		}

		private bool IsRecipientChange(Entity caseEntity) {
			var supportEmailFromCurrentEmail = GetSupportMailBoxFromActivity(ActivityId);
			var supportEmailFromParentCaseEmail = GetSupportMailBoxFromActivity(caseEntity.GetTypedColumnValue<Guid>("ParentActivityId"));
			return string.Compare(supportEmailFromCurrentEmail, supportEmailFromParentCaseEmail, StringComparison.OrdinalIgnoreCase) != 0;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Checks if case status is reopen.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <returns><c>true</c> if case status is reopen.</returns>
		protected bool IsReopenStatus(Entity caseEntity) {
			return CaseStatusStore.StatusIsReopened(caseEntity.GetTypedColumnValue<Guid>(StatusIdColumnName));
		}

		/// <summary>
		/// Checks if case status is resolved or paused.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <returns><c>true</c> if case status is resolved or paused.</returns>
		protected bool IsResolvedOrPaused(Entity caseEntity) {
			Guid caseStatusId = caseEntity.GetTypedColumnValue<Guid>(StatusIdColumnName);
			return CaseStatusStore.StatusIsResolved(caseStatusId) || CaseStatusStore.StatusIsPaused(caseStatusId);
		}

		/// <summary>
		/// Checks if case status is final.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <returns><c>true</c> if case status is final.</returns>
		protected bool IsFinalStatus(Entity caseEntity) {
			Guid caseStatusId = caseEntity.GetTypedColumnValue<Guid>(StatusIdColumnName);
			return CaseStatusStore.StatusIsFinal(caseStatusId);
		}

		/// <summary>
		/// Checks reopening condition and returns <c>true</c> if need reopen case.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <returns><c>true</c> if need reopen case</returns>
		protected virtual bool ReopeningCondition(Entity caseEntity) {
			return !IsReopenStatus(caseEntity) && !IsFinalStatus(caseEntity) && IsResolvedOrPaused(caseEntity);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Run execution reopen Case process.
		/// </summary>
		public void Run() {
			_log.InfoFormat($"[CaseId: {CaseId}] Case will be reopen.");
			Entity caseEntity = LoadCase(CaseId, ColumnAliases);
			if (caseEntity == null) {
				_log.InfoFormat($"[CaseId: {CaseId}] Case wasn't found.");
				return;
			}
			if (IsNeedToCreateNewCase(caseEntity)) {
				RegistrationHelper.CreateNewIncidentByEmail(LoadActivity(ActivityId));
				_log.InfoFormat($"[CaseId: {CaseId}] Was created new case because of " +
					$"CreateNewCaseInsteadOfReopenWhenRecipientChange feature.");
			} else {
				ReopenedCount = CaseBroker.ReopenOnCondition(new[] { caseEntity }, ReopeningCondition, true);
				_log.InfoFormat($"[CaseId: {CaseId}] There were {ReopenedCount} cases reopened.");
			}
			if (caseEntity.GetTypedColumnValue<Guid>(OwnerIdColumnName) != default(Guid)) {
				NotifyOwner(caseEntity);
			}
			if (ActivityId != default(Guid) && !IsFinalStatus(caseEntity)) {
				UpdateActivityProcessed(ActivityId);
			}
		}

		#endregion

	}

	#endregion
}
