namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingPageAnalyticsAppEventListenerSchema

	/// <exclude/>
	public class LandingPageAnalyticsAppEventListenerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingPageAnalyticsAppEventListenerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingPageAnalyticsAppEventListenerSchema(LandingPageAnalyticsAppEventListenerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("c6059d2a-3748-45e8-a1bb-6b6783eb90e3");
			Name = "LandingPageAnalyticsAppEventListener";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fa0347ef-084e-4088-83fa-fc7c1afcdf4a");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,81,77,107,2,49,16,61,43,248,31,6,122,177,80,118,239,42,130,110,91,44,40,149,214,210,243,152,140,107,96,55,89,50,163,84,196,255,222,100,215,21,43,30,122,10,153,247,230,125,36,22,75,226,10,21,65,230,101,142,86,27,155,47,49,167,137,197,226,32,70,113,175,123,236,117,59,59,14,243,72,153,173,22,243,136,63,19,155,220,146,31,94,192,21,121,143,236,54,146,100,206,83,242,138,74,156,55,196,247,24,223,180,14,172,178,116,54,160,1,127,240,148,27,103,33,43,144,121,0,247,114,76,170,234,101,79,86,230,134,133,130,113,189,151,166,41,140,120,87,150,232,15,227,243,61,16,11,163,80,162,30,11,122,97,160,184,8,197,121,19,54,206,67,209,88,64,21,60,0,91,147,164,213,76,175,68,171,221,58,8,130,138,217,254,21,13,6,112,59,154,34,83,144,58,214,169,47,117,23,36,91,167,57,240,151,181,71,131,214,1,140,221,146,55,162,157,74,99,134,54,132,219,135,55,52,154,96,239,140,134,119,27,124,62,99,199,126,107,152,57,43,244,35,160,154,243,17,226,239,117,58,235,224,159,92,209,91,120,88,163,245,179,55,31,118,72,62,104,106,172,30,189,205,164,44,98,205,25,242,246,171,210,40,228,159,224,210,249,14,58,238,55,114,167,115,73,178,186,233,89,223,155,233,223,225,233,23,214,69,136,162,126,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("c6059d2a-3748-45e8-a1bb-6b6783eb90e3"));
		}

		#endregion

	}

	#endregion

}

