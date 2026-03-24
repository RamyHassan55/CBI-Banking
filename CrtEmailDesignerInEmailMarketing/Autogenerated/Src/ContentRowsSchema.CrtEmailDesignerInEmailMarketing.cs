namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ContentRowsSchema

	/// <exclude/>
	public class ContentRowsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ContentRowsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ContentRowsSchema(ContentRowsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("23867393-778c-4c39-bf36-1a4c608e05a5");
			Name = "ContentRows";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("9ef2e2c4-cc1b-4c63-a8fa-19c5996a0dad");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,144,193,110,194,48,12,134,207,84,234,59,88,226,178,93,218,251,202,122,160,7,46,28,16,123,130,144,186,93,180,52,169,98,87,12,33,222,125,73,218,2,99,210,36,78,209,31,251,247,255,217,3,41,211,194,199,137,24,187,172,178,90,163,100,101,13,101,27,52,232,148,44,210,36,77,242,60,135,21,13,93,39,220,169,28,229,30,123,135,132,134,9,4,200,171,15,108,227,149,97,95,0,103,143,190,104,234,240,81,171,88,61,104,43,191,8,240,155,157,144,140,53,52,206,118,190,7,176,19,74,131,103,232,181,96,204,166,200,252,150,217,15,7,173,36,72,45,136,160,26,19,246,62,32,77,206,129,112,177,116,216,134,132,157,179,61,58,86,72,111,176,139,158,88,126,216,32,234,13,122,120,235,128,194,203,159,8,90,17,135,5,34,184,50,241,111,90,38,155,103,220,33,45,38,166,173,183,173,214,141,167,41,33,32,193,25,90,228,34,204,45,224,2,239,96,240,120,223,244,242,90,60,207,244,231,134,79,242,85,179,127,29,236,37,252,214,255,48,63,24,103,248,37,154,122,60,121,144,151,52,249,1,58,3,172,67,73,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("23867393-778c-4c39-bf36-1a4c608e05a5"));
		}

		#endregion

	}

	#endregion

}

