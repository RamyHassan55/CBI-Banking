namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: SatisfactionLevelPointSchema

	/// <exclude/>
	public class SatisfactionLevelPointSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public SatisfactionLevelPointSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public SatisfactionLevelPointSchema(SatisfactionLevelPointSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("5f9c9fd0-00ee-4522-83fe-fc6de0e0459a");
			Name = "SatisfactionLevelPoint";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c3c90037-274c-4793-841e-197eb228d3cd");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,145,193,106,195,48,12,134,207,9,228,29,4,189,199,247,117,236,146,235,14,133,246,5,84,87,14,134,88,14,150,82,24,165,239,94,187,46,93,86,232,97,62,24,36,255,223,255,75,152,49,144,204,104,9,14,148,18,74,116,218,15,145,157,31,151,132,234,35,119,237,165,107,33,159,77,162,49,215,48,76,40,2,31,176,207,207,226,208,22,209,55,157,105,218,69,207,218,181,85,109,140,129,79,89,66,192,244,243,245,219,170,176,141,172,232,153,18,184,152,64,86,70,48,23,19,1,228,83,86,133,64,185,232,87,142,230,175,229,188,28,39,111,193,222,93,223,13,212,228,5,154,230,101,160,218,184,43,32,58,32,81,31,80,41,167,162,80,255,4,204,154,120,164,21,164,130,23,24,73,183,32,229,186,150,205,223,196,12,117,19,208,248,175,32,209,228,121,124,210,47,105,77,77,220,16,159,234,207,116,237,245,6,196,4,109,225,206,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("5f9c9fd0-00ee-4522-83fe-fc6de0e0459a"));
		}

		#endregion

	}

	#endregion

}

