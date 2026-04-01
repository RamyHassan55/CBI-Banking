namespace Terrasoft.Configuration
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Store;

	#region Class : CaseActiveIntervalReader

	/// <summary>
	/// Get collection of Active status dates.
	/// </summary>
	public class CaseActiveIntervalReader
	{
		#region Class : CaseData

		/// <summary>
		/// Nested class-container for specific strategy data.
		/// </summary>
		private class CaseData
		{
			public Guid CaseId {
				get;
				set;
			}
			public Boolean SolutionOverdue {
				get;
				set;
			}
			public bool CheckCaseResponseOverdue {
				get; set;
			}

			public bool CheckCaseSolutionOverdue {
				get; set;
			}
		}

		#endregion

		#region Properties : Protected

		/// <summary>
		/// Gets the user connection.
		/// </summary>
		/// <value>
		/// The user connection.
		/// </value>
		protected UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Fields : Private

		/// <summary>
		/// Specific case data.
		/// </summary>
		private readonly CaseData _caseData;
		private readonly TermCalculationLogStore _calculationLogStore;

		#endregion

		#region Constructors : Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseActiveIntervalReader"/> class.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="arguments">The arguments.</param>
		public CaseActiveIntervalReader(UserConnection userConnection, Dictionary<string, object> arguments) {
			UserConnection = userConnection;
			_calculationLogStore = TermCalculationLogStoreInitializer.GetStore(userConnection);
			_caseData = arguments.ToObject<CaseData>();
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Check uniqueness of case lifecycle status.
		/// </summary>
		/// <param name="source">Caselifecycle collection.</param>
		/// <param name="statusColumnName">Status column name.</param>
		/// <returns>Uniqueness of case lifecycle collection by status.</returns>
		private bool CheckDistinctStatus(EntityCollection source, string statusColumnName) {
			Guid firstElement = source[0].GetTypedColumnValue<Guid>(statusColumnName);
			bool isDistinct = source.All(element =>
					element.GetTypedColumnValue<Guid>(statusColumnName) == firstElement);
			return isDistinct;
		}

		private bool GetIsCaseOverdue(Guid caseId, bool checkResponse, bool checkSolution) {
			if (!checkResponse && !checkSolution) {
				return false;
			}
			Select select = new Select(UserConnection)
				.Column("Case", "SolutionOverdue")
				.Column("Case", "ResponseOverdue")
				.Column("Case", "SolutionDate")
				.Column("Case", "ResponseDate")
				.From("Case").Where("Id").IsEqual(Column.Parameter(caseId)) as Select;
			select.ExecuteSingleRecord(reader =>
					checkResponse && CheckDateOverdue(reader.GetColumnValue<DateTime>("ResponseDate") > DateTime.MinValue || reader.GetColumnValue<bool>("ResponseOverdue"),
					reader.GetColumnValue<DateTime>("ResponseDate")) ||
					checkSolution && CheckDateOverdue(reader.GetColumnValue<bool>("SolutionOverdue"), reader.GetColumnValue<DateTime>("SolutionDate")),
				out bool isOverdue);
			if (_calculationLogStore != null && isOverdue) {
				_calculationLogStore.IsOverdue = isOverdue;
			}
			return isOverdue;
		}

		private bool CheckDateOverdue(bool overdueFlag, DateTime overdueDate) {
			DateTime nowTime = DateTime.UtcNow;
			return overdueFlag || (overdueDate == default(DateTime) ? false : overdueDate < nowTime);
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Returns time interval collection from CaseLifecycle.
		/// </summary>
		/// <returns>Time interval collection.</returns>
		public virtual IEnumerable<DateTimeInterval> GetActiveIntervals() {
			List<DateTimeInterval> result = new List<DateTimeInterval>();
			if (_caseData.SolutionOverdue) {
				return result;
			}
			if (GetIsCaseOverdue(_caseData.CaseId, _caseData.CheckCaseResponseOverdue, _caseData.CheckCaseSolutionOverdue)) {
				return result;
			}
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "CaseLifecycle");
			string startDateColumnName = esq.AddColumn("StartDate").OrderByAsc().Name;
			string endDateColumnName = esq.AddColumn("EndDate").Name;
			string statusColumnName = esq.AddColumn("Status.Id").Name;
			string statusNameColumnName = esq.AddColumn("Status.Name").Name;
			string isFinalStatusNameColumnName = esq.AddColumn("Status.IsFinal").Name;
			string isResolvedStatusNameColumnName = esq.AddColumn("Status.IsResolved").Name;
			string isPausedStatusNameColumnName = esq.AddColumn("Status.IsPaused").Name;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Case", _caseData.CaseId));
			EntityCollection entityCollection = esq.GetEntityCollection(UserConnection);
			if (entityCollection.Any()) {
				bool isDistinct = CheckDistinctStatus(entityCollection, statusColumnName);
				if (!isDistinct) {
					var activeIntervals
					= entityCollection.Where(interval => !interval.GetTypedColumnValue<bool>(isPausedStatusNameColumnName) &&
					!interval.GetTypedColumnValue<bool>(isResolvedStatusNameColumnName) && !interval.GetTypedColumnValue<bool>(isFinalStatusNameColumnName));
					foreach (Entity interval in activeIntervals) {
						DateTimeInterval caseTimeInterval = new DateTimeInterval();
						caseTimeInterval.Start = interval.GetTypedColumnValue<DateTime>(startDateColumnName);
						var endDate = interval.GetTypedColumnValue<DateTime>(endDateColumnName);
						caseTimeInterval.End = endDate == DateTime.MinValue ?
							UserConnection.CurrentUser.GetCurrentDateTime() :
							endDate;
						result.Add(caseTimeInterval);
						if (_calculationLogStore != null) {
							_calculationLogStore.ActiveTimeIntervals.Add(new UsedTimeIntervals
							{
								TimeInterval = caseTimeInterval,
								CaseStatusName = interval.GetTypedColumnValue<string>(statusNameColumnName)
							});
						}
					}
				}
			}
			return result;
		}

		#endregion
	}

	#endregion
}
