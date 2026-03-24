namespace MatomoConnector.API
{
	using System;

	/// <summary>
	/// Implements exception type for auth errors.
	/// </summary>
	public class AuthException : Exception
	{

		#region Properties: Public

		public override string Message => $"Unable to send request because URL or API_KEY is incorrect.";
		
		#endregion

	}
}
