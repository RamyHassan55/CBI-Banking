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

	#region Class: ClearResponseOverdueDateRuleHandler

	public class ClearResponseOverdueDateRuleHandler //: ICaseRuleHandler
	{
		#region Constructors: Public

		public ClearResponseOverdueDateRuleHandler(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Properties: Public

		public UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Methods: Public

		public void Handle(Entity entity) {
			if (entity.GetTypedColumnValue<int>("ResponseOverdue") != 0) {
				entity.SetColumnValue("ResponseOverdue", 0);
			}
		}

		#endregion
	}

	#endregion

}

