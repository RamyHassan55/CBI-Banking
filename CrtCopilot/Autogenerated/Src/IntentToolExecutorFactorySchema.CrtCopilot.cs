namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IntentToolExecutorFactorySchema

	/// <exclude/>
	public class IntentToolExecutorFactorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IntentToolExecutorFactorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IntentToolExecutorFactorySchema(IntentToolExecutorFactorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("f261816a-c1b7-43f5-898e-530e594c56c8");
			Name = "IntentToolExecutorFactory";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("41e46e3a-7898-448f-b0fb-31200d6989e0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,147,77,106,195,48,16,133,247,129,220,97,150,54,136,28,160,161,155,58,45,100,145,149,211,85,41,70,149,39,137,64,145,140,126,218,154,146,187,87,177,28,98,185,86,26,109,140,164,121,111,52,223,195,146,30,209,52,148,33,20,26,169,229,106,81,168,134,11,101,231,179,159,249,12,252,114,134,203,61,108,81,107,106,212,206,250,123,141,203,244,213,226,133,50,171,52,71,227,139,66,217,219,10,119,212,9,251,196,101,237,5,153,109,27,84,187,108,189,150,22,165,221,42,37,158,191,145,57,175,10,218,54,207,223,131,178,113,31,130,51,96,130,26,3,201,114,120,128,180,87,48,234,103,233,60,53,255,164,22,193,143,91,43,41,90,120,53,168,11,37,37,50,63,190,132,202,69,251,229,13,229,186,103,181,49,251,226,64,189,66,148,40,107,212,80,177,233,139,155,102,43,197,220,177,31,2,170,122,176,187,231,13,37,26,227,159,187,161,146,238,207,47,48,209,190,203,98,224,18,184,38,161,101,35,38,49,18,146,106,202,166,78,201,181,237,101,37,185,37,176,145,152,205,16,77,62,140,246,188,70,241,193,35,36,243,236,202,19,45,189,238,255,12,59,131,225,107,188,42,145,91,87,26,103,114,109,81,142,179,186,40,78,119,133,22,126,93,204,122,170,161,160,100,7,60,210,75,135,225,217,31,100,26,173,211,18,36,126,77,152,103,19,14,100,76,153,36,57,146,24,16,25,67,200,227,105,251,207,233,23,89,39,52,34,151,4,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("f261816a-c1b7-43f5-898e-530e594c56c8"));
		}

		#endregion

	}

	#endregion

}

