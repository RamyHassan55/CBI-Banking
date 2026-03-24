namespace CrtDigitalAdsApp.Utilities
{
	using System.Collections.Generic;
	using CrtCloudIntegration.Utilities.Errors;

	/// <summary>
	/// Collection of validation errors
	/// </summary>
	public class ValidationErrors
	{

		#region Fields: Private

		private readonly List<IError> _errors;

		#endregion

		#region Constructors: Public

		public ValidationErrors() {
			_errors = new List<IError>();
		}

		#endregion

		#region Properties: Public

		public IReadOnlyList<IError> Errors => _errors;

		#endregion

		#region Methods: Public

		public IReadOnlyList<IError> Add(IError error) {
			_errors.Add(error);
			return Errors;
		}

		#endregion

	}
}