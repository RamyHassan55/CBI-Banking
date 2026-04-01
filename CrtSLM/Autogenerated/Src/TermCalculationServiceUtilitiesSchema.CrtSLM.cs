namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: TermCalculationServiceUtilitiesSchema

	/// <exclude/>
	public class TermCalculationServiceUtilitiesSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public TermCalculationServiceUtilitiesSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public TermCalculationServiceUtilitiesSchema(TermCalculationServiceUtilitiesSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("609e8015-fad5-45e5-8005-deb9093949ca");
			Name = "TermCalculationServiceUtilities";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d862795b-510a-489d-988e-e22493fe3a79");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,173,149,193,106,227,48,16,134,207,9,228,29,4,57,175,11,61,54,165,203,146,66,201,161,16,146,176,151,210,131,162,76,130,168,45,185,35,169,144,134,125,247,142,164,216,241,186,105,93,185,245,193,150,198,210,204,231,209,63,99,197,11,48,37,23,192,86,128,200,141,222,218,108,170,213,86,238,28,114,43,181,202,200,94,76,121,46,92,30,230,75,192,23,41,96,52,60,140,134,163,225,192,25,169,118,108,185,55,22,138,73,107,158,45,156,178,178,128,140,246,72,158,203,215,224,96,18,246,141,17,118,52,97,211,156,27,115,197,142,94,125,172,5,60,59,48,54,172,186,160,139,93,27,87,20,28,247,55,209,192,198,254,138,247,211,51,12,178,227,138,235,139,198,150,135,91,110,57,125,146,69,46,236,35,25,74,183,206,165,96,37,71,75,84,76,120,130,51,0,127,47,105,109,252,200,154,118,142,186,4,218,6,132,60,15,110,226,251,54,230,137,51,48,189,135,138,84,247,80,172,1,61,83,5,117,231,228,166,98,153,81,10,103,27,118,96,254,253,96,7,118,18,6,230,56,248,231,111,29,193,251,197,159,163,212,40,237,254,243,224,159,134,62,197,79,37,48,22,189,130,22,148,112,26,6,197,172,72,67,61,211,112,76,37,29,182,176,253,79,98,78,187,187,147,49,6,181,137,58,9,243,104,109,26,7,109,221,179,150,240,77,169,149,129,74,249,239,132,255,39,106,245,151,32,49,115,169,0,217,86,35,67,32,58,239,210,151,26,227,106,195,140,206,93,109,73,170,137,51,181,80,49,125,92,10,236,11,181,176,104,66,166,29,4,89,193,43,224,119,237,228,59,122,104,167,166,15,71,229,164,139,163,159,40,174,232,59,11,58,94,179,146,226,201,212,221,48,161,23,54,158,255,181,199,180,254,24,181,112,158,229,208,213,20,59,186,82,77,147,220,23,150,218,161,128,234,44,122,247,167,70,150,210,48,114,29,154,211,41,41,63,129,144,76,177,214,58,103,51,58,144,186,212,191,65,209,235,31,49,229,6,210,90,226,121,237,147,241,13,40,48,23,121,131,8,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("609e8015-fad5-45e5-8005-deb9093949ca"));
		}

		#endregion

	}

	#endregion

}

