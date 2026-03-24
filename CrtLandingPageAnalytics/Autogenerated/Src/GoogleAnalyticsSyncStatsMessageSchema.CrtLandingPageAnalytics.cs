namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsSyncStatsMessageSchema

	/// <exclude/>
	public class GoogleAnalyticsSyncStatsMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsSyncStatsMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsSyncStatsMessageSchema(GoogleAnalyticsSyncStatsMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("3d976706-e6f1-4b44-a80a-446483f265e8");
			Name = "GoogleAnalyticsSyncStatsMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("0fb3085b-d38c-4333-ad19-d589593a41f0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,82,193,110,218,64,16,61,59,82,254,97,68,46,68,170,236,123,67,144,16,105,115,41,82,18,232,169,202,97,187,30,204,74,120,215,157,217,37,113,162,254,123,199,107,3,6,210,112,224,176,195,155,55,239,61,63,171,74,228,74,105,132,41,249,31,202,230,198,22,15,170,192,137,85,235,218,27,205,151,23,239,151,23,73,96,153,55,144,123,231,138,245,254,223,116,134,204,2,231,155,29,104,129,68,138,221,210,167,83,103,151,166,8,164,188,113,246,99,0,225,255,230,233,119,165,189,35,19,169,5,115,69,88,8,13,76,215,138,249,43,28,233,152,215,86,207,189,242,220,233,137,43,89,150,193,136,67,89,42,170,199,221,59,174,131,119,64,88,17,50,90,223,81,193,142,11,88,200,128,133,205,112,124,151,45,103,186,165,204,122,156,191,22,46,232,213,99,192,128,221,233,103,153,86,225,247,218,104,208,241,216,25,169,112,98,230,132,82,24,223,163,163,125,10,206,178,167,208,36,36,97,60,196,115,45,226,216,116,235,122,15,135,165,252,70,140,8,154,112,121,59,56,163,110,144,141,211,29,109,223,248,214,227,153,253,225,53,52,253,73,146,69,93,33,220,194,137,183,102,158,54,91,55,17,246,132,127,130,145,15,115,135,121,168,132,63,118,71,246,68,125,172,74,242,183,11,2,109,222,102,113,24,204,12,253,202,229,31,101,98,236,10,201,248,220,233,172,175,223,109,164,118,38,71,216,56,147,195,183,87,212,193,227,240,39,35,73,104,22,117,188,31,14,158,91,75,27,69,177,43,51,101,197,8,137,202,216,174,182,184,117,122,143,126,116,156,78,147,204,124,191,50,30,70,162,196,226,75,255,27,77,168,8,165,84,115,56,56,60,60,248,114,172,228,186,77,173,167,34,102,185,34,103,205,155,132,255,73,100,237,244,112,40,179,127,235,39,180,74,17,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("3d976706-e6f1-4b44-a80a-446483f265e8"));
		}

		#endregion

	}

	#endregion

}

