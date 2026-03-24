namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ICopilotToolProcessorSchema

	/// <exclude/>
	public class ICopilotToolProcessorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ICopilotToolProcessorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ICopilotToolProcessorSchema(ICopilotToolProcessorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("45ff5045-0bfb-49fc-bfdd-55d44048d58c");
			Name = "ICopilotToolProcessor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ed753793-30d5-4797-a3f9-3019dcc6e358");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,81,237,106,194,64,16,252,29,193,119,88,240,143,130,228,1,172,13,148,40,54,80,65,170,47,176,38,171,30,92,238,194,237,5,42,210,119,239,222,37,90,236,7,133,254,74,118,111,118,118,102,214,96,77,220,96,73,144,59,66,175,108,154,219,70,105,235,135,131,203,112,144,180,172,204,17,182,103,246,84,203,139,214,84,10,198,112,186,34,67,78,149,15,55,204,151,241,116,77,30,43,244,248,137,216,145,115,200,246,224,211,165,145,209,83,77,198,167,133,241,228,14,178,159,211,252,132,62,183,117,163,41,172,248,239,92,250,42,126,68,33,177,48,8,199,200,209,81,218,112,27,152,65,209,107,220,89,171,55,206,10,7,91,23,193,42,128,12,106,80,87,244,111,224,228,18,7,110,244,98,247,100,43,158,193,166,221,107,85,118,143,227,23,197,126,30,38,23,116,80,70,5,125,25,120,169,121,10,11,21,163,68,119,158,179,119,98,116,10,69,128,46,223,168,108,189,117,25,212,216,52,210,159,192,138,252,61,9,143,3,125,82,44,77,91,147,195,189,166,121,47,243,41,146,134,240,11,57,89,6,24,235,240,47,43,127,192,135,88,140,223,150,39,170,49,139,182,141,103,120,4,211,106,61,233,18,76,162,139,30,191,22,255,120,164,12,158,209,84,154,130,172,28,181,230,241,253,21,174,71,128,242,91,107,10,61,213,86,168,66,116,220,125,167,209,210,223,169,132,248,214,125,50,189,192,17,153,170,187,67,172,223,187,195,223,53,165,247,1,200,144,65,51,235,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("45ff5045-0bfb-49fc-bfdd-55d44048d58c"));
		}

		#endregion

	}

	#endregion

}

