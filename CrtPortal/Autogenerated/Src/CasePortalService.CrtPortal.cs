namespace Terrasoft.Configuration.CasePortalService
{
	using System;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.ServiceModel.Web;
	using System.ServiceModel.Activation;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.CaseService;
	using Terrasoft.Core.DB;
	using Terrasoft.Web.Common;

	#region Class: CasePortalService

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class CasePortalService : BaseService
	{

		#region Constants: Private

		private const string StatusIdName = "Status.Id";
		private const string StatusIsPausedName = "Status.IsPaused";
		private const string StatusIsFinalName = "Status.IsFinal";
		private const string StatusIsResolvedName = "Status.IsResolved";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="CasePortalService"/>
		/// </summary>
		public CasePortalService() {
			AlliesColumnMap = new Dictionary<string, string>();
		}

		/// <summary>
		/// Initialize new instance of <see cref="CasePortalService"/>.
		/// <param name="caseBroker">Instance of <see cref="CaseBroker"/>.</param>
		/// </summary>
		public CasePortalService(CaseBroker caseBroker)
				: this() {
			_caseBroker = caseBroker;
		}

		#endregion

		#region Properties: Protected

		public Dictionary<string, string> AlliesColumnMap { get; private set; }

		#endregion

		#region Properties: Public

		private CaseBroker _caseBroker;
		/// <summary>
		/// Instance of <see cref="CaseBroker"/> that processes Case entity.
		/// </summary>
		public CaseBroker CaseBroker {
			get {
				return _caseBroker ?? (_caseBroker = new CaseBroker());
			}
		}

		#endregion

		#region Methods: Private

		private void AddColumns(EntitySchemaQuery esq, IEnumerable<string> columns) {
			foreach (string colum in columns) {
				AlliesColumnMap[colum] = esq.AddColumn(colum).Name;
			}
		}

		private bool IsNeedReopen(Entity entity) {
			return (entity.GetTypedColumnValue<bool>(AlliesColumnMap[StatusIsPausedName]) ||
					entity.GetTypedColumnValue<bool>(AlliesColumnMap[StatusIsResolvedName])) &&
				   entity.GetTypedColumnValue<Guid>(AlliesColumnMap[StatusIdName]) != CaseConsts.CaseStatusReopenedId;
		}

		private bool IsNotFinalStatus(Entity entity) {
			return !entity.GetTypedColumnValue<bool>(AlliesColumnMap[StatusIsFinalName]);
		}

		/// <summary>
		/// Finalize case.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		/// <param name="statusId">Case status identifier.</param>
		/// <param name="closureCodeId">Closure code identifier.</param>
		private void FinalizeCase(Guid caseId, Guid statusId, Guid closureCodeId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			AddColumns(esq, new[] { StatusIsFinalName });
			Entity @case = esq.GetEntity(UserConnection, caseId);
			ChangeCaseStatus(@case, statusId, IsNotFinalStatus, closureCodeId);
		}

		/// <summary>
		/// Cancel case.
		/// </summary>
		/// <param name="recordId">Case identifier.</param>
		private void CancelCase(Guid recordId) {
			FinalizeCase(recordId, CaseConsts.CaseStatusCancelId, CaseConsts.ClosureCodeCanceledByUserId);
		}

		/// <summary>
		/// Close case.
		/// </summary>
		/// <param name="recordId">Case identifier.</param>
		private void CloseCase(Guid recordId) {
			FinalizeCase(recordId, CaseConsts.CaseStatusClosedId, CaseConsts.ClosureCodeClosedByUserId);
		}

		/// <summary>
		/// Reopen case.
		/// </summary>
		/// <param name="recordId">Case identifier.</param>
		private void ReopenCase(Guid recordId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			AddColumns(esq, new[] { StatusIdName, StatusIsPausedName, StatusIsResolvedName });
			Entity @case = esq.GetEntity(UserConnection, recordId);
			ChangeCaseStatus(@case, CaseConsts.CaseStatusReopenedId, IsNeedReopen);
		}

		/// <summary>
		/// Generate exception then not possible close/cancel/reopen case.
		/// </summary>
		/// <returns>Instance of <see cref="FinalCaseStatusException"/></returns>
		private Exception GenerateChangeStatusException() {
			string errorMessage = new LocalizableString(UserConnection.Workspace.ResourceStorage,
				"CasePortalService", "LocalizableStrings.CancelCaseError.Value");
			return new FinalCaseStatusException(errorMessage);
		}

		/// <summary>
		/// Change case status and closure code.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <param name="statusId">Case status identifier.</param>
		/// <param name="closureCodeId">Closure code identifier.</param>
		/// <param name="predicate">Predicate to check the condition for case changing.</param>
		private void ChangeCaseStatus(Entity caseEntity, Guid statusId, Predicate<Entity> predicate,
			Guid closureCodeId = default(Guid)) {
			if (closureCodeId == Guid.Empty) {
				IList<Entity> caseEntities = new List<Entity> { caseEntity };
				int reopenedCases = CaseBroker.ReopenOnCondition(caseEntities, predicate, true);
				if (reopenedCases == 0) {
					throw GenerateChangeStatusException();
				}
			} else if (!CaseBroker.CloseOnCondition(caseEntity, predicate, statusId, closureCodeId, true)) {
				throw GenerateChangeStatusException();
			}
		}

		private bool CheckIsCaseHasVisibleMessages(Guid caseId) {
			Select portalEmailMessage = new Select(UserConnection)
				.Top(1)
				.Column("pem", "CaseMessageHistoryId")
				.From("PortalEmailMessage").As("pem")
				.Where("pem", "CaseMessageHistoryId").IsEqual("cmh", "Id") as Select;
			Select portalMessage = new Select(UserConnection)
				.Top(1)
				.Column("pm", "EntityId")
				.From("PortalMessage").As("pm")
				.Where("pm", "EntityId").IsEqual("cmh", "CaseId")
				.And("HideOnPortal").IsEqual(Column.Const(false)) as Select;
			Select caseMessageHistory = new Select(UserConnection)
				.Column(Column.Const(true)).As("PortalMessageExists")
				.From("CaseMessageHistory", "cmh")
				.Where()
				.OpenBlock("cmh", "Id")
					.IsEqual(portalEmailMessage)
					.Or("cmh", "CaseId").IsEqual(portalMessage)
				.CloseBlock()
				.And("cmh", "CaseId").IsEqual(Column.Parameter(caseId)) as Select;
			return caseMessageHistory.ExecuteScalar<bool>();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Sets case into cancel status.
		/// </summary>
		/// <param name="recordId">Case record identifier.</param>
		/// <returns>Service response.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public ConfigurationServiceResponse CancelCase(string recordId) {
			var response = new ConfigurationServiceResponse();
			try {
				var caseId = Guid.Parse(recordId);
				CancelCase(caseId);
			} catch (Exception e) {
				response.Exception = e;
			}
			return response;
		}

		/// <summary>
		/// Sets case into close status.
		/// </summary>
		/// <param name="recordId">Case record identifier.</param>
		/// <returns>Service response.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public ConfigurationServiceResponse CloseCase(string recordId) {
			var response = new ConfigurationServiceResponse();
			try {
				var caseId = Guid.Parse(recordId);
				CloseCase(caseId);
			} catch (Exception e) {
				response.Exception = e;
			}
			return response;
		}

		/// <summary>
		/// Sets case into reopen status.
		/// </summary>
		/// <param name="recordId">Case record identifier.</param>
		/// <returns>Service response.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public ConfigurationServiceResponse ReopenCase(string recordId) {
			var response = new ConfigurationServiceResponse();
			try {
				var caseId = Guid.Parse(recordId);
				ReopenCase(caseId);
			} catch (Exception e) {
				response.Exception = e;
			}
			return response;
		}
		
		/// <summary>
		/// Returns does the case have at least 1 visible message on Portal.
		/// </summary>
		/// <param name="caseId">Case record identifier.</param>
		/// <returns>True if case has 1 or more visible on Portal message.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
			ResponseFormat = WebMessageFormat.Json)]
		public bool IsCaseHasVisibleMessages(Guid caseId) {
			var result = CheckIsCaseHasVisibleMessages(caseId);
			return result;
		}

		#endregion

		#region Exceptions

		/// <summary>
		/// Exception that determines that case is in closed/canceled status.
		/// </summary>
		private class FinalCaseStatusException : Exception
		{
			/// <summary>
			/// Initialize new instace of <see cref="FinalCaseStatusException"/>
			/// </summary>
			/// <param name="message">Error message.</param>
			public FinalCaseStatusException(string message) : base(message) { }
		}

		#endregion

	}

	#endregion

}

