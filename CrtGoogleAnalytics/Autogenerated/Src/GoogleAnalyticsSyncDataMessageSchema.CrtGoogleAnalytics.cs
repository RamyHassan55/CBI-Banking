namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsSyncDataMessageSchema

	/// <exclude/>
	public class GoogleAnalyticsSyncDataMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsSyncDataMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsSyncDataMessageSchema(GoogleAnalyticsSyncDataMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("00fc9232-c7f4-4246-8bd7-b378eec9cbae");
			Name = "GoogleAnalyticsSyncDataMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ee36a851-184d-48fc-93e8-63ff7d03af9c");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,82,77,79,194,64,16,61,151,196,255,48,193,11,36,166,189,43,146,16,80,78,36,126,224,201,120,88,183,67,217,132,238,214,217,93,180,26,255,187,211,109,129,242,33,135,30,58,121,243,230,189,183,79,139,28,109,33,36,194,152,220,212,152,108,133,35,45,86,165,83,210,198,51,180,86,100,104,47,58,63,23,157,200,91,165,51,152,35,145,176,102,225,226,177,209,11,149,121,18,78,25,125,115,18,64,248,223,60,190,23,210,25,82,104,25,193,152,75,194,140,105,96,188,18,214,94,195,129,146,231,82,203,137,112,162,17,20,54,146,36,129,129,245,121,46,168,28,54,255,97,27,156,1,194,130,208,162,118,13,19,108,169,192,50,23,164,76,6,121,205,22,111,200,146,22,219,235,220,120,185,124,244,232,177,57,250,198,211,194,191,175,148,4,25,206,156,215,8,71,38,142,24,153,240,39,88,217,185,55,218,58,242,85,50,28,194,67,184,86,35,14,221,214,118,119,112,88,240,55,176,136,32,9,23,183,221,243,226,186,201,48,222,178,182,109,111,28,158,95,239,245,161,42,68,20,205,203,2,225,22,142,156,85,243,184,90,186,9,176,39,252,240,138,223,99,130,169,47,152,62,52,134,247,88,123,40,72,244,219,196,128,58,173,147,216,143,101,134,110,105,210,83,137,40,189,68,82,46,53,50,105,203,55,107,46,155,74,17,214,70,165,112,247,133,210,59,236,189,88,36,142,76,163,12,247,253,222,239,198,210,90,80,168,200,76,104,54,66,172,50,148,170,174,107,25,79,209,13,14,194,169,130,121,222,109,12,123,129,39,210,248,217,126,160,17,101,62,231,66,246,186,251,119,187,87,135,66,250,117,104,45,17,33,202,37,25,173,190,57,251,51,137,213,211,253,33,207,254,0,183,188,80,82,229,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("00fc9232-c7f4-4246-8bd7-b378eec9cbae"));
		}

		#endregion

	}

	#endregion

}

