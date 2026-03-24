namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotExtensionsSchema

	/// <exclude/>
	public class CopilotExtensionsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotExtensionsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotExtensionsSchema(CopilotExtensionsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("84f2730c-7d56-495e-9fdb-10feef1e0da0");
			Name = "CopilotExtensions";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ed753793-30d5-4797-a3f9-3019dcc6e358");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,80,65,106,195,64,12,60,219,224,63,8,122,73,46,126,64,19,122,49,165,244,16,8,180,37,103,117,173,218,75,29,201,72,114,41,132,252,61,235,141,123,72,105,47,98,53,35,205,236,136,241,72,54,98,32,104,148,208,163,212,141,140,113,16,175,202,83,85,22,147,69,238,224,149,84,209,228,195,19,169,180,169,202,196,68,118,82,198,1,204,211,90,128,48,160,25,44,203,143,223,78,108,81,216,210,228,41,207,23,119,74,93,66,96,71,222,75,107,247,176,159,222,135,24,174,228,152,223,63,90,139,202,115,178,96,127,9,61,29,113,135,140,29,41,60,209,95,240,202,251,104,240,102,164,141,48,83,240,217,104,186,105,215,48,231,41,10,37,159,244,55,89,31,68,63,243,25,234,27,221,189,202,87,108,73,235,100,187,64,219,255,63,247,176,90,111,102,143,243,18,152,184,189,102,206,125,70,83,185,0,83,62,71,104,114,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("84f2730c-7d56-495e-9fdb-10feef1e0da0"));
		}

		#endregion

	}

	#endregion

}

