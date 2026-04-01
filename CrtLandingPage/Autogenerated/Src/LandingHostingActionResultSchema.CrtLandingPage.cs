namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingHostingActionResultSchema

	/// <exclude/>
	public class LandingHostingActionResultSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingHostingActionResultSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingHostingActionResultSchema(LandingHostingActionResultSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("c1b3fc94-9c86-4862-97b4-aaf92598fd2d");
			Name = "LandingHostingActionResult";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,147,65,79,194,64,16,133,207,146,240,31,38,112,209,75,123,23,53,49,72,130,137,40,129,163,241,176,221,14,101,205,118,183,238,206,38,34,225,191,187,221,182,180,37,134,224,165,237,76,223,123,253,38,59,85,44,71,91,48,142,48,53,244,194,84,42,84,182,100,25,14,7,251,225,224,202,89,95,194,122,103,9,243,201,73,29,173,156,34,145,99,180,70,35,152,20,63,140,132,86,173,170,159,23,173,240,203,161,37,235,5,94,50,54,152,121,53,76,37,179,246,22,106,225,92,91,242,183,71,94,38,173,208,58,73,65,29,199,49,220,89,151,231,204,236,30,234,122,133,133,65,139,138,44,208,22,193,4,53,232,77,168,100,149,7,219,42,16,88,72,140,154,168,184,147,245,126,196,79,36,126,148,141,39,70,108,170,21,25,239,42,27,133,75,164,224,192,75,212,179,164,87,251,64,123,28,110,105,116,129,134,4,250,9,151,33,164,122,127,58,78,104,84,81,205,28,214,113,142,254,115,27,201,178,232,232,233,114,87,156,11,204,19,52,215,175,254,20,225,30,70,181,109,116,83,114,55,224,137,214,18,214,117,224,30,50,164,9,216,242,114,240,22,50,14,39,103,176,222,252,4,225,96,1,141,209,6,252,182,216,242,56,47,103,10,190,69,101,235,131,89,50,229,225,204,58,130,62,223,25,174,217,55,199,34,112,233,228,19,57,253,7,168,177,246,105,218,196,246,233,82,154,121,111,207,32,69,98,66,98,10,66,109,180,201,89,179,124,23,2,214,91,251,236,205,125,196,206,255,52,111,53,208,125,254,131,120,140,42,173,54,50,212,85,183,223,60,252,2,253,56,15,51,7,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("c1b3fc94-9c86-4862-97b4-aaf92598fd2d"));
		}

		#endregion

	}

	#endregion

}

