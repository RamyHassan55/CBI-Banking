namespace MatomoConnector.API
{
	using System;

	/// <summary>
	/// Implements exception type for empty responses.
	/// </summary>
	public class EmptyResponseException : Exception
	{
		#region Fields: Private
		
		private readonly string _url;

		#endregion

		#region Constructors: Public

		public EmptyResponseException(string url) : base() {
			_url = url;
		}

		#endregion

		#region Properties: Public

		public override string Message => $"Response from {_url} doesn't contain any data.";
		
		#endregion

	}
}
