namespace MatomoConnector.API
{
	using System;

	#region Class: MatomoPeriod

	public class MatomoPeriod
	{

		#region Fields: Private

		private readonly string _period;

		#endregion

		#region Fields: Public

		public static readonly MatomoPeriod Day = new MatomoPeriod("day");
		public static readonly MatomoPeriod Week = new MatomoPeriod("week");
		public static readonly MatomoPeriod Month = new MatomoPeriod("month");
		public static readonly MatomoPeriod Year = new MatomoPeriod("year");
		public static readonly MatomoPeriod Range = new MatomoPeriod("range");

		#endregion

		#region Constructors: Private

		private MatomoPeriod(string period) => _period = period;

		#endregion

		#region Methods: Public

		public override string ToString() => _period;

		#endregion

	}

	#endregion
}
