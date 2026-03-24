namespace MatomoConnector.API
{

	#region Class: SimpleParameter

	public class SimpleParameter : Parameter
	{

		#region Fields: Private

		private string _value;

		#endregion

		#region Constructors: Public

		public SimpleParameter(string name, string value) : base(name) => _value = value;

		public SimpleParameter(string name, bool value) : this(name, value ? "1" : "0") { }

		public SimpleParameter(string name, int value) : this(name, value.ToString()) { }

		#endregion

		#region Methods: Public

		public override string ToString() {
			if (string.IsNullOrWhiteSpace(_value)) {
				return string.Empty;
			}
			return GetUrlString(_value);
		}

		#endregion

	}

	#endregion

}
