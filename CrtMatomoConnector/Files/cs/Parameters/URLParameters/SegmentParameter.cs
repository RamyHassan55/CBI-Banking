namespace MatomoConnector.API
{
	using System.Text;

	#region Class: SegmentParameter

	public class SegmentParameter : Parameter
	{

		#region Fields: Private

		private MatomoSegment _segment;

		#endregion

		#region Constructors: Public

		public SegmentParameter(MatomoSegment segment) : base("segment") {
			_segment = segment;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns string representation of segment parameter.
		/// </summary>
		/// <example>
		/// &segment=countryCode==US
		/// or
		/// &segment=countryCode==US,countryCode==DE
		/// </example>
		public override string ToString() {
			if (_segment == default) {
				return string.Empty;
			}
			return GetUrlString(_segment.ToString());
		}

		#endregion
	}

	#endregion

}
