namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsStreamDtoSchema

	/// <exclude/>
	public class GoogleAnalyticsStreamDtoSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsStreamDtoSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsStreamDtoSchema(GoogleAnalyticsStreamDtoSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("b8f8d841-14ea-41d9-bb28-2d5a08cac245");
			Name = "GoogleAnalyticsStreamDto";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("402fd175-323a-469d-847e-2eee398115b9");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,146,65,107,132,48,16,133,207,43,248,31,6,246,174,247,110,41,200,22,164,135,194,194,30,122,30,227,172,164,152,68,50,201,65,164,255,189,154,232,118,187,180,197,226,69,200,203,188,247,190,48,106,84,196,29,10,130,163,117,165,49,77,75,133,198,182,119,82,112,246,236,76,154,12,105,178,243,44,117,3,231,158,29,169,67,154,140,202,222,82,35,141,134,99,139,204,15,112,231,60,59,75,168,130,125,156,205,243,28,30,217,43,133,182,127,154,207,193,7,206,128,165,206,18,147,118,80,163,67,112,22,53,95,200,130,169,222,73,56,184,24,11,101,1,28,2,179,37,44,191,73,235,124,213,74,1,34,4,254,206,177,27,2,203,21,252,100,77,71,214,73,26,233,79,33,34,222,223,195,6,225,141,42,192,37,116,134,1,89,103,87,195,45,208,66,84,122,89,67,36,120,169,97,128,134,220,1,120,250,124,252,81,21,95,240,255,182,113,108,218,81,89,108,111,84,132,236,45,169,105,41,43,107,95,191,44,91,154,187,184,148,126,109,237,188,196,126,75,39,10,97,252,250,151,22,113,252,231,198,61,233,58,254,95,225,28,213,239,226,168,125,2,43,133,231,176,116,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("b8f8d841-14ea-41d9-bb28-2d5a08cac245"));
		}

		#endregion

	}

	#endregion

}

