namespace Terrasoft.Configuration.CaseService
{
	using System;
	using System.Collections.Generic;
	using global::Common.Logging;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: CaseBroker

	/// <summary>
	/// Class represents an intermediary for working with Case entity.
	/// </summary>
	public class CaseBroker
	{
		#region Fields: Private

		private static readonly ILog _log = LogManager.GetLogger("CaseMessageListener");

		#endregion

		#region Methods: Private

		private Entity GetEntity(Entity currentEntity, bool needFetch) {
			_log.InfoFormat($"[CaseId: {currentEntity.PrimaryColumnValue}] - Passed Case to GetEntity method");
			Entity localEntity = currentEntity;
			if (needFetch) {
				_log.InfoFormat($"[CaseId: {currentEntity.PrimaryColumnValue}] - Case will be fetched");
				var clearSchema = currentEntity.EntitySchemaManager.FindInstanceByName(currentEntity.SchemaName);
				localEntity = clearSchema.CreateEntity(currentEntity.UserConnection);
				localEntity.UseAdminRights = false;
				_log.InfoFormat($"[CaseId: {currentEntity.PrimaryColumnValue}] - Start fetching Case");
				localEntity.FetchFromDB(currentEntity.PrimaryColumnValue);
				_log.InfoFormat($"[CaseId: {currentEntity.PrimaryColumnValue}] - Case was fetched");
			}
			_log.InfoFormat($"[CaseId: {currentEntity.PrimaryColumnValue}] - Returned Case from GetEntity method");
			return localEntity;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Reopen case entity whether condition of predicate is correct.
		/// </summary>
		/// <param name="entities">List of case entities.</param>
		/// <param name="predicate">Predicate to check the condition for reopen.</param>
		/// <param name="needFetch">Need reload entity from data base.</param>
		/// <returns>Count of reopened cases.</returns>
		public virtual int ReopenOnCondition(IEnumerable<Entity> entities, Predicate<Entity> predicate,
				bool needFetch = false) {
			int reopened = 0;
			foreach (Entity entity in entities) {
				if (predicate(entity)) {
					_log.InfoFormat($"[CaseId: {entity.PrimaryColumnValue}] - Start reopening Case");
					var localEntity = GetEntity(entity, needFetch);
					localEntity.SetColumnValue("StatusId", CaseConsts.CaseStatusReopenedId);
					var userConnection = localEntity.UserConnection;
					var clearAssigneeOnCaseReopening = Core.Configuration.SysSettings
						.GetValue<bool>(userConnection, "ClearAssigneeOnCaseReopening", true);
					if (clearAssigneeOnCaseReopening) {
						localEntity.SetColumnValue("OwnerId", null);
					}
					_log.InfoFormat($"[CaseId: {entity.PrimaryColumnValue}] - Condition satisfied. Start Case saving process");
					localEntity.Save(false);
					reopened++;
					_log.InfoFormat($"[CaseId: {entity.PrimaryColumnValue}] - Case saved. Status is reopened");
				}
			}
			return reopened;
		}

		/// <summary>
		/// Close case entity whether condition of predicate is correct.
		/// </summary>
		/// <param name="entity">Case entity.</param>
		/// <param name="predicate">Predicate to check the condition for case closing.</param>
		/// <param name="needFetch">Need reload entity from database.</param>
		/// <returns>Is case saved.</returns>
		public virtual bool CloseOnCondition(Entity entity, Predicate<Entity> predicate,
			bool needFetch = false) {
			var userConnection = entity.UserConnection;
			if (userConnection.GetIsFeatureEnabled("CommonCaseClosureCode")) {
				return CloseOnCondition(entity, predicate, CaseConsts.CaseStatusClosedId, needFetch);
			}
			var closureCodeId = SystemSettings.GetValue(userConnection, "CaseClosureCodeDef", Guid.Empty);
			return CloseOnCondition(entity, predicate, CaseConsts.CaseStatusClosedId, closureCodeId, needFetch);
		}

		/// <summary>
		/// Close case entity whether condition of predicate is correct.
		/// </summary>
		/// <param name="entity">Case entity.</param>
		/// <param name="predicate">Predicate to check the condition for case closing.</param>
		/// <param name="statusId">Status identifier.</param>
		/// <param name="closureCodeId">Closure code identifier.</param>
		/// <param name="needFetch">Need reload entity from database.</param>
		/// <returns>Is case saved.</returns>
		public virtual bool CloseOnCondition(Entity entity, Predicate<Entity> predicate, Guid statusId, 
			Guid closureCodeId, bool needFetch = false) {
			if (predicate(entity)) {
				var userConnection = entity.UserConnection;
				var localEntity = GetEntity(entity, needFetch);
				localEntity.SetColumnValue("StatusId", statusId);
				localEntity.SetColumnValue("ClosureDate", userConnection.CurrentUser.GetCurrentDateTime());
				localEntity.SetColumnValue("ClosureCodeId", closureCodeId);
				localEntity.Save(false);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Close case entity whether condition of predicate is correct.
		/// </summary>
		/// <param name="entity">Case entity.</param>
		/// <param name="predicate">Predicate to check the condition for case closing.</param>
		/// <param name="statusId">Status identifier.</param>
		/// <param name="needFetch">Need reload entity from database.</param>
		/// <returns>Is case saved.</returns>
		public virtual bool CloseOnCondition(Entity entity, Predicate<Entity> predicate, Guid statusId, bool needFetch = false) {
			if (predicate(entity)) {
				var userConnection = entity.UserConnection;
				var localEntity = GetEntity(entity, needFetch);
				localEntity.SetColumnValue("StatusId", statusId);
				localEntity.SetColumnValue("ClosureDate", userConnection.CurrentUser.GetCurrentDateTime());
				localEntity.Save(false);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks whether case status is final.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		/// <param name="userConnection">User connection.</param>
		/// <returns></returns>
		public virtual bool IsCaseFinal(Guid caseId, UserConnection userConnection) {
			if (caseId == default(Guid)) {
				return true;
			}
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "Case");
			esq.AddColumn("Status.IsFinal");
			var @case = esq.GetEntity(userConnection, caseId);
			bool isFinal = @case == null ? true : @case.GetTypedColumnValue<bool>("Status_IsFinal");
			return isFinal;
		}

		#endregion

	}

	#endregion

}
