namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: GoogleAnalyticsTouchQueueMessageSchema

	/// <exclude/>
	public class GoogleAnalyticsTouchQueueMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public GoogleAnalyticsTouchQueueMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public GoogleAnalyticsTouchQueueMessageSchema(GoogleAnalyticsTouchQueueMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("88098d78-6438-47fa-8ff3-15fff0e55788");
			Name = "GoogleAnalyticsTouchQueueMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("ee36a851-184d-48fc-93e8-63ff7d03af9c");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,148,193,110,219,48,12,134,207,46,208,119,32,178,75,2,12,246,189,77,2,4,25,26,244,16,160,91,179,243,160,202,140,35,204,150,60,74,234,16,20,123,247,209,146,147,216,110,147,244,144,131,104,241,227,207,95,100,180,168,208,214,66,34,44,201,173,140,41,74,92,104,81,238,157,146,54,93,163,181,162,64,123,123,243,118,123,147,120,171,116,1,207,123,235,176,186,63,158,223,167,157,190,109,144,72,88,179,117,233,210,232,173,42,60,9,167,140,254,248,2,225,185,120,250,32,164,51,164,176,65,243,157,47,132,5,99,96,89,10,107,239,96,80,126,99,188,220,125,247,232,177,149,31,114,178,44,131,169,245,85,37,104,63,111,207,33,31,156,1,194,154,208,162,118,240,34,44,194,106,193,65,134,192,159,134,2,92,188,169,86,69,90,10,7,90,214,193,213,254,165,84,18,100,32,94,19,4,119,240,129,200,228,45,8,61,118,247,160,176,204,185,189,39,82,175,194,197,46,146,58,30,96,229,85,14,191,28,9,249,155,253,138,111,242,152,223,183,4,212,121,132,244,137,252,8,214,145,111,188,108,184,65,114,188,49,116,39,218,115,186,14,91,254,77,45,34,72,194,237,108,116,173,195,81,54,79,143,220,174,79,7,163,174,1,198,19,104,70,46,73,222,245,8,179,97,114,208,105,211,65,244,169,20,142,85,71,87,146,228,223,101,107,214,232,118,38,186,109,28,74,135,249,5,99,126,160,243,164,45,40,174,43,52,175,142,217,130,219,225,5,131,190,9,39,158,247,90,174,133,230,230,232,188,61,33,82,11,18,21,104,222,204,217,200,91,36,110,80,99,24,194,209,252,241,124,205,159,253,171,92,99,154,5,212,137,76,81,249,37,202,117,229,211,236,128,137,19,217,26,54,124,150,65,38,172,208,117,142,227,190,90,232,247,57,129,89,128,39,97,69,227,250,239,83,38,76,47,23,153,143,67,86,162,241,111,119,124,23,84,248,138,215,123,60,180,243,235,176,110,72,159,92,217,163,211,176,12,87,72,233,29,146,114,185,145,89,119,218,205,43,255,159,169,188,93,91,110,99,211,14,245,184,105,244,179,123,28,71,184,31,228,216,127,13,119,43,47,194,5,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("88098d78-6438-47fa-8ff3-15fff0e55788"));
		}

		#endregion

	}

	#endregion

}

