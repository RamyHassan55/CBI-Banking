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

	#region Class: SaveContentRequestUserTask

	[DesignModeProperty(Name = "RecordId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.RecordId.Caption", DescriptionResourceItem = "Parameters.RecordId.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ContentId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.ContentId.Caption", DescriptionResourceItem = "Parameters.ContentId.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "LanguageId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.LanguageId.Caption", DescriptionResourceItem = "Parameters.LanguageId.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ContentJson", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.ContentJson.Caption", DescriptionResourceItem = "Parameters.ContentJson.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ContentHtml", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.ContentHtml.Caption", DescriptionResourceItem = "Parameters.ContentHtml.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "DesignerType", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "de04e7f56f6a410285e9926743916e60", CaptionResourceItem = "Parameters.DesignerType.Caption", DescriptionResourceItem = "Parameters.DesignerType.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class SaveContentRequestUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public SaveContentRequestUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("de04e7f5-6f6a-4102-85e9-926743916e60");
		}

		#endregion

		#region Properties: Public

		public virtual Guid RecordId {
			get;
			set;
		}

		public virtual Guid ContentId {
			get;
			set;
		}

		public virtual Guid LanguageId {
			get;
			set;
		}

		public virtual string ContentJson {
			get;
			set;
		}

		public virtual string ContentHtml {
			get;
			set;
		}

		public virtual string DesignerType {
			get;
			set;
		}

		private LocalizableString _pageSavingError;
		public LocalizableString PageSavingError {
			get {
				return _pageSavingError ?? (_pageSavingError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.PageSavingError.Value"));
			}
		}

		private LocalizableString _emailSavingError;
		public LocalizableString EmailSavingError {
			get {
				return _emailSavingError ?? (_emailSavingError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.EmailSavingError.Value"));
			}
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
				if (!HasMapping("RecordId")) {
					writer.WriteValue("RecordId", RecordId, Guid.Empty);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ContentId")) {
					writer.WriteValue("ContentId", ContentId, Guid.Empty);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("LanguageId")) {
					writer.WriteValue("LanguageId", LanguageId, Guid.Empty);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ContentJson")) {
					writer.WriteValue("ContentJson", ContentJson, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ContentHtml")) {
					writer.WriteValue("ContentHtml", ContentHtml, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("DesignerType")) {
					writer.WriteValue("DesignerType", DesignerType, null);
				}
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "RecordId":
					if (!UseFlowEngineMode) {
						break;
					}
					RecordId = reader.GetGuidValue();
				break;
				case "ContentId":
					if (!UseFlowEngineMode) {
						break;
					}
					ContentId = reader.GetGuidValue();
				break;
				case "LanguageId":
					if (!UseFlowEngineMode) {
						break;
					}
					LanguageId = reader.GetGuidValue();
				break;
				case "ContentJson":
					if (!UseFlowEngineMode) {
						break;
					}
					ContentJson = reader.GetStringValue();
				break;
				case "ContentHtml":
					if (!UseFlowEngineMode) {
						break;
					}
					ContentHtml = reader.GetStringValue();
				break;
				case "DesignerType":
					if (!UseFlowEngineMode) {
						break;
					}
					DesignerType = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

