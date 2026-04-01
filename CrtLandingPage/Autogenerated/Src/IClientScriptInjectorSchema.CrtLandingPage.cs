namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IClientScriptInjectorSchema

	/// <exclude/>
	public class IClientScriptInjectorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IClientScriptInjectorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IClientScriptInjectorSchema(IClientScriptInjectorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("95ca7583-de2d-4679-946f-7c5df720e2b8");
			Name = "IClientScriptInjector";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fcd19c09-7532-4da4-a534-367aa5418760");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,146,201,106,195,48,16,134,207,49,248,29,6,159,90,40,241,3,212,205,37,135,198,208,67,32,125,1,69,26,199,74,181,152,145,148,16,74,223,189,35,103,105,154,46,160,131,52,203,255,127,51,200,9,139,97,16,18,97,78,241,69,56,165,221,102,41,54,88,22,239,101,49,73,129,159,176,58,132,136,246,177,44,56,82,215,53,52,33,89,43,232,48,59,189,151,228,119,90,97,0,1,210,43,132,125,175,101,15,194,24,191,15,16,123,4,105,52,186,8,65,146,30,34,135,60,172,17,132,82,168,192,19,16,90,191,227,107,71,222,130,57,34,192,192,12,211,179,95,125,101,56,164,181,209,18,180,139,72,93,230,110,231,163,250,106,20,111,221,22,101,244,196,133,153,255,7,238,24,56,22,133,91,44,237,160,143,214,0,215,190,165,97,122,105,175,111,251,155,65,144,176,224,120,117,79,213,9,184,85,213,236,149,71,229,61,184,168,59,141,4,190,251,62,77,83,143,125,191,203,228,138,5,187,87,179,197,23,67,150,200,251,251,95,134,48,38,114,97,180,39,12,201,196,235,57,254,208,56,55,101,149,16,41,103,142,107,185,123,78,90,193,101,170,7,56,101,207,128,247,252,15,38,31,101,193,231,19,9,65,113,229,60,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("95ca7583-de2d-4679-946f-7c5df720e2b8"));
		}

		#endregion

	}

	#endregion

}

