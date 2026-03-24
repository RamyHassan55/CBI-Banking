namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: DelayedNotificationCleaningSchema

	/// <exclude/>
	public class DelayedNotificationCleaningSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public DelayedNotificationCleaningSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public DelayedNotificationCleaningSchema(DelayedNotificationCleaningSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("df08478c-376b-45c2-ab1f-44f5d2bfcce8");
			Name = "DelayedNotificationCleaning";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,82,193,78,227,48,16,61,7,137,127,24,101,47,137,132,156,59,148,74,208,194,170,43,129,16,5,237,217,117,166,173,145,99,87,99,187,187,21,234,191,239,196,73,11,97,17,220,60,111,222,188,121,51,99,43,27,244,27,169,16,158,144,72,122,183,12,98,226,236,82,175,34,201,160,157,61,61,121,61,61,201,162,215,118,5,243,157,15,216,112,222,24,84,109,210,139,159,104,145,180,186,56,114,38,142,112,24,137,233,245,7,96,174,214,88,71,131,196,56,103,126,16,174,88,12,38,70,122,15,231,48,69,35,119,88,223,187,160,151,90,37,23,19,131,210,114,125,226,87,85,5,35,31,155,70,210,110,220,199,143,184,33,244,104,131,7,9,170,103,195,139,91,128,91,130,52,132,178,222,65,155,7,251,78,214,139,131,92,245,78,111,19,23,70,43,86,105,237,124,97,134,173,206,126,185,197,205,95,84,49,56,226,202,215,228,239,56,208,29,134,181,171,219,145,30,146,100,151,253,104,63,1,220,6,3,250,111,172,254,239,181,67,54,146,100,3,150,111,121,153,71,143,196,23,180,221,133,242,241,51,199,160,142,128,24,85,137,253,121,113,122,179,17,242,249,248,225,248,30,212,244,219,217,106,10,81,26,216,58,93,67,183,2,44,158,7,189,97,104,229,12,102,83,157,94,236,125,228,3,241,10,207,192,45,94,56,61,134,183,206,37,180,63,46,203,182,146,160,78,107,129,75,176,248,167,223,81,49,84,45,19,55,19,183,228,154,34,255,228,90,249,129,241,123,141,132,69,126,165,90,223,115,180,245,84,6,204,75,193,228,162,20,51,127,31,141,41,74,144,190,111,116,145,234,58,7,226,48,97,153,208,125,150,245,151,102,153,238,216,41,222,119,255,121,0,238,255,1,240,220,72,119,99,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("df08478c-376b-45c2-ab1f-44f5d2bfcce8"));
		}

		#endregion

	}

	#endregion

}

