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

	#region Class: CaseTermStrategyByPriority

	/// <summary>
	/// Base class for priority strategies that determines time terms by priority and service.
	/// </summary>
	public class CaseTermStrategyByPriority : BaseTermStrategy<CaseTermInterval, CaseTermStates>
	{

		#region Class: StrategyData

		/// <summary>
		/// Nested class-container for specific strategy data.
		/// </summary>
		protected class StrategyData
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
		}

		#endregion

		#region Fields: Protected

		/// <summary>
		/// Specific strategy data.
		/// </summary>
		protected StrategyData StrategyDataItem;

		#endregion

		#region Constructors: Protected

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByPriority"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="args">The arguments.</param>
		protected CaseTermStrategyByPriority(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection) {
			StrategyDataItem = args.ToObject<StrategyData>();
		}

		#endregion

		#region Methods: Private

		private Guid GetBaseCalendarFromSysSettings() {
			return SystemSettings.GetValue(UserConnection, CalendarConsts.BaseCalendarCode, default(Guid));
		}

		private TimeTerm GetTerm(Entity entity, string timeUnitColumnName, string timeValueColumnName,
			Guid calendarId) {
			var term = new TimeTerm();
			var timeUnitName = entity.GetTypedColumnValue<string>(timeUnitColumnName);
			CalendarsTimeUnit timeUnit;
			if (Enum.TryParse(timeUnitName, out timeUnit)) {
				term.Type = timeUnit;
			}
			term.Value = entity.GetTypedColumnValue<int>(timeValueColumnName);
			term.CalendarId = calendarId;
			return term.ConvertToMinutes();
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier from system settings.</returns>
		/// <remarks>Should be used at the end.</remarks>
		protected virtual Guid GetCalendarId() {
			return GetBaseCalendarFromSysSettings();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns time interval for this strategy.
		/// </summary>
		/// <param name="state">Case term state.</param>
		/// <returns>Case term interval.</returns>
		public override CaseTermInterval GetTermInterval(CaseTermStates state) {
			var result = new CaseTermInterval();
			Guid calendarId = GetCalendarId();
			if (calendarId == default(Guid)) {
				return result;
			}
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "CasePriority");
			string reactionTimeUnitColumnName = esq.AddColumn("ReactionTimeUnit.Code").Name;
			string reactionTimeValueColumnName = esq.AddColumn("ReactionTimeValue").Name;
			string solutionTimeUnitColumnName = esq.AddColumn("SolutionTimeUnit.Code").Name;
			string solutionTimeValueColumnName = esq.AddColumn("SolutionTimeValue").Name;
			Entity entity = esq.GetEntity(UserConnection, StrategyDataItem.PriorityId);
			if (!state.HasFlag(CaseTermStates.ContainsResponse)) {
				result.ResponseTerm =
					GetTerm(entity, reactionTimeUnitColumnName, reactionTimeValueColumnName, calendarId);
			}
			if (!state.HasFlag(CaseTermStates.ContainsResolve)) {
				result.ResolveTerm =
					GetTerm(entity, solutionTimeUnitColumnName, solutionTimeValueColumnName, calendarId);
			}
			return result;
		}

		#endregion

	}

	#endregion

}
