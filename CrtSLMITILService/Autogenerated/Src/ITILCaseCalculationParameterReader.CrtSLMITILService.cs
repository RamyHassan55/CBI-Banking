namespace Terrasoft.Configuration
{
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;


	/// <summary>
	/// Forms parameters for CaseEntryPoint
	/// </summary>
	[Override]
	public class ITILCaseCalculationParameterReader: CaseCalculationParameterReader
	{
		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="ITILCaseCalculationParameterReader"/> class.
		/// </summary>
		public ITILCaseCalculationParameterReader() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ITILCaseCalculationParameterReader"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public ITILCaseCalculationParameterReader(UserConnection userConnection)
			: base(userConnection) {
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Get list of params for calculation.
		/// </summary>
		/// <returns>List of params for calculation.</returns>
		protected override Dictionary<string, string> GetParamsDictionary() {
			Dictionary<string, string> result = base.GetParamsDictionary();
			result.Add("ServicePact.Id", "ServicePactId");
			result.Add("SupportLevel.Id", "SupportLevelId");
			return result;
		}

		/// <summary>
		/// Returns list of calculation parameters for case.
		/// </summary>
		/// <returns>List of calculation parameters.</returns>	
		protected override Dictionary<string, string> GetCaseEntityParamsDictionary() {
			Dictionary<string, string> result = base.GetCaseEntityParamsDictionary();
			result.Add("ServicePactId", "ServicePactId");
			result.Add("SupportLevelId", "SupportLevelId");
			return result;
		}

		#endregion
	}
}
