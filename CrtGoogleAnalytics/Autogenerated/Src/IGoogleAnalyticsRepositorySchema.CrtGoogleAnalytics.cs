namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IGoogleAnalyticsRepositorySchema

	/// <exclude/>
	public class IGoogleAnalyticsRepositorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IGoogleAnalyticsRepositorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IGoogleAnalyticsRepositorySchema(IGoogleAnalyticsRepositorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("393b1330-0740-41fe-9a61-449366d12fd0");
			Name = "IGoogleAnalyticsRepository";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("09b9c59a-1bca-4fd8-85b6-b08874b4ad91");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,146,207,74,195,64,16,198,207,41,228,29,230,88,65,146,7,176,20,98,43,33,96,65,90,209,131,120,216,36,211,186,176,127,194,206,166,16,164,239,238,110,54,169,210,82,17,17,111,201,206,204,55,191,111,102,20,147,72,13,171,16,22,198,230,90,239,4,102,138,137,206,242,138,226,201,123,60,137,90,226,106,7,155,142,44,202,100,161,133,192,202,114,173,40,201,81,161,225,213,205,73,206,231,255,185,98,146,61,20,201,74,215,40,200,165,185,68,174,44,26,23,133,254,99,235,57,138,147,154,53,54,154,184,213,166,115,249,30,40,74,211,20,102,212,74,201,76,55,31,31,54,108,143,4,123,238,82,201,171,105,176,111,8,53,179,172,100,132,201,177,46,253,90,120,177,213,26,169,21,182,23,221,32,145,247,59,45,238,84,43,209,176,82,224,44,207,158,177,28,34,189,159,57,208,144,119,13,121,203,235,64,162,77,81,95,5,167,191,161,134,173,54,32,29,8,111,4,142,138,244,159,94,188,112,84,44,121,191,114,215,105,70,214,184,213,6,143,47,175,243,35,212,138,53,141,11,252,192,44,11,231,243,167,59,202,130,230,185,173,16,24,92,13,157,191,101,92,163,109,141,163,19,140,236,56,6,79,136,80,118,176,235,81,128,141,44,163,123,224,245,5,248,165,171,124,228,18,33,71,123,239,36,135,49,251,231,219,238,105,60,145,105,152,42,84,130,163,178,225,100,162,67,60,57,124,0,177,162,113,145,158,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("393b1330-0740-41fe-9a61-449366d12fd0"));
		}

		#endregion

	}

	#endregion

}

