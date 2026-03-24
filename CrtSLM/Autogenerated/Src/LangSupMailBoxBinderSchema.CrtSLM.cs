namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: LangSupMailBoxBinderSchema

	/// <exclude/>
	public class LangSupMailBoxBinderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public LangSupMailBoxBinderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public LangSupMailBoxBinderSchema(LangSupMailBoxBinderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("eab7a1a7-4947-4699-89fb-5232e0e5fe99");
			Name = "LangSupMailBoxBinder";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,144,65,107,195,48,12,133,207,41,244,63,136,236,210,193,72,238,109,23,88,202,6,131,150,21,58,216,217,77,148,204,144,216,65,118,66,75,233,127,159,98,39,91,11,217,201,72,239,249,147,244,148,168,209,52,34,67,248,68,34,97,116,97,163,141,86,133,44,91,18,86,106,53,159,93,230,179,160,53,82,149,119,22,194,232,77,100,86,147,68,179,154,112,124,225,145,93,117,173,21,171,172,63,16,150,140,131,77,37,140,89,194,86,168,242,208,54,59,33,171,84,159,82,169,114,36,231,139,227,24,214,166,173,107,65,231,100,168,189,12,133,38,150,16,33,35,44,158,195,247,191,255,61,109,79,186,147,108,11,227,36,26,57,241,13,168,105,143,149,204,32,235,231,79,142,135,37,188,52,205,107,135,202,110,165,177,168,144,82,97,144,191,94,220,102,191,39,236,208,126,235,156,143,216,59,164,23,7,188,238,56,1,94,3,58,45,115,248,80,76,60,88,65,118,49,162,57,92,139,39,11,153,127,31,161,143,55,8,142,60,41,186,177,143,242,202,169,46,52,31,247,217,53,130,168,223,121,253,79,4,79,48,221,79,22,225,189,16,122,254,117,184,15,85,238,79,116,181,239,222,55,175,63,195,79,18,133,48,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("eab7a1a7-4947-4699-89fb-5232e0e5fe99"));
		}

		#endregion

	}

	#endregion

}

