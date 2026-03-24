namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ISupportMailBoxRepositorySchema

	/// <exclude/>
	public class ISupportMailBoxRepositorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ISupportMailBoxRepositorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ISupportMailBoxRepositorySchema(ISupportMailBoxRepositorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("8b7ac775-ccba-4dbf-ac65-c1caf69c4152");
			Name = "ISupportMailBoxRepository";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("77fa8847-960e-4748-ad77-e37bb90e03a0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,145,193,106,195,48,12,134,207,13,228,29,124,220,46,201,3,172,228,176,209,149,30,10,101,217,11,56,153,82,4,177,21,36,121,52,140,190,251,236,100,165,165,217,97,236,104,249,251,245,255,146,188,117,32,131,109,193,188,3,179,21,234,180,120,33,223,225,49,176,85,36,159,103,95,121,182,10,130,254,104,234,81,20,220,211,221,59,242,125,15,109,130,165,216,130,7,198,246,202,220,182,101,40,54,94,81,17,36,2,17,41,203,210,172,37,56,103,121,172,126,222,7,166,79,252,0,49,212,168,69,159,122,80,103,36,12,3,177,26,103,177,111,232,4,82,92,228,229,141,126,8,77,143,173,65,175,192,93,26,106,87,207,186,125,148,61,211,233,13,6,18,84,226,49,194,105,174,69,130,169,176,5,149,223,29,151,150,115,133,65,3,123,169,234,133,104,93,94,254,18,188,219,248,224,128,109,211,195,122,90,197,88,37,183,253,5,127,120,156,23,243,247,92,166,99,114,70,166,75,24,1,213,184,176,127,36,189,203,41,202,105,241,215,104,175,209,101,62,119,61,123,76,73,87,231,60,59,127,3,59,101,164,232,67,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("8b7ac775-ccba-4dbf-ac65-c1caf69c4152"));
		}

		#endregion

	}

	#endregion

}

