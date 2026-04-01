using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;

namespace Terrasoft.Configuration
{
	using Creatio.FeatureToggling;

	#region Class: CaseSLMEventListener

	[EntityEventListener(SchemaName = "Case")]
	public class CaseSLMEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private UserConnection _userConnection;
		private Entity _case;
		private static Guid _portalOriginId = Guid.Parse("DEBF4124-F46B-1410-3592-0050BA5D6C38");

		#endregion

		#region Properties: Private

		private Guid CreatedCaseLifecycleId {
			get; set;
		}

		private Guid ClosedCaseLifeCycleId {
			get; set;
		}

		private bool StatusChanged {
			get; set;
		}

		public bool NeedSaveLifecycle {
			get; set;
		}

		#endregion

		#region Methods: Private

		private void SaveLifecycle() {
			DateTime actionDate = _userConnection.CurrentUser.GetCurrentDateTime();
			bool isNotFirstDate = ClosePreviousInterval(_case.GetTypedColumnValue<Guid>("Id"), actionDate);
			actionDate = isNotFirstDate ? _userConnection.CurrentUser.GetCurrentDateTime() : _case.GetTypedColumnValue<DateTime>("RegisteredOn");
			OpenNewInterval(actionDate);
		}

		private void CloseAllIntervals(Guid CaseId) {
			Select caseIntervalsSelect = new Select(_userConnection)
				.Column("CL", "Id")
				.Column("CL", "StartDate")
				.Column("CL", "EndDate")
				.From("CaseLifecycle").As("CL")
				.Where("CL", "CaseId").IsEqual(Column.Parameter(CaseId))
				.OrderByDesc("CL", "StartDate") as Select;
			Dictionary<Guid, DateTime> caseLifecycleRecordsToClose = new Dictionary<Guid, DateTime>();
			DateTime nextStartDate = default(DateTime);
			using (DBExecutor dbExecutor = _userConnection.EnsureDBConnection()) {
				dbExecutor.StartTransaction(IsolationLevel.ReadCommitted);
				try {
					caseIntervalsSelect.ExecuteReader(r => {
						if (r.GetColumnValue<DateTime>("EndDate") == default(DateTime) && nextStartDate != default(DateTime)) {
							caseLifecycleRecordsToClose.Add(r.GetColumnValue<Guid>("Id"), nextStartDate);
						}
						nextStartDate = r.GetColumnValue<DateTime>("StartDate");
					});
					ForceCloseCaseLifecycleRecords(caseLifecycleRecordsToClose);
					dbExecutor.CommitTransaction();
				} catch {
					dbExecutor.RollbackTransaction();
				}
			}

		}

		private void ForceCloseCaseLifecycleRecords(Dictionary<Guid, DateTime> caseLifecycleRecordsToClose) {
			caseLifecycleRecordsToClose.ForEach(record => {
				new Update(_userConnection, "CaseLifecycle")
					.Set("EndDate", Column.Parameter(record.Value))
					.Set("ModifiedOn", Column.Parameter(DateTime.UtcNow))
					.Where("Id").IsEqual(Column.Parameter(record.Key)).Execute();
			});
		}

		private bool ClosePreviousInterval(Guid CaseId, DateTime Date) {
			var previousIntervalESQ = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "CaseLifecycle");
			previousIntervalESQ.UseAdminRights = false;
			previousIntervalESQ.AddAllSchemaColumns();
			previousIntervalESQ.AddColumn("StartDate").OrderByDesc();
			var caseRecordFilter = previousIntervalESQ.CreateFilterWithParameters(
					FilterComparisonType.Equal, "CaseRecordId", CaseId);
			if (Features.GetIsEnabled("AddCaseColumnInLifecycleFilter")) {
				previousIntervalESQ.AddColumn("Case");
				var caseFilterGroup = new EntitySchemaQueryFilterCollection(previousIntervalESQ, LogicalOperationStrict.Or) {
					caseRecordFilter,
					previousIntervalESQ.CreateFilterWithParameters(
					FilterComparisonType.Equal, "Case", CaseId)
				};
				previousIntervalESQ.Filters.Add(caseFilterGroup);
			} else {
				previousIntervalESQ.Filters.Add(caseRecordFilter);
			}
			previousIntervalESQ.Filters.Add(previousIntervalESQ.CreateIsNullFilter("EndDate"));
			var previousIntervals = previousIntervalESQ.GetEntityCollection(_userConnection);
			if (previousIntervals.Count == 0) {
				return false;
			}
			var lastInterval = previousIntervals[0];
			lastInterval.UseAdminRights = false;
			DateTime startDate = lastInterval.GetTypedColumnValue<DateTime>("StartDate");
			TimeSpan difference = (TimeSpan)(Date - startDate);
			lastInterval.SetColumnValue("EndDate", Date);
			lastInterval.SetColumnValue("StateDurationInMinutes", difference.TotalMinutes);
			lastInterval.SetColumnValue("StateDurationInHours", difference.TotalHours);
			lastInterval.SetColumnValue("StateDurationInDays", difference.TotalDays);
			lastInterval.Save();
			ClosedCaseLifeCycleId = lastInterval.GetTypedColumnValue<Guid>("Id");
			if (previousIntervals.Count > 1 && Features.GetIsEnabled("CheckAndCloseAllCaseIntervals")) {
				CloseAllIntervals(CaseId);
			}
			return true;
		}

		private void OpenNewInterval(DateTime Date) {
			var entitySchemaCaseLifecycle = _userConnection.EntitySchemaManager.GetInstanceByName("CaseLifecycle");
			var newCaseLifecycle = entitySchemaCaseLifecycle.CreateEntity(_userConnection);
			newCaseLifecycle.UseAdminRights = false;
			newCaseLifecycle.SetDefColumnValues();
			newCaseLifecycle.SetColumnValue("StartDate", Date);
			newCaseLifecycle.SetColumnValue("CaseRecordId", _case.GetColumnValue("Id"));
			newCaseLifecycle.Save();
			CreatedCaseLifecycleId = newCaseLifecycle.GetTypedColumnValue<Guid>("Id");

		}

		private void LogChangedColumns() {
			var entitySchemaCaseLifecycle = _userConnection.EntitySchemaManager.GetInstanceByName("CaseLifecycle");
			var entitySchemaCase = _userConnection.EntitySchemaManager.GetInstanceByName("Case");
			var newCaseLifecycle = entitySchemaCaseLifecycle.CreateEntity(_userConnection);
			newCaseLifecycle.UseAdminRights = false;
			if (newCaseLifecycle.FetchFromDB(CreatedCaseLifecycleId)) {
				newCaseLifecycle.SetDefColumnValues();
				var disableCommonColumns = new List<string>() {
					"Id",
					"CreatedOn",
					"CreatedById"
				};
				var commonColumns = entitySchemaCaseLifecycle.Columns
					.Select(c => new { Name = c.ColumnValueName, DataValueType = c.DataValueType })
					.Intersect(entitySchemaCase.Columns.Select(c => new { Name = c.ColumnValueName, DataValueType = c.DataValueType }))
					.Select(c => c.Name)
					.Except(disableCommonColumns);

				foreach (string columnName in commonColumns) {
					newCaseLifecycle.SetColumnValue(columnName, _case.GetColumnValue(columnName));
				}
				newCaseLifecycle.SetColumnValue("CaseId", _case.GetColumnValue("Id"));
				newCaseLifecycle.Save();
			}
		}

		private bool GetIsNeedToLogLifecycle() {
			var logColumns = (IEnumerable<string>)GetLoggingColumns();
			IEnumerable<EntityColumnValue> changedValues = GetChangedColumns();
			return changedValues.Select(v => v.Name).Intersect(logColumns).Any();
		}

		private IEnumerable<EntityColumnValue> GetChangedColumns() {
			return _case.GetChangedColumnValues().Where(v =>
							((v.Value != null) && !v.Value.Equals(v.OldValue)) ||
							((v.Value == null) && v.OldValue != null)
						);
		}

		private bool GetIsStatusActive(Guid statusId) {
			var columns = new[] {
				"IsPaused",
				"IsFinal",
				"IsResolved"
			};
			return !GetStatusIs(statusId, columns);
		}

		private bool CheckIsStatusChanged() {
			var statusIdColumnName = "StatusId";
			var oldStatusId = _case.GetTypedOldColumnValue<Guid>(statusIdColumnName);
			var statusId = _case.GetTypedColumnValue<Guid>(statusIdColumnName);
			return oldStatusId != statusId;
		}

		private void NotifyUser() {
			if (StatusChanged) {
				ICasePushNotifier notifier = ClassFactory.Get<ICasePushNotifier>(new ConstructorArgument("userConnection", _userConnection));
				notifier.NotifyNewStatus(_case.GetTypedColumnValue<Guid>("Id"));
			}
		}

		private void UpdateResponse() {
			var statusIdColumnName = "StatusId";
			var oldStatusId = _case.GetTypedOldColumnValue<Guid>(statusIdColumnName);
			var statusId = _case.GetTypedColumnValue<Guid>(statusIdColumnName);
			if (oldStatusId == Guid.Empty || oldStatusId == statusId) {
				return;
			}
			var respondedOnColumnName = "RespondedOn";
			var respondedOn = _case.GetTypedColumnValue<DateTime>(respondedOnColumnName);
			if (respondedOn == default(DateTime)) {
				var dateTimeNow = GetDateTimeInCurrentTimeZone();
				_case.SetColumnValue(respondedOnColumnName, dateTimeNow);
			}
		}

		private void UpdateSolution() {
			var statusIdColumnName = "StatusId";
			var oldStatusId = _case.GetTypedOldColumnValue<Guid>(statusIdColumnName);
			var statusId = _case.GetTypedColumnValue<Guid>(statusIdColumnName);
			if (oldStatusId == statusId) {
				return;
			}
			var isSolutionOverdue = _case.GetTypedColumnValue<bool>("SolutionOverdue");
			DateTime? nullValue = null;
			var isNewStatusFinalOrResolved = GetIsStatusFinalOrResolved(statusId);
			if (Features.GetIsEnabled("CaseRuleApplier")) {
				if (!GetIsStatusActive(oldStatusId)) {
					var isOldStatusFinalOrResolved = GetIsStatusFinalOrResolved(oldStatusId);
					DateTime? dateTimeNow;
					if (isNewStatusFinalOrResolved || GetIsStatusPaused(statusId)) {
						if (isOldStatusFinalOrResolved) {
							return;
						}
						dateTimeNow = GetDateTimeInCurrentTimeZone();
					} else {
						dateTimeNow = nullValue;
					}
					_case.SetColumnValue("SolutionProvidedOn", dateTimeNow);
				}
				if (GetIsStatusPaused(statusId) && !GetIsStatusResolved(oldStatusId) && !isSolutionOverdue) {
					_case.SetColumnValue("SolutionDate", nullValue);
				}
			} else {
				if (GetIsStatusPaused(statusId) && !isSolutionOverdue) {
					_case.SetColumnValue("SolutionDate", nullValue);
				}
				if (!GetIsStatusFinal(statusId) && GetIsStatusResolved(oldStatusId)) {
					_case.SetColumnValue("SolutionProvidedOn", nullValue);
					if (Features.GetIsEnabled("ClearSolutionDateAfterReopenCase") && !isSolutionOverdue) {
						_case.SetColumnValue("SolutionDate", nullValue);
					}
				}
				if (!GetIsStatusActive(oldStatusId)) {
					var isOldStatusFinalOrResolved = GetIsStatusFinalOrResolved(oldStatusId);
					DateTime? dateTimeNow;
					if (isNewStatusFinalOrResolved) {
						if (isOldStatusFinalOrResolved) {
							return;
						}
						dateTimeNow = GetDateTimeInCurrentTimeZone();
					} else {
						dateTimeNow = nullValue;
					}
					_case.SetColumnValue("SolutionProvidedOn", dateTimeNow);
				}
			}
			if (isNewStatusFinalOrResolved) {
				SetCurrentDateTime("FirstSolutionProvidedOn");
				SetCurrentDateTime("SolutionProvidedOn");
			}
		}

		private bool GetStatusIs(Guid statusId, params string[] columns) {
			var result = false;
			if (!Features.GetIsEnabled("UseCaseStatusStore")) {
				var store = new CaseStatusStore(_userConnection);
				foreach (string column in columns) {
					switch (column) {
						case "IsPaused":
							result |= store.StatusIsPaused(statusId);
							break;
						case "IsFinal":
							result |= store.StatusIsFinal(statusId);
							break;
						case "IsResolved":
							result |= store.StatusIsResolved(statusId);
							break;
					}
				}
			} else {
				var select = new Select(_userConnection)
					.From("CaseStatus")
					.Where("Id").IsEqual(Column.Parameter(statusId)) as Select;
				foreach (var column in columns) {
					select = select.Column(column);
				}
				using (var dbExecutor = _userConnection.EnsureDBConnection()) {
					using (IDataReader dr = select.ExecuteReader(dbExecutor)) {
						if (dr.Read()) {
							foreach (var column in columns) {
								result |= dr.GetColumnValue<bool>(column);
							}
						}
					}
				}
			}
			return result;
		}

		private bool GetIsStatusPaused(Guid statusId) {
			return GetStatusIs(statusId, "IsPaused");
		}

		private bool GetIsStatusResolved(Guid statusId) {
			return GetStatusIs(statusId, "IsResolved");
		}

		private bool GetIsStatusFinal(Guid statusId) {
			return GetStatusIs(statusId, "IsFinal");
		}

		private bool GetIsStatusFinalOrResolved(Guid id) {
			var columns = new[] {
				"IsFinal",
				"IsResolved"
			};
			return GetStatusIs(id, columns);
		}

		private DateTime GetDateTimeInCurrentTimeZone() {
			return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, _userConnection.CurrentUser.TimeZone);
		}

		private void UpdateClosureDate() {
			var statusId = _case.GetTypedColumnValue<Guid>("StatusId");
			var closureDate = _case.GetTypedColumnValue<DateTime>("ClosureDate");
			if (IsStatusChanged(_case) && GetIsStatusFinal(statusId) && closureDate == default(DateTime)) {
				var currentUserDateTime =
				CaseTimezoneHelper.GetContactTimezone(_userConnection, _userConnection.CurrentUser.ContactId.ToString());
				if (currentUserDateTime != null) {
					_case.SetColumnValue("ClosureDate", DateTime.Parse(currentUserDateTime));
				}
			}
		}

		private void SetCurrentDateTime(string dateTimeField) {
			if (_case.GetTypedColumnValue<DateTime>(dateTimeField) == default(DateTime)) {
				var currentUserDateTime =
				CaseTimezoneHelper.GetContactTimezone(_userConnection, _userConnection.CurrentUser.ContactId.ToString());
				if (currentUserDateTime != null) {
					_case.SetColumnValue(dateTimeField, DateTime.Parse(currentUserDateTime));
				}
			}
		}

		private bool NeedCalculateTerms() {
			if (!Features.GetIsEnabled("CalculateTermOnCaseEntity")) {
				return false;
			}
			if (_case.GetTypedColumnValue<DateTime>("SolutionDate") == DateTime.MinValue && !GetIsStatusPaused(_case.GetTypedColumnValue<Guid>("StatusId"))) {
				return true;
			}
			return GetIsTermNeededColumnsChanges() &&
				_case.GetTypedColumnValue<DateTime>("SolutionDate") != DateTime.MinValue &&
				!GetIsStatusPaused(_case.GetTypedColumnValue<Guid>("StatusId"));
		}

		private bool GetIsTermNeededColumnsChanges() {
			var changedValues = GetChangedColumns();
			return changedValues.Select(v => v.Name).Intersect(new List<string>() { "ServiceItemId", "ServicePactId", "PriorityId", "SupportLevelId" }).Any();
		}

		private void CalculateTerms() {
			if (NeedCalculateTerms()) {
				new CaseTermCalculationManager(_userConnection).Calculate(_case);
			}
		}

		private bool GetIsStatusWaiting(Guid statusId) {
			Guid waitingForResponseId = Guid.Parse("3859C6E7-CBCB-486B-BA53-77808FE6E593");
			return statusId.Equals(waitingForResponseId);
		}

		private bool IsStatusChanged(Entity entity) {
			Guid? value = entity.GetTypedColumnValue<Guid>("StatusId");
			Guid? oldValue = entity.GetTypedOldColumnValue<Guid>("StatusId");
			return ((value != null) && !value.Equals(oldValue)) ||
				((value == null) && oldValue != null);
		}

		private void FillContactAndAccount(Entity entity) {
			if (entity.GetTypedColumnValue<Guid>("OriginId") == _portalOriginId && entity.GetTypedColumnValue<Guid>("ContactId") == Guid.Empty
				&& entity.UserConnection.CurrentUser.ContactId != Guid.Empty) {
				entity.SetColumnValue("ContactId", entity.UserConnection.CurrentUser.ContactId);
				if (entity.GetTypedColumnValue<Guid>("AccountId") == Guid.Empty && entity.UserConnection.CurrentUser.AccountId != Guid.Empty) {
					entity.SetColumnValue("AccountId", entity.UserConnection.CurrentUser.AccountId);
				}
			}
		}

		#endregion

		#region Methods: Protected

		protected virtual List<string> GetLoggingColumns() {
			return new List<string>() {
				"StatusId",
				"PriorityId",
				"ServiceItemId",
				"OwnerId",
				"GroupId",
				"SolutionDate",
				"SolutionProvidedOn",
				"SupportLevelId",
			};
		}

		#endregion

		#region Methods: Public

		public override void OnInserting(object sender, EntityBeforeEventArgs e) {
			base.OnInserting(sender, e);
			FillContactAndAccount(sender as Entity);
		}

		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			base.OnSaving(sender, e);
			if (!Features.GetIsEnabled("UseCaseInSLMOldFunc")) {
				_case = (Entity)sender;
				_userConnection = _case.UserConnection;
				StatusChanged = CheckIsStatusChanged();
				UpdateResponse();
				UpdateSolution();
				UpdateClosureDate();
				CalculateTerms();
				NeedSaveLifecycle = GetIsNeedToLogLifecycle();
				if (NeedSaveLifecycle) {
					SaveLifecycle();
				}
			}
		}

		public override void OnSaved(object sender, EntityAfterEventArgs e) {
			base.OnSaved(sender, e);
			if (!Features.GetIsEnabled("UseCaseInSLMOldFunc")) {
				_case = (Entity)sender;
				_userConnection = _case.UserConnection;
				if (NeedSaveLifecycle) {
					LogChangedColumns();
				}
				NotifyUser();
			}
		}

		#endregion

	}

	#endregion

}
