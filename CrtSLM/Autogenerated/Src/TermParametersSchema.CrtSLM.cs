namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: TermParametersSchema

	/// <exclude/>
	public class TermParametersSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public TermParametersSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public TermParametersSchema(TermParametersSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("cce52b8e-5cff-471e-bee8-6f28959e4246");
			Name = "TermParameters";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,181,148,109,75,195,48,16,199,95,111,176,239,112,190,17,5,105,241,181,219,64,134,136,136,32,78,125,31,211,219,12,164,73,185,75,132,34,126,119,211,244,97,237,116,62,11,45,52,185,255,93,254,191,227,26,207,202,172,97,89,178,195,252,100,50,246,189,101,178,176,90,163,116,202,26,78,206,209,32,41,25,36,147,177,17,57,114,33,36,194,45,18,9,182,43,23,180,102,165,214,158,68,37,79,150,72,79,74,98,8,231,147,241,243,100,60,74,211,20,166,236,243,92,80,57,111,214,85,20,10,65,161,154,67,226,164,149,165,61,93,225,31,180,146,32,181,96,142,9,215,157,62,68,171,202,163,130,212,147,112,8,236,194,217,18,8,69,102,141,46,65,25,7,87,202,220,170,28,239,140,114,247,66,123,132,25,28,71,132,209,27,71,113,99,33,52,154,76,16,168,12,141,83,43,133,148,116,226,190,175,214,216,185,87,89,151,117,145,65,116,52,90,163,59,137,31,220,124,188,236,60,113,195,3,43,75,193,61,23,161,221,8,89,133,36,133,150,94,215,45,253,208,198,37,150,145,239,90,40,154,182,118,56,105,217,143,170,102,204,225,166,41,30,207,228,95,123,101,171,125,101,237,95,188,46,155,226,159,122,221,233,246,6,157,39,195,224,30,133,219,120,221,204,27,8,10,174,45,81,24,241,29,142,227,14,213,117,230,167,65,62,44,195,93,250,52,109,85,61,208,7,107,53,92,240,162,214,12,129,14,14,27,164,58,111,11,55,169,135,117,62,123,51,192,223,163,238,166,233,119,212,189,236,111,0,15,167,109,27,120,24,253,43,96,55,188,82,126,2,187,93,226,139,196,65,242,62,232,238,126,236,239,127,48,29,33,216,187,86,246,102,241,166,73,206,242,194,149,221,95,26,222,240,188,2,144,29,25,132,194,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("cce52b8e-5cff-471e-bee8-6f28959e4246"));
		}

		#endregion

	}

	#endregion

}

