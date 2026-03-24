namespace Terrasoft.Configuration.Calendars
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.TermCalculationService;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Store;

	/// <summary>
	/// Entry point for calculation terms
	/// </summary>
	public class CaseTermCalculateEntryPoint
	{

		#region Fields: Private

		private readonly TermCalculationLogStore _calculationLogStore;

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

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermCalculateEntryPoint"/> class.
		/// </summary>
		public CaseTermCalculateEntryPoint() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermCalculateEntryPoint"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public CaseTermCalculateEntryPoint(UserConnection userConnection) {
			UserConnection = userConnection;
			_calculationLogStore = TermCalculationLogStoreInitializer.GetStore(userConnection);
		}

		#endregion

		#region Methods: Private

		private DateTime ConvertFromUtc(DateTime dateTime, TimeZoneInfo timeZoneInfo) {
			return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
		}

		private ServiceTermResponse ExecuteCalculateTerms(DateTime startDate, CaseTermInterval termInterval,
				TimeZoneInfo userTimeZone, CaseTermStates mask) {
			var calendarutility = new CalendarUtility(UserConnection);
			var response = new ServiceTermResponse();
			response.ReactionTime = mask.HasFlag(CaseTermStates.ContainsResponse)
				? calendarutility.Add(startDate, termInterval.ResponseTerm, userTimeZone) as DateTime?
				: null;
			response.SolutionTime = mask.HasFlag(CaseTermStates.ContainsResolve)
				? calendarutility.Add(startDate, termInterval.ResolveTerm, userTimeZone) as DateTime?
				: null;
			return response;
		}

		private ServiceTermResponse ExecuteRecalculateTerms(DateTime startDate, CaseTermInterval termInterval,
				IEnumerable<DateTimeInterval> intervals, TimeZoneInfo userTimeZone, CaseTermStates mask) {
			var calendarutility = new CalendarUtility(UserConnection);
			var response = new ServiceTermResponse();
			var dateTime = ConvertFromUtc(DateTime.UtcNow, userTimeZone);
			response.ReactionTime = mask.HasFlag(CaseTermStates.ContainsResponse)
				? calendarutility.Add(dateTime, termInterval.ResponseTerm, intervals, userTimeZone) as DateTime?
				: null;
			response.SolutionTime = mask.HasFlag(CaseTermStates.ContainsResolve)
				? calendarutility.Add(dateTime, termInterval.ResolveTerm, intervals, userTimeZone) as DateTime?
				: null;
			return response;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Counts reaction time and a solution time to Case.
		/// </summary>
		/// <param name="arguments">Dictionary with params for strategies.</param>
		/// <param name="startDate">Start date for calculation.</param>
		/// <returns>An object that contains the reaction time and solution time.</returns>
		public ServiceTermResponse CalculateTerms(Dictionary<string, object> arguments, DateTime startDate) {
			var response = new ServiceTermResponse();
			var userConnectionArg = new ConstructorArgument("userConnection", UserConnection);
			var argumentsArg = new ConstructorArgument("arguments", arguments);
			var selector = ClassFactory.Get<CaseTermIntervalSelector>(userConnectionArg);
			var termInterval = selector.Get(arguments) as CaseTermInterval;
			CaseTermStates mask;
			if (_calculationLogStore != null) {
				mask = new TermCalculationLogger(_calculationLogStore).GetCaseTermState(termInterval);
			} else {
				mask = termInterval.GetMask();
			}
			if (mask != CaseTermStates.None) {
				var intervalReader = ClassFactory.Get<CaseActiveIntervalReader>(userConnectionArg, argumentsArg);
				var userTimeZone = UserConnection.CurrentUser.TimeZone;
				IEnumerable<DateTimeInterval> intervals = intervalReader.GetActiveIntervals();
				if (intervals.IsEmpty()) {
					response = ExecuteCalculateTerms(startDate, termInterval, userTimeZone, mask);
				} else {
					var dateTime = ConvertFromUtc(DateTime.UtcNow, userTimeZone);
					response = ExecuteRecalculateTerms(dateTime, termInterval, intervals, userTimeZone, mask);
				}
			}
			return response;
		}

		/// <summary>
		/// Start calculation reaction time and a solution time to Case.
		/// </summary>
		/// <param name="arguments">Dictionary with params for strategies.</param>
		/// <param name="startDate">Start date for calculation.</param>
		/// <returns></returns>
		public ServiceTermResponse ForceCalculateTerms(Dictionary<string, object> arguments, DateTime startDate) {
			var response = new ServiceTermResponse();
			var userConnectionArg = new ConstructorArgument("userConnection", UserConnection);
			var selector = ClassFactory.Get<CaseTermIntervalSelector>(userConnectionArg);
			var termInterval = selector.Get(arguments) as CaseTermInterval;
			var mask = termInterval.GetMask();
			if (mask != CaseTermStates.None) {
				var userTimeZone = UserConnection.CurrentUser.TimeZone;
				response = ExecuteCalculateTerms(startDate, termInterval, userTimeZone, mask);
			}
			return response;
		}

		#endregion
	}
}
