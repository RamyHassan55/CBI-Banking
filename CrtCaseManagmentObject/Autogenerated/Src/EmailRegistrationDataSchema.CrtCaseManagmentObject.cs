namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: EmailRegistrationDataSchema

	/// <exclude/>
	public class EmailRegistrationDataSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public EmailRegistrationDataSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public EmailRegistrationDataSchema(EmailRegistrationDataSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("472ffed7-d0b2-439a-902c-a1ccb20b2456");
			Name = "EmailRegistrationData";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c3c90037-274c-4793-841e-197eb228d3cd");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,83,77,107,195,48,12,61,167,208,255,32,218,203,6,163,185,175,31,48,186,49,122,24,148,109,127,64,75,148,32,104,156,96,59,133,82,250,223,39,59,105,235,182,217,8,219,205,146,245,244,158,172,103,133,5,153,10,19,130,79,210,26,77,153,217,201,178,84,25,231,181,70,203,165,26,14,246,195,65,84,27,86,57,124,236,140,165,98,58,28,72,102,172,41,151,107,88,110,208,152,71,120,41,144,55,239,146,50,182,193,61,163,69,41,139,227,24,102,166,46,10,212,187,69,27,75,127,139,172,72,67,86,106,32,135,4,81,97,48,39,51,57,98,226,0,84,213,95,27,78,32,113,84,63,50,57,153,39,85,107,93,86,164,45,147,72,91,123,180,23,125,35,199,39,86,41,41,203,25,139,160,50,131,68,19,90,74,225,41,177,188,101,187,155,156,112,161,164,163,166,215,154,207,165,171,52,218,67,78,118,10,149,230,173,116,1,35,65,116,232,205,237,103,123,107,158,194,141,213,131,187,155,19,250,115,86,168,37,252,11,245,218,35,91,204,42,133,255,205,46,174,216,146,54,126,165,61,200,195,114,199,237,238,35,207,239,120,93,212,159,250,184,242,4,13,245,161,150,178,243,184,225,115,143,73,165,141,3,93,28,26,82,228,138,95,235,196,150,186,151,37,165,26,149,252,74,81,55,51,68,78,98,54,31,117,122,127,20,47,192,238,170,64,185,172,20,11,80,242,181,231,35,76,216,54,222,28,45,46,167,62,25,124,22,123,192,162,19,207,55,184,27,167,252,138,175,46,77,114,221,236,39,247,221,244,236,218,70,231,115,220,249,29,157,199,126,104,150,198,199,195,149,160,251,214,58,231,63,12,243,0,237,157,20,249,36,183,193,181,237,231,215,45,67,247,133,142,104,115,97,234,240,13,93,80,144,42,128,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("472ffed7-d0b2-439a-902c-a1ccb20b2456"));
		}

		#endregion

	}

	#endregion

}

