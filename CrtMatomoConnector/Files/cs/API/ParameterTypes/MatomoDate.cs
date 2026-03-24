namespace MatomoConnector.API
{
	using System;

	#region Enum: MatomoRange

	public enum MatomoRange
	{
		last,
		previous
	}

	#endregion

	#region Enum: MatomoMagicDate

	public enum MatomoMagicDate
	{
		today,
		yesterday
	}

	#endregion

	#region Class: MatomoDate

	/// <summary>
	/// Formatted and stringified date.
	/// </summary>
	public class MatomoDate
	{

		#region Fields: Private

		private readonly string _dateString;

		#endregion

		#region Constructors: Private

		private MatomoDate(string dateString) => _dateString = dateString;

		#endregion

		#region Methods: Public

		/// <summary>
		/// Simple date.
		/// </summary>
		/// <example>
		/// 2000-01-01
		/// </example>
		public static MatomoDate Get(DateTime date) => new MatomoDate(FormatDate(date));

		/// <summary>
		/// Range date.
		/// </summary>
		/// <example>
		/// 2000-01-01,2000-01-02
		/// </example>
		public static MatomoDate Get(DateTime start, DateTime end) => new MatomoDate(GetRangeValue(start, end));

		/// <summary>
		/// Relative Range date.
		/// </summary>
		/// <example>
		/// last1
		/// previous365
		/// </example>
		public static MatomoDate Get(MatomoRange range, uint daysNumber) => new MatomoDate(GetRelativeRangeValue(range, daysNumber));

		/// <summary>
		/// Magic date.
		/// </summary>
		/// <example>
		/// today
		/// yesterday
		/// </example>
		public static MatomoDate Get(MatomoMagicDate magicDate) => new MatomoDate(magicDate.ToString());

		/// <summary>
		/// Returns range with the magic date.
		/// </summary>
		/// <example>
		/// 2000-01-01,today
		/// </example>
		public static MatomoDate Get(DateTime startDate, MatomoMagicDate magicDate) {
			var dateString = $"{FormatDate(startDate)},{magicDate}";
			return new MatomoDate(dateString);
		}

		#endregion

		#region Methods: Private

		private static string FormatDate(DateTime date) => string.Format("{0:yyyy-MM-dd}", date);

		private static string GetRangeValue(DateTime start, DateTime end) {
			var dateDiff = DateTime.Compare(start, end);
			var formattedStart = dateDiff < 0 ? FormatDate(start) : FormatDate(end);
			var formattedEnd = dateDiff < 0 ? FormatDate(end) : FormatDate(start);
			return $"{formattedStart},{formattedEnd}";
		}

		private static string GetRelativeRangeValue(MatomoRange range, uint daysNumber) => $"{range}{daysNumber}";

		#endregion

		#region Methods: Public

		public override string ToString() => _dateString;

		#endregion

	}

	#endregion
}
