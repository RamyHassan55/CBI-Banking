namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ILandingPageHostingProviderSchema

	/// <exclude/>
	public class ILandingPageHostingProviderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ILandingPageHostingProviderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ILandingPageHostingProviderSchema(ILandingPageHostingProviderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("994cc465-ca9b-4df0-9dea-238e23800e27");
			Name = "ILandingPageHostingProvider";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,205,147,77,106,195,48,16,133,215,49,248,14,131,179,73,55,213,62,113,12,165,155,26,90,48,129,30,64,181,199,63,96,203,174,70,42,148,144,187,87,150,100,55,117,73,200,170,116,103,141,102,190,121,239,33,11,222,33,13,60,71,120,148,234,153,139,162,17,85,198,43,12,131,99,24,172,52,153,227,226,230,254,128,239,26,73,209,46,12,76,203,90,98,213,244,2,82,161,80,150,6,180,133,244,172,251,169,39,53,126,202,254,163,41,80,218,145,65,191,181,77,14,205,52,113,125,96,117,180,67,243,162,23,84,117,95,208,22,50,139,113,151,140,49,136,73,119,29,151,159,201,84,240,12,2,187,144,234,209,75,95,130,170,17,90,183,16,134,209,208,12,96,75,66,60,112,201,59,16,38,164,125,36,157,239,40,73,5,41,46,140,110,15,139,9,17,114,137,229,62,58,51,146,185,165,62,173,136,37,49,179,180,111,184,68,165,165,160,27,120,62,148,135,92,153,4,14,72,186,117,192,137,48,34,47,183,130,151,178,185,168,14,188,183,187,221,45,113,106,241,59,80,95,193,226,47,162,125,157,4,252,135,112,103,49,155,43,10,151,1,175,81,20,238,61,219,243,201,253,74,63,138,167,47,168,27,113,176,155,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("994cc465-ca9b-4df0-9dea-238e23800e27"));
		}

		#endregion

	}

	#endregion

}

