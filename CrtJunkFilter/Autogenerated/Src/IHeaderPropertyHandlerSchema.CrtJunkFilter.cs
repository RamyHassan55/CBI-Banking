namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IHeaderPropertyHandlerSchema

	/// <exclude/>
	public class IHeaderPropertyHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IHeaderPropertyHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IHeaderPropertyHandlerSchema(IHeaderPropertyHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("d2905c9d-29e0-4bdd-9d08-907bf3906707");
			Name = "IHeaderPropertyHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d875cf44-2089-4c90-a894-025355d0d9a7");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,144,193,78,195,48,12,134,207,84,234,59,88,59,193,165,121,0,74,47,147,80,119,152,196,97,47,224,166,238,18,72,147,200,73,55,85,136,119,39,233,186,129,6,82,46,113,252,127,254,98,139,35,5,143,146,224,64,204,24,220,16,171,173,179,131,62,78,140,81,59,91,22,159,101,241,32,132,128,58,76,227,136,60,55,235,125,103,35,241,144,163,110,0,26,81,27,80,132,61,49,120,118,158,56,206,160,208,246,134,184,186,18,196,47,132,159,58,163,37,232,27,101,215,46,233,183,53,220,94,178,169,51,11,252,49,88,10,123,138,202,245,16,21,198,4,234,181,196,72,1,206,42,149,147,198,81,159,200,194,9,205,68,160,3,160,148,228,35,118,134,160,155,83,38,149,86,63,112,12,214,197,234,54,71,220,15,170,61,50,142,96,211,182,94,54,11,113,211,180,119,159,93,202,85,45,150,214,159,36,83,156,216,134,230,192,89,99,0,252,215,40,137,192,43,154,144,150,153,221,207,58,100,212,53,155,97,157,115,6,182,138,228,199,163,235,222,73,198,11,231,233,57,61,126,149,69,58,223,210,135,199,67,204,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("d2905c9d-29e0-4bdd-9d08-907bf3906707"));
		}

		#endregion

	}

	#endregion

}

