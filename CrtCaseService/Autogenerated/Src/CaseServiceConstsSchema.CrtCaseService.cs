namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseServiceConstsSchema

	/// <exclude/>
	public class CaseServiceConstsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseServiceConstsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseServiceConstsSchema(CaseServiceConstsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("4c96d0a7-eeac-44d3-afd1-3e92a22d0b77");
			Name = "CaseServiceConsts";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,146,205,106,2,49,20,70,215,10,190,67,176,155,118,145,193,249,115,156,74,23,147,168,165,155,34,234,11,164,153,235,16,24,147,105,110,70,144,226,187,55,51,226,166,180,84,220,37,57,31,223,185,132,171,197,1,176,17,18,200,14,172,21,104,246,46,224,70,239,85,213,90,225,148,209,228,107,52,28,13,7,45,42,93,145,237,9,29,28,230,253,203,131,133,170,227,188,22,136,207,100,221,126,212,74,246,164,233,143,4,157,47,144,68,118,156,112,129,176,5,123,84,18,124,61,58,236,122,7,63,162,22,68,105,116,125,34,175,173,42,201,6,208,212,109,55,195,187,113,106,175,100,63,207,174,169,223,74,242,210,71,130,181,176,8,143,227,40,78,179,156,231,140,22,139,56,162,73,26,77,232,44,78,99,154,177,34,46,10,206,210,184,200,199,79,243,255,132,75,116,234,208,75,54,240,217,2,186,223,92,124,182,98,225,100,21,82,230,149,52,97,211,132,230,139,48,165,89,180,74,38,203,101,22,78,51,118,131,171,144,78,29,149,59,249,86,15,85,35,180,219,152,26,118,198,235,174,236,242,81,193,159,209,123,37,92,222,44,225,242,94,9,147,183,91,124,182,211,156,47,123,5,186,188,172,86,119,61,127,3,184,175,181,218,161,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("4c96d0a7-eeac-44d3-afd1-3e92a22d0b77"));
		}

		#endregion

	}

	#endregion

}

