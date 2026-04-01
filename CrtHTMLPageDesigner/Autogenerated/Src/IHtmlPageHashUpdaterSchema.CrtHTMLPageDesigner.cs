namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IHtmlPageHashUpdaterSchema

	/// <exclude/>
	public class IHtmlPageHashUpdaterSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IHtmlPageHashUpdaterSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IHtmlPageHashUpdaterSchema(IHtmlPageHashUpdaterSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("1ca09cc5-e0ce-4cdc-b23d-f6a1356f7576");
			Name = "IHtmlPageHashUpdater";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c8382e17-7ef4-48e4-b928-4bcb7ca99534");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,144,193,110,131,48,16,68,207,65,226,31,86,244,210,94,224,158,16,46,73,37,34,53,85,164,166,31,224,194,2,150,176,141,214,75,91,20,229,223,107,155,146,170,149,122,179,215,179,111,198,163,133,66,59,136,10,97,71,92,158,143,79,39,209,226,30,173,108,53,82,28,93,226,104,53,90,169,91,56,35,145,176,166,225,116,103,8,211,71,205,146,37,218,77,28,57,201,29,97,43,141,134,131,102,164,198,209,214,112,40,89,245,30,86,10,219,189,14,181,96,207,115,218,97,124,235,101,5,114,145,254,163,92,93,130,250,134,62,34,119,166,182,107,56,133,253,249,49,203,50,200,237,168,148,160,169,88,6,47,200,22,52,126,64,231,120,240,46,250,17,189,155,1,238,16,188,7,160,15,63,65,101,250,81,233,244,6,202,254,146,242,65,144,80,160,93,71,219,100,112,9,147,194,87,4,254,232,124,17,161,34,108,182,73,40,99,74,178,34,205,179,176,242,67,32,228,145,180,45,158,151,60,140,159,236,100,203,220,11,45,147,111,120,254,186,15,120,63,3,131,207,195,230,187,7,212,245,92,69,184,95,231,222,127,13,175,95,19,188,96,64,205,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("1ca09cc5-e0ce-4cdc-b23d-f6a1356f7576"));
		}

		#endregion

	}

	#endregion

}

