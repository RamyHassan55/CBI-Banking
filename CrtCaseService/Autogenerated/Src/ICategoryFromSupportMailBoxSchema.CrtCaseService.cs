namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ICategoryFromSupportMailBoxSchema

	/// <exclude/>
	public class ICategoryFromSupportMailBoxSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ICategoryFromSupportMailBoxSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ICategoryFromSupportMailBoxSchema(ICategoryFromSupportMailBoxSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("f9f6082f-e4c2-4b11-b02e-75f27d626477");
			Name = "ICategoryFromSupportMailBox";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,145,193,106,195,48,12,134,207,11,228,29,116,220,96,196,15,176,144,195,58,22,118,24,12,58,216,217,77,148,84,52,182,131,100,183,13,163,239,62,187,77,187,209,13,118,50,178,126,127,250,127,217,106,131,50,234,6,225,29,153,181,184,206,23,11,103,59,234,3,107,79,206,230,217,103,158,221,4,33,219,195,114,18,143,230,225,170,142,250,97,192,38,137,165,168,209,34,83,19,53,81,165,148,130,82,130,49,154,167,106,174,223,216,109,169,69,144,48,142,142,61,24,77,3,172,220,30,5,118,228,215,208,104,143,189,227,169,56,191,87,63,0,99,88,13,212,0,89,143,220,37,211,47,139,89,254,204,206,44,79,200,215,72,124,116,251,40,79,206,127,153,56,94,212,232,255,117,0,93,100,130,156,50,94,72,234,26,85,50,250,192,86,170,39,58,238,32,246,96,183,70,70,216,224,4,36,127,205,209,109,203,40,241,180,45,108,245,16,48,233,46,201,75,117,70,166,25,223,216,82,60,199,181,223,67,29,168,173,82,134,57,42,202,71,52,62,175,130,80,110,239,210,31,29,242,236,240,5,180,64,71,7,223,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("f9f6082f-e4c2-4b11-b02e-75f27d626477"));
		}

		#endregion

	}

	#endregion

}

