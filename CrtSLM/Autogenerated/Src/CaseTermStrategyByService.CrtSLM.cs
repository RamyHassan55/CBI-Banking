namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Common;
	using Core;
	using Core.Entities;
	using Terrasoft.Configuration.Calendars;
	using Terrasoft.Configuration.SLMExtensions;
	using CalendarsTimeUnit = Terrasoft.Configuration.Calendars.TimeUnit;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: CaseTermStrategyByService

	/// <summary>
	/// Case term strategy by service implementation.
	/// </summary>
	public class CaseTermStrategyByService : BaseTermStrategy<CaseTermInterval, CaseTermStates>
	{
		#region Properties: Private

		private readonly ServiceStrategyData _serviceDataModel;

		#endregion

		#region Classes: Private

		/// <summary>
		/// Nested class-container for specific strategy data.
		/// </summary>
		private class ServiceStrategyData
		{
			public Guid ServiceItemId {
				get;
				set;
			}
		}

		/// <summary>
		/// Nested class-container for time unit type columns names.
		/// </summary>
		private class TimeUnitColumnsNames
		{
			public string ReactionType {
				get;
				set;
			}

			public string SolutionType {
				get;
				set;
			}
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByService"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="args">The arguments.</param>
		public CaseTermStrategyByService(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection) {
			_serviceDataModel = args.ToObject<ServiceStrategyData>();
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Gets default calendar identifier by system setting.
		/// </summary>
		/// <returns>Default calendar identifier.</returns>
		private Guid GetCalendarBySysSetting() {
			return SystemSettings.GetValue(UserConnection, CalendarConsts.BaseCalendarCode, default(Guid));
		}

		/// <summary>
		/// Prepares query columns and returns their names.
		/// </summary>
		/// <param name="esq"><seealso cref="EntitySchemaQuery"/> instance.</param>
		/// <param name="state">Case term state.</param>
		/// <returns>Time unit columns names.</returns>
		private TimeUnitColumnsNames PrepareEntitySchemaQuery(EntitySchemaQuery esq, CaseTermStates state) {
			TimeUnitColumnsNames names = new TimeUnitColumnsNames();
			esq.AddColumn("Calendar");
			if (!state.HasFlag(CaseTermStates.ContainsResponse)) {
				names.ReactionType = esq.AddColumn("[TimeUnit:Id:ReactionTimeUnit].Code").Name;
				esq.AddColumn("ReactionTimeValue");
			}
			if (!state.HasFlag(CaseTermStates.ContainsResolve)) {
				names.SolutionType = esq.AddColumn("[TimeUnit:Id:SolutionTimeUnit].Code").Name;
				esq.AddColumn("SolutionTimeValue");
			}
			return names;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets case term interval for this strategy.
		/// </summary>
		/// <param name="state">Case term state.</param>
		/// <returns>Case term interval.</returns>
		public override CaseTermInterval GetTermInterval(CaseTermStates state) {
			var result = new CaseTermInterval();
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceItem");
			TimeUnitColumnsNames names = PrepareEntitySchemaQuery(esq, state);
			var serviceItem = esq.GetEntity(UserConnection, _serviceDataModel.ServiceItemId);
			if (serviceItem == null) {
				return result;
			}
			var serviceItemCalendar = serviceItem.GetTypedColumnValue<Guid>("CalendarId");
			var calendarId = serviceItemCalendar != default(Guid) ? serviceItemCalendar : GetCalendarBySysSetting();
			if (calendarId == default(Guid)) {
				return result;
			}
			if (!state.HasFlag(CaseTermStates.ContainsResponse)) {
				var term = new TimeTerm();
				var reactionTimeUnitName = serviceItem.GetTypedColumnValue<string>(names.ReactionType);
				CalendarsTimeUnit timeUnit;
				if (Enum.TryParse(reactionTimeUnitName, out timeUnit)) {
					term.Type = timeUnit;
				}
				term.Value = serviceItem.GetTypedColumnValue<int>("ReactionTimeValue");
				term.CalendarId = calendarId;
				result.ResponseTerm = term.ConvertToMinutes();
			}
			if (!state.HasFlag(CaseTermStates.ContainsResolve)) {
				var term = new TimeTerm();
				var solutionTimeUnitName = serviceItem.GetTypedColumnValue<string>(names.SolutionType);
				CalendarsTimeUnit timeUnit;
				if (Enum.TryParse(solutionTimeUnitName, out timeUnit)) {
					term.Type = timeUnit;
				}
				term.Value = serviceItem.GetTypedColumnValue<int>("SolutionTimeValue");
				term.CalendarId = calendarId;
				result.ResolveTerm = term.ConvertToMinutes();
			}
			return result;
		}

		#endregion

	}

	#endregion

}
