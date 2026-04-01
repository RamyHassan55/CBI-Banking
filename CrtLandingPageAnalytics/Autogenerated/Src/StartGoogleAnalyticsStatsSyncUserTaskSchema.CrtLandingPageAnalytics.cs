namespace Terrasoft.Core.Process.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;

	#region Class: StartGoogleAnalyticsStatsSyncUserTask

	/// <exclude/>
	public partial class StartGoogleAnalyticsStatsSyncUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public StartGoogleAnalyticsStatsSyncUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("5bab2c28-548b-4fbb-8756-3e3d3efa60fe");
		}

		#endregion

		#region Methods: Public

		public override void WritePropertiesData(DataWriter writer) {
			writer.WriteStartObject(Name);
			base.WritePropertiesData(writer);
			if (Status == Core.Process.ProcessStatus.Inactive) {
				writer.WriteFinishObject();
				return;
			}
			writer.WriteFinishObject();
		}

		#endregion

	}

	#endregion

}

