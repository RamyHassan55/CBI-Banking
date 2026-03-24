namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: HideCampaignFromAppHubInstallScriptExecutorSchema

	/// <exclude/>
	public class HideCampaignFromAppHubInstallScriptExecutorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public HideCampaignFromAppHubInstallScriptExecutorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public HideCampaignFromAppHubInstallScriptExecutorSchema(HideCampaignFromAppHubInstallScriptExecutorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("a9309691-f5fd-411f-b0a1-64ed3ada8d17");
			Name = "HideCampaignFromAppHubInstallScriptExecutor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("e93a4a73-bac6-4521-8c50-1b385f4aa491");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,65,110,219,64,12,60,43,64,254,192,250,164,0,129,30,208,52,5,26,199,73,124,112,47,110,123,45,232,21,237,108,179,218,21,184,92,183,66,224,191,151,210,218,78,108,168,64,1,233,32,13,57,28,206,208,99,67,177,69,67,240,141,152,49,134,181,84,211,224,215,118,147,24,197,6,127,121,241,122,121,81,164,104,253,6,150,93,20,106,20,119,142,76,15,198,234,145,60,177,53,55,199,154,247,52,76,255,250,95,205,188,88,177,20,181,64,75,218,180,114,214,128,113,24,35,60,217,154,166,216,180,104,55,254,129,67,243,165,109,159,210,106,238,163,160,115,75,195,182,149,217,31,50,73,2,195,71,152,143,2,202,217,203,46,90,182,91,20,2,38,172,131,119,29,68,225,94,205,79,108,219,105,168,9,110,97,50,101,89,32,191,144,40,112,152,27,117,232,36,107,59,114,108,131,173,7,113,138,149,223,35,177,250,228,179,15,144,78,62,175,96,24,94,12,75,118,75,243,76,13,46,208,227,134,24,104,228,223,237,89,127,53,210,120,243,142,17,98,23,23,161,78,142,102,181,21,109,31,33,213,100,36,87,223,117,95,53,228,114,162,233,237,189,162,186,95,239,250,92,117,30,177,69,6,19,188,18,247,1,43,185,167,223,112,111,135,18,228,238,83,118,240,26,194,234,151,246,125,222,175,90,188,170,145,106,168,178,30,189,221,13,200,46,211,218,53,148,31,78,116,87,15,36,230,185,79,248,254,174,124,155,120,117,112,175,96,146,196,62,183,103,174,211,254,37,137,158,98,106,252,15,116,73,23,156,71,77,167,38,175,26,132,19,237,247,57,235,193,45,149,25,217,237,227,205,183,55,164,155,207,135,254,47,221,195,41,140,185,216,203,213,87,159,191,120,168,246,129,97,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("a9309691-f5fd-411f-b0a1-64ed3ada8d17"));
		}

		#endregion

	}

	#endregion

}

