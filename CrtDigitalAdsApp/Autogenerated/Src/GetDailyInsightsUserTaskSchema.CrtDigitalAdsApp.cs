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

	#region Class: GetDailyInsightsUserTask

	[DesignModeProperty(Name = "AreDailyTasksReceived", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ed7b02c195a64033b2a4b1836a846f8d", CaptionResourceItem = "Parameters.AreDailyTasksReceived.Caption", DescriptionResourceItem = "Parameters.AreDailyTasksReceived.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorCode", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ed7b02c195a64033b2a4b1836a846f8d", CaptionResourceItem = "Parameters.ErrorCode.Caption", DescriptionResourceItem = "Parameters.ErrorCode.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class GetDailyInsightsUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public GetDailyInsightsUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("ed7b02c1-95a6-4033-b2a4-b1836a846f8d");
		}

		#endregion

		#region Properties: Public

		public virtual bool AreDailyTasksReceived {
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
				if (!HasMapping("AreDailyTasksReceived")) {
					writer.WriteValue("AreDailyTasksReceived", AreDailyTasksReceived, false);
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
				case "AreDailyTasksReceived":
					if (!UseFlowEngineMode) {
						break;
					}
					AreDailyTasksReceived = reader.GetBoolValue();
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

