namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ITemplateLoadSchema

	/// <exclude/>
	public class ITemplateLoadSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ITemplateLoadSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ITemplateLoadSchema(ITemplateLoadSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("3a05bb03-71a3-4bde-a75b-6f47b93c1763");
			Name = "ITemplateLoad";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d0843e01-a902-455d-9128-3897f691b1df");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,81,203,106,195,48,16,60,219,224,127,88,210,75,11,197,190,39,105,46,201,161,133,6,74,201,15,168,214,218,22,232,225,106,101,74,9,249,247,234,97,83,59,13,61,206,106,102,52,179,171,153,66,234,89,141,176,183,110,111,180,67,237,14,72,162,213,104,139,252,92,228,217,64,66,183,55,94,203,128,45,171,29,109,254,99,29,13,71,25,40,158,84,85,21,108,105,80,138,217,239,221,136,15,216,8,141,4,174,67,80,232,58,195,9,44,126,14,194,34,7,103,64,26,198,129,105,64,197,132,4,135,170,151,204,33,149,147,93,53,243,235,135,15,41,106,16,62,129,109,66,167,151,211,200,127,13,46,235,95,252,204,52,151,161,97,118,142,201,178,59,139,173,48,26,142,41,194,26,222,162,87,122,188,206,29,7,193,50,197,94,70,131,47,225,186,56,167,30,107,209,8,223,35,150,8,173,144,28,112,116,158,31,27,252,173,144,38,61,179,76,129,246,183,121,90,5,237,123,146,174,118,39,111,59,234,193,52,241,151,185,119,185,173,162,244,182,83,204,57,109,32,121,93,69,247,251,238,173,169,145,104,225,52,105,226,45,99,241,105,114,63,7,99,74,152,37,126,132,165,118,250,233,97,51,174,29,53,79,155,143,248,82,228,151,31,86,179,92,164,146,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("3a05bb03-71a3-4bde-a75b-6f47b93c1763"));
		}

		#endregion

	}

	#endregion

}

