namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotContextSanitizerSchema

	/// <exclude/>
	public class CopilotContextSanitizerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotContextSanitizerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotContextSanitizerSchema(CopilotContextSanitizerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("5613b6a0-96bf-4441-a75b-e810dd24a055");
			Name = "CopilotContextSanitizer";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c23dfaf0-0051-4966-86f8-cbe8197258d0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,101,145,65,107,194,64,16,133,207,17,252,15,67,188,88,40,9,45,244,100,244,226,65,4,107,75,21,175,101,93,199,184,37,217,13,59,19,107,91,252,239,77,118,147,84,237,33,44,153,249,222,236,219,121,90,228,72,133,144,8,83,139,130,149,137,166,166,80,153,97,232,247,126,250,189,160,40,183,153,146,64,92,245,36,200,76,16,65,67,76,141,102,60,241,74,104,197,234,27,45,212,124,80,88,117,20,140,32,141,38,6,165,25,158,197,105,197,86,233,116,129,58,229,3,140,225,233,225,113,244,159,37,7,193,194,232,212,243,107,145,86,112,56,88,188,44,103,239,171,245,219,124,57,27,132,149,176,150,198,113,12,9,149,121,46,236,215,164,45,188,90,35,145,8,9,248,128,144,170,35,106,56,138,172,196,8,230,123,87,115,127,160,8,68,123,157,208,59,80,76,144,121,115,120,146,136,59,170,45,130,60,8,43,36,163,165,123,104,175,80,12,22,139,172,90,24,93,12,252,84,149,180,254,189,242,10,44,210,168,51,27,223,186,77,138,106,124,14,186,74,96,28,186,57,225,100,221,141,100,3,133,127,78,148,196,142,252,19,90,228,210,106,114,116,3,225,174,121,105,18,183,93,183,225,171,248,204,246,3,37,183,107,218,212,252,176,169,57,241,157,143,48,80,123,24,118,155,106,246,228,143,205,37,230,184,139,122,212,4,60,185,141,188,19,4,222,219,117,198,35,223,59,187,227,236,227,109,65,231,194,1,117,187,250,206,191,255,130,20,69,177,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("5613b6a0-96bf-4441-a75b-e810dd24a055"));
		}

		#endregion

	}

	#endregion

}

