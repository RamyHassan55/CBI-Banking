namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: SupportMailBoxBinderSchema

	/// <exclude/>
	public class SupportMailBoxBinderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public SupportMailBoxBinderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public SupportMailBoxBinderSchema(SupportMailBoxBinderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("bc5ec1ee-8ee0-40d6-ba02-de9f93ecefc3");
			Name = "SupportMailBoxBinder";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,80,209,106,131,64,16,124,54,144,127,88,236,75,10,69,223,147,84,168,161,133,66,67,75,83,232,243,121,174,246,64,239,142,189,83,34,33,255,222,211,211,18,105,242,116,236,206,220,236,204,72,86,163,209,140,35,124,33,17,51,170,176,209,78,201,66,148,13,49,43,148,92,46,78,203,69,208,24,33,203,25,133,48,122,97,220,42,18,104,54,87,24,223,152,57,86,93,43,233,80,135,223,17,150,78,14,118,21,51,102,13,135,70,107,69,118,207,68,149,170,99,42,100,142,52,240,226,56,134,173,105,234,154,81,151,140,179,135,161,80,228,32,68,224,132,197,99,248,58,215,248,68,173,140,112,134,186,48,78,162,73,41,190,144,210,77,86,9,14,188,119,112,213,0,172,225,73,235,231,22,165,125,19,198,162,68,74,153,65,247,245,52,120,251,11,177,71,251,163,114,23,227,99,144,244,224,40,175,90,215,129,200,17,90,37,114,120,151,78,241,96,25,217,213,36,237,234,181,120,180,192,253,123,15,125,193,65,144,185,75,209,5,125,130,55,3,58,212,230,11,239,162,222,237,246,102,252,7,184,133,36,171,240,127,89,254,192,121,12,136,50,247,25,135,217,111,231,203,243,47,91,231,60,72,51,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("bc5ec1ee-8ee0-40d6-ba02-de9f93ecefc3"));
		}

		#endregion

	}

	#endregion

}

