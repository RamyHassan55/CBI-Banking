namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotAgentQueryExecutorSchema

	/// <exclude/>
	public class CopilotAgentQueryExecutorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotAgentQueryExecutorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotAgentQueryExecutorSchema(CopilotAgentQueryExecutorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("568ca349-5c60-4333-8da4-76e697ff5f8b");
			Name = "CopilotAgentQueryExecutor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c23dfaf0-0051-4966-86f8-cbe8197258d0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,82,77,107,194,64,16,61,43,248,31,6,189,68,40,249,1,106,132,106,173,4,106,105,209,210,67,233,97,221,140,113,33,217,77,247,163,52,72,255,123,103,147,136,77,219,92,10,57,100,222,204,188,247,102,102,65,178,28,77,193,56,194,82,35,179,66,133,75,85,136,76,217,65,255,52,232,247,156,17,50,133,109,105,44,230,148,201,50,228,84,35,77,184,70,137,90,240,233,207,154,59,33,223,46,224,14,181,102,70,29,44,245,106,236,194,195,149,180,194,10,52,157,5,183,140,91,165,155,10,250,70,26,83,178,1,203,140,25,51,129,198,242,117,138,210,62,58,212,229,234,3,185,163,142,65,159,138,95,110,240,192,92,102,23,66,38,196,29,216,178,64,117,8,226,74,181,108,213,143,175,224,158,22,2,17,12,59,57,135,227,87,34,45,220,62,19,28,184,55,208,173,15,19,88,48,131,77,62,150,246,183,193,158,95,243,101,32,218,173,213,206,79,75,115,61,84,34,213,16,103,193,78,169,224,201,160,166,118,89,95,8,92,43,28,123,138,222,4,246,228,38,248,145,130,19,124,214,26,35,148,73,109,164,137,27,87,27,180,71,149,120,67,90,89,234,194,164,241,116,14,65,189,211,193,68,130,64,91,117,57,106,182,207,112,22,111,249,17,115,182,97,146,165,168,99,122,30,179,214,38,234,244,124,14,107,108,144,111,165,38,32,99,149,235,127,81,10,79,65,119,244,3,135,29,252,211,138,94,163,117,90,214,13,225,243,17,53,6,254,31,162,154,36,140,233,34,76,114,12,119,244,110,32,138,160,37,232,193,176,186,70,77,247,247,38,107,180,13,18,246,5,34,160,213,58,126,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("568ca349-5c60-4333-8da4-76e697ff5f8b"));
		}

		#endregion

	}

	#endregion

}

