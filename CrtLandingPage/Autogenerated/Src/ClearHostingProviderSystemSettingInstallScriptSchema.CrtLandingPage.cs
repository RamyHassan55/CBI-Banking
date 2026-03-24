namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ClearHostingProviderSystemSettingInstallScriptSchema

	/// <exclude/>
	public class ClearHostingProviderSystemSettingInstallScriptSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ClearHostingProviderSystemSettingInstallScriptSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ClearHostingProviderSystemSettingInstallScriptSchema(ClearHostingProviderSystemSettingInstallScriptSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("652a7c14-1d21-4f9a-90ad-466fcbc8677e");
			Name = "ClearHostingProviderSystemSettingInstallScript";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fcd19c09-7532-4da4-a534-367aa5418760");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,83,219,106,219,64,16,125,86,32,255,48,184,47,54,24,233,61,182,3,197,117,218,64,27,12,78,251,190,149,198,246,130,180,171,206,238,42,49,33,255,222,217,139,93,75,110,66,43,208,101,231,114,102,206,153,145,18,13,154,86,148,8,143,72,36,140,222,218,124,169,213,86,238,28,9,43,181,186,190,122,185,190,202,156,145,106,7,155,131,177,216,204,78,231,243,20,66,182,3,95,167,208,13,90,203,159,102,89,11,99,96,49,136,238,87,201,207,226,25,135,43,124,32,220,177,3,66,246,13,191,80,208,23,109,124,196,154,116,39,43,164,216,78,74,187,87,198,138,186,222,148,36,91,27,16,138,162,128,185,113,77,35,232,112,155,206,41,10,76,8,3,187,23,22,74,15,109,248,27,225,171,80,149,47,32,118,56,168,117,231,234,58,180,242,192,130,129,9,149,193,196,210,9,91,110,65,90,3,157,168,29,130,52,128,191,156,168,193,106,24,189,13,59,133,37,217,51,247,40,63,54,94,156,117,222,186,159,181,44,185,81,47,228,255,41,1,55,112,223,51,172,158,177,116,86,19,195,190,4,153,78,74,223,73,172,43,150,122,77,178,19,22,189,203,223,109,60,66,169,25,6,140,165,63,155,144,10,46,117,133,60,223,119,104,246,212,27,205,222,132,93,19,118,82,59,243,35,104,248,46,228,133,114,179,196,6,85,21,9,245,217,125,67,187,215,129,94,16,51,58,135,43,18,12,81,33,140,27,33,163,118,97,73,211,214,228,167,204,98,152,58,111,5,137,6,20,179,92,140,156,65,226,37,87,88,250,228,209,237,35,195,121,155,167,156,140,249,188,8,25,1,32,77,185,211,178,58,246,48,254,222,195,128,62,228,4,252,175,153,101,188,120,227,190,7,22,11,80,172,249,49,34,179,123,210,79,160,240,9,62,210,206,53,168,236,3,187,87,207,37,182,62,126,236,59,214,219,1,202,100,18,38,149,189,134,103,26,81,233,136,56,253,56,161,225,127,158,127,198,232,27,96,77,47,87,102,154,166,158,175,154,214,30,82,45,207,165,95,98,209,223,138,64,9,6,215,69,23,124,250,132,219,127,110,36,104,117,70,246,245,175,187,20,173,125,35,219,126,3,202,88,82,9,67,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("652a7c14-1d21-4f9a-90ad-466fcbc8677e"));
		}

		#endregion

	}

	#endregion

}

