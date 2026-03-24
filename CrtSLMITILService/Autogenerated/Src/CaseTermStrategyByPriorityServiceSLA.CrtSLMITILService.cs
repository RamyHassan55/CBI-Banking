namespace Terrasoft.Configuration
{
	using System;
	using System.Web;
	using Newtonsoft.Json;
	using Terrasoft.Core;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Configuration.Calendars;
	using CalendarsTimeUnit = Terrasoft.Configuration.Calendars.TimeUnit;
	using SLMExtensions;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;
	using System.Collections.Generic;

	#region Class

	/// <summary>
	/// Class that realize strategy by priority considering service in SLA and fills CaseTermInterval.
	/// </summary>
	public class CaseTermStrategyByPriorityServiceSLA : BaseTermStrategy<CaseTermInterval, CaseTermStates>
	{
		#region Properties: Private
		private PriorityServiceSLAStrategyData _priorityServiceSLADataModel;

		#endregion

		#region Classes: Private

		/// <summary>
		/// Class that contains actual data needed for strategy by priority considering service in SLA.
		/// </summary>
		private class PriorityServiceSLAStrategyData
		{
			public Guid ServiceItemId {
				get;
				set;
			}

			public Guid ServicePactId {
				get;
				set;
			}

			public Guid PriorityId {
				get;
				set;
			}
		}

		/// <summary>
		/// Class that contains names of time unit type and values columns.
		/// </summary>
		private class IntervalColumnsNames
		{
			public string ReactionType {
				get;
				set;
			}

			public string SolutionType {
				get;
				set;
			}

			public string ReactionValue {
				get;
				set;
			}

			public string SolutionValue {
				get;
				set;
			}

			public string Calendar {
				get;
				set;
			}
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Public constructor of CaseTermStrategyByPriorityServiceSLA with input parameters.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="args">Dictionary of input parameters.</param>
		public CaseTermStrategyByPriorityServiceSLA(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection) {
			_priorityServiceSLADataModel = args.ToObject<PriorityServiceSLAStrategyData>();
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Method that returns default calendar id by service item or system setting.
		/// </summary>
		/// <returns>Default calendar guid.</returns>
		private Guid GetCalendar() {
			EntitySchemaQuery esqServiceItem = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceItem");
			esqServiceItem.AddColumn("Calendar");
			var serviceItemEntity = esqServiceItem.GetEntity(UserConnection, _priorityServiceSLADataModel.ServiceItemId);
			var calendarServiceItem = serviceItemEntity.GetTypedColumnValue<Guid>("CalendarId");
			if (calendarServiceItem != Guid.Empty) {
				return calendarServiceItem;
			}
			return SystemSettings.GetValue<Guid>(UserConnection, CalendarConsts.BaseCalendarCode, Guid.Empty);
		}

		/// <summary>
		/// Method that prepares data for EntitySchemaQuery.
		/// </summary>
		/// <param name="esqServiceItem">EntitySchemaQuery for ServiceItem.</param>
		/// <param name="mask">Mask for case term interval.</param>
		private IntervalColumnsNames PrepareEntitySchemaQuery(EntitySchemaQuery esqTimeToPrioritize, CaseTermStates mask) {
			IntervalColumnsNames names = new IntervalColumnsNames();
			names.Calendar = esqTimeToPrioritize.AddColumn("ServiceInServicePact.Calendar.Id").Name;
			if (!mask.HasFlag(CaseTermStates.ContainsResponse)) {
				names.ReactionType = esqTimeToPrioritize.AddColumn("[TimeUnit:Id:ReactionTimeUnit].Code").Name;
				names.ReactionValue = esqTimeToPrioritize.AddColumn("ReactionTimeValue").Name;
			}
			if (!mask.HasFlag(CaseTermStates.ContainsResolve)) {
				names.SolutionType =
					esqTimeToPrioritize.AddColumn("[TimeUnit:Id:SolutionTimeUnit].Code").Name;
				names.SolutionValue = esqTimeToPrioritize.AddColumn("SolutionTimeValue").Name;
			}
			esqTimeToPrioritize.Filters.LogicalOperation = LogicalOperationStrict.And;
			var casePriorityFilter = esqTimeToPrioritize.CreateFilterWithParameters(FilterComparisonType.Equal,
				"CasePriority", _priorityServiceSLADataModel.PriorityId);
			var serviceItemFilter = esqTimeToPrioritize.CreateFilterWithParameters(FilterComparisonType.Equal,
				"ServiceInServicePact.ServiceItem", _priorityServiceSLADataModel.ServiceItemId);
			var servicePactFilter = esqTimeToPrioritize.CreateFilterWithParameters(FilterComparisonType.Equal,
				"ServiceInServicePact.ServicePact", _priorityServiceSLADataModel.ServicePactId);
			esqTimeToPrioritize.Filters.Add(casePriorityFilter);
			esqTimeToPrioritize.Filters.Add(serviceItemFilter);
			esqTimeToPrioritize.Filters.Add(servicePactFilter);
			return names;
		}

		#endregion

		#region Methods: Public
		/// <summary>
		/// Method that returns case term interval for calculation.
		/// </summary>
		/// <param name="mask">Mask for case term interval.</param>
		/// <returns>Case term interval.</returns>
		public override CaseTermInterval GetTermInterval(CaseTermStates mask) {
			CaseTermInterval result = new CaseTermInterval();
			EntitySchemaQuery esqTimeToPrioritize = new EntitySchemaQuery(UserConnection.EntitySchemaManager,
				"TimeToPrioritize");
			IntervalColumnsNames names = PrepareEntitySchemaQuery(esqTimeToPrioritize, mask);
			var entityCollection = esqTimeToPrioritize.GetEntityCollection(UserConnection);
			if (entityCollection.IsEmpty()) {
				return result;
			}
			Entity entity = entityCollection[0];
			var entityCalendar = entity.GetTypedColumnValue<Guid>(names.Calendar);
			var calendarId = entityCalendar != Guid.Empty ? entityCalendar : GetCalendar();
			if (calendarId == Guid.Empty) {
				return result;
			}
			if (!mask.HasFlag(CaseTermStates.ContainsResponse)) {
				var reactionTimeUnitName = entity.GetTypedColumnValue<string>(names.ReactionType);
				var timeUnit = default(CalendarsTimeUnit);
				var term = new TimeTerm();
				if (Enum.TryParse<CalendarsTimeUnit>(reactionTimeUnitName, out timeUnit)) {
					term.Type = timeUnit;
				}
				term.Value = entity.GetTypedColumnValue<int>(names.ReactionValue);
				term.CalendarId = calendarId;
				result.ResponseTerm = term.ConvertToMinutes();
			}
			if (!mask.HasFlag(CaseTermStates.ContainsResolve)) {
				var solutionTimeUnitName = entity.GetTypedColumnValue<string>(names.SolutionType);
				var timeUnit = default(CalendarsTimeUnit);
				var term = new TimeTerm();
				if (Enum.TryParse<CalendarsTimeUnit>(solutionTimeUnitName, out timeUnit)) {
					term.Type = timeUnit;
				}
				term.Value = entity.GetTypedColumnValue<int>(names.SolutionValue);
				term.CalendarId = calendarId;
				result.ResolveTerm = term.ConvertToMinutes();
			}
			return result;
		}

		#endregion

	}

	#endregion

}
