namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseMLangBinderSchema

	/// <exclude/>
	public class CaseMLangBinderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseMLangBinderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseMLangBinderSchema(CaseMLangBinderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("e64afc31-7e45-4a5f-b9cc-d14c8c88f3f2");
			Name = "CaseMLangBinder";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,81,95,139,130,64,16,127,46,232,59,12,221,75,7,135,190,87,4,37,6,130,102,92,114,247,188,234,228,45,232,174,236,174,114,17,125,247,27,87,61,42,122,90,102,126,255,102,102,5,171,80,215,44,67,72,80,41,166,229,217,56,158,20,103,94,52,138,25,46,197,108,122,157,77,39,141,230,162,120,160,40,116,246,44,51,82,113,212,171,23,140,111,76,137,85,85,82,16,74,184,235,66,20,127,249,144,196,224,49,141,39,84,45,167,212,237,62,241,63,33,136,142,161,31,249,135,100,155,4,241,193,177,130,55,133,5,229,131,87,50,173,151,86,20,133,76,20,59,46,114,84,131,167,11,107,221,84,21,83,151,205,80,119,60,168,154,210,240,146,200,13,43,16,178,206,1,82,171,115,70,153,123,167,171,155,180,228,217,192,123,10,90,194,182,174,253,22,133,9,185,54,40,80,237,136,64,162,171,29,225,127,204,8,205,143,204,105,208,163,53,235,193,193,88,182,116,22,158,35,180,146,231,16,11,114,60,25,166,204,98,180,166,139,27,252,53,144,245,239,59,116,55,159,76,82,74,114,238,232,35,188,178,168,61,76,255,7,23,167,27,118,29,132,195,202,129,65,250,61,169,62,236,54,207,221,205,98,222,181,231,189,207,109,216,3,69,222,175,98,235,190,251,216,188,253,1,115,126,53,33,45,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("e64afc31-7e45-4a5f-b9cc-d14c8c88f3f2"));
		}

		#endregion

	}

	#endregion

}

