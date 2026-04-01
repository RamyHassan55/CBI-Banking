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

	#region Class: GetDigitalAdsAdAccountsUserTask

	/// <exclude/>
	public partial class GetDigitalAdsAdAccountsUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public GetDigitalAdsAdAccountsUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("be7316ca-8535-46e1-8c49-1ce45182029f");
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

