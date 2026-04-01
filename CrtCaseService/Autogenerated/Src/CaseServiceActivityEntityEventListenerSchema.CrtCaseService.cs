namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseServiceActivityEntityEventListenerSchema

	/// <exclude/>
	public class CaseServiceActivityEntityEventListenerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseServiceActivityEntityEventListenerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseServiceActivityEntityEventListenerSchema(CaseServiceActivityEntityEventListenerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("a4b683c7-72c3-4c1e-8098-c1f01ecac343");
			Name = "CaseServiceActivityEntityEventListener";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("f1773fe1-394d-47f6-b668-13c7c22c835e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,229,85,75,107,219,64,16,62,59,144,255,48,40,23,27,140,116,119,108,129,99,210,54,208,23,196,57,149,30,198,171,145,189,197,154,53,187,107,129,9,253,239,29,237,74,34,46,54,73,3,133,148,158,140,102,103,190,199,236,39,153,177,34,183,67,69,176,36,107,209,153,210,167,11,195,165,94,239,45,122,109,248,242,226,241,242,98,176,119,154,215,71,45,150,210,91,246,218,107,114,215,207,53,164,183,53,177,63,223,247,14,149,55,54,34,73,207,149,165,181,48,195,98,139,206,77,96,129,142,238,201,214,90,209,92,121,93,107,127,8,192,135,128,250,81,59,79,76,54,76,126,59,113,48,188,87,27,170,240,179,24,133,25,36,29,68,50,134,182,196,242,99,202,225,203,104,70,163,239,194,163,217,147,101,220,130,106,36,190,80,33,76,224,70,26,79,106,31,60,6,253,189,245,79,228,55,166,16,243,95,247,171,173,86,241,48,203,50,152,186,125,85,161,61,228,31,144,139,45,57,160,0,7,119,236,200,250,102,183,212,32,167,211,172,107,236,39,119,104,177,10,110,103,137,35,46,200,38,121,144,1,241,73,102,66,203,233,9,74,242,229,134,132,159,8,148,165,114,150,44,39,103,175,59,104,186,161,82,170,129,97,110,215,46,129,44,135,14,90,179,243,200,146,58,101,216,163,230,70,184,23,248,32,30,10,244,120,164,102,23,182,0,166,22,66,93,16,212,70,23,240,133,163,105,42,134,102,245,131,84,231,99,12,145,127,94,202,37,245,244,64,35,104,146,60,24,212,104,97,19,182,103,229,246,67,200,98,0,15,233,123,242,211,187,83,183,25,242,27,87,110,243,225,232,186,7,194,182,67,144,34,59,160,107,249,99,83,203,148,198,225,14,176,87,222,205,143,123,164,244,65,142,228,21,100,113,36,81,104,185,86,34,42,125,226,184,179,74,241,252,231,243,9,121,216,201,90,255,179,128,116,158,79,7,228,55,254,183,149,144,94,250,31,38,164,159,123,101,66,168,120,59,1,57,126,131,255,86,62,254,197,239,71,39,252,53,225,56,251,245,184,146,106,252,255,9,207,177,122,92,148,218,47,140,237,68,250,50,8,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("a4b683c7-72c3-4c1e-8098-c1f01ecac343"));
		}

		#endregion

	}

	#endregion

}

