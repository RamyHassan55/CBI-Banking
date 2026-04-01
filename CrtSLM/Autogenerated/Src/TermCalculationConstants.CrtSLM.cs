using System;

namespace Terrasoft.Configuration
{
	
	#region Class: ResponseTimeColumnsConfig

	public class ResponseTimeColumnsConfig
	{
		#region Properties: Public

		public string ValueColumnName { 
			get; 
			set; 
		}

		public string TimeUnitColumnName { 
			get; 
			set; 
		}

		#endregion
	}

	#endregion
	
	#region Class: TermCalculationConstants
	
	public static class TermCalculationConstants
	{
		#region Constants: Public
		
		public static readonly string BaseCalendarSysSettingsCode = "BaseCalendar";
		
		//NOTE: TimeUnit
		public static readonly Guid CalendarDayTimeUnitId = new Guid("36DF302E-5AB6-43A0-AEC7-45C2795D839D");

		public static readonly Guid CalendarHourTimeUnitId = new Guid("B788B4DE-5AE9-42E2-AF34-CD3AD9E6C96F");

		public static readonly Guid CalendarMinuteTimeUnitId = new Guid("48B4FF98-E3BF-4F59-A6CF-284E4084FB2F");

		public static readonly Guid WorkingDayTimeUnitId = new Guid("BDCBB819-9B26-4627-946F-D00645A2D401");

		public static readonly Guid WorkingHourTimeUnitId = new Guid("2A608ED7-D118-402A-99C0-2F583291ED2E");

		public static readonly Guid WorkingMinuteTimeUnitId = new Guid("3AB432A6-CA84-4315-BA33-F343C758A8F0");

		//NOTE: ResponseTimeColumnsConfig
		public static readonly ResponseTimeColumnsConfig ReactionTimeColumnsConfig = new ResponseTimeColumnsConfig {
			TimeUnitColumnName = "ReactionTimeUnitId",
			ValueColumnName = "ReactionTimeValue"
		};

		public static readonly ResponseTimeColumnsConfig SolutionTimeColumnsConfig = new ResponseTimeColumnsConfig {
			TimeUnitColumnName = "SolutionTimeUnitId",
			ValueColumnName = "SolutionTimeValue"
		};
		
		#endregion
	}
	
	#endregion
}
