namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsLandingStreamDtoSchema

	/// <exclude/>
	public class GoogleAnalyticsLandingStreamDtoSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsLandingStreamDtoSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsLandingStreamDtoSchema(GoogleAnalyticsLandingStreamDtoSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("244d5dba-a679-4847-987d-2e26ffb312db");
			Name = "GoogleAnalyticsLandingStreamDto";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fa0347ef-084e-4088-83fa-fc7c1afcdf4a");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,81,193,106,195,48,12,61,183,208,127,16,244,158,220,215,49,40,45,132,194,14,129,108,31,224,38,74,230,17,219,65,86,6,33,244,223,167,56,109,150,118,163,219,46,198,146,245,222,211,243,179,202,160,111,84,142,176,35,126,86,182,208,182,74,85,133,91,171,234,142,117,238,87,203,126,181,92,180,94,250,144,117,158,209,108,166,90,32,137,115,85,253,53,29,237,217,201,187,76,172,9,43,237,44,236,106,229,253,3,220,204,157,149,50,38,84,70,48,3,68,64,113,28,195,163,111,141,81,212,61,157,235,64,0,236,128,176,33,244,104,25,10,197,10,152,148,245,37,18,184,227,59,230,12,165,35,168,71,90,104,196,1,200,29,146,45,248,160,17,93,216,227,25,125,211,30,107,157,67,30,20,126,217,16,190,121,152,45,191,232,195,250,147,233,148,92,131,196,26,197,121,26,68,198,247,91,127,161,113,40,196,147,46,245,96,165,4,126,195,43,27,209,132,155,111,126,89,61,105,117,1,179,216,14,5,244,80,33,111,192,15,199,233,142,236,43,213,255,215,147,207,28,6,103,138,3,205,95,37,51,86,20,210,195,49,79,249,64,253,129,99,156,37,57,35,113,221,151,223,11,244,69,27,12,151,159,100,215,104,139,49,131,80,143,221,235,230,233,19,79,171,5,247,243,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("244d5dba-a679-4847-987d-2e26ffb312db"));
		}

		#endregion

	}

	#endregion

}

