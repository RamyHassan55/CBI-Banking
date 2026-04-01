namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;


	#region Class : CaseTermStrategyByServiceInServicePact

	/// <summary>
	/// A strategy that determines time terms by priority and service.
	/// </summary>
	public class CaseTermStrategyByServiceInServicePact : BaseCaseTermStrategy
	{
		#region Constructors : Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByServiceInServicePact"/> class.
		/// </summary>
		/// <param name="userConnection">UserConnection.</param>
		/// <param name="args">The arguments.</param>
		public CaseTermStrategyByServiceInServicePact(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection, args) {
			EntitySchemaName = "ServiceInServicePact";
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier from system settings.</returns>
		protected override Guid GetCalendarId() {
			var calendarId = default(Guid);
			if (TrySetCalendarFromServicePact(ref calendarId)) {
				return calendarId;
			}
			return GetBaseCalendarFromSysSettings();
		}

		/// <summary>
		/// Filters for EntitySchemaQuery
		/// </summary>
		/// <param name="esq">reference to EntityShemaQuery instance.</param>
		protected override void ApplyFilters(ref EntitySchemaQuery esq) {
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServiceItem",
				BaseStrategyDataItem.ServiceItemId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ServicePact",
				BaseStrategyDataItem.ServicePactId));
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Returns time interval for this strategy.
		/// </summary>
		/// <param name="mask">Flags which indicate which values are already filled.</param>
		/// <returns>Time interval.</returns>
		public override CaseTermInterval GetTermInterval(CaseTermStates mask) {
			var result = new CaseTermInterval();
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, EntitySchemaName);
			TimeColumnData timeColumns = AddTimeColumns(ref esq);
			string calendarColumnName = esq.AddColumn("Calendar.Id").Name;
			ApplyFilters(ref esq);
			EntityCollection entityCollection = esq.GetEntityCollection(UserConnection);
			if (entityCollection.IsNotEmpty()) {
				var entity = entityCollection[0];
				Guid calendarId = entity.GetTypedColumnValue<Guid>(calendarColumnName) == default(Guid) ?
					GetCalendarId() : entity.GetTypedColumnValue<Guid>(calendarColumnName);
				if (calendarId == default(Guid)) {
					return result;
				}
				result = PrepareResult(entity, mask, timeColumns, calendarId);
			}
			return result;
		}

		#endregion
	}

	#endregion

}
