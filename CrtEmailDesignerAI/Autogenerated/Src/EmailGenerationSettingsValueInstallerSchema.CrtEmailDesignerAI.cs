namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: EmailGenerationSettingsValueInstallerSchema

	/// <exclude/>
	public class EmailGenerationSettingsValueInstallerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public EmailGenerationSettingsValueInstallerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public EmailGenerationSettingsValueInstallerSchema(EmailGenerationSettingsValueInstallerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("09f18c32-50d4-4135-b031-f474044e57f0");
			Name = "EmailGenerationSettingsValueInstaller";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("a9703c1e-0f28-4b07-b23b-ccb3fbfc0c20");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,203,106,195,48,16,60,43,144,127,88,220,139,3,197,238,165,151,188,160,184,33,4,218,67,73,211,107,81,157,181,43,106,203,102,37,153,132,146,127,175,36,59,161,78,114,136,13,194,146,103,102,103,86,11,146,151,168,106,158,34,36,132,92,139,42,90,73,165,121,81,172,83,18,181,86,195,193,239,112,192,140,18,50,135,119,36,226,170,202,116,148,84,132,147,211,185,219,173,247,106,141,90,219,173,130,217,25,210,46,50,19,185,33,167,47,163,127,80,171,225,94,118,71,152,219,95,144,20,92,169,49,44,74,46,138,37,74,108,25,71,244,7,47,12,118,246,144,60,81,72,141,36,121,1,169,99,222,70,132,49,172,122,33,23,59,76,141,174,172,34,115,97,89,77,162,225,26,33,173,44,10,148,38,23,242,211,40,164,55,131,180,127,65,153,235,239,164,218,162,77,26,116,109,123,90,189,242,221,166,15,9,38,151,106,214,240,133,148,183,103,181,30,31,236,227,91,194,88,28,199,48,85,166,44,57,237,231,173,65,84,96,47,234,135,231,104,85,188,125,80,222,127,52,141,143,200,19,181,230,196,75,112,183,59,11,92,57,123,5,18,83,215,148,96,238,108,58,59,221,129,165,123,180,39,215,230,171,16,41,52,149,216,66,87,54,220,244,248,208,151,27,129,239,25,107,56,65,211,5,9,109,202,209,217,84,68,75,212,62,104,216,231,223,95,237,236,200,183,142,137,12,194,86,116,122,189,107,199,234,236,188,154,253,120,198,236,246,130,151,167,173,126,235,227,224,86,183,28,218,113,69,185,109,39,214,109,15,127,217,68,183,77,69,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("09f18c32-50d4-4135-b031-f474044e57f0"));
		}

		#endregion

	}

	#endregion

}

