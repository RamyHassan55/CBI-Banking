namespace MatomoConnector.API
{
	using System.Web;

	#region Class: Parameter

	public abstract class Parameter
	{

		#region Constructors: Public

		public Parameter(string name) => Name = UrlEncode(name);

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Property name.
		/// </summary>
		protected string Name { get; set; }

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Encodes URL string.
		/// </summary>
		protected string UrlEncode(string value) => HttpUtility.UrlEncode(value);

		/// <summary>
		/// Returns string with encoded url parameter.
		/// </summary>
		/// <example>
		/// &Name=value%20value
		/// </example>
		protected virtual string GetUrlString(string value) => $"&{Name}={UrlEncode(value)}";

		#endregion

		#region Methods: Public

		public override bool Equals(object obj) => Name?.Equals((obj as Parameter)?.Name) ?? (obj == null);

		public override int GetHashCode() => Name.GetHashCode();

		public override string ToString() => Name;

		#endregion

	}

	#endregion

}
