namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: PortalConstantsSchema

	/// <exclude/>
	public class PortalConstantsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public PortalConstantsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public PortalConstantsSchema(PortalConstantsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("0d4ff333-6c25-49dc-97f3-5f38c4c7f52d");
			Name = "PortalConstants";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b54a206c-0c3a-4346-bc7a-d3b009155be5");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,146,65,107,27,49,16,133,207,49,248,63,44,57,181,135,105,36,173,100,73,132,30,180,90,169,244,96,26,226,228,84,122,80,215,178,189,176,214,154,149,150,96,74,255,123,229,56,109,234,4,218,128,65,39,241,102,222,55,111,102,140,109,88,23,139,125,76,126,123,61,157,76,39,193,109,125,220,185,198,23,119,126,24,92,236,87,233,131,238,195,170,93,143,131,75,109,31,166,147,31,211,201,197,110,252,222,181,77,17,83,254,107,138,166,115,49,22,55,253,144,92,151,197,249,55,164,152,85,7,229,197,213,213,215,220,223,132,212,166,253,162,217,248,173,187,245,205,109,187,222,164,69,63,14,141,255,118,16,157,246,27,188,91,246,161,219,23,159,198,118,89,252,179,250,203,67,240,67,241,177,8,254,225,81,253,238,114,54,51,10,115,203,161,182,184,6,138,152,0,137,181,2,36,21,65,140,215,70,25,113,249,254,250,44,87,53,166,77,127,106,75,9,65,218,86,217,135,97,10,148,106,3,170,174,17,96,36,43,70,43,70,144,160,231,218,206,93,24,93,151,165,127,27,11,69,168,80,168,4,35,21,59,12,141,65,74,33,129,10,86,149,82,99,65,57,58,215,184,246,43,55,118,233,196,215,82,108,16,153,9,48,37,201,3,19,146,115,54,210,0,211,21,215,90,162,89,41,127,231,252,124,6,243,126,57,118,111,90,249,81,121,188,169,185,107,195,141,91,251,207,203,19,2,44,165,230,146,106,192,212,26,160,21,33,160,48,86,64,144,145,25,174,164,76,212,175,8,212,114,219,134,251,208,166,183,64,252,17,171,174,59,162,220,71,63,196,23,28,156,160,138,115,172,193,112,197,129,218,50,111,32,159,2,48,162,107,130,117,201,75,105,31,57,158,40,158,134,242,49,230,153,238,246,187,255,231,241,170,66,247,219,93,151,83,73,47,80,24,43,181,172,148,133,138,228,92,168,70,28,170,82,48,80,212,82,37,102,86,91,74,158,35,249,57,157,228,247,11,107,72,143,197,4,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("0d4ff333-6c25-49dc-97f3-5f38c4c7f52d"));
		}

		#endregion

	}

	#endregion

}

