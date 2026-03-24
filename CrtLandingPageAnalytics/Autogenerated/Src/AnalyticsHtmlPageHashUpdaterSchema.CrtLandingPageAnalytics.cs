namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: AnalyticsHtmlPageHashUpdaterSchema

	/// <exclude/>
	public class AnalyticsHtmlPageHashUpdaterSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public AnalyticsHtmlPageHashUpdaterSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public AnalyticsHtmlPageHashUpdaterSchema(AnalyticsHtmlPageHashUpdaterSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("e71fa98c-8206-49ec-8483-655fb4f19dc7");
			Name = "AnalyticsHtmlPageHashUpdater";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fa0347ef-084e-4088-83fa-fc7c1afcdf4a");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,82,203,106,195,48,16,60,187,208,127,80,219,139,67,193,31,208,71,160,121,180,49,164,80,154,180,151,210,195,198,218,36,2,89,50,210,170,96,74,254,189,178,236,56,14,113,115,18,218,157,153,157,89,73,65,142,182,128,12,217,216,208,28,20,23,106,243,6,27,124,82,32,75,18,153,189,188,248,189,188,136,156,245,117,182,40,45,97,126,223,222,61,101,182,124,157,87,248,9,90,177,81,104,142,154,29,189,67,125,137,198,128,213,107,74,198,58,207,181,234,239,24,76,166,138,4,9,180,255,2,158,33,35,109,106,132,199,220,24,220,8,173,216,88,130,181,119,172,77,48,163,92,86,22,102,96,183,31,5,7,66,19,240,95,19,92,131,147,52,18,193,101,76,101,129,122,29,167,61,248,193,224,219,19,10,183,146,34,99,89,165,127,86,158,221,177,222,161,209,111,24,220,58,125,69,218,106,238,189,190,25,77,152,17,242,186,95,236,175,76,255,248,200,130,35,179,100,170,13,140,156,144,124,161,157,201,112,17,42,113,216,82,201,48,28,3,86,61,86,20,253,128,97,54,160,216,35,91,129,197,228,148,216,48,238,91,130,60,188,214,59,22,218,10,191,220,210,243,195,62,235,93,151,201,11,210,67,58,239,67,14,227,83,173,148,123,126,61,168,98,46,253,134,249,88,75,151,171,79,144,14,31,94,156,224,195,248,186,163,151,242,235,70,70,172,89,220,202,36,169,157,230,5,149,241,96,31,49,50,72,206,168,38,102,77,217,245,101,241,14,122,147,85,134,58,131,71,101,202,15,243,78,61,4,169,171,71,166,156,148,173,135,102,199,183,71,35,206,37,109,127,141,127,4,132,252,144,182,35,181,192,2,12,120,139,221,80,167,105,119,205,95,66,197,235,239,20,238,117,245,184,184,251,3,124,252,230,193,229,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("e71fa98c-8206-49ec-8483-655fb4f19dc7"));
		}

		#endregion

	}

	#endregion

}

