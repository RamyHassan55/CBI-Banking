namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotToolsContextSchema

	/// <exclude/>
	public class CopilotToolsContextSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotToolsContextSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotToolsContextSchema(CopilotToolsContextSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("42e41678-3e68-47c4-bb06-274ca465f6bb");
			Name = "CopilotToolsContext";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c23dfaf0-0051-4966-86f8-cbe8197258d0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,81,219,138,194,48,16,125,174,176,255,48,224,75,5,233,7,168,171,15,181,44,130,11,130,254,64,236,142,54,144,38,221,100,10,74,241,223,55,55,169,46,101,221,183,228,204,153,115,102,206,72,86,163,105,88,137,144,107,100,196,85,150,171,134,11,69,111,163,238,109,148,180,134,203,51,236,175,134,176,182,21,33,176,180,28,105,178,15,148,168,121,57,255,205,217,114,249,221,131,7,212,154,25,117,162,172,144,150,93,213,40,41,219,72,66,125,178,150,38,203,43,70,185,170,27,129,78,213,246,217,206,177,198,179,253,64,46,152,49,51,136,227,28,148,18,185,178,157,23,242,44,238,68,36,19,80,58,218,32,43,233,60,179,23,180,115,147,110,75,82,218,234,238,218,163,224,101,96,52,254,61,32,146,186,106,146,108,182,220,208,194,225,107,60,113,201,221,176,75,32,251,55,211,192,88,115,159,11,211,215,133,245,176,171,79,97,227,248,197,5,203,214,26,46,161,102,77,227,240,168,88,200,182,70,205,142,2,23,209,214,197,34,105,95,86,88,179,37,112,255,51,19,232,124,131,211,50,240,30,60,231,30,250,12,130,22,140,210,1,14,50,142,27,37,96,181,130,222,45,43,234,134,174,131,158,233,196,43,220,94,101,146,254,21,7,252,59,137,137,239,136,162,19,8,185,204,128,42,110,210,135,74,22,117,31,161,123,150,32,91,33,98,68,113,236,49,202,175,112,239,231,227,239,180,106,80,19,199,225,211,15,175,20,82,239,224,140,52,135,219,3,253,245,146,247,235,12,52,191,188,253,253,132,125,239,192,102,1,125,6,111,63,70,163,38,69,206,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("42e41678-3e68-47c4-bb06-274ca465f6bb"));
		}

		#endregion

	}

	#endregion

}

