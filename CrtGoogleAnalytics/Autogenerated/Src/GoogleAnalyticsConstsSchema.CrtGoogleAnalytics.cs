namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsConstsSchema

	/// <exclude/>
	public class GoogleAnalyticsConstsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsConstsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsConstsSchema(GoogleAnalyticsConstsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("cd577303-1b48-4d66-aa5d-ac173020f643");
			Name = "GoogleAnalyticsConsts";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("04eed549-e1cf-4e3c-9d22-ce076df2f40c");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,93,142,77,10,194,48,20,132,215,13,244,14,143,174,116,81,48,154,180,21,113,209,31,21,119,130,39,136,105,44,129,52,41,125,41,82,196,187,27,93,22,102,49,12,243,13,99,69,175,112,16,82,65,61,250,139,115,157,81,165,21,102,246,90,98,76,222,49,137,38,212,182,131,251,140,94,245,135,152,132,100,152,30,70,75,64,47,66,11,164,17,136,176,64,107,103,209,135,129,232,183,176,0,70,37,90,103,205,12,151,73,183,75,240,102,132,127,186,177,191,182,112,4,171,94,255,210,42,217,209,13,205,89,121,74,105,115,230,41,171,42,154,22,123,30,92,206,56,43,26,190,205,178,44,89,135,123,209,39,38,65,95,228,238,196,202,215,0,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("cd577303-1b48-4d66-aa5d-ac173020f643"));
		}

		#endregion

	}

	#endregion

}

