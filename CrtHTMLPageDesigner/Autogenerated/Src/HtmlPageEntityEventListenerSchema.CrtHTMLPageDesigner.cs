namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: HtmlPageEntityEventListenerSchema

	/// <exclude/>
	public class HtmlPageEntityEventListenerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public HtmlPageEntityEventListenerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public HtmlPageEntityEventListenerSchema(HtmlPageEntityEventListenerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("d8cd1156-a0d3-4e99-925b-b7ad178e223a");
			Name = "HtmlPageEntityEventListener";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c8382e17-7ef4-48e4-b928-4bcb7ca99534");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,82,219,78,132,48,16,125,102,19,255,97,196,23,72,12,31,224,45,113,113,189,36,222,146,93,125,49,62,116,97,22,106,160,37,109,33,49,234,191,59,180,176,178,134,245,242,66,233,204,153,51,103,166,71,176,18,117,197,18,132,88,153,75,83,22,247,44,195,51,212,60,19,168,118,38,111,59,19,175,214,92,100,54,189,184,185,30,166,15,215,201,5,42,197,180,92,153,40,150,10,163,153,48,220,112,212,191,2,162,89,131,194,108,199,157,179,196,72,229,152,8,179,167,48,227,82,64,92,48,173,15,160,151,107,217,94,45,213,53,215,6,173,114,130,63,141,36,130,121,146,99,201,110,105,108,56,6,191,31,201,15,159,169,128,11,131,74,176,2,146,182,193,79,252,112,0,83,166,199,59,123,111,182,251,90,237,13,154,92,166,164,247,190,94,22,60,113,201,202,254,131,108,104,96,158,34,52,146,167,112,39,230,172,161,53,4,114,249,130,137,1,141,34,69,181,15,174,205,20,87,180,19,219,236,84,101,26,48,132,246,121,60,175,97,10,208,66,104,164,192,129,67,87,123,184,6,36,181,82,4,154,23,117,70,40,7,143,46,208,44,94,43,76,99,89,212,165,120,100,69,141,71,218,40,146,112,18,248,177,164,117,184,10,63,252,34,170,20,54,92,214,122,156,233,174,248,23,89,167,170,221,244,31,85,181,15,210,194,199,36,141,211,108,147,244,157,137,175,32,24,110,105,247,120,115,214,247,247,13,185,131,116,123,239,31,195,10,202,153,206,31,170,148,145,157,72,143,181,171,179,178,85,117,116,213,59,235,242,11,119,18,116,50,188,65,113,228,206,22,22,184,169,58,208,135,253,46,201,130,209,218,52,189,91,208,97,62,58,23,82,212,25,209,222,93,116,51,72,177,79,62,32,156,197,7,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("d8cd1156-a0d3-4e99-925b-b7ad178e223a"));
		}

		#endregion

	}

	#endregion

}

