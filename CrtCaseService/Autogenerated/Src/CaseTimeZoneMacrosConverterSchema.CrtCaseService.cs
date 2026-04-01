namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CaseTimeZoneMacrosConverterSchema

	/// <exclude/>
	public class CaseTimeZoneMacrosConverterSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CaseTimeZoneMacrosConverterSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CaseTimeZoneMacrosConverterSchema(CaseTimeZoneMacrosConverterSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("def50b48-4f91-4a9d-ab1b-2977e4e375ae");
			Name = "CaseTimeZoneMacrosConverter";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("33bac096-c819-4c57-86af-fe71bbb0cb56");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,83,77,111,219,48,12,61,187,64,255,3,225,93,28,32,176,239,105,98,96,205,182,194,135,14,67,215,94,122,83,101,58,21,16,75,6,37,119,200,138,254,247,81,146,243,97,175,77,79,9,233,199,199,199,71,74,139,22,109,39,36,194,61,18,9,107,26,151,175,141,110,212,166,39,225,148,209,151,23,175,151,23,73,111,149,222,192,239,157,117,216,94,29,226,211,18,194,143,242,249,119,237,148,83,104,63,4,252,16,210,25,138,8,198,124,33,220,112,103,88,111,133,181,11,88,11,139,247,170,197,71,163,241,86,72,50,150,5,190,32,57,164,0,47,138,2,150,182,111,91,65,187,114,136,239,176,35,180,168,157,5,185,7,67,99,8,190,9,23,200,160,245,76,18,45,40,29,58,0,122,149,187,124,79,88,156,48,118,253,211,86,73,144,94,207,57,57,11,184,62,167,53,121,13,122,143,243,25,109,29,245,126,118,30,243,87,104,18,17,211,145,66,162,210,236,162,216,170,191,44,90,227,31,22,110,157,208,188,57,211,48,24,17,36,97,179,74,207,232,75,139,50,14,145,31,154,20,211,46,203,78,144,104,65,243,93,172,210,222,34,113,181,70,233,79,33,45,31,56,246,134,14,137,124,89,4,244,251,197,86,62,99,43,126,242,255,180,12,39,176,131,152,10,223,71,181,131,193,103,164,103,15,35,41,48,86,54,7,54,210,31,214,177,231,204,243,38,11,120,98,206,236,63,244,17,6,254,188,147,183,97,51,168,235,184,156,241,166,110,209,61,155,218,47,137,140,99,18,172,227,247,110,31,130,97,153,164,106,126,70,131,254,74,55,6,110,208,237,227,236,166,87,245,112,100,119,40,13,213,85,61,52,79,170,61,136,233,95,152,132,150,209,175,18,186,33,1,171,248,26,226,75,217,229,76,60,96,166,165,101,22,40,19,127,33,39,7,246,149,54,125,203,221,179,233,78,231,48,118,118,54,255,164,94,178,161,85,205,117,147,89,66,217,236,42,252,16,186,158,244,65,125,126,234,67,132,188,111,120,204,142,147,111,255,0,212,144,232,177,163,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("def50b48-4f91-4a9d-ab1b-2977e4e375ae"));
		}

		#endregion

	}

	#endregion

}

