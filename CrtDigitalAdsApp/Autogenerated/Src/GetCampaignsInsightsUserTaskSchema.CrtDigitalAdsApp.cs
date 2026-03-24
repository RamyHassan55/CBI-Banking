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

	#region Class: GetCampaignsInsightsUserTask

	[DesignModeProperty(Name = "AreCampaignInsightsReceived", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "f2daed8aeb284bc2ab97373818277b4d", CaptionResourceItem = "Parameters.AreCampaignInsightsReceived.Caption", DescriptionResourceItem = "Parameters.AreCampaignInsightsReceived.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorCode", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "f2daed8aeb284bc2ab97373818277b4d", CaptionResourceItem = "Parameters.ErrorCode.Caption", DescriptionResourceItem = "Parameters.ErrorCode.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class GetCampaignsInsightsUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public GetCampaignsInsightsUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("f2daed8a-eb28-4bc2-ab97-373818277b4d");
		}

		#endregion

		#region Properties: Public

		public virtual bool AreCampaignInsightsReceived {
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
				if (!HasMapping("AreCampaignInsightsReceived")) {
					writer.WriteValue("AreCampaignInsightsReceived", AreCampaignInsightsReceived, false);
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
				case "AreCampaignInsightsReceived":
					if (!UseFlowEngineMode) {
						break;
					}
					AreCampaignInsightsReceived = reader.GetBoolValue();
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

