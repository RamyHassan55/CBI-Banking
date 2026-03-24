namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: JunkFilterConstsSchema

	/// <exclude/>
	public class JunkFilterConstsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public JunkFilterConstsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public JunkFilterConstsSchema(JunkFilterConstsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("2a61b393-d894-421c-9a81-38a44ebfda70");
			Name = "JunkFilterConsts";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("59955e0a-90db-4796-8f0c-f9403b7d622f");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,145,177,106,195,48,16,134,231,26,252,14,194,147,77,192,116,78,9,4,210,148,146,33,4,106,186,148,14,170,124,9,162,242,157,57,73,131,9,126,247,156,146,22,226,193,144,245,215,247,253,210,157,80,119,224,123,109,64,53,192,172,61,29,67,189,33,60,218,83,100,29,44,97,158,157,243,236,169,143,63,206,26,229,131,100,70,25,167,189,87,187,136,191,111,214,5,96,17,124,240,130,37,244,159,53,41,20,131,45,158,212,182,211,214,53,67,15,27,106,65,173,84,113,13,138,151,57,254,149,228,28,239,133,91,50,111,108,49,240,48,185,33,5,243,252,59,232,22,248,192,212,3,135,161,161,164,4,122,148,255,212,46,130,63,232,32,211,163,168,235,162,60,63,143,213,215,106,249,93,214,139,106,82,243,183,52,150,2,66,55,76,71,188,175,88,151,117,181,120,68,253,0,108,211,214,93,236,112,47,31,152,222,126,203,174,246,152,103,227,5,141,36,4,64,216,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("2a61b393-d894-421c-9a81-38a44ebfda70"));
		}

		#endregion

	}

	#endregion

}

