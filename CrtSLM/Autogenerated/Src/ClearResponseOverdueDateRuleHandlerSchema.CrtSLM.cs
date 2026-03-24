namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ClearResponseOverdueDateRuleHandlerSchema

	/// <exclude/>
	public class ClearResponseOverdueDateRuleHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ClearResponseOverdueDateRuleHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ClearResponseOverdueDateRuleHandlerSchema(ClearResponseOverdueDateRuleHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("1d4ab598-48ce-4320-8df6-59d7c39071fd");
			Name = "ClearResponseOverdueDateRuleHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,203,110,194,48,16,60,7,137,127,112,233,37,72,85,232,153,150,30,26,250,58,84,69,64,123,119,147,37,181,228,216,209,218,70,66,21,255,222,141,29,4,4,162,114,178,118,60,179,179,179,182,226,37,152,138,103,192,150,128,200,141,94,217,36,213,106,37,10,135,220,10,173,250,189,223,126,47,114,70,168,130,45,54,198,66,121,215,170,147,41,183,252,4,76,181,148,144,213,29,76,242,2,10,80,100,123,206,161,87,89,106,117,254,6,161,11,79,166,143,157,87,79,202,10,43,192,116,18,158,121,102,53,118,50,14,178,39,41,151,160,114,142,53,149,200,215,8,5,193,44,149,220,152,49,29,192,113,78,235,163,140,240,177,6,204,29,208,46,96,238,36,188,114,149,75,64,47,171,220,183,20,25,203,106,213,37,34,54,26,141,217,91,202,205,113,167,168,126,136,253,12,164,183,232,234,40,52,202,204,91,120,183,157,221,5,70,241,167,1,164,70,42,60,20,115,71,229,144,121,195,168,69,154,180,104,245,22,163,109,176,190,166,109,133,249,154,186,25,118,134,186,2,172,95,229,236,168,45,135,86,25,166,40,192,122,167,168,66,177,166,24,204,52,192,63,214,239,96,127,116,126,222,119,173,69,206,194,46,98,255,109,54,12,252,177,139,46,86,44,14,8,253,97,187,220,84,144,211,191,118,165,250,226,210,193,189,80,246,33,30,180,118,60,24,178,171,9,187,221,181,136,26,253,2,236,129,244,84,117,67,146,16,112,219,145,170,193,142,131,18,246,7,247,148,160,217,195,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("1d4ab598-48ce-4320-8df6-59d7c39071fd"));
		}

		#endregion

	}

	#endregion

}

