namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ServiceTermCalculatorServiceSchema

	/// <exclude/>
	public class ServiceTermCalculatorServiceSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ServiceTermCalculatorServiceSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ServiceTermCalculatorServiceSchema(ServiceTermCalculatorServiceSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("3f571728-8b0d-420c-b89e-e23ca6aab2b4");
			Name = "ServiceTermCalculatorService";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,237,85,193,110,211,64,16,61,167,82,255,97,21,46,142,100,249,3,18,64,74,91,129,64,74,27,197,65,61,68,61,108,236,73,88,176,119,205,204,58,200,138,242,239,204,238,218,174,11,37,229,130,184,112,113,180,179,111,222,188,153,121,118,180,44,129,42,153,129,88,3,162,36,179,179,201,181,209,59,181,175,81,90,101,116,114,45,11,208,185,68,186,188,56,94,94,140,106,82,122,255,91,112,10,120,80,25,240,117,57,235,193,105,67,22,74,70,22,5,100,14,70,201,123,208,128,42,251,5,211,230,47,76,14,197,217,203,228,30,182,231,1,115,46,117,240,170,102,47,202,118,122,185,207,172,46,252,185,229,121,46,143,203,114,110,89,122,86,190,223,180,88,230,179,40,51,251,224,98,115,170,110,193,50,172,98,186,173,42,148,109,86,240,173,86,8,37,104,75,209,240,224,196,138,55,226,133,20,135,74,218,64,62,113,69,170,122,91,168,76,100,133,36,18,131,177,119,109,24,108,131,98,42,174,36,65,123,226,204,163,23,62,122,133,176,231,94,197,2,236,103,147,211,84,44,61,99,184,220,220,85,16,102,51,108,108,180,225,246,63,232,131,249,10,81,72,99,229,227,229,93,186,30,199,194,169,3,178,239,12,150,210,114,156,161,11,32,146,123,8,161,228,35,25,29,139,43,147,55,169,109,10,120,2,233,163,201,61,202,170,130,60,118,229,70,43,246,38,219,5,206,147,78,130,54,4,91,163,158,138,22,176,148,200,222,182,128,209,45,255,58,161,110,60,29,227,56,36,181,83,28,204,175,3,136,110,144,62,74,209,141,242,214,149,216,188,38,139,108,138,88,152,237,23,246,243,91,145,25,157,43,239,235,137,56,122,221,7,137,34,235,23,193,181,53,124,127,126,73,209,99,114,44,62,17,32,207,91,135,183,100,50,235,185,176,21,117,195,114,152,237,145,58,233,85,174,6,144,104,144,74,166,168,29,219,153,212,116,0,233,82,195,48,127,214,221,15,39,180,201,251,145,94,234,90,249,9,15,101,134,5,142,58,238,22,49,84,227,1,39,95,238,228,30,255,125,247,212,119,195,181,204,119,76,184,148,53,193,191,53,226,95,117,211,31,123,197,127,190,248,79,41,124,193,248,196,177,211,15,253,248,103,45,202,6,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("3f571728-8b0d-420c-b89e-e23ca6aab2b4"));
		}

		#endregion

	}

	#endregion

}

