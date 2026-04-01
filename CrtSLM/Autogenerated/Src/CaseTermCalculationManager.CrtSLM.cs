namespace Terrasoft.Configuration
{
	using System;
	using Creatio.FeatureToggling;
	using global::Common.Logging;
	using Terrasoft.Configuration.Calendars;
	using Terrasoft.Configuration.TermCalculationService;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Class for case term calculation.
	/// </summary>
	public class CaseTermCalculationManager
	{
		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Properties: Protected

		/// <summary>
		/// <see cref="ILog"/> implementation instance.
		/// </summary>
		private ILog _termsLogger;
		protected ILog CaseTermCalculationLogger {
			get {
				return _termsLogger ?? (_termsLogger = LogManager.GetLogger("CaseTermCalculation"));
			}
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes new instance of <see cref="CaseTermCalculationManager"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public CaseTermCalculationManager(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Calculates term for case entity.
		/// </summary>
		/// <param name="entityCase">Case entity.</param>
		public void Calculate(Entity entityCase) {
			try {
				var parameterReader = ClassFactory.Get<CaseCalculationParameterReader>(
					new ConstructorArgument("userConnection", _userConnection));
				var dictionaryParams = parameterReader.GetParams(entityCase);
				var caseTermCalculateEntryPoint = ClassFactory.Get<CaseTermCalculateEntryPoint>(
									new ConstructorArgument("userConnection", _userConnection));
				ServiceTermResponse response = caseTermCalculateEntryPoint.CalculateTerms(dictionaryParams, entityCase.GetTypedColumnValue<DateTime>("RegisteredOn"));
				entityCase.SetColumnValue("SolutionDate", response.SolutionTime);
				if (Features.GetIsEnabled("DoNotRecalculateResponseDateIfAlreadyResponded")) {
					if (entityCase.GetTypedColumnValue<DateTime>("RespondedOn") == default(DateTime) || entityCase.GetTypedColumnValue<DateTime>("ResponseDate") == default(DateTime)) {
						entityCase.SetColumnValue("ResponseDate", response.ReactionTime);
					}
				} else {
					entityCase.SetColumnValue("ResponseDate", response.ReactionTime);
				}
			} catch (Exception ex) {
				CaseTermCalculationLogger.Error($"Term calculaion error in case ${entityCase.PrimaryColumnValue}: ${ex.Message}");
			}
		}

		#endregion

	}
}
