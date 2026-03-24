namespace MatomoConnector.API
{
	using System;

	#region Class: DateParameter

	public class DateParameter : Parameter
	{

		#region Fields: Private

		private readonly MatomoDate Date;

		#endregion

		#region Constructors: Public

		public DateParameter(string name, MatomoDate date) : base(name) => Date = date;

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns string representation of date parameter.
		/// </summary>
		/// <example>&Name=2000-01-01</example>
		public override string ToString() {
			if (Date == default) {
				return string.Empty;
			}
			return GetUrlString(Date.ToString());
		}

		#endregion

	}

	#endregion

}
