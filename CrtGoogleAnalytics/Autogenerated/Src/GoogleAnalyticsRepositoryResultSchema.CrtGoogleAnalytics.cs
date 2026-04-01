namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsRepositoryResultSchema

	/// <exclude/>
	public class GoogleAnalyticsRepositoryResultSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsRepositoryResultSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsRepositoryResultSchema(GoogleAnalyticsRepositoryResultSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("afdddc14-4bd7-443f-8e46-8061d640616a");
			Name = "GoogleAnalyticsRepositoryResult";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("09b9c59a-1bca-4fd8-85b6-b08874b4ad91");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,82,193,74,195,64,16,61,167,208,127,24,234,69,65,140,103,171,150,82,138,23,133,82,252,129,237,118,26,23,146,221,48,59,65,67,233,191,59,187,105,99,210,138,85,47,203,230,237,155,188,55,111,198,170,2,125,169,52,194,140,248,201,185,44,199,169,85,121,205,70,251,225,96,59,28,36,198,50,146,64,160,115,229,61,28,113,150,88,58,111,216,81,189,68,95,229,44,5,82,36,103,114,65,152,25,103,97,230,172,103,170,180,112,252,29,44,170,85,110,116,195,40,227,253,220,31,47,197,0,168,205,6,53,227,122,233,222,61,60,192,237,53,4,148,29,171,188,15,33,145,232,204,92,37,247,0,6,157,36,17,3,198,102,32,13,240,60,16,94,208,123,149,161,48,70,163,43,216,70,210,180,47,209,85,28,71,194,107,71,173,85,110,158,230,61,213,142,135,230,249,249,84,247,196,202,100,34,94,34,125,183,207,15,237,186,137,176,159,231,130,92,137,196,6,143,210,76,211,20,238,125,85,20,138,234,199,3,16,61,3,5,211,135,134,96,85,3,191,33,80,155,243,77,91,158,118,235,247,227,9,169,246,178,217,66,134,60,6,31,142,221,89,109,91,21,43,36,112,27,40,201,105,105,86,28,68,63,173,141,63,248,248,26,193,127,77,52,179,129,117,21,23,34,36,169,88,82,61,47,221,29,241,111,197,101,245,89,25,235,99,163,197,126,208,98,34,124,134,5,0,252,208,88,6,125,112,20,81,44,74,174,161,89,215,159,61,237,87,250,100,181,190,241,118,180,73,130,238,62,1,23,22,66,5,247,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("afdddc14-4bd7-443f-8e46-8061d640616a"));
		}

		#endregion

	}

	#endregion

}

