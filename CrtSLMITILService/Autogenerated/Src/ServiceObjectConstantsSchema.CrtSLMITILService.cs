namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ServiceObjectConstantsSchema

	/// <exclude/>
	public class ServiceObjectConstantsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ServiceObjectConstantsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ServiceObjectConstantsSchema(ServiceObjectConstantsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("87b3bcc1-054b-4d22-b62d-b3d765973e0c");
			Name = "ServiceObjectConstants";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("e9cdff4a-a92d-4d26-906f-df90167f5485");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,146,91,75,195,48,20,128,159,45,244,63,132,237,69,31,98,239,151,205,11,200,4,241,73,97,251,3,39,39,73,169,172,105,105,82,101,136,255,221,180,179,108,108,19,188,28,18,146,115,255,224,156,78,151,170,32,203,141,54,162,186,114,29,215,81,80,9,221,0,10,178,18,109,11,186,150,230,114,81,43,89,22,93,11,166,172,149,235,188,187,206,217,180,21,133,85,200,98,13,90,207,201,82,180,175,37,138,39,246,34,208,216,112,109,64,25,109,227,236,241,60,143,92,235,174,170,160,221,220,126,233,211,81,200,209,167,151,49,203,219,75,107,58,182,46,145,216,202,198,62,216,247,253,190,109,143,184,99,28,29,115,242,60,20,233,157,253,61,36,27,209,200,41,166,57,217,193,29,211,29,224,61,116,37,239,251,26,64,179,218,52,226,145,147,27,162,196,219,224,56,159,176,16,128,39,92,82,144,9,210,56,76,24,5,12,145,134,50,136,120,158,7,62,100,193,228,98,152,199,95,41,127,3,122,135,88,119,234,20,104,148,248,60,149,60,163,152,199,22,52,66,159,50,129,41,141,253,32,72,145,177,25,15,217,255,65,127,78,122,47,26,104,77,37,78,194,178,148,137,32,10,115,42,51,201,105,156,99,66,103,126,22,209,56,139,33,74,242,44,157,249,209,0,187,157,254,84,40,190,221,16,171,125,108,119,117,223,102,77,159,70,88,34,103,30,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("87b3bcc1-054b-4d22-b62d-b3d765973e0c"));
		}

		#endregion

	}

	#endregion

}

