namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class : CaseTermStrategyByPriorityInSupportLevel

	/// <summary>
	/// A strategy that determines time terms by priority and service.
	/// </summary>
	public class CaseTermStrategyByPriorityInSupportLevel : BaseCaseTermStrategy
	{

		#region Constructors : Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByPriorityInSupportLevel"/> class.
		/// </summary>
		/// <param name="userConnection">UserConnection.</param>
		/// <param name="args">The arguments.</param>
		public CaseTermStrategyByPriorityInSupportLevel(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection, args) {
			EntitySchemaName = "PriorityInSupportLevel";
			GetSupportLevelFromServicePact();
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Filters for EntitySchemaQuery
		/// </summary>
		/// <param name="esq">reference to EntityShemaQuery instance.</param>
		protected override void ApplyFilters(ref EntitySchemaQuery esq) {
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "CasePriority",
				BaseStrategyDataItem.PriorityId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SupportLevel",
				BaseStrategyDataItem.SupportLevelId));
		}

		/// <summary>
		/// Gets support level from ServicePact table.
		/// </summary>
		protected void GetSupportLevelFromServicePact() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServicePact");
			var supportLevelIdColumnName = esq.AddColumn("SupportLevel.Id").Name;
			Entity servicePact = esq.GetEntity(UserConnection, BaseStrategyDataItem.ServicePactId);
			if (servicePact != null) {
				BaseStrategyDataItem.SupportLevelId = servicePact.GetTypedColumnValue<Guid>(supportLevelIdColumnName);
			}
		}

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier.</returns>
		protected override Guid GetCalendarId() {
			var calendarId = default(Guid);
			if (TrySetCalendarFromServiceInServicePact(ref calendarId)) {
				return calendarId;
			}
			if (TrySetCalendarFromServicePact(ref calendarId)) {
				return calendarId;
			}
			return base.GetCalendarId();
		}

		#endregion

	}

	#endregion

}

