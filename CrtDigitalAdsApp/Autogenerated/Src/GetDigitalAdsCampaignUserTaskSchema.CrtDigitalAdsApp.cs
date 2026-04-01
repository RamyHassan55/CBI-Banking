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

	#region Class: GetDigitalAdsCampaignUserTask

	[DesignModeProperty(Name = "AreCampaignsReceived", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "f7cd1970492b4133a6f44a07a48b93e4", CaptionResourceItem = "Parameters.AreCampaignsReceived.Caption", DescriptionResourceItem = "Parameters.AreCampaignsReceived.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorCode", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "f7cd1970492b4133a6f44a07a48b93e4", CaptionResourceItem = "Parameters.ErrorCode.Caption", DescriptionResourceItem = "Parameters.ErrorCode.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class GetDigitalAdsCampaignUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public GetDigitalAdsCampaignUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("f7cd1970-492b-4133-a6f4-4a07a48b93e4");
		}

		#endregion

		#region Properties: Public

		public virtual bool AreCampaignsReceived {
			get;
			set;
		}

		public virtual string ErrorCode {
			get;
			set;
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
			if (UseFlowEngineMode) {
				if (!HasMapping("AreCampaignsReceived")) {
					writer.WriteValue("AreCampaignsReceived", AreCampaignsReceived, false);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ErrorCode")) {
					writer.WriteValue("ErrorCode", ErrorCode, null);
				}
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "AreCampaignsReceived":
					if (!UseFlowEngineMode) {
						break;
					}
					AreCampaignsReceived = reader.GetBoolValue();
				break;
				case "ErrorCode":
					if (!UseFlowEngineMode) {
						break;
					}
					ErrorCode = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

