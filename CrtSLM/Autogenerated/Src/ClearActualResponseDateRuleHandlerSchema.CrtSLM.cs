namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ClearActualResponseDateRuleHandlerSchema

	/// <exclude/>
	public class ClearActualResponseDateRuleHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ClearActualResponseDateRuleHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ClearActualResponseDateRuleHandlerSchema(ClearActualResponseDateRuleHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("49896338-f1b3-42b4-a897-a58e4a82829a");
			Name = "ClearActualResponseDateRuleHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,203,110,194,48,16,60,7,137,127,112,225,2,82,21,238,180,84,106,67,95,135,170,8,104,239,219,100,73,45,57,118,180,182,145,80,197,191,215,142,131,32,41,17,61,89,59,158,217,217,89,91,66,129,186,132,20,217,26,137,64,171,141,137,19,37,55,60,183,4,134,43,217,239,253,244,123,145,213,92,230,108,181,211,6,139,155,86,29,207,193,192,31,48,81,66,96,234,59,232,248,25,37,18,79,143,156,83,175,162,80,242,252,13,97,23,30,207,31,58,175,30,165,225,134,163,238,36,60,65,106,20,117,50,78,178,199,9,8,148,25,144,167,58,242,144,48,119,48,75,4,104,61,117,7,2,221,167,198,130,88,186,37,186,164,232,86,129,75,43,240,5,100,38,144,42,85,105,191,4,79,89,234,69,255,208,176,201,100,202,94,19,208,205,70,145,127,134,227,4,78,104,200,250,32,110,144,69,229,80,153,29,220,46,251,140,62,52,146,235,35,195,43,49,219,40,199,172,242,139,90,164,89,139,230,87,24,237,131,243,208,173,42,140,87,215,245,172,11,82,37,146,127,146,179,147,182,28,90,101,152,34,71,83,57,69,37,241,173,139,193,116,13,92,176,126,67,243,173,178,243,190,91,197,51,22,118,49,170,254,204,142,97,117,28,162,243,13,27,5,196,125,96,179,222,149,152,185,79,109,11,249,9,194,226,173,95,231,154,23,120,55,26,132,13,103,152,189,203,193,152,93,205,152,180,66,28,218,68,117,143,21,154,19,121,83,116,29,20,33,227,190,35,88,141,53,179,58,236,23,254,132,199,159,195,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("49896338-f1b3-42b4-a897-a58e4a82829a"));
		}

		#endregion

	}

	#endregion

}

