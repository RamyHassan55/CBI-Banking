namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ActivityCaseValuePairSchema

	/// <exclude/>
	public class ActivityCaseValuePairSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ActivityCaseValuePairSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ActivityCaseValuePairSchema(ActivityCaseValuePairSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("64d1bc1c-8aa8-4e74-bc6c-174824b86bb2");
			Name = "ActivityCaseValuePair";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,144,79,75,196,64,12,197,207,45,244,59,4,246,222,222,93,17,164,7,241,86,86,241,30,103,210,26,104,167,101,50,179,80,150,253,238,206,76,255,176,10,42,222,146,151,188,151,31,49,56,144,76,168,8,94,201,90,148,177,117,101,61,154,150,59,111,209,241,104,138,252,82,228,153,23,54,29,188,204,226,104,56,22,121,80,14,150,186,48,134,186,71,145,59,120,84,142,207,236,230,26,133,222,176,247,212,32,219,180,88,85,21,220,139,31,6,180,243,195,218,159,104,178,36,100,156,128,251,32,56,71,3,76,193,1,99,11,42,68,0,26,13,184,102,150,91,74,117,19,51,249,247,158,21,8,97,79,26,84,164,248,9,34,187,36,144,29,185,177,227,68,214,49,5,238,38,197,44,243,239,164,73,216,50,129,117,224,229,150,201,150,251,242,45,208,70,244,228,89,239,174,103,13,241,125,89,214,145,59,166,66,214,226,250,203,205,200,255,191,123,209,241,231,173,3,25,189,188,32,245,139,250,85,188,126,2,105,190,189,221,16,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("64d1bc1c-8aa8-4e74-bc6c-174824b86bb2"));
		}

		#endregion

	}

	#endregion

}

