namespace MatomoConnector.API
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;

	#region Class: ParametersCollection

	/// <summary>
	/// Represents collection of URL parameters.
	/// </summary>
	public class ParametersCollection : ICollection<Parameter>
	{

		#region Fields: Private
		
		private readonly List<Parameter> _parameters = new List<Parameter>();

		#endregion

		#region Properties: Public

		public int Count => _parameters.Count;
		public bool IsReadOnly => false;

		#endregion

		#region Methods: Public

		public void Add(Parameter item) => _parameters.Add(item);

		public void AddOnce(Parameter item) {
			if (Contains(item)) {
				return;
			}
			_parameters.Add(item);
		}

		public void AddOrReplace(Parameter item) {
			if (Contains(item)) {
				Remove(item);
			}
			Add(item);
		}

		public void Clear() => _parameters.Clear();

		public bool Contains(Parameter item) => _parameters.Contains(item);

		public void CopyTo(Parameter[] array, int arrayIndex) => _parameters.CopyTo(array, arrayIndex);

		public IEnumerator<Parameter> GetEnumerator() => _parameters.GetEnumerator();

		public bool Remove(Parameter item) => _parameters.Remove(item);

		IEnumerator IEnumerable.GetEnumerator() => _parameters.GetEnumerator();

		public override string ToString() {
			var sb = new StringBuilder();
			_parameters.ForEach(x => sb.Append(x));
			return sb.ToString();
		}

		#endregion

	}

	#endregion

}
