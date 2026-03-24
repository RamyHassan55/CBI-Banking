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

	#region Class: ConvertContentConfigToHTMLUserTask

	[DesignModeProperty(Name = "ContentJson", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "6c8e193781b744a4ae67c7de19d3fbf9", CaptionResourceItem = "Parameters.ContentJson.Caption", DescriptionResourceItem = "Parameters.ContentJson.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "DesignerType", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "6c8e193781b744a4ae67c7de19d3fbf9", CaptionResourceItem = "Parameters.DesignerType.Caption", DescriptionResourceItem = "Parameters.DesignerType.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ContentHtml", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "6c8e193781b744a4ae67c7de19d3fbf9", CaptionResourceItem = "Parameters.ContentHtml.Caption", DescriptionResourceItem = "Parameters.ContentHtml.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "SimpleJson", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "6c8e193781b744a4ae67c7de19d3fbf9", CaptionResourceItem = "Parameters.SimpleJson.Caption", DescriptionResourceItem = "Parameters.SimpleJson.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class ConvertContentConfigToHTMLUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public ConvertContentConfigToHTMLUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("6c8e1937-81b7-44a4-ae67-c7de19d3fbf9");
		}

		#endregion

		#region Properties: Public

		public virtual string ContentJson {
			get;
			set;
		}

		public virtual string DesignerType {
			get;
			set;
		}

		public virtual string ContentHtml {
			get;
			set;
		}

		public virtual string SimpleJson {
			get;
			set;
		}

		private LocalizableString _pageHtmlGenerationError;
		public LocalizableString PageHtmlGenerationError {
			get {
				return _pageHtmlGenerationError ?? (_pageHtmlGenerationError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.PageHtmlGenerationError.Value"));
			}
		}

		private LocalizableString _emailHtmlGenerationError;
		public LocalizableString EmailHtmlGenerationError {
			get {
				return _emailHtmlGenerationError ?? (_emailHtmlGenerationError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.EmailHtmlGenerationError.Value"));
			}
		}

		private LocalizableString _pageJsonGenerationError;
		public LocalizableString PageJsonGenerationError {
			get {
				return _pageJsonGenerationError ?? (_pageJsonGenerationError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.PageJsonGenerationError.Value"));
			}
		}

		private LocalizableString _emailJsonGenerationError;
		public LocalizableString EmailJsonGenerationError {
			get {
				return _emailJsonGenerationError ?? (_emailJsonGenerationError = new LocalizableString(Storage, Schema.GetResourceManagerName(), "LocalizableStrings.EmailJsonGenerationError.Value"));
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
				if (!HasMapping("ContentJson")) {
					writer.WriteValue("ContentJson", ContentJson, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("DesignerType")) {
					writer.WriteValue("DesignerType", DesignerType, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ContentHtml")) {
					writer.WriteValue("ContentHtml", ContentHtml, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("SimpleJson")) {
					writer.WriteValue("SimpleJson", SimpleJson, null);
				}
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "ContentJson":
					if (!UseFlowEngineMode) {
						break;
					}
					ContentJson = reader.GetStringValue();
				break;
				case "DesignerType":
					if (!UseFlowEngineMode) {
						break;
					}
					DesignerType = reader.GetStringValue();
				break;
				case "ContentHtml":
					if (!UseFlowEngineMode) {
						break;
					}
					ContentHtml = reader.GetStringValue();
				break;
				case "SimpleJson":
					if (!UseFlowEngineMode) {
						break;
					}
					SimpleJson = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

