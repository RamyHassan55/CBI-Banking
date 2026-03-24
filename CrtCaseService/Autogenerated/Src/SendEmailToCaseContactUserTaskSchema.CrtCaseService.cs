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

	#region Class: SendEmailToCaseContactUserTask

	[DesignModeProperty(Name = "EmailTemplateId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "9b724c7bd3554ec68d5a0fd0b37bedbc", CaptionResourceItem = "Parameters.EmailTemplateId.Caption", DescriptionResourceItem = "Parameters.EmailTemplateId.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "CaseId", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "9b724c7bd3554ec68d5a0fd0b37bedbc", CaptionResourceItem = "Parameters.CaseId.Caption", DescriptionResourceItem = "Parameters.CaseId.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class SendEmailToCaseContactUserTask : ProcessUserTask
	{

		#region Constructors: Public

		public SendEmailToCaseContactUserTask(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("9b724c7b-d355-4ec6-8d5a-0fd0b37bedbc");
		}

		#endregion

		#region Properties: Public

		public virtual Guid EmailTemplateId {
			get;
			set;
		}

		public virtual Guid CaseId {
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
			if (!HasMapping("EmailTemplateId")) {
				writer.WriteValue("EmailTemplateId", EmailTemplateId, Guid.Empty);
			}
			if (!HasMapping("CaseId")) {
				writer.WriteValue("CaseId", CaseId, Guid.Empty);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "EmailTemplateId":
					EmailTemplateId = reader.GetGuidValue();
				break;
				case "CaseId":
					CaseId = reader.GetGuidValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

