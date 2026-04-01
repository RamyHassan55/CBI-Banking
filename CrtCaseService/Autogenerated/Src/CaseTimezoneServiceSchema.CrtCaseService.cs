namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseTimezoneServiceSchema

	/// <exclude/>
	public class CaseTimezoneServiceSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseTimezoneServiceSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseTimezoneServiceSchema(CaseTimezoneServiceSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("992549c5-f56f-432d-80e7-1157f156a8c6");
			Name = "CaseTimezoneService";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d90d0856-ba58-4278-952e-572fe1ed6e53");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,83,77,111,226,64,12,61,83,169,255,193,226,68,165,40,185,119,41,82,139,212,150,74,44,43,66,197,1,245,48,36,46,29,53,51,147,29,59,72,217,85,255,123,61,33,64,186,84,236,129,72,243,252,252,241,158,141,85,6,169,84,25,194,2,189,87,228,94,57,30,59,251,170,55,149,87,172,157,189,188,248,123,121,209,171,72,219,13,164,53,49,154,56,69,191,213,25,78,93,142,197,143,115,193,248,54,99,189,109,202,156,231,45,113,125,66,16,76,72,68,146,156,178,98,60,18,186,147,250,111,241,144,59,118,198,52,125,37,158,36,9,12,169,50,70,249,122,212,190,199,138,16,88,27,132,63,206,34,208,110,158,120,207,78,58,244,85,59,172,24,195,94,101,252,18,176,91,42,127,34,75,151,82,4,174,117,161,185,158,227,239,74,123,52,104,153,6,221,71,80,9,55,240,159,148,192,138,91,32,191,10,77,202,106,93,232,12,178,66,17,53,3,47,100,222,48,110,59,16,92,195,157,160,237,43,130,201,28,85,62,179,69,221,53,78,234,132,21,158,152,208,0,15,200,144,137,44,81,117,52,227,192,78,254,165,15,75,229,149,1,43,87,115,211,111,243,38,121,127,52,76,154,192,145,231,145,43,111,105,116,82,123,152,236,67,129,187,154,149,184,187,179,174,183,189,149,44,112,98,183,238,29,7,83,228,55,151,139,121,253,95,179,116,209,143,32,24,132,196,247,206,27,197,130,11,117,42,106,213,6,119,80,252,68,206,70,112,231,242,58,229,186,192,47,148,3,26,47,189,42,75,204,163,208,110,46,127,1,103,9,207,215,108,54,178,95,9,177,15,39,247,16,214,217,40,220,111,102,208,70,14,230,92,65,227,126,111,39,251,203,22,31,177,16,249,241,55,69,158,229,28,5,179,152,5,111,162,78,181,112,238,189,15,249,200,239,227,19,150,197,156,230,190,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("992549c5-f56f-432d-80e7-1157f156a8c6"));
		}

		#endregion

	}

	#endregion

}

