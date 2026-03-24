namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IClientScriptProviderSchema

	/// <exclude/>
	public class IClientScriptProviderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IClientScriptProviderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IClientScriptProviderSchema(IClientScriptProviderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("4d73cd50-bcdf-4195-8f5d-cf5a7d384cb3");
			Name = "IClientScriptProvider";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("fcd19c09-7532-4da4-a534-367aa5418760");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,81,205,110,194,48,12,62,175,82,223,193,226,180,93,218,7,24,227,194,38,132,180,3,26,123,129,144,184,96,41,77,42,199,97,170,166,189,251,146,82,6,133,73,57,196,63,223,143,237,178,112,170,197,208,41,141,176,100,121,87,206,144,219,111,212,30,203,226,187,44,30,98,72,33,108,251,32,216,62,223,196,213,210,91,139,90,200,187,80,173,208,33,147,190,244,76,217,170,87,241,169,150,170,117,93,195,60,196,182,85,220,47,198,120,195,254,72,6,3,40,208,222,32,236,122,248,58,144,62,128,28,16,180,37,116,2,65,51,117,146,90,56,213,35,89,1,207,192,40,76,120,68,3,77,138,236,73,15,186,44,120,150,170,175,180,186,184,179,164,129,156,32,55,121,228,245,114,32,223,14,220,163,11,78,141,121,244,59,167,67,226,3,37,178,11,183,174,254,147,191,215,63,101,58,197,170,133,188,247,151,217,136,89,155,217,226,51,205,154,228,157,80,67,200,224,155,41,225,188,30,112,23,26,62,57,25,112,150,130,100,196,196,21,24,241,33,225,206,141,25,185,126,115,177,69,86,59,139,243,241,60,215,43,88,192,10,199,111,120,92,69,50,240,103,240,41,159,246,167,44,210,251,5,214,38,163,146,53,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("4d73cd50-bcdf-4195-8f5d-cf5a7d384cb3"));
		}

		#endregion

	}

	#endregion

}

