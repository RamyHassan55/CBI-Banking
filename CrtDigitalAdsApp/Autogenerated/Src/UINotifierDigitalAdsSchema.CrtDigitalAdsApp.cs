namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: UINotifierDigitalAdsSchema

	/// <exclude/>
	public class UINotifierDigitalAdsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public UINotifierDigitalAdsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public UINotifierDigitalAdsSchema(UINotifierDigitalAdsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("3b0c8870-a728-42e8-89a5-34f66697348d");
			Name = "UINotifierDigitalAds";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("abf715ae-fc66-421f-87dc-256931a52a49");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,145,75,107,195,48,16,132,207,13,228,63,44,238,37,129,98,223,243,130,196,161,224,75,40,109,115,42,61,168,246,218,21,200,146,216,93,23,66,232,127,175,252,200,131,16,122,211,136,79,163,217,89,171,106,100,175,114,132,148,100,171,43,45,202,172,11,94,123,31,239,69,27,45,26,121,60,58,142,71,15,13,107,91,181,84,106,92,83,100,86,176,34,37,218,217,11,56,63,99,239,72,164,216,149,18,167,142,48,126,86,185,56,234,137,192,60,18,86,225,33,164,70,49,207,96,159,237,156,232,82,35,93,2,116,92,146,36,176,224,166,174,21,29,86,131,126,69,79,200,104,133,65,190,17,116,237,13,214,65,118,81,192,149,193,13,236,96,23,159,60,146,27,147,5,35,42,195,14,114,194,114,25,253,59,84,156,93,242,69,144,180,22,31,91,44,85,99,100,163,109,17,166,157,200,193,163,43,39,87,224,244,9,118,161,89,88,66,116,219,107,52,253,12,22,190,249,50,58,135,188,109,224,110,1,215,181,108,20,99,120,115,236,74,57,183,247,66,206,35,181,17,103,237,89,48,23,44,122,196,159,36,184,159,176,9,93,32,176,80,187,152,55,180,5,82,159,109,117,39,220,124,248,35,96,253,55,157,254,237,215,118,190,12,42,92,253,1,112,178,242,231,60,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("3b0c8870-a728-42e8-89a5-34f66697348d"));
		}

		#endregion

	}

	#endregion

}

