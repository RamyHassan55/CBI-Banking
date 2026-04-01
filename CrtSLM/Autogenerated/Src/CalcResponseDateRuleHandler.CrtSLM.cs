namespace Terrasoft.Configuration
{
	using System;
	using System.Data;
	using System.Collections.Generic;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Configuration.Calendars;

	#region Class: CaseRuleApplier

	public class CalcResponseDateRuleHandler //: ICaseRuleHandler
	{
		
		#region Fields: Private

		private readonly string[] allColumns = new string[] { "ServiceItem", "ServicePact", "Priority", "SolutionOverdue" };

		#endregion

		#region Constructors: Public

		public CalcResponseDateRuleHandler(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Properties: Public

		public UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Create parameters for term recalculation.
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>Dictionary with column names and column values.</returns>
		private Dictionary<string, object> GetPresentColumns(Entity entity) {
			var idColumnValue = entity.GetTypedColumnValue<Guid>("Id");
			Dictionary<string, object> arguments = new Dictionary<string, object> {
				{"CaseId", idColumnValue}
			};
			foreach (var columnName in allColumns) {
				EntitySchemaColumn column = entity.Schema.Columns.FindByName(columnName);
				if (IsColumnValid(column, entity)) {
					object columnValue = entity.GetColumnValue(column);
					arguments.Add(column.ColumnValueName, columnValue);
				}
			}
			return arguments;
		}

		/// <summary>
		/// Check if column present and loaded. 
		/// </summary>
		/// <param name="column">Entity column</param>
		/// <param name="entity">Entity</param>
		/// <returns>Is column valid.</returns>
		private bool IsColumnValid(EntitySchemaColumn column, Entity entity) {
			return (column != null && entity.IsColumnValueLoaded(column));
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Entry point for rule handling.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Handle(Entity entity) {
			DateTime startDate = UserConnection.CurrentUser.GetCurrentDateTime();
			var columnArguments = GetPresentColumns(entity);
			CaseTermCalculateEntryPoint entryPoint = new CaseTermCalculateEntryPoint(UserConnection);
			var responseTime = entryPoint.ForceCalculateTerms(columnArguments, startDate);
			entity.SetColumnValue("ResponseDate", responseTime.ReactionTime);
		}

		#endregion
	}

	#endregion

}

