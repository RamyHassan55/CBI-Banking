namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: BfRowSchema

	/// <exclude/>
	public class BfRowSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public BfRowSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public BfRowSchema(BfRowSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("4428b5c7-975d-4439-9c58-75455f268fe6");
			Name = "BfRow";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("9ef2e2c4-cc1b-4c63-a8fa-19c5996a0dad");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,181,82,203,106,195,48,16,60,199,224,127,88,200,61,186,39,161,135,244,144,4,218,18,76,127,64,177,214,206,18,89,50,146,140,91,66,255,189,146,82,187,15,210,214,113,233,201,172,181,59,51,59,179,140,49,88,218,166,170,184,121,190,73,19,230,203,12,107,131,22,149,179,192,193,232,22,72,129,59,32,96,197,73,130,64,75,165,66,51,59,55,47,217,251,112,221,236,37,229,144,75,110,45,172,138,76,183,105,114,74,147,52,153,76,13,150,164,21,236,140,174,209,56,66,59,135,93,236,142,207,236,179,136,88,175,209,243,107,3,54,124,3,61,41,129,79,160,139,88,120,89,179,110,240,131,130,201,155,4,82,14,182,177,255,4,37,186,69,64,89,192,203,53,100,126,69,216,60,222,223,65,174,149,243,102,12,100,182,206,144,42,61,185,159,223,184,74,142,21,16,125,23,158,151,10,138,102,255,184,171,183,122,43,46,80,77,81,137,179,245,161,140,255,216,47,129,251,117,5,185,16,214,94,234,252,56,46,252,219,14,100,21,49,230,255,112,13,189,55,93,48,189,240,1,241,244,250,46,186,54,72,64,105,116,83,255,81,198,58,96,140,151,240,253,145,126,9,241,26,71,30,120,133,163,143,182,145,8,62,127,157,19,119,40,160,37,119,24,159,77,22,208,6,220,244,43,22,203,124,50,195,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("4428b5c7-975d-4439-9c58-75455f268fe6"));
		}

		#endregion

	}

	#endregion

}

