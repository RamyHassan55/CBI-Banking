namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class : CaseTermStrategyByPriorityAndService

	/// <summary>
	/// A strategy that determines time terms by priority and service.
	/// </summary>
	public class CaseTermStrategyByPriorityAndService : CaseTermStrategyByPriority
	{

		#region Constructors : Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CaseTermStrategyByPriorityAndService"/> class.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public CaseTermStrategyByPriorityAndService(UserConnection userConnection, Dictionary<string, object> args)
			: base(userConnection, args) {
		}

		#endregion

		#region Methods : Private

		private bool TrySetCalendarFromServiceItem(ref Guid calendarId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceItem");
			string calendarIdColumnName = esq.AddColumn("Calendar.Id").Name;
			Entity entity = esq.GetEntity(UserConnection, StrategyDataItem.ServiceItemId);
			if (entity != null) {
				calendarId = entity.GetTypedColumnValue<Guid>(calendarIdColumnName);
			}
			return calendarId != default(Guid);
		}
		
		#endregion

		#region Methods : Protected

		/// <summary>
		/// Gets the calendar identifier.
		/// </summary>
		/// <returns>Calendar identifier.</returns>
		protected override Guid GetCalendarId() {
			var calendarId = default(Guid);
			if (TrySetCalendarFromServiceItem(ref calendarId)) {
				return calendarId;
			}
			return base.GetCalendarId();
		}

		#endregion

	}

	#endregion

}
