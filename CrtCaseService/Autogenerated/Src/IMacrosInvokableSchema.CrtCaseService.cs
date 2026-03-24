namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IMacrosInvokableSchema

	/// <exclude/>
	public class IMacrosInvokableSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IMacrosInvokableSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IMacrosInvokableSchema(IMacrosInvokableSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("18dfac77-86d7-4509-8aee-d9ad2c50cf9c");
			Name = "IMacrosInvokable";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,81,185,110,195,48,12,157,109,192,255,64,32,75,187,216,123,19,100,201,80,120,200,214,102,151,21,218,113,107,73,1,69,5,8,138,252,123,117,217,77,175,141,164,222,69,74,11,133,246,44,36,194,11,18,9,107,122,174,119,70,247,227,224,72,240,104,116,85,126,84,101,225,236,168,135,111,16,194,117,85,250,151,21,225,224,97,208,106,70,234,189,208,19,180,123,33,201,120,198,197,188,139,110,194,136,107,154,6,54,214,41,37,232,186,205,253,194,1,211,131,138,36,232,13,65,100,98,61,179,154,59,218,217,117,211,40,61,98,102,102,179,246,203,172,8,129,127,249,197,193,171,69,2,105,180,70,25,118,171,23,224,189,69,17,80,187,5,4,63,218,168,94,12,200,235,88,216,92,220,226,150,127,219,238,145,79,230,8,124,18,12,132,236,72,91,176,76,225,164,23,49,57,140,75,167,253,255,137,20,39,153,186,37,180,110,226,172,176,105,230,113,192,101,213,103,228,116,150,67,144,127,48,221,155,15,15,130,6,167,80,179,125,76,95,151,34,175,80,31,211,31,134,246,246,9,129,106,188,95,17,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("18dfac77-86d7-4509-8aee-d9ad2c50cf9c"));
		}

		#endregion

	}

	#endregion

}

