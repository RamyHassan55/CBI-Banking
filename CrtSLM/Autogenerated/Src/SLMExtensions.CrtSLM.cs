namespace Terrasoft.Configuration.SLMExtensions
{
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.Calendars;
	using CalendarTimeUnit = Terrasoft.Configuration.Calendars.TimeUnit;

	#region Class : SLMExtensions

	/// <summary>
	/// Specific SLM extention methods.
	/// </summary>
	public static class SLMExtensions
	{

		#region Methods : Public

		/// <summary>
		/// Converts current time term to the corresponding time term with minute time unit.
		/// </summary>
		/// <param name="term">Current term</param>
		/// <returns>Time term.</returns>
		public static TimeTerm ConvertToMinutes(this TimeTerm term) {
			var result = new TimeTerm();
			result.CalendarId = term.CalendarId;
			switch (term.Type) {
				case CalendarTimeUnit.Hour:
					result.Type = CalendarTimeUnit.Minute;
					result.Value = term.Value * CalendarConsts.MinutesInHour;
					break;
				case CalendarTimeUnit.WorkingHour:
					result.Type = CalendarTimeUnit.WorkingMinute;
					result.Value = term.Value * CalendarConsts.MinutesInHour;
					break;
				case CalendarTimeUnit.Day:
					result.Type = CalendarTimeUnit.Minute;
					result.Value = term.Value * CalendarConsts.MinutesInHour * CalendarConsts.HoursInDay;
					break;
				default:
					result = term;
					break;
			}
			result.NativeTimeTerm = new TimeTerm {
				CalendarId = term.CalendarId,
				Type = term.Type,
				Value = term.Value
			};
			return result;
		}

		#endregion

	}

	#endregion

}
