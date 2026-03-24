namespace MatomoConnector.API
{
	using System;

	/// <summary>
	/// Implements exception type for Matomo errors.
	/// </summary>
	public class MatomoErrorException : Exception
	{
		#region Fields: Private
		
		private readonly MatomoError _errorDetails;

		#endregion

		#region Constructors: Public

		public MatomoErrorException(MatomoError errorDetails) : base() {
			_errorDetails = errorDetails;
		}

		#endregion

		#region Properties: Public

		public override string Message => $"{_errorDetails.result} : {_errorDetails.message}";
		
		#endregion

	}
}
