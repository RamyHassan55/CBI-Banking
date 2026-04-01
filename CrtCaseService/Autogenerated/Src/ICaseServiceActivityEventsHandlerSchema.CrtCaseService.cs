namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ICaseServiceActivityEventsHandlerSchema

	/// <exclude/>
	public class ICaseServiceActivityEventsHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ICaseServiceActivityEventsHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ICaseServiceActivityEventsHandlerSchema(ICaseServiceActivityEventsHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("0c523a32-702b-4a95-b0d6-e068df7e5eb7");
			Name = "ICaseServiceActivityEventsHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ff867e3a-49a7-422a-a992-de581439f8d9");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,146,207,106,194,64,16,198,207,10,190,195,128,151,22,130,15,160,173,32,162,109,14,133,162,181,247,117,119,146,78,89,103,211,253,19,8,210,119,239,38,49,98,160,210,66,161,151,132,153,157,223,124,51,223,46,139,3,186,66,72,132,23,180,86,56,147,249,201,210,112,70,121,176,194,147,225,209,240,56,26,14,130,35,206,97,91,57,143,135,217,57,190,68,44,94,203,79,86,236,201,19,186,88,16,75,198,22,243,216,23,82,246,104,179,168,60,133,116,41,28,110,209,150,36,113,33,61,149,228,171,85,137,236,221,163,96,165,209,54,96,17,246,154,36,80,199,253,6,27,28,27,244,44,250,132,254,205,40,55,133,231,166,89,123,88,26,82,208,34,93,155,148,29,90,143,234,166,153,189,2,209,181,111,194,4,118,241,56,218,196,40,107,143,32,244,194,164,110,58,88,180,71,108,60,101,36,27,47,79,169,123,224,160,245,237,236,170,250,174,80,177,158,243,63,168,175,3,203,187,189,49,122,14,57,250,212,45,133,199,220,216,106,77,58,218,215,218,112,26,36,129,166,248,33,144,74,160,69,200,109,240,35,144,69,181,38,212,202,189,10,77,170,15,93,236,120,66,235,239,28,28,250,78,171,183,238,73,197,121,27,23,75,160,255,159,195,187,33,222,160,164,130,234,251,251,70,234,98,198,142,137,139,93,65,126,242,246,95,46,118,140,172,218,103,215,196,159,237,235,239,37,99,238,11,220,245,17,182,129,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("0c523a32-702b-4a95-b0d6-e068df7e5eb7"));
		}

		#endregion

	}

	#endregion

}

