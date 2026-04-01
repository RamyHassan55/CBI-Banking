namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseLanguageIteratorSchema

	/// <exclude/>
	public class CaseLanguageIteratorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseLanguageIteratorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseLanguageIteratorSchema(CaseLanguageIteratorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("2765b5d5-b370-4bb8-88bf-c51f5ee29ecf");
			Name = "CaseLanguageIterator";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,83,203,106,195,48,16,60,59,144,127,88,220,75,2,193,190,231,117,72,122,9,180,80,210,230,84,122,80,212,181,17,200,178,209,131,54,45,254,247,174,20,39,177,83,211,199,193,8,237,206,204,206,174,214,138,21,104,42,198,17,158,80,107,102,202,204,38,235,82,101,34,119,154,89,81,170,225,224,115,56,136,156,17,42,239,64,52,206,134,3,202,220,104,204,9,6,107,201,140,153,194,154,25,188,99,42,119,44,199,141,69,210,40,117,192,165,105,10,115,227,138,130,233,195,178,185,159,0,144,209,39,133,177,190,200,254,0,156,68,64,54,42,38,57,177,211,22,189,114,123,41,56,112,95,181,183,40,76,97,213,235,37,242,253,92,108,151,202,88,237,56,165,200,253,67,80,13,126,191,25,62,58,86,194,10,38,197,7,26,80,248,6,130,216,76,209,244,202,140,192,136,192,53,102,139,184,207,80,156,46,19,56,11,167,215,202,243,138,105,86,128,162,247,88,196,206,160,38,103,10,185,127,130,120,185,163,59,240,115,32,153,167,1,29,200,205,36,250,74,142,118,29,29,232,202,142,61,59,154,194,158,152,163,171,20,132,33,69,39,193,173,147,212,240,34,180,188,105,7,159,95,26,100,228,83,247,76,200,85,249,238,173,108,105,188,52,216,176,66,109,194,149,165,241,228,194,190,197,140,57,105,41,105,25,183,63,144,38,16,6,28,183,201,77,233,191,151,122,116,85,85,106,251,47,142,183,247,59,186,158,249,163,246,91,20,54,13,213,235,113,217,194,189,62,254,53,157,96,253,5,90,25,24,80,134,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("2765b5d5-b370-4bb8-88bf-c51f5ee29ecf"));
		}

		#endregion

	}

	#endregion

}

