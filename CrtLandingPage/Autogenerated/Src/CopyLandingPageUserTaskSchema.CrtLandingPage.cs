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

	#region Class: CopyLandingPageUserTask

	[DesignModeProperty(Name = "LandingPageGoal", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "d195828b0ec6491e8f8eca353e54b1d9", CaptionResourceItem = "Parameters.LandingPageGoal.Caption", DescriptionResourceItem = "Parameters.LandingPageGoal.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "LandingPageName", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "d195828b0ec6491e8f8eca353e54b1d9", CaptionResourceItem = "Parameters.LandingPageName.Caption", DescriptionResourceItem = "Parameters.LandingPageName.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "LandingPageId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "d195828b0ec6491e8f8eca353e54b1d9", CaptionResourceItem = "Parameters.LandingPageId.Caption", DescriptionResourceItem = "Parameters.LandingPageId.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "NewLandingPageId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "d195828b0ec6491e8f8eca353e54b1d9", CaptionResourceItem = "Parameters.NewLandingPageId.Caption", DescriptionResourceItem = "Parameters.NewLandingPageId.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class CopyLandingPageUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public CopyLandingPageUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("d195828b-0ec6-491e-8f8e-ca353e54b1d9");
		}

		#endregion

		#region Properties: Public

		public virtual string LandingPageGoal {
			get;
			set;
		}

		public virtual string LandingPageName {
			get;
			set;
		}

		public virtual Guid LandingPageId {
			get;
			set;
		}

		public virtual Guid NewLandingPageId {
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
				if (!HasMapping("LandingPageGoal")) {
					writer.WriteValue("LandingPageGoal", LandingPageGoal, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("LandingPageName")) {
					writer.WriteValue("LandingPageName", LandingPageName, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("LandingPageId")) {
					writer.WriteValue("LandingPageId", LandingPageId, Guid.Empty);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("NewLandingPageId")) {
					writer.WriteValue("NewLandingPageId", NewLandingPageId, Guid.Empty);
				}
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "LandingPageGoal":
					if (!UseFlowEngineMode) {
						break;
					}
					LandingPageGoal = reader.GetStringValue();
				break;
				case "LandingPageName":
					if (!UseFlowEngineMode) {
						break;
					}
					LandingPageName = reader.GetStringValue();
				break;
				case "LandingPageId":
					if (!UseFlowEngineMode) {
						break;
					}
					LandingPageId = reader.GetGuidValue();
				break;
				case "NewLandingPageId":
					if (!UseFlowEngineMode) {
						break;
					}
					NewLandingPageId = reader.GetGuidValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

