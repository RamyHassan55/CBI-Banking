namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingPageHtmlSchema

	/// <exclude/>
	public class LandingPageHtmlSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingPageHtmlSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingPageHtmlSchema(LandingPageHtmlSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("190e8726-b630-4e2b-acc2-c624db386a6a");
			Name = "LandingPageHtml";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,146,63,79,195,48,16,197,231,84,234,119,56,181,11,44,201,78,129,37,72,20,9,80,213,138,9,49,184,201,53,181,100,59,193,62,15,165,234,119,231,156,52,33,253,67,165,46,145,238,221,249,249,247,206,49,66,163,171,68,134,144,90,122,21,38,151,166,152,137,2,227,57,126,123,116,228,134,131,237,112,16,121,199,58,44,54,142,80,79,142,234,120,238,13,73,141,241,2,173,20,74,254,8,146,165,225,41,158,27,91,44,184,128,84,9,231,238,160,119,193,148,180,170,71,146,36,129,123,231,181,22,118,243,184,175,231,88,89,116,104,200,129,0,93,230,168,160,92,1,173,17,84,227,0,21,91,192,154,61,226,214,34,233,121,124,118,40,75,133,95,65,120,18,36,210,210,144,21,25,5,161,242,75,37,51,200,2,215,41,86,180,173,209,58,252,153,45,43,180,36,145,51,204,234,147,77,255,152,189,22,62,140,228,213,129,204,153,95,174,36,218,150,61,224,54,224,171,210,246,211,196,157,87,63,68,3,253,134,122,137,246,230,157,223,9,30,96,20,60,2,231,75,62,186,13,57,218,32,207,94,230,48,237,154,16,30,45,138,10,164,9,56,254,132,106,119,129,57,156,132,140,247,195,204,45,110,32,189,146,236,144,201,145,13,79,85,123,255,207,115,30,40,221,179,56,229,139,147,253,93,65,181,207,180,96,155,179,112,233,95,255,2,227,24,77,222,252,10,117,221,168,135,226,238,23,98,184,110,7,74,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("190e8726-b630-4e2b-acc2-c624db386a6a"));
		}

		#endregion

	}

	#endregion

}

