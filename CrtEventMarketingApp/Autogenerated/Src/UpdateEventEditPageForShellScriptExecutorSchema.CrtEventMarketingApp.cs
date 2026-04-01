namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: UpdateEventEditPageForShellScriptExecutorSchema

	/// <exclude/>
	public class UpdateEventEditPageForShellScriptExecutorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public UpdateEventEditPageForShellScriptExecutorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public UpdateEventEditPageForShellScriptExecutorSchema(UpdateEventEditPageForShellScriptExecutorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("c366fa95-e780-47a4-8f2b-1c88ba858bea");
			Name = "UpdateEventEditPageForShellScriptExecutor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("2f5ac969-aacf-4614-bc89-b4e40dd20659");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,83,81,107,219,48,16,126,118,161,255,65,243,147,3,141,89,151,102,89,151,117,80,47,114,241,195,70,193,205,94,139,42,95,18,13,91,50,39,217,171,41,249,239,147,172,172,77,93,15,6,246,131,78,223,125,119,223,221,39,201,42,208,53,227,64,238,0,145,105,181,49,241,55,37,55,98,219,32,51,66,201,211,147,167,211,147,160,209,66,110,73,222,105,3,213,114,112,182,248,178,4,238,192,58,190,1,9,40,248,11,230,152,22,225,95,241,152,74,35,140,0,109,1,22,82,55,15,165,224,132,151,76,107,178,174,11,102,128,182,32,13,45,132,185,101,91,72,21,230,59,40,203,156,163,168,13,125,4,222,24,133,228,51,201,50,169,13,27,94,88,70,39,34,168,81,180,150,138,32,176,66,201,178,35,218,160,235,229,30,92,245,46,231,59,168,216,15,59,17,114,69,194,190,96,184,28,205,187,105,68,65,238,55,8,80,168,106,157,89,184,139,196,183,12,53,68,97,122,177,90,204,23,244,124,250,225,154,38,211,139,203,143,233,52,89,204,207,167,179,213,60,153,93,46,222,127,162,215,179,112,226,149,62,115,183,202,82,142,72,141,214,26,208,110,68,250,9,147,230,213,113,66,122,97,1,61,18,240,157,73,155,135,4,70,98,87,131,252,120,36,113,121,196,72,224,208,134,94,103,119,93,237,6,51,66,107,183,110,60,62,233,220,248,162,208,159,232,235,100,29,158,13,219,247,181,90,134,132,43,105,209,206,67,182,134,132,223,100,37,122,8,195,238,139,95,211,25,81,15,191,108,222,215,131,230,224,137,132,116,176,56,91,225,237,50,247,61,124,239,107,137,13,137,222,13,84,197,41,24,190,75,81,85,171,36,122,105,100,242,119,186,1,130,105,80,122,2,207,54,100,200,193,216,103,208,84,242,39,43,27,231,1,239,141,172,112,29,61,27,229,160,247,77,50,107,33,242,119,251,131,43,252,3,232,77,225,93,252,159,70,24,115,208,216,204,157,10,251,219,239,15,140,9,248,116,2,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("c366fa95-e780-47a4-8f2b-1c88ba858bea"));
		}

		#endregion

	}

	#endregion

}

