namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotContentTypeSchema

	/// <exclude/>
	public class CopilotContentTypeSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotContentTypeSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotContentTypeSchema(CopilotContentTypeSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("9bb83cd8-c94e-4aab-ad1a-77a733ec50ec");
			Name = "CopilotContentType";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ed753793-30d5-4797-a3f9-3019dcc6e358");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,204,75,10,2,49,12,6,224,181,133,222,33,204,94,47,32,174,138,55,24,220,215,16,36,208,73,202,36,190,16,239,110,198,199,82,8,132,159,124,127,164,78,100,189,34,65,153,169,58,235,166,104,231,166,158,211,35,167,85,63,31,27,35,152,199,9,1,91,53,131,47,40,42,78,226,227,189,83,192,5,255,52,170,152,71,103,102,57,193,72,55,135,29,12,30,123,216,254,83,7,166,235,190,209,20,15,23,124,137,184,166,79,126,151,158,57,197,188,0,123,207,69,8,173,0,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("9bb83cd8-c94e-4aab-ad1a-77a733ec50ec"));
		}

		#endregion

	}

	#endregion

}

