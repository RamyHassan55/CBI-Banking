using System;
using System.Collections.Generic;

namespace Terrasoft.Configuration.ServiceTerm
{
	/// <summary>
	/// Term parameters.
	/// </summary>
	public class TermParameters
	{
		private static readonly int MinTimeUnitValue = 1;

		/// <summary>
		/// Calendar identifier.
		/// </summary>
		public Guid CalendarId {
			get;
			set;
		}
		/// <summary>
		/// Parameters for response date calculation.
		/// </summary>
		public KeyValuePair<Calendars.TimeUnit, int> ResponseParams {
			get;
			set;
		}
		/// <summary>
		/// Parameters for solution date calculation.
		/// </summary>
		public KeyValuePair<Calendars.TimeUnit, int> SolutionParams {
			get;
			set;
		}

		/// <summary>
		/// Returns that solution parameters are correct.
		/// </summary>
		/// <returns>Are solution params correct.</returns>
		public bool IsCorrectSolutionParams() {
			return SolutionParams.Value >= MinTimeUnitValue;
		}

		/// <summary>
		/// Returns that response parameters are correct.
		/// </summary>
		/// <returns>Are parameters correct.</returns>
		public bool IsCorrectResponseParams() {
			return ResponseParams.Value >= MinTimeUnitValue;
		}

		/// <summary>
		/// Returns that term parameters are correct.
		/// </summary>
		/// <returns>Are term parameters correct.</returns>
		public bool IsCorectParams() {
			return IsCorrectResponseParams() && IsCorrectSolutionParams() && CalendarId != Guid.Empty;
		}
	}
}

