namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: SLMExtensionsSchema

	/// <exclude/>
	public class SLMExtensionsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public SLMExtensionsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public SLMExtensionsSchema(SLMExtensionsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("7e7af41a-b3ac-4f70-b275-48bf9b53119e");
			Name = "SLMExtensions";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,84,193,110,219,48,12,61,187,64,255,129,232,46,233,48,216,247,38,205,37,43,208,2,203,48,160,222,118,86,109,38,54,106,75,134,40,165,11,138,252,251,40,201,178,235,164,107,177,75,143,36,31,249,30,169,103,75,209,34,117,162,64,200,81,107,65,106,99,210,149,146,155,122,107,181,48,181,146,233,253,183,245,205,31,131,146,56,160,243,179,231,243,179,196,82,45,183,255,106,152,191,7,72,87,162,65,89,10,77,35,52,166,242,186,197,159,178,54,112,253,126,119,26,193,60,134,7,125,210,184,229,58,172,26,65,4,87,112,164,155,17,89,150,193,130,108,219,10,189,95,246,241,125,135,69,189,169,11,7,7,116,120,71,2,45,154,74,149,148,198,174,236,69,91,103,31,26,110,32,195,122,10,40,60,221,17,89,242,236,9,7,77,235,48,142,85,253,240,205,161,122,172,199,39,120,217,29,106,67,80,88,173,89,13,24,222,18,12,234,22,140,2,83,33,20,138,11,212,41,89,186,203,141,229,167,218,84,208,214,210,26,12,89,203,183,73,7,162,236,152,105,209,9,45,90,144,236,128,235,11,55,225,98,185,138,156,28,45,50,95,31,225,26,141,213,146,150,121,100,76,23,89,204,57,208,244,46,14,149,59,89,253,66,185,90,123,105,52,51,85,77,99,217,13,186,4,231,170,36,217,9,13,188,154,109,220,251,75,124,26,80,179,203,185,7,132,226,96,129,187,146,113,94,201,152,9,64,226,99,20,21,204,124,49,223,119,24,41,146,66,16,158,216,45,189,85,86,95,5,64,36,113,93,60,254,4,26,214,152,79,193,191,68,99,49,138,9,193,231,161,149,47,64,134,250,70,186,147,142,44,246,63,104,20,143,243,183,164,253,86,250,145,31,250,63,20,246,29,31,45,244,171,216,127,232,9,79,203,46,203,69,22,242,154,234,18,55,130,137,166,26,123,194,87,240,135,151,142,251,206,158,222,225,96,218,169,59,163,181,222,178,229,151,0,233,47,50,216,178,79,159,238,30,36,68,215,187,143,172,255,50,124,234,208,255,94,120,120,248,195,248,56,100,167,201,195,95,179,40,193,60,225,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("7e7af41a-b3ac-4f70-b275-48bf9b53119e"));
		}

		#endregion

	}

	#endregion

}

