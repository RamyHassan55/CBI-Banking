namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsSyncStatsBatchMessageSchema

	/// <exclude/>
	public class GoogleAnalyticsSyncStatsBatchMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsSyncStatsBatchMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsSyncStatsBatchMessageSchema(GoogleAnalyticsSyncStatsBatchMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("183538d6-b070-492d-8745-2a54f81b16bf");
			Name = "GoogleAnalyticsSyncStatsBatchMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("0fb3085b-d38c-4333-ad19-d589593a41f0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,148,77,139,27,49,12,134,207,89,216,255,32,210,75,2,101,230,214,67,243,1,219,116,187,20,26,72,73,122,42,61,56,30,101,98,152,177,131,165,73,59,13,253,239,149,61,147,239,237,54,75,72,136,133,252,88,122,245,218,86,149,72,27,165,17,38,158,191,40,155,25,155,207,84,142,15,86,21,53,27,77,247,119,187,251,187,78,69,18,135,121,77,140,101,50,113,69,129,154,141,179,148,60,161,69,111,244,224,144,35,152,39,231,242,226,72,72,166,72,36,72,58,38,45,208,123,69,110,197,194,178,43,147,87,94,5,220,243,9,30,255,21,79,62,41,205,206,155,136,14,159,206,27,143,185,128,96,82,40,162,247,112,81,201,188,182,122,206,138,233,131,98,189,110,203,146,93,105,154,194,144,170,178,84,190,30,183,235,72,0,118,224,113,227,145,208,114,75,131,3,14,72,120,64,2,52,20,215,27,244,176,12,104,40,27,118,178,135,167,39,244,239,11,87,233,245,215,10,43,108,75,248,33,209,77,181,44,140,6,29,143,189,165,110,184,106,239,138,43,216,93,148,229,168,139,204,140,125,21,84,19,121,102,241,204,38,227,82,131,70,132,99,58,172,228,59,36,68,208,30,87,163,238,45,37,118,211,113,114,96,159,74,176,239,246,22,72,239,243,163,173,74,244,106,89,224,240,98,67,107,216,57,123,84,229,71,118,99,153,70,248,75,125,8,174,237,116,22,245,6,97,4,87,202,132,120,18,142,27,196,180,6,64,146,217,238,143,225,63,173,118,104,179,70,190,115,45,103,222,201,192,89,220,119,131,146,177,35,112,43,216,26,50,65,253,96,173,232,159,76,177,122,89,165,87,9,176,111,101,7,57,242,0,40,252,252,167,145,41,242,218,101,207,117,97,236,90,110,55,103,78,167,167,5,185,173,92,67,147,33,152,112,43,144,103,222,200,45,228,186,215,135,209,24,222,13,94,65,216,58,147,193,227,47,212,21,99,239,27,161,23,203,217,230,113,129,234,108,185,31,233,86,249,40,220,84,89,25,164,151,153,197,171,218,60,5,181,60,72,124,41,82,180,212,252,184,101,220,139,160,142,197,159,167,14,127,240,185,200,108,185,215,61,63,184,251,246,178,146,126,227,154,147,42,162,151,214,222,89,243,27,123,237,8,250,47,152,168,137,158,7,37,246,23,95,216,114,247,142,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("183538d6-b070-492d-8745-2a54f81b16bf"));
		}

		#endregion

	}

	#endregion

}

