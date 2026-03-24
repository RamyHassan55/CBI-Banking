namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingPageHostingInfoSchema

	/// <exclude/>
	public class LandingPageHostingInfoSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingPageHostingInfoSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingPageHostingInfoSchema(LandingPageHostingInfoSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("13901c6c-31a5-4897-9e5b-b9be246aab89");
			Name = "LandingPageHostingInfo";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,144,49,79,195,48,16,133,231,84,234,127,56,149,5,150,100,167,192,18,6,144,10,138,82,117,66,12,151,244,26,89,178,157,112,119,30,74,213,255,142,157,180,85,65,98,177,252,158,159,191,123,58,143,142,100,192,150,160,100,93,161,223,26,223,85,216,81,94,211,87,32,81,153,207,14,243,89,22,36,250,176,222,139,146,91,254,209,121,29,188,26,71,249,154,216,160,53,223,168,166,247,49,21,115,55,76,93,20,80,90,20,185,135,171,1,47,189,104,188,190,250,93,63,38,63,46,191,27,75,159,201,120,70,197,178,247,202,216,106,50,134,208,88,211,66,155,80,255,146,178,195,72,187,12,174,184,31,136,213,80,156,94,141,128,233,189,40,10,120,144,224,28,242,254,233,108,156,160,48,68,42,52,40,4,155,122,149,95,226,197,117,126,172,247,70,174,33,190,125,143,75,132,71,88,4,182,139,187,84,245,220,85,148,19,111,195,22,210,22,179,172,35,93,130,196,35,169,227,169,42,249,237,212,118,212,147,251,219,60,254,0,10,60,142,119,167,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("13901c6c-31a5-4897-9e5b-b9be246aab89"));
		}

		#endregion

	}

	#endregion

}

