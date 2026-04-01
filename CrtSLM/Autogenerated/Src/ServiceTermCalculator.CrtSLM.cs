using System;
using System.Collections.Generic;
using Terrasoft.Configuration.Calendars;
using Terrasoft.Core;

namespace Terrasoft.Configuration.ServiceTerm
{
	/// <summary>
	/// Represents service term calculator.
	/// </summary>
	public class ServiceTermCalculator : ITermCalculator
	{
		#region Fields

		private readonly UserConnection _userConnection;
		private readonly ISpecificationTermParameters _specification;
		private readonly DateTime _date;

		#endregion

		#region Properties

		/// <summary>
		/// Calendar utility.
		/// </summary>
		private CalendarUtility _calendarUtility;
		public CalendarUtility Utility {
			get {
				return _calendarUtility ?? (_calendarUtility = new CalendarUtility(_userConnection));
			}
			set {
				_calendarUtility = value;
			}
		}

		/// <summary>
		/// Term parameters.
		/// </summary>
		private TermParameters _termParameters;
		protected TermParameters TermParameters {
			get {
				return _termParameters ?? (_termParameters = _specification.GetTermParameters());
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Create new service term calculator item.
		/// </summary>
		/// <param name="parameters">Term parameters.</param>
		/// <param name="utility">Calendar utility.</param>
		public ServiceTermCalculator(TermParameters parameters, CalendarUtility utility) {
			_termParameters = parameters;
			_calendarUtility = utility;
		}

		/// <summary>
		/// Create new service term calculator item.
		/// </summary>
		/// <param name="conditions">Term parameter select condition.</param>
		/// <param name="userConnection">User connection.</param>
		public ServiceTermCalculator(Dictionary<string, object> conditions, UserConnection userConnection) {
			_userConnection = userConnection;
			var factory = new TermProviderFactory();
			_specification = factory.GetSpecificationTermParameters(conditions, _userConnection);
			if (conditions.ContainsKey("Date")) {
				_date = DateTime.Parse(conditions["Date"].ToString());
			}

		}

		#endregion

		#region Methods

		/// <summary>
		/// Calculate response date by service term.
		/// </summary>
		/// <returns>Response date.</returns>
		public DateTime CalculateResponseDate() {
			if (_date == DateTime.MinValue) {
				throw new ArgumentException();
			}
			return CalculateResponseDate(_date);
		}

		/// <summary>
		/// Calculate response date by service term.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <returns>Response date.</returns>
		public DateTime CalculateResponseDate(DateTime startDate) {
			if (_userConnection == null) {
				throw new ArgumentException();
			}
			return CalculateResponseDate(startDate, _userConnection.CurrentUser.TimeZone);
		}

		/// <summary>
		/// Calculate response date by service term and time zone.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="timeZone">Time zone info.</param>
		/// <returns>Response date.</returns>
		public DateTime CalculateResponseDate(DateTime startDate, TimeZoneInfo timeZone) {
			var responseCalculationParams = TermParameters.ResponseParams;
			TimeTerm timeTerm = new TimeTerm {
				Type = responseCalculationParams.Key,
				Value = responseCalculationParams.Value,
				CalendarId = TermParameters.CalendarId
			};
			return Utility.Add(startDate, timeTerm, timeZone);
		}

		/// <summary>
		/// Calculate solution date by service term and time zone.
		/// </summary>

		/// <returns>Solution date.</returns>
		public DateTime CalculateSolutionDate() {
			if (_date == DateTime.MinValue) {
				throw new ArgumentException();
			}
			return CalculateSolutionDate(_date);
		}

		/// <summary>
		/// Calculate solution date by service term and time zone.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <returns>Solution date.</returns>
		public DateTime CalculateSolutionDate(DateTime startDate) {
			if (_userConnection == null) {
				throw new ArgumentException();
			}
			return CalculateSolutionDate(startDate, _userConnection.CurrentUser.TimeZone);
		}

		/// <summary>
		/// Calculate solution date by service term and time zone.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="timeZone">Time zone info.</param>
		/// <returns>Solution date.</returns>
		public DateTime CalculateSolutionDate(DateTime startDate, TimeZoneInfo timeZone) {
			var responseCalculationParams = TermParameters.SolutionParams;
			TimeTerm timeTerm = new TimeTerm {
				Type = responseCalculationParams.Key,
				Value = responseCalculationParams.Value,
				CalendarId = TermParameters.CalendarId
			};
			return Utility.Add(startDate, timeTerm, timeZone);
		}

		/// <summary>
		/// Calculate solution date after pause.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="timeUnit">Time unit.</param>
		/// <param name="value">Time unit value.</param>
		/// <returns>Solution date.</returns>
		public DateTime CalculateSolutionDateAfterPause(DateTime startDate, Calendars.TimeUnit timeUnit, int value) {
			return CalculateSolutionDateAfterPause(startDate, timeUnit, value, _userConnection.CurrentUser.TimeZone);
		}

		/// <summary>
		/// Calculate solution date after pause.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="timeUnit">Time unit.</param>
		/// <param name="value">Time unit value.</param>
		/// <param name="timeZone">Time zone.</param>
		/// <returns>Solution date.</returns>
		public DateTime CalculateSolutionDateAfterPause(DateTime startDate, Calendars.TimeUnit timeUnit, int value, TimeZoneInfo timeZone) {
			if (_userConnection == null) {
				throw new ArgumentException();
			}
			startDate = _userConnection.CurrentUser.GetCurrentDateTime();
			var parameters = GetTimeUnitDifference(new KeyValuePair<Calendars.TimeUnit, int>(timeUnit, value));
			TimeTerm timeTerm = new TimeTerm {
				Type = parameters.Key,
				Value = parameters.Value,
				CalendarId = TermParameters.CalendarId
			};
			return Utility.Add(startDate, timeTerm, timeZone);
		}

		protected virtual KeyValuePair<Calendars.TimeUnit, int> GetTimeUnitDifference(KeyValuePair<Calendars.TimeUnit, int> processingParams) {
			return GetTimeUnitDifference(TermParameters.SolutionParams, processingParams);
		}

		protected virtual KeyValuePair<Calendars.TimeUnit, int> GetTimeUnitDifference(KeyValuePair<Calendars.TimeUnit, int> sourceParams,
			KeyValuePair<Calendars.TimeUnit, int> processingParams) {
			var timeUnit = sourceParams.Key;
			var timeUnitValue = 0;
			if (sourceParams.Key != processingParams.Key) {
				if (sourceParams.Key.IsCalendarUnit() && processingParams.Key.IsCalendarUnit()) {
					var sourceTimeUnitValue = ConvertTimeUnitValueToMinute(sourceParams.Key, sourceParams.Value);
					var processingTimeUnitValue = ConvertTimeUnitValueToMinute(sourceParams.Key, sourceParams.Value);
					timeUnitValue = sourceTimeUnitValue - processingTimeUnitValue;
					return new KeyValuePair<Calendars.TimeUnit, int>(Calendars.TimeUnit.Minute, timeUnitValue);
				}
			}
			timeUnitValue = sourceParams.Value - processingParams.Value;
			return new KeyValuePair<Calendars.TimeUnit, int>(timeUnit, timeUnitValue);
		}

		private int ConvertTimeUnitValueToMinute(Calendars.TimeUnit timeUnit, int value) {
			switch (timeUnit) {
				case Calendars.TimeUnit.Minute:
					return value;
				case Calendars.TimeUnit.Hour:
					return value * 60;
				case Calendars.TimeUnit.Day:
					return value * 60 * 24;
				default:
					throw new ArgumentException();
			}
		}

		#endregion
	}
}
