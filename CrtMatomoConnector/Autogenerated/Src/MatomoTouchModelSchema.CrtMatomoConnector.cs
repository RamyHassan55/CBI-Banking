namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoTouchModelSchema

	/// <exclude/>
	public class MatomoTouchModelSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoTouchModelSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoTouchModelSchema(MatomoTouchModelSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("dafbf523-a1de-416f-9214-73f6caf17712");
			Name = "MatomoTouchModel";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ad22e7a2-e416-4653-a99c-2d68ec80b318");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,143,177,10,194,64,12,134,231,22,250,14,1,23,93,250,0,138,147,14,42,40,130,226,34,14,177,77,207,131,51,41,119,57,68,164,239,110,79,29,4,93,2,73,190,228,255,127,198,43,133,22,43,130,61,121,143,65,26,45,103,194,141,53,209,163,90,225,34,127,20,121,22,131,101,3,27,186,169,240,139,89,5,225,73,145,247,171,129,39,211,115,48,115,24,194,24,214,168,114,149,189,196,234,178,150,154,220,139,177,172,228,25,29,84,9,250,97,96,12,223,7,89,82,204,142,73,98,235,165,37,175,247,225,156,26,140,78,15,232,34,45,144,107,151,252,76,225,223,184,92,26,22,79,163,83,250,210,198,179,179,21,244,6,62,170,59,171,180,172,225,1,134,116,2,33,149,174,7,187,119,22,226,250,29,39,181,221,19,120,14,168,213,29,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("dafbf523-a1de-416f-9214-73f6caf17712"));
		}

		#endregion

	}

	#endregion

}

