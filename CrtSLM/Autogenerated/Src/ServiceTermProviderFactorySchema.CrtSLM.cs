namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ServiceTermProviderFactorySchema

	/// <exclude/>
	public class ServiceTermProviderFactorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ServiceTermProviderFactorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ServiceTermProviderFactorySchema(ServiceTermProviderFactorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("af9b1e78-e018-487e-9a15-deed4c262d47");
			Name = "ServiceTermProviderFactory";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,82,193,110,194,48,12,61,131,196,63,88,156,64,170,218,15,160,171,52,49,13,237,54,9,248,128,16,92,148,169,77,145,157,48,85,19,255,62,39,20,70,97,99,135,70,141,253,158,253,252,28,207,198,238,96,217,178,195,58,157,55,85,133,218,153,198,114,186,64,139,100,244,108,52,244,17,178,66,34,197,77,233,4,69,248,71,56,125,85,218,53,100,144,5,48,26,90,85,35,239,149,198,30,204,150,102,231,73,133,54,233,18,233,96,52,74,186,30,13,191,70,195,65,150,101,144,179,175,107,69,109,209,221,67,22,246,212,28,204,22,9,202,216,162,77,207,224,236,10,189,247,155,202,104,208,149,98,142,180,247,142,117,210,213,10,36,52,185,235,18,3,11,116,12,188,71,109,74,163,163,60,112,177,179,34,153,67,126,57,189,112,179,91,114,30,81,16,38,126,26,235,198,110,77,180,113,92,172,122,37,128,49,56,12,23,68,154,103,49,249,123,33,207,72,98,152,61,45,101,92,172,229,30,184,93,224,158,76,232,60,89,46,150,15,199,200,179,51,46,16,59,211,14,134,156,87,21,188,245,184,209,196,11,51,120,244,32,61,121,49,81,151,24,147,179,35,121,31,9,52,155,15,17,91,252,76,204,9,172,123,99,65,127,202,41,196,21,13,78,18,97,30,118,217,173,79,30,165,203,31,244,47,38,145,57,176,248,9,82,80,36,248,192,123,166,157,175,209,186,201,245,98,146,43,69,211,228,31,222,205,30,146,91,201,211,89,40,112,148,67,190,227,55,4,164,219,140,84,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("af9b1e78-e018-487e-9a15-deed4c262d47"));
		}

		#endregion

	}

	#endregion

}

