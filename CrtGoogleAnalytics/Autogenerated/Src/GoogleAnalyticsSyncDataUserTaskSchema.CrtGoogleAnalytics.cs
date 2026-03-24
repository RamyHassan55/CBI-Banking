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

	#region Class: GoogleAnalyticsSyncDataUserTask

	/// <exclude/>
	public partial class GoogleAnalyticsSyncDataUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public GoogleAnalyticsSyncDataUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("ef2accf9-4fec-40de-aad2-d086e8f5cf47");
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

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
			}
		}

		#endregion

	}

	#endregion

}

