using System;
using Terrasoft.Core;

namespace Terrasoft.Configuration
{
	using Terrasoft.Core.Entities;

	#region Class: CalendarRemindCalculatorCustomerService

	public class CalendarRemindCalculatorCustomerService : CalendarRemindCalculatorV2
	{
		#region Constructors: Private

		private CalendarRemindCalculatorCustomerService(UserConnection userConnection)
			: base(userConnection) {
		}

		#endregion

		#region Methods: Public

		public static CalendarRemindCalculatorCustomerService CreateInstanceCustomerService(
				UserConnection userConnection, Guid caseId) {
			Entity caseEntity = GetCaseEntity(userConnection, caseId, new[] { "ServiceItemId", "PriorityId" });
			if (caseEntity == null || caseEntity.GetTypedColumnValue<Guid>("PriorityId") == Guid.Empty) {
				return null;
			}
			return new CalendarRemindCalculatorCustomerService(userConnection) { CaseEntity = caseEntity };
		}

		#endregion
	}

	#endregion
}
