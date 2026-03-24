namespace Terrasoft.Configuration
{
	using Core;
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core.Entities;
	using SystemSettings = Core.Configuration.SysSettings;
	using Terrasoft.Configuration.CaseService;

	#region Class : CaseCloserJob

	/// <summary>
	/// Represents a case closer job.
	/// </summary>
	public class CaseCloserJob : IJobExecutor
	{
		#region Methods : Private

		private EntityCollection GetCases(UserConnection userConnection)	{
			var caseEsq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "Case");
			caseEsq.UseAdminRights = false;
			caseEsq.PrimaryQueryColumn.IsAlwaysSelect = true;
			caseEsq.AddAllSchemaColumns();
			var statusFilter = caseEsq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"Status.IsResolved", true);
			caseEsq.Filters.Add(statusFilter);
			AddSolutionDateFilters(userConnection, caseEsq);
			return caseEsq.GetEntityCollection(userConnection);
		}

		private void AddSolutionDateFilters(UserConnection userConnection, EntitySchemaQuery caseEsq) {
			var date = GetStartClosingDate(userConnection);
			var dateStartFilter = caseEsq.CreateFilterWithParameters(FilterComparisonType.LessOrEqual,
				"SolutionProvidedOn", date);
			caseEsq.Filters.Add(dateStartFilter);
			var dateEndFilter = caseEsq.CreateFilterWithParameters(FilterComparisonType.GreaterOrEqual,
				"SolutionProvidedOn", date.AddDays(-1));
			caseEsq.Filters.Add(dateEndFilter);
		}

		private DateTime GetStartClosingDate(UserConnection userConnection) {
			int firstNotificationDate = SystemSettings.GetValue(userConnection,
				"FirstReevaluationWaitingDays", 0);
			int secondNotificationDate = SystemSettings.GetValue(userConnection,
				"SecondReevaluationWaitingDays", 0);
			return DateTime.UtcNow.AddDays(-firstNotificationDate).AddDays(-secondNotificationDate);
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Close cases that must be closed.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void Execute(UserConnection userConnection, IDictionary<string, object> parameters) {
			bool isAutoClose = SystemSettings.GetValue(userConnection, "CloseResolvedCases", true);
			if (isAutoClose) {
				var cases = GetCases(userConnection);
				var caseBroker = new CaseBroker();
				foreach (Entity closingCase in cases) {
					caseBroker.CloseOnCondition(closingCase, delegate (Entity entity) { return true; });
				}
			}
		}

		#endregion

	}

	#endregion

}
