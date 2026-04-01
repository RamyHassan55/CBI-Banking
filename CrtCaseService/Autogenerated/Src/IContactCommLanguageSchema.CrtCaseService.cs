namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IContactCommLanguageSchema

	/// <exclude/>
	public class IContactCommLanguageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IContactCommLanguageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IContactCommLanguageSchema(IContactCommLanguageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("eb716256-d7b9-4bb9-a683-e956c5354404");
			Name = "IContactCommLanguage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("77fa8847-960e-4748-ad77-e37bb90e03a0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,144,65,106,195,48,16,69,215,54,248,14,67,86,233,198,58,64,93,111,82,8,129,236,218,11,76,237,145,43,176,36,51,26,81,66,201,221,43,57,118,26,82,218,238,196,232,255,63,239,143,67,75,97,194,142,224,149,152,49,120,45,245,206,59,109,134,200,40,198,187,170,252,172,202,34,6,227,6,120,57,5,33,251,88,149,105,50,197,183,209,116,96,156,16,235,236,63,36,155,96,39,59,111,237,17,221,16,113,160,164,203,238,66,41,5,77,136,214,34,159,218,117,176,39,129,113,17,130,233,201,137,209,134,24,152,70,20,234,225,195,200,59,116,151,208,250,154,162,238,99,154,9,25,45,184,84,228,105,179,200,15,253,166,93,112,110,146,235,70,205,218,111,43,147,68,118,161,61,254,196,72,226,245,55,203,247,209,244,153,120,59,63,174,107,30,46,199,248,181,96,128,158,52,198,241,166,169,102,111,33,204,151,132,64,34,233,176,127,181,91,33,158,239,115,254,69,93,28,219,204,88,156,171,242,252,5,67,3,190,240,236,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("eb716256-d7b9-4bb9-a683-e956c5354404"));
		}

		#endregion

	}

	#endregion

}

