namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: FreedomEmailPageUrlProviderSchema

	/// <exclude/>
	public class FreedomEmailPageUrlProviderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public FreedomEmailPageUrlProviderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public FreedomEmailPageUrlProviderSchema(FreedomEmailPageUrlProviderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("a11e8ebc-b3e8-4511-91f0-264383870a8e");
			Name = "FreedomEmailPageUrlProvider";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("0da80398-3713-4925-86c7-e11402d989a0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,83,93,107,219,64,16,124,118,32,255,97,81,94,108,168,173,212,77,237,214,95,15,118,98,8,148,98,138,253,84,74,57,235,246,196,81,233,36,246,78,129,96,244,223,187,39,43,198,118,28,209,208,71,237,205,206,238,206,140,140,72,209,230,34,66,88,35,145,176,153,114,189,69,102,148,142,11,18,78,103,230,250,106,119,125,213,42,172,54,241,9,132,112,252,70,189,183,20,145,203,72,163,101,4,99,194,48,132,137,45,210,84,208,243,172,254,94,18,162,204,82,192,84,232,4,114,17,35,108,126,124,131,156,178,39,45,145,122,47,109,225,81,223,207,123,84,162,72,220,92,27,201,83,219,238,57,199,76,181,31,31,60,199,138,41,54,148,172,106,130,206,7,248,206,151,193,20,130,122,212,37,84,208,249,197,188,218,56,36,35,18,136,18,97,45,52,224,97,4,151,202,115,97,145,121,118,213,181,173,27,194,152,117,3,86,209,58,42,188,20,118,4,171,98,155,232,104,143,56,23,164,42,44,8,133,67,11,154,187,132,97,63,50,5,254,66,70,34,66,68,168,166,141,167,132,179,222,129,59,60,39,159,228,130,68,10,134,37,153,6,133,69,226,229,12,70,222,223,96,182,225,111,136,14,133,73,88,129,171,222,188,218,186,73,146,246,230,132,13,78,201,59,158,164,53,130,45,43,212,62,123,2,159,171,86,89,139,134,70,238,117,59,21,145,199,228,72,142,179,244,15,18,206,139,228,207,113,162,252,185,111,136,82,31,150,61,113,114,249,12,96,167,124,144,61,195,225,202,125,128,102,16,28,170,246,247,50,163,212,191,5,227,134,61,238,209,234,216,176,168,222,191,247,45,240,210,186,230,206,71,89,77,87,114,123,55,252,252,233,75,87,14,111,251,221,187,190,84,221,175,145,18,221,104,48,80,98,168,6,114,171,6,141,235,60,156,254,99,14,211,60,225,168,189,111,177,99,239,215,53,67,181,222,238,182,188,89,8,146,225,238,99,25,162,212,46,220,245,203,198,125,214,164,227,152,213,249,47,167,106,146,87,102,189,114,112,124,49,96,28,187,242,47,98,92,28,54,251,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("a11e8ebc-b3e8-4511-91f0-264383870a8e"));
		}

		#endregion

	}

	#endregion

}

