namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: SupportDefMailBoxSchema

	/// <exclude/>
	public class SupportDefMailBoxSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public SupportDefMailBoxSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public SupportDefMailBoxSchema(SupportDefMailBoxSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("6f94f800-c500-433f-bc1d-fcfd77df71cb");
			Name = "SupportDefMailBox";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("77fa8847-960e-4748-ad77-e37bb90e03a0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,147,193,78,195,48,12,134,207,153,196,59,88,229,50,164,169,189,195,182,3,99,32,14,72,147,10,92,81,214,186,35,82,155,84,78,50,54,16,239,78,154,180,99,237,24,71,59,191,237,223,95,18,201,43,212,53,207,16,158,145,136,107,85,152,120,161,100,33,54,150,184,17,74,94,140,190,46,70,204,106,33,55,61,9,225,205,33,159,238,181,193,42,69,99,92,164,97,54,16,246,27,198,78,221,73,93,11,215,228,146,112,227,14,96,81,114,173,175,33,181,117,173,200,220,97,241,196,69,121,171,118,94,148,36,9,76,181,173,42,78,251,121,27,175,72,109,69,142,26,42,39,132,181,218,65,65,170,2,237,237,128,110,135,116,197,201,81,117,109,215,165,200,32,107,38,158,14,132,107,120,252,195,5,107,72,28,220,222,11,44,115,103,119,69,98,203,13,122,147,172,14,1,16,242,92,201,114,15,47,26,201,173,47,49,107,118,135,55,219,139,195,254,236,18,101,30,186,182,113,7,68,73,109,200,102,70,81,51,200,123,14,138,33,13,159,120,148,194,8,94,138,79,4,137,31,32,92,49,151,238,102,85,225,180,136,144,17,22,179,232,100,177,40,57,116,232,49,10,153,154,19,175,64,186,103,50,139,250,230,163,121,179,28,100,135,196,52,241,98,95,219,2,62,25,54,30,0,233,183,188,2,143,152,13,48,185,23,117,194,141,177,239,255,225,61,161,121,87,249,47,54,198,206,130,123,64,3,58,56,245,79,233,204,75,138,255,193,68,104,44,73,61,79,251,109,226,105,210,157,28,81,113,151,218,124,27,55,118,220,109,28,84,131,159,20,59,197,43,47,45,78,67,197,124,60,0,51,129,238,58,83,164,173,200,112,217,204,141,38,237,132,120,89,213,102,127,117,142,150,39,18,242,125,136,223,63,20,145,135,53,22,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("6f94f800-c500-433f-bc1d-fcfd77df71cb"));
		}

		#endregion

	}

	#endregion

}

