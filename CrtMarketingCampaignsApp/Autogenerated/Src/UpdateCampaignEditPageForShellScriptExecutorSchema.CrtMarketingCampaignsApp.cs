namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: UpdateCampaignEditPageForShellScriptExecutorSchema

	/// <exclude/>
	public class UpdateCampaignEditPageForShellScriptExecutorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public UpdateCampaignEditPageForShellScriptExecutorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public UpdateCampaignEditPageForShellScriptExecutorSchema(UpdateCampaignEditPageForShellScriptExecutorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("e91adfa3-90f8-4174-be3a-e1eb6c3502a2");
			Name = "UpdateCampaignEditPageForShellScriptExecutor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("1dbaf0b6-57fc-4f61-bc13-1b67faceaf44");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,83,193,78,219,64,16,61,27,137,127,152,250,228,72,196,42,13,33,165,41,149,48,113,144,15,173,144,76,122,69,203,122,146,108,101,239,90,179,235,180,22,202,191,119,55,27,32,24,35,33,217,135,93,191,247,102,222,204,179,100,21,234,154,113,132,59,36,98,90,45,77,124,173,228,82,172,26,98,70,40,121,124,244,120,124,20,52,90,200,21,228,173,54,88,77,59,103,139,47,75,228,14,172,227,27,148,72,130,191,96,14,101,9,223,187,143,83,105,132,17,168,45,192,66,234,230,161,20,28,120,201,180,134,69,93,48,131,215,172,170,153,88,201,180,16,230,150,173,112,174,40,95,99,89,230,156,68,109,210,127,200,27,163,8,190,65,150,73,109,88,247,131,21,117,62,130,154,196,198,170,1,33,43,148,44,91,208,134,92,59,247,232,26,104,115,190,198,138,253,178,67,129,75,8,159,106,134,211,94,234,77,35,10,184,95,18,98,161,170,69,102,25,238,38,190,101,164,49,10,231,103,179,201,120,146,158,14,191,92,165,201,240,236,226,124,62,76,38,227,211,225,104,54,78,70,23,147,207,95,211,171,81,56,240,126,159,181,55,202,74,246,27,142,22,26,201,174,70,250,81,67,243,234,56,128,157,189,32,61,176,241,147,73,203,35,192,158,187,203,14,63,238,33,78,15,20,1,247,109,232,69,118,215,214,110,60,61,178,118,253,198,227,147,214,13,49,10,253,41,125,77,214,225,73,183,125,95,107,195,8,184,146,22,237,194,100,107,72,252,11,51,177,131,48,106,191,251,101,157,128,122,248,99,121,63,246,158,131,71,8,211,206,250,108,133,183,43,221,238,224,91,95,75,44,33,250,212,113,21,207,209,240,245,156,84,53,75,162,151,70,6,79,211,13,8,77,67,210,11,120,181,174,66,142,198,254,15,77,37,127,179,178,113,49,240,241,200,10,215,209,115,86,246,126,223,144,217,6,35,255,109,187,15,134,255,19,118,185,240,89,254,96,16,222,9,81,223,216,157,17,251,218,231,63,128,177,33,250,14,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("e91adfa3-90f8-4174-be3a-e1eb6c3502a2"));
		}

		#endregion

	}

	#endregion

}

