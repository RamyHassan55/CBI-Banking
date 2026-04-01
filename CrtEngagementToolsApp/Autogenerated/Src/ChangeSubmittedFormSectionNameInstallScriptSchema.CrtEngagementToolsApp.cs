namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ChangeSubmittedFormSectionNameInstallScriptSchema

	/// <exclude/>
	public class ChangeSubmittedFormSectionNameInstallScriptSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ChangeSubmittedFormSectionNameInstallScriptSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ChangeSubmittedFormSectionNameInstallScriptSchema(ChangeSubmittedFormSectionNameInstallScriptSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("624a87f7-5d84-4a4a-b2ff-1b615daacab2");
			Name = "ChangeSubmittedFormSectionNameInstallScript";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("709c9f3d-93db-4113-8c9c-d7041a1eb4a9");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,84,77,107,27,49,16,61,219,224,255,48,40,151,93,8,235,123,18,7,210,77,82,124,104,41,108,67,143,65,89,77,28,17,173,180,213,135,91,83,250,223,59,146,108,135,93,219,37,57,234,233,189,55,51,79,131,52,239,208,245,188,69,168,45,114,47,77,181,212,206,115,165,154,214,202,222,187,217,244,207,108,58,9,78,234,21,52,27,231,177,187,28,157,171,218,40,133,45,73,181,171,62,163,70,43,219,3,206,45,247,252,13,252,142,214,114,103,158,61,105,187,206,232,227,55,22,79,225,213,237,39,186,162,203,51,139,43,170,11,181,226,206,93,64,253,194,245,10,155,240,212,73,239,81,220,27,219,53,185,179,175,52,230,96,176,36,151,218,163,213,92,65,27,245,31,145,195,5,44,7,192,221,111,108,131,55,22,200,54,38,54,233,173,92,115,143,64,28,47,91,160,112,133,209,106,67,103,27,7,122,212,248,107,80,41,150,128,5,176,4,58,23,211,100,151,239,49,50,74,156,54,138,32,60,19,250,78,51,119,100,248,218,136,100,152,160,116,207,114,250,123,183,7,135,182,54,90,103,62,60,134,193,121,68,94,27,41,96,185,77,62,199,134,197,200,97,104,80,66,74,116,50,242,165,158,14,10,17,107,205,45,132,94,196,82,11,160,148,225,33,29,138,33,247,156,2,218,184,47,70,4,133,172,76,194,73,213,160,47,88,205,251,72,96,231,64,139,29,58,93,125,227,150,34,165,126,139,163,143,86,238,212,63,94,208,34,233,41,46,86,86,75,119,247,51,112,85,28,154,156,202,120,111,116,163,197,91,27,255,115,58,246,244,101,153,99,200,17,84,187,128,51,250,55,63,197,124,62,135,43,23,186,142,219,205,245,150,225,128,126,129,87,190,66,144,121,175,193,165,197,174,174,230,59,230,94,218,199,22,64,83,181,5,27,198,202,174,227,75,66,187,7,72,158,216,73,220,135,39,69,91,151,54,224,67,47,63,94,151,17,107,55,219,118,190,51,212,34,127,12,179,41,33,255,0,254,223,115,58,227,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("624a87f7-5d84-4a4a-b2ff-1b615daacab2"));
		}

		#endregion

	}

	#endregion

}

