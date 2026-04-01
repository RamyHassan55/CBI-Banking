namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LandingPageUnpublishRequestSchema

	/// <exclude/>
	public class LandingPageUnpublishRequestSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LandingPageUnpublishRequestSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LandingPageUnpublishRequestSchema(LandingPageUnpublishRequestSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("8c00204a-8efb-4edd-89fa-e9dda205772a");
			Name = "LandingPageUnpublishRequest";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c37d542d-00b1-40bf-868b-316e67855219");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,146,77,107,195,48,12,134,207,45,244,63,136,246,58,146,251,218,5,70,96,37,208,67,217,232,238,110,172,166,6,199,206,44,167,163,148,254,247,217,206,199,210,116,107,14,1,189,145,244,62,82,164,88,137,84,177,28,33,53,118,195,20,23,170,216,178,2,163,119,252,170,145,44,205,166,151,217,116,82,147,211,225,227,76,22,203,229,40,142,82,45,37,230,86,104,69,209,26,21,26,145,187,28,151,181,48,88,56,21,82,201,136,158,97,208,126,167,170,122,47,5,29,91,155,144,30,199,49,172,168,46,75,102,206,73,27,183,223,193,106,168,187,26,176,71,4,217,52,131,202,195,118,197,241,160,58,36,231,144,123,239,199,214,19,63,97,15,187,53,186,66,99,5,58,226,109,232,17,224,238,232,130,176,83,194,53,1,193,81,89,113,16,104,64,31,238,240,250,234,33,94,199,183,174,5,239,240,50,14,23,40,208,46,129,252,235,234,211,254,181,126,147,172,112,86,204,130,112,197,57,179,72,240,125,68,103,110,252,182,12,150,250,132,192,164,236,89,78,204,8,166,44,61,1,23,228,166,101,38,200,43,66,132,220,224,225,101,254,96,77,209,103,83,157,113,154,199,73,18,61,28,106,175,181,116,191,206,19,188,58,128,209,80,179,41,180,207,104,176,161,156,245,43,37,191,211,241,12,55,7,17,65,86,40,109,144,251,5,168,129,177,112,121,166,246,247,113,227,248,23,242,70,144,93,249,159,145,192,239,164,16,46,99,210,211,251,232,218,156,195,2,21,111,46,38,196,141,122,43,94,127,0,67,254,179,104,93,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("8c00204a-8efb-4edd-89fa-e9dda205772a"));
		}

		#endregion

	}

	#endregion

}

