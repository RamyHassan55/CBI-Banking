namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class : CaseTermStrategyByPriorityAndSLA

	/// <summary>
	/// A strategy that determines time terms by priority and SLA.
	/// </summary>
	public class CaseTermStrategyByPriorityAndSLA : CaseTermStrategyByPriority
	{	

		#region Constructors : Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByPriorityAndSLA"/> class.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public CaseTermStrategyByPriorityAndSLA(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection, args) {
		}

		#endregion

		#region Methods : Private

		private bool TrySetCalendarFromServiceInServicePact(ref Guid calendarId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceInServicePact");
			var calendarIdColumnName = esq.AddColumn("Calendar.Id").Name;
			esq.Filters.LogicalOperation = LogicalOperationStrict.And;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServiceItem", StrategyDataItem.ServiceItemId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServicePact", StrategyDataItem.ServicePactId));
			EntityCollection collection = esq.GetEntityCollection(UserConnection);
			if (collection.IsNotEmpty()) {
				calendarId = collection[0].GetTypedColumnValue<Guid>(calendarIdColumnName);
			}
			return calendarId != default(Guid);
		}

		private bool TrySetCalendarServicePact(ref Guid calendarId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServicePact");
			var calendarIdColumnName = esq.AddColumn("Calendar.Id").Name;
			Entity servicePact = esq.GetEntity(UserConnection, StrategyDataItem.ServicePactId);
			if (servicePact != null) {
				calendarId = servicePact.GetTypedColumnValue<Guid>(calendarIdColumnName);
			}
			return calendarId != default(Guid);
		}

		#endregion

		#region Methods : Protected

		protected override Guid GetCalendarId() {
			var calendarId = default(Guid);
			if (TrySetCalendarFromServiceInServicePact(ref calendarId)) {
				return calendarId;
			}
			if (TrySetCalendarServicePact(ref calendarId)) {
				return calendarId;
			}
			return base.GetCalendarId();
		}

		#endregion

	}

	#endregion

}
