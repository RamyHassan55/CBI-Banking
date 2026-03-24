namespace MatomoConnector.API
{
	using System.Collections.Generic;
	using System.Text;

	#region Class: DictionaryParameter

	public class DictionaryParameter : Parameter
	{

		#region Fields: Private

		private readonly Dictionary<string, object> Values;

		#endregion

		#region Constructors: Public

		public DictionaryParameter(string name, Dictionary<string, object> values) : base(name) => Values = values;

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns string representation of dictionary parameter.
		/// </summary>
		/// <example>
		/// &Name[key1]=value1&Name[key2]=vale2
		/// &Name[key][]=value1&Name[key][]=value2
		public override string ToString() {
			if (Values == default) {
				return string.Empty;
			}
			var urlString = new StringBuilder();
			foreach (var keyValue in Values) {
				if (keyValue.Value == default) {
					continue;
				}
				if (keyValue.Value is string[]) {
					var array = keyValue.Value as string[];
					foreach (var str in array) {
						var parameter = $"&{Name}[{keyValue.Key}][]={UrlEncode(str)}";
						urlString.Append(parameter);
					}
				} else {
					var parameter = $"&{Name}[{keyValue.Key}]={UrlEncode(keyValue.Value.ToString())}";
					urlString.Append(parameter);
				}
			}
			return urlString.ToString();
		}

		#endregion

	}

	#endregion

}