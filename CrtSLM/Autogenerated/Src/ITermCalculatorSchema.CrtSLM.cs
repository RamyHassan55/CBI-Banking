namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ITermCalculatorSchema

	/// <exclude/>
	public class ITermCalculatorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ITermCalculatorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ITermCalculatorSchema(ITermCalculatorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("bf14023c-5dd2-42fc-a1c9-2ee0dcd85fa4");
			Name = "ITermCalculator";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,181,145,205,78,3,33,20,70,215,146,240,14,55,93,233,102,120,128,142,179,169,155,110,59,125,1,196,59,13,201,240,147,123,193,164,49,125,119,161,149,169,213,68,221,184,3,238,57,124,124,33,179,245,7,24,143,156,208,173,165,144,194,107,135,28,181,65,216,35,145,230,48,165,110,19,252,100,15,153,116,178,193,119,35,210,171,53,88,198,78,138,55,41,238,148,82,208,115,118,78,211,113,248,216,239,48,18,50,250,196,192,23,30,82,17,192,232,217,228,89,167,64,93,51,213,39,53,230,231,217,26,176,190,192,83,125,196,182,198,108,22,169,32,53,241,91,228,249,160,97,8,37,58,6,207,8,47,101,215,45,130,250,106,244,81,147,118,80,59,63,174,56,105,74,79,69,88,13,99,93,94,228,94,157,153,171,66,152,50,121,30,118,55,25,189,106,231,21,172,183,236,173,195,235,147,26,93,39,247,203,120,137,124,88,255,161,21,135,57,215,31,248,207,86,227,77,198,111,173,26,253,83,171,147,20,167,119,159,198,109,179,102,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("bf14023c-5dd2-42fc-a1c9-2ee0dcd85fa4"));
		}

		#endregion

	}

	#endregion

}

