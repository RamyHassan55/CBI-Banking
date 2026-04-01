namespace Terrasoft.Configuration
{
	using global::Common.Logging;
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Entities;

	/// <summary>
	/// Forms parameters for CaseEntryPoint
	/// </summary>
	public class CaseCalculationParameterReader
	{

		#region Properties : Protected

		/// <summary>
		/// Gets the user connection.
		/// </summary>
		/// <value>
		/// The user connection.
		/// </value>
		protected UserConnection UserConnection {
			get;
			private set;
		}

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
		/// Initializes a new instance of the <see cref="CaseCalculationParameterReader"/> class.
		/// </summary>
		public CaseCalculationParameterReader() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseCalculationParameterReader"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public CaseCalculationParameterReader(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private bool isNewCase(Entity caseEntity) {
			return caseEntity.StoringState == StoringObjectState.New;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Get list of params for calculation.
		/// </summary>
		/// <returns>List of params for calculation.</returns>
		protected virtual Dictionary<string, string> GetParamsDictionary() {
			return new Dictionary<string, string>() {
				{"Id", "CaseId"},
				{"Status.Id", "StatusId"},
				{"Priority.Id", "PriorityId"},
				{"ServiceItem.Id", "ServiceItemId"},
				{"SolutionOverdue", "SolutionOverdue"}
			};
		}

		/// <summary>
		/// Returns list of calculation parameters for case.
		/// </summary>
		/// <returns>List of calculation parameters.</returns>
		protected virtual Dictionary<string, string> GetCaseEntityParamsDictionary() {
			return new Dictionary<string, string>() {
				{"Id", "CaseId"},
				{"StatusId", "StatusId"},
				{"PriorityId", "PriorityId"},
				{"ServiceItemId", "ServiceItemId"},
				{"SolutionOverdue", "SolutionOverdue"}
			};
		}

		/// <summary>
		/// Get dictionary of params and values for calculation.
		/// </summary>
		/// <param name="paramsDictionary">List of params.</param>
		/// <param name="caseId">Id of current case.</param>
		/// <returns>Dictionary of params and values for calculation.</returns>
		protected virtual Dictionary<string, object> GetParamsDictionary(Dictionary<string, string> paramsDictionary, Guid caseId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			Dictionary<string, string> paramNames = new Dictionary<string, string>();
			foreach (var name in paramsDictionary.Keys) {
				paramNames.Add(esq.AddColumn(name).Name, paramsDictionary[name]);
			}
			Entity entity = esq.GetEntity(UserConnection, caseId);
			Dictionary<string, object> resultDictionary = new Dictionary<string, object>();
			foreach (var name in paramNames.Keys) {
				resultDictionary.Add(paramNames[name], entity.GetColumnValue(name));
			}
			return resultDictionary;
		}


		/// <summary>
		/// Get dictionary of params and values for calculation.
		/// </summary>
		/// <param name="paramsDictionary">List of params.</param>
		/// <param name="caseEntity">Current case entity.</param>
		/// <returns>Dictionary of params and values for calculation.</returns>
		protected virtual Dictionary<string, object> GetCaseEntityParamsDictionary(Dictionary<string, string> paramsDictionary, Entity caseEntity) {
			Dictionary<string, object> calculationParamsDictionary = new Dictionary<string, object>();
			Dictionary<string, string> notLoadedColumns = new Dictionary<string, string>();
			foreach (var name in paramsDictionary.Keys) {
				if (caseEntity.GetIsColumnValueLoaded(name)) {
					calculationParamsDictionary.Add(paramsDictionary[name], caseEntity.GetColumnValue(name));
				} else {
					notLoadedColumns[name] = paramsDictionary[name];
				}
			}
			if (notLoadedColumns.Count > 0 && !isNewCase(caseEntity)) {
				EntitySchema caseSchema = UserConnection.EntitySchemaManager.GetInstanceByName("Case");
				Entity caseFromDb = caseSchema.CreateEntity(UserConnection);
				caseFromDb.FetchFromDB(caseEntity.PrimaryColumnValue);
				foreach (var column in notLoadedColumns) {
					calculationParamsDictionary.Add(column.Value, caseFromDb.GetColumnValue(column.Key));
				}
			}
			if (!isNewCase(caseEntity)) {
				Dictionary<string, object> clientOverridenCalculationParams = GetParamsDictionary(GetParamsDictionary(), caseEntity.PrimaryColumnValue);
				foreach (var param in clientOverridenCalculationParams) {
					if (!calculationParamsDictionary.ContainsKey(param.Key)) {
						calculationParamsDictionary.Add(param.Key, param.Value);
						CaseTermCalculationLogger.Info($"There is a custom parameter in calculation for case ${caseEntity.PrimaryColumnValue}: ${param.Key} - ${param.Value}");
					}
				}
			}

			return calculationParamsDictionary;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get dictionary of params and values for calculation.
		/// </summary>
		/// <param name="caseId">Id of current case.</param>
		/// <returns>Dictionary of params and values for calculation.</returns>
		public virtual Dictionary<string, object> GetParams(Guid caseId) {
			Dictionary<string, string> paramsDictionary = GetParamsDictionary();
			Dictionary<string, object> resultDictionary = GetParamsDictionary(paramsDictionary, caseId);
			return resultDictionary;
		}

		/// <summary>
		/// Get dictionary of params and values for calculation.
		/// </summary>
		/// <param name="caseEntity">Current case entity.</param>
		/// <returns>Dictionary of params and values for calculation.</returns>
		public virtual Dictionary<string, object> GetParams(Entity caseEntity) {
			Dictionary<string, string> paramsDictionary = GetCaseEntityParamsDictionary();
			Dictionary<string, object> resultDictionary = GetCaseEntityParamsDictionary(paramsDictionary, caseEntity);
			return resultDictionary;
		}

		#endregion
	}
}

