namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Common;
	using Core;
	using Core.Entities;
	using Terrasoft.Configuration.Calendars;
	using Terrasoft.Configuration.SLMExtensions;
	using CalendarsTimeUnit = Calendars.TimeUnit;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: BaseCaseTermStrategy

	/// <summary>
	/// Base class for case term strategies.
	/// </summary>
	public abstract class BaseCaseTermStrategy : BaseTermStrategy<CaseTermInterval, CaseTermStates>
	{

		#region Struct: TimeColumn

		/// <summary>
		/// Struct for time column data.
		/// </summary>
		protected struct TimeColumn
		{
			/// <summary>
			/// Time unit column name.
			/// </summary>
			public string UnitCode;

			/// <summary>
			/// Value column name.
			/// </summary>
			public string Value;
		}

		#endregion

		#region Class: TimeColumnData

		/// <summary>
		/// Nested class-container for time column data.
		/// </summary>
		protected class TimeColumnData
		{
			/// <summary>
			/// Reaction time column data.
			/// </summary>
			public TimeColumn ReactionColumn {
				get;
				set;
			}
			/// <summary>
			/// Solution time column data.
			/// </summary>
			public TimeColumn SolutionColumn {
				get;
				set;
			}
		}

		#endregion

		#region Class: BaseStrategyData

		/// <summary>
		/// Nested class-container for specific strategy data.
		/// </summary>
		protected class BaseStrategyData
		{
			public Guid PriorityId {
				get;
				set;
			}
			public Guid ServiceItemId {
				get;
				set;
			}
			public Guid ServicePactId {
				get;
				set;
			}
			public Guid SupportLevelId {
				get;
				set;
			}
		}

		#endregion

		#region Fields: Protected

		/// <summary>
		/// Specific strategy data.
		/// </summary>
		protected BaseStrategyData BaseStrategyDataItem;

		/// <summary>
		/// Entity schema name.
		/// </summary>
		protected string EntitySchemaName;

		#endregion

		#region Constructors: Protected

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseTermStrategy{TTermInterval, TMask}"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		protected BaseCaseTermStrategy(UserConnection userConnection)
			: base(userConnection) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCaseTermStrategy"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="args">The arguments.</param>
		protected BaseCaseTermStrategy(UserConnection userConnection, Dictionary<string, object> args)
			: this(userConnection) {
			BaseStrategyDataItem = args.ToObject<BaseStrategyData>();
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier from system settings.</returns>
		protected virtual Guid GetBaseCalendarFromSysSettings() {
			return SystemSettings.GetValue(UserConnection, CalendarConsts.BaseCalendarCode, default(Guid));
		}

		/// <summary>
		/// Try to set calendar from SericeInServicePact table.
		/// </summary>
		/// <param name="calendarId">reference to CalendarId.</param>
		/// <returns>Result of operation.</returns>
		protected bool TrySetCalendarFromServiceInServicePact(ref Guid calendarId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceInServicePact");
			var calendarIdColumnName = esq.AddColumn("Calendar.Id").Name;
			esq.Filters.LogicalOperation = LogicalOperationStrict.And;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServiceItem",
				BaseStrategyDataItem.ServiceItemId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServicePact",
				BaseStrategyDataItem.ServicePactId));
			EntityCollection collection = esq.GetEntityCollection(UserConnection);
			if (collection.IsNotEmpty()) {
				calendarId = collection[0].GetTypedColumnValue<Guid>(calendarIdColumnName);
			}
			return calendarId != default(Guid);
		}

		/// <summary>
		/// Tries to set calendar identifier from service pact.
		/// </summary>
		/// <param name="calendarId">Reference to calendar identifier.</param>
		/// <returns>Success.</returns>
		protected bool TrySetCalendarFromServicePact(ref Guid calendarId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServicePact");
			var calendarIdColumnName = esq.AddColumn("Calendar.Id").Name;
			Entity servicePact = esq.GetEntity(UserConnection, BaseStrategyDataItem.ServicePactId);
			if (servicePact != null) {
				calendarId = servicePact.GetTypedColumnValue<Guid>(calendarIdColumnName);
			}
			return calendarId != default(Guid);
		}

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier from system settings.</returns>
		protected virtual Guid GetCalendarId() {
			return GetBaseCalendarFromSysSettings();
		}

		/// <summary>
		/// Adds time columns to given entity schema query.
		/// </summary>
		/// <param name="esq">Reference to <seealso cref="EntitySchemaQuery"/> instance.</param>
		/// <returns>Added time column data.</returns>
		protected TimeColumnData AddTimeColumns(ref EntitySchemaQuery esq) {
			var timeColumns = new TimeColumnData {
				ReactionColumn = new TimeColumn {
					UnitCode = esq.AddColumn("ReactionTimeUnit.Code").Name,
					Value = esq.AddColumn("ReactionTimeValue").Name
				},
				SolutionColumn = new TimeColumn {
					UnitCode = esq.AddColumn("SolutionTimeUnit.Code").Name,
					Value = esq.AddColumn("SolutionTimeValue").Name
				}
			};
			return timeColumns;
		}

		/// <summary>
		/// Prepares result as an interval (<seealso cref="CaseTermInterval"/>).
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="state">Case term state.</param>
		/// <param name="timeColumns">Time columns.</param>
		/// <param name="calendarId">Calendar identifier.</param>
		/// <returns>Case term interval.</returns>
		protected CaseTermInterval PrepareResult(Entity entity, CaseTermStates state, TimeColumnData timeColumns,
				Guid calendarId) {
			var result = new CaseTermInterval();
			if (!state.HasFlag(CaseTermStates.ContainsResponse)) {
				var reactionTimeUnitName = entity.GetTypedColumnValue<string>(timeColumns.ReactionColumn.UnitCode);
				CalendarsTimeUnit timeUnit;
				Enum.TryParse(reactionTimeUnitName, out timeUnit);
				var term = new TimeTerm {
					Type = timeUnit,
					Value = entity.GetTypedColumnValue<int>(timeColumns.ReactionColumn.Value),
					CalendarId = calendarId
				};
				result.ResponseTerm = term.ConvertToMinutes();
			}
			if (!state.HasFlag(CaseTermStates.ContainsResolve)) {
				var solutionTimeUnitName = entity.GetTypedColumnValue<string>(timeColumns.SolutionColumn.UnitCode);
				CalendarsTimeUnit timeUnit;
				Enum.TryParse(solutionTimeUnitName, out timeUnit);
				var term = new TimeTerm {
					Type = timeUnit,
					Value = entity.GetTypedColumnValue<int>(timeColumns.SolutionColumn.Value),
					CalendarId = calendarId
				};
				result.ResolveTerm = term.ConvertToMinutes();
			}
			return result;
		}

		/// <summary>
		/// Applies filters to given entity schema query.
		/// </summary>
		/// <param name="esq">Reference to <seealso cref="EntitySchemaQuery"/> instance.</param>
		protected abstract void ApplyFilters(ref EntitySchemaQuery esq);

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns case term interval for this strategy.
		/// </summary>
		/// <param name="state">Case term state.</param>
		/// <returns>Case term interval.</returns>
		public override CaseTermInterval GetTermInterval(CaseTermStates state) {
			var result = new CaseTermInterval();
			Guid calendarId = GetCalendarId();
			if (calendarId == default(Guid)) {
				return result;
			}
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, EntitySchemaName);
			TimeColumnData timeColumns = AddTimeColumns(ref esq);
			ApplyFilters(ref esq);
			EntityCollection entityCollection = esq.GetEntityCollection(UserConnection);
			if (entityCollection.IsNotEmpty()) {
				result = PrepareResult(entityCollection[0], state, timeColumns, calendarId);
			}
			return result;
		}

		#endregion

	}

	#endregion

}

