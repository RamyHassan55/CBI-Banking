namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingPagePublishRequestSchema

	/// <exclude/>
	public class LandingPagePublishRequestSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingPagePublishRequestSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingPagePublishRequestSchema(LandingPagePublishRequestSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("42c19df3-d0d9-479f-9627-d967fa4430c5");
			Name = "LandingPagePublishRequest";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,81,193,106,2,49,16,61,43,248,15,131,94,218,203,238,189,90,47,219,98,133,182,136,66,47,165,135,152,140,107,32,201,110,147,73,193,46,254,123,147,93,119,89,91,17,122,9,204,203,155,55,239,205,24,166,209,149,140,35,100,150,158,153,17,210,228,43,150,99,178,198,79,143,142,220,104,88,141,134,3,239,2,14,155,131,35,212,211,95,117,146,21,74,33,39,89,24,151,44,208,160,149,252,15,103,237,13,73,141,201,38,252,50,37,191,89,164,7,86,224,77,44,230,161,128,76,49,231,238,160,103,98,229,183,74,186,253,201,74,77,126,239,4,182,10,63,34,240,192,136,101,133,33,203,56,69,160,140,77,28,120,84,187,38,54,168,106,193,110,252,202,22,37,90,146,24,60,212,92,222,252,167,105,10,51,231,181,102,246,48,111,129,204,98,76,0,222,200,32,7,82,96,136,183,147,104,161,216,1,237,17,84,51,24,202,184,203,78,38,237,235,212,206,95,80,111,209,222,188,134,51,192,61,140,79,109,75,49,190,141,89,218,48,11,47,69,155,101,41,160,130,28,105,10,46,62,199,107,46,187,195,92,178,5,95,44,172,210,144,251,135,191,182,229,220,222,242,209,120,141,54,158,100,214,219,248,19,105,53,135,183,83,203,37,215,19,52,162,89,127,93,55,232,57,120,252,1,1,0,92,219,162,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("42c19df3-d0d9-479f-9627-d967fa4430c5"));
		}

		#endregion

	}

	#endregion

}

