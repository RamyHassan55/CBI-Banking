using System;
using System.Collections.Generic;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace Terrasoft.Configuration
{

	#region Class: TermCalculatorActions

	public class TermCalculatorActions
	{
		#region Constructors: Public

		public TermCalculatorActions(UserConnection userConnection, Guid calendarId) {
			UserConnection = userConnection;
			Utils = new CalendarUtilsBase(UserConnection);
			CalendarId = calendarId;
			CalendarIdsChain = Utils.GetCalendarIdsChain(calendarId);
		}

		#endregion

		#region Properties: Protected

		protected UserConnection UserConnection { 
			get; 
			set; 
		}

		protected CalendarUtilsBase Utils { 
			get; 
			set; 
		}

		protected Guid CalendarId { 
			get; 
			set; 
		}

		protected IEnumerable<Guid> CalendarIdsChain { 
			get; 
			set; 
		}

		#endregion

		#region Methods: Private

		private KeyValuePair<DateTime, Entity> GetNearestNotWeekendDay(DateTime registrationDateTime,
			bool moveForvard = true) {
			Entity day = Utils.GetCalendarDay(CalendarIdsChain, registrationDateTime);
			if (!day.GetTypedColumnValue<bool>("DayType_IsWeekend")) {
				return new KeyValuePair<DateTime, Entity>(registrationDateTime, day);
			}
			registrationDateTime = registrationDateTime.Date.AddDays(moveForvard ? 1 : -1);
			return GetNearestNotWeekendDay(registrationDateTime, moveForvard);
		}

		private TimeSpan GetLastWorkingTimeIntervalEnd(Guid dayPrimaryColumnValue) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			EntitySchemaQueryColumn toColumn = esq.AddColumn("To");
			toColumn.SummaryType = AggregationType.Max;
			esq.RowCount = 1;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				dayPrimaryColumnValue));
			Entity entity = esq.GetSummaryEntity(UserConnection);

			return entity != null ? entity.GetTypedColumnValue<DateTime>(toColumn.Name).TimeOfDay : TimeSpan.Zero;
		}
		
		private DateTime GetLastWorkingDayDateTime (Entity day) {
			return day.GetTypedColumnValue<DateTime>("Date").Date
				+ GetLastWorkingTimeIntervalEnd(day.PrimaryColumnValue);
		}

		private DateTime RemoveWorkingDays(DateTime registrationDateTime, int daysCount) {
			DateTime fromDay = registrationDateTime.Date.AddDays(-daysCount);
			DateTime toDay = registrationDateTime.Date.AddDays(-1);

			List<Entity> days = Utils.GetCalendarDays(CalendarIdsChain, fromDay, toDay).ToList();
			int weekendDaysCount = days.Count(d => d.GetTypedColumnValue<bool>("DayType_IsWeekend"));
			while (weekendDaysCount != 0) {
				toDay = fromDay.AddDays(-1);
				fromDay = fromDay.AddDays(-weekendDaysCount);
				days = Utils.GetCalendarDays(CalendarIdsChain, fromDay, toDay).ToList();
				weekendDaysCount = days.Count(d => d.GetTypedColumnValue<bool>("DayType_IsWeekend"));
			}

			DateTime minDate = days.Min(d => d.GetTypedColumnValue<DateTime>("Date"));
			Entity minDay = days.Single(d => d.GetTypedColumnValue<DateTime>("Date") == minDate);
			return minDay.GetTypedColumnValue<DateTime>("Date")
				+ GetLastWorkingTimeIntervalEnd(minDay.PrimaryColumnValue);
		}

		private DateTime AddWorkingPeriod(DateTime periodDateTime) {
			periodDateTime = GetNextWorkingTimeIntervalStart(periodDateTime) + 
					(periodDateTime.TimeOfDay - GetPreviousWorkingTimeIntervalEnd(periodDateTime).TimeOfDay);
			if (!IsTimeInWorkingInterval(periodDateTime)) {
				 periodDateTime = AddWorkingPeriod(periodDateTime);
			}
			return periodDateTime;
		}

		#endregion

		#region Methods: Public

		public bool IsTimeInWorkingInterval(DateTime registrationDateTime) {
			Entity day = Utils.GetCalendarDay(CalendarIdsChain, registrationDateTime);
			if (day.GetTypedColumnValue<bool>("DayType_IsWeekend")) {
				return false;
			}

			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("Id");
			esq.RowCount = 1;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				day.PrimaryColumnValue));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.GreaterOrEqual, "To",
				registrationDateTime.TimeOfDay));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.LessOrEqual, "From",
				registrationDateTime.TimeOfDay));
			return esq.GetEntityCollection(UserConnection).Any();
		}

		public DateTime GetNextWorkingTimeIntervalStart(DateTime registrationDateTime) {
			KeyValuePair<DateTime, Entity> foundDay = GetNearestNotWeekendDay(registrationDateTime);
			Entity day = foundDay.Value;
			DateTime dateTime = foundDay.Key;
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("From");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				day.PrimaryColumnValue));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.GreaterOrEqual, "From", 
				dateTime.TimeOfDay));

			EntityCollection timeIntervals = esq.GetEntityCollection(UserConnection);
			if (!timeIntervals.Any()) {
				return GetNextWorkingTimeIntervalStart(dateTime.Date.AddDays(1));
			}

			return dateTime.Date + timeIntervals.Min(d => d.GetTypedColumnValue<DateTime>("From").TimeOfDay);
		}

		public DateTime GetPreviousWorkingTimeIntervalEnd(DateTime registrationDateTime) {
			KeyValuePair<DateTime, Entity> foundDay = GetNearestNotWeekendDay(registrationDateTime, false);
			Entity day = foundDay.Value;
			DateTime dateTime = foundDay.Key;

			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("To");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				day.PrimaryColumnValue));
			if (dateTime.TimeOfDay != TimeSpan.Zero) {
				esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.LessOrEqual, "To", 
					dateTime.TimeOfDay));
			}

			EntityCollection timeIntervals = esq.GetEntityCollection(UserConnection);
			if (!timeIntervals.Any()) {
				return GetPreviousWorkingTimeIntervalEnd(dateTime.Date.AddDays(-1));
			}

			return dateTime.Date + timeIntervals.Max(d => d.GetTypedColumnValue<DateTime>("To").TimeOfDay);
		}

		public DateTime GetFirstDateTimeInDay(DateTime registrationDateTime) {
			KeyValuePair<DateTime, Entity> foundDay = GetNearestNotWeekendDay(registrationDateTime, false);
			Entity day = foundDay.Value;
			DateTime dateTime = foundDay.Key;
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("From");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				day.PrimaryColumnValue));
			EntityCollection timeIntervals = esq.GetEntityCollection(UserConnection);
			if (!timeIntervals.Any()) {
				return GetPreviousWorkingTimeIntervalEnd(dateTime.Date.AddDays(-1));
			}
			return dateTime.Date + timeIntervals.Min(d => d.GetTypedColumnValue<DateTime>("From").TimeOfDay);
		}

		public DateTime GetLastDateTimeInDay(DateTime registrationDateTime) {
			KeyValuePair<DateTime, Entity> foundDay = GetNearestNotWeekendDay(registrationDateTime, false);
			Entity day = foundDay.Value;
			DateTime dateTime = foundDay.Key;
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("To");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar", 
				day.PrimaryColumnValue));
			EntityCollection timeIntervals = esq.GetEntityCollection(UserConnection);
			if (!timeIntervals.Any()) {
				return GetPreviousWorkingTimeIntervalEnd(dateTime.Date.AddDays(1));
			}
			return dateTime.Date + timeIntervals.Max(d => d.GetTypedColumnValue<DateTime>("To").TimeOfDay);
		}

		public TimeSpan GetWeekendTimeSpan(DateTime registrationDateTime, DateTime responseDateTime) {
			TimeSpan weekendTimeSpan = responseDateTime - registrationDateTime;
			TimeSpan workingTimeSpan = GetWorkingTimeSpan(registrationDateTime, responseDateTime);
			return weekendTimeSpan > workingTimeSpan ? weekendTimeSpan - workingTimeSpan : TimeSpan.Zero;
		}

		public TimeSpan GetWorkingTimeSpan(DateTime fromDateTime, DateTime toDateTime) {
			bool isNegative = false;
			if (fromDateTime > toDateTime) {
				DateTime tempDateTime = fromDateTime;
				fromDateTime = toDateTime;
				toDateTime = tempDateTime;
				isNegative = true;
			}

			List<Entity> days =
				Utils.GetCalendarDays(CalendarIdsChain, fromDateTime, toDateTime)
					.OrderBy(d => d.GetTypedColumnValue<DateTime>("Date")).ToList();
			Entity fromDay = days.First();
			Entity toDay = days.Last();

			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "WorkingTimeInterval");
			esq.AddColumn("From");
			esq.AddColumn("To");
			esq.AddColumn("DayInCalendar");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "DayInCalendar",
				days.Select(d => d.GetTypedColumnValue<Guid>("Id"))
					.Distinct().Select(d => (object)d).ToArray()));
			List<Entity> timeIntervals =
				esq.GetEntityCollection(UserConnection).ToList();

			TimeSpan workingTimeSpan = TimeSpan.Zero;
			foreach (Entity day in days) {
				var dayId = day.GetTypedColumnValue<Guid>("Id");
				List<Entity> dayTimeIntervals = timeIntervals
					.Where(t => t.GetTypedColumnValue<Guid>("DayInCalendarId") == dayId)
					.ToList();
				foreach (Entity interval in dayTimeIntervals) {
					var from = interval.GetTypedColumnValue<DateTime>("From");
					var to = interval.GetTypedColumnValue<DateTime>("To");
					if (Equals(day, fromDay) && from.TimeOfDay < fromDateTime.TimeOfDay) {
						interval.SetColumnValue("From", fromDateTime);
					}
					if (Equals(day, toDay) && to.TimeOfDay > toDateTime.TimeOfDay) {
						interval.SetColumnValue("To", toDateTime);
					}
					if (from.TimeOfDay < to.TimeOfDay) {
						workingTimeSpan = workingTimeSpan.Add(to.TimeOfDay - from.TimeOfDay);
					}
				}
			}

			return isNegative ? -workingTimeSpan : workingTimeSpan;
		}

		public DateTime AddWorkingDays(DateTime registrationDateTime, int daysCount) {
			if (daysCount < 0) {
				return RemoveWorkingDays(registrationDateTime, Math.Abs(daysCount));
			}
			var registrationDay = Utils.GetCalendarDay(CalendarIdsChain, registrationDateTime, null);
			var firstDateInDay = GetFirstDateTimeInDay(registrationDateTime);
			var lastDateInDay = GetLastDateTimeInDay(registrationDateTime);
			var lessFirstDate = registrationDateTime <= firstDateInDay;
			var moreLastDate = registrationDateTime >= lastDateInDay;
			DateTime fromDay = registrationDateTime.Date.AddDays(daysCount == 0 ? 0 : 1);
			DateTime toDay = registrationDateTime.Date.AddDays(daysCount);

			List<Entity> days = Utils.GetCalendarDays(CalendarIdsChain, fromDay, toDay).ToList();
			int weekendDaysCount = days.Count(d => d.GetTypedColumnValue<bool>("DayType_IsWeekend"));
			while (weekendDaysCount != 0) {
				fromDay = toDay.AddDays(1);
				toDay = toDay.AddDays(weekendDaysCount);
				days = Utils.GetCalendarDays(CalendarIdsChain, fromDay, toDay).ToList();
				weekendDaysCount = days.Count(d => d.GetTypedColumnValue<bool>("DayType_IsWeekend"));
			}
			DateTime maxDate = days.Max(d => d.GetTypedColumnValue<DateTime>("Date"));
			Entity maxDay = days.Single(d => d.GetTypedColumnValue<DateTime>("Date") == maxDate);
			Entity almostMaxDay = GetNearestNotWeekendDay(maxDay.GetTypedColumnValue<DateTime>("Date").AddDays(-1), false).Value;
			var maxDayDate = lessFirstDate ?  GetLastWorkingDayDateTime(almostMaxDay): GetLastWorkingDayDateTime(maxDay);
			maxDay.SetColumnValue("Date", maxDayDate);
			return maxDayDate;
		}

		public TimeSpan GetWorkingTimeSpanByWorkingDays(DateTime fromDate, DateTime toDay) {
			bool isNegative = false;
			if (fromDate > toDay) {
				DateTime tempDateTime = fromDate;
				fromDate = toDay;
				toDay = tempDateTime;
				isNegative = true;
			}

			DateTime fromDay = fromDate.Date.AddDays(1);

			List<Entity> days = Utils.GetCalendarDays(CalendarIdsChain, fromDay, toDay).ToList();
			int workingDaysCount = days.Count(d => !d.GetTypedColumnValue<bool>("DayType_IsWeekend"));

			var workingTimeSpan = new TimeSpan(workingDaysCount, 0, 0, 0);
			return isNegative ? -workingTimeSpan : workingTimeSpan;
		}

		#endregion
	}

	#endregion
}
