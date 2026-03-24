namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ICategoryProviderWrapperSchema

	/// <exclude/>
	public class ICategoryProviderWrapperSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ICategoryProviderWrapperSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ICategoryProviderWrapperSchema(ICategoryProviderWrapperSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("f178ef2e-9d2a-41d3-899c-ea2ef4382810");
			Name = "ICategoryProviderWrapper";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c3c90037-274c-4793-841e-197eb228d3cd");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,101,145,203,106,195,48,16,69,215,49,248,31,134,172,90,40,214,7,212,245,162,134,134,46,10,165,41,116,173,216,99,163,96,61,24,73,37,166,228,223,59,142,95,105,186,17,210,48,231,222,59,35,35,53,122,39,43,132,79,36,146,222,54,33,43,173,105,84,27,73,6,101,77,154,252,164,201,38,122,101,90,216,247,62,160,126,76,19,174,8,33,32,247,81,107,73,125,49,189,223,201,126,171,26,161,146,126,56,2,182,150,122,104,200,106,208,82,117,112,176,167,108,70,197,21,235,226,161,83,21,40,19,144,154,33,203,107,57,193,147,34,125,145,116,14,137,123,135,52,255,204,47,133,29,134,213,148,176,227,107,61,154,251,232,156,165,176,132,0,215,177,75,13,214,112,95,165,156,66,19,124,182,8,139,91,229,220,73,146,26,12,239,234,105,187,18,219,226,99,185,63,192,209,42,195,154,202,128,15,196,219,202,114,113,193,86,21,194,16,201,248,162,252,179,153,219,112,204,205,141,3,185,139,170,30,70,155,161,23,102,246,35,242,198,196,179,61,221,141,126,87,163,220,243,23,109,206,105,114,254,5,151,214,217,95,222,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("f178ef2e-9d2a-41d3-899c-ea2ef4382810"));
		}

		#endregion

	}

	#endregion

}

