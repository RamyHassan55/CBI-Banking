namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ITemplateSaveSchema

	/// <exclude/>
	public class ITemplateSaveSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ITemplateSaveSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ITemplateSaveSchema(ITemplateSaveSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("5bafa3c2-6d61-4e27-9d51-74ca96ece520");
			Name = "ITemplateSave";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d0843e01-a902-455d-9128-3897f691b1df");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,145,203,110,194,48,16,69,215,68,202,63,140,232,166,221,144,61,80,164,10,22,237,162,82,213,242,3,131,51,73,44,249,145,122,108,80,133,248,247,218,78,16,233,99,57,175,227,123,175,13,106,226,30,5,193,214,249,173,53,158,140,223,17,203,214,144,43,139,115,89,204,2,75,211,254,51,93,164,218,161,240,188,42,139,184,87,85,21,172,57,104,141,238,107,51,214,59,106,164,33,6,223,17,104,242,157,173,25,28,125,6,233,168,6,111,129,241,72,128,6,72,163,84,224,73,247,10,61,241,226,138,171,38,188,62,28,148,20,32,163,8,215,36,193,47,251,113,255,35,81,150,183,250,25,77,173,146,252,217,57,43,155,221,57,106,165,53,240,58,72,88,194,91,102,13,195,223,186,115,35,33,7,217,63,165,193,73,250,46,247,185,39,33,27,25,125,100,19,201,21,177,135,154,124,220,207,14,254,90,24,58,61,58,212,96,98,240,143,243,116,251,62,156,206,55,251,136,29,239,193,54,195,43,19,246,98,93,229,211,27,201,145,15,206,240,230,9,14,214,42,138,65,30,81,5,138,25,213,82,160,79,255,118,234,162,103,114,55,152,237,201,197,81,76,227,132,12,28,132,32,230,38,168,72,191,226,18,63,1,115,10,215,80,239,167,197,40,25,38,242,31,86,99,214,100,234,33,238,92,95,202,226,242,13,85,248,170,168,100,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("5bafa3c2-6d61-4e27-9d51-74ca96ece520"));
		}

		#endregion

	}

	#endregion

}

