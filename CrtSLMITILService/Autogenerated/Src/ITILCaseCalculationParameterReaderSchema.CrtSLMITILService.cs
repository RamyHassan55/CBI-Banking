namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ITILCaseCalculationParameterReaderSchema

	/// <exclude/>
	public class ITILCaseCalculationParameterReaderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ITILCaseCalculationParameterReaderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ITILCaseCalculationParameterReaderSchema(ITILCaseCalculationParameterReaderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("b6eb7ae2-e22e-4845-925b-505d5771ca69");
			Name = "ITILCaseCalculationParameterReader";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("5be0374d-f3b5-4c63-b887-7fd46e962c93");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,205,84,77,107,219,64,16,61,43,144,255,48,40,23,7,130,116,247,23,20,183,9,6,151,154,196,61,149,30,54,171,145,179,32,239,138,217,149,139,91,242,223,59,187,146,28,57,117,100,98,114,40,58,136,157,221,247,102,222,155,217,213,98,131,182,20,18,97,133,68,194,154,220,37,51,163,115,181,174,72,56,101,244,229,197,159,203,139,168,178,74,175,225,97,103,29,110,120,191,40,80,250,77,155,220,161,70,82,114,180,63,211,165,33,124,43,158,220,10,233,12,41,180,124,194,127,81,154,166,48,182,213,102,35,104,55,109,214,183,134,54,22,74,65,92,164,67,178,144,27,130,153,176,248,69,59,218,45,141,210,174,69,166,29,232,143,111,91,78,166,50,252,201,139,178,122,44,148,4,89,8,107,97,190,154,47,60,126,38,10,89,21,65,223,178,101,191,71,145,33,13,161,127,159,25,189,31,209,21,225,154,119,129,189,178,142,42,47,198,14,97,25,146,5,61,255,8,10,129,185,86,78,137,66,253,70,11,2,52,254,2,197,120,161,217,127,147,131,123,66,134,32,130,36,204,39,241,233,106,227,116,90,43,75,246,25,187,70,180,226,79,19,13,174,33,200,122,254,127,106,15,145,208,123,208,140,155,196,149,69,98,187,117,61,122,241,116,197,25,125,12,228,62,152,140,211,128,120,167,248,239,7,204,112,152,232,218,115,69,67,120,100,142,193,171,173,3,211,174,80,103,245,84,52,235,102,68,190,162,123,50,153,159,14,50,142,129,152,245,152,124,135,14,10,101,157,183,52,72,169,135,94,190,20,223,103,23,161,171,72,219,233,162,159,97,156,182,7,131,79,109,89,96,154,123,3,159,85,208,199,220,99,158,110,190,190,55,80,255,167,190,192,96,159,125,57,211,14,79,212,3,35,180,85,225,96,18,108,76,142,146,140,2,71,125,48,249,148,101,131,248,1,105,171,36,46,249,173,72,230,89,124,3,221,8,7,142,65,170,178,52,228,22,184,197,162,197,116,66,93,144,119,160,169,107,116,106,246,239,107,191,246,173,233,184,249,250,125,146,94,224,59,122,116,156,170,211,163,232,172,38,53,207,164,114,187,15,105,87,31,93,111,227,206,232,219,89,109,235,222,62,31,123,254,11,52,223,137,115,218,6,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("b6eb7ae2-e22e-4845-925b-505d5771ca69"));
		}

		#endregion

	}

	#endregion

}

