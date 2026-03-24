namespace MatomoConnector.API
{
	using System.Text;

	#region Class: ArrayParameter

	public class ArrayParameter : Parameter
	{

		#region Fields: Private

		private readonly string[] Values;
		private readonly bool IsArraySyntax;

		#endregion

		#region Constructors: Public

		public ArrayParameter(string name, string[] values, bool isArraySyntax) : this(name, values) {
			IsArraySyntax = isArraySyntax;
		}

		public ArrayParameter(string name, string[] values) : base(name) {
			Values = values;
			IsArraySyntax = true;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns string representation of array parameter.
		/// </summary>
		/// <example>
		/// &Name=1,2,3
		/// or
		/// &Name[0]=1&Name[1]=2&Name[2]=3
		/// </example>
		public override string ToString() {
			if (Values == default) {
				return string.Empty;
			}
			var urlString = new StringBuilder();
			if (IsArraySyntax) {
				for (int i = 0; i < Values.Length; i++) {
					var param = $"&{Name}[{i}]={UrlEncode(Values[i])}";
					urlString.Append(param);
				}
			} else {
				urlString.Append(GetUrlString(string.Empty));
				for (int i = 0; i < Values.Length; i++) {
					urlString.Append($"{UrlEncode(Values[i])},");
				}
			}
			return urlString.ToString();
		}

		#endregion
	}

	#endregion

}
