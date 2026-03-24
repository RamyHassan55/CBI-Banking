namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ISupportDefMailBoxSchema

	/// <exclude/>
	public class ISupportDefMailBoxSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ISupportDefMailBoxSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ISupportDefMailBoxSchema(ISupportDefMailBoxSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("c365918f-f01b-41e0-998d-6395d5736989");
			Name = "ISupportDefMailBox";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("77fa8847-960e-4748-ad77-e37bb90e03a0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,93,141,65,10,131,48,16,69,215,10,222,97,150,237,198,28,160,226,162,45,148,46,186,178,23,136,50,145,128,73,100,102,2,74,241,238,77,40,118,225,106,224,255,247,223,120,237,144,103,61,32,188,145,72,115,48,82,223,130,55,118,140,164,197,6,95,149,159,170,44,230,216,79,118,0,235,5,201,100,250,217,197,121,14,36,119,52,47,109,167,107,88,18,149,201,66,41,5,13,71,231,52,173,237,30,60,80,128,127,11,112,137,135,62,44,96,40,56,224,149,5,211,65,17,235,199,250,111,80,71,69,67,40,145,60,183,221,193,83,55,106,175,50,203,66,73,148,63,158,206,151,20,108,85,185,125,1,49,250,8,107,230,0,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("c365918f-f01b-41e0-998d-6395d5736989"));
		}

		#endregion

	}

	#endregion

}

