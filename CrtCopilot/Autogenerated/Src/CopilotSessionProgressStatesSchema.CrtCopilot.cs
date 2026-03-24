namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CopilotSessionProgressStatesSchema

	/// <exclude/>
	public class CopilotSessionProgressStatesSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CopilotSessionProgressStatesSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CopilotSessionProgressStatesSchema(CopilotSessionProgressStatesSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("c97cec0c-44eb-481a-82a1-0e11ea2c192d");
			Name = "CopilotSessionProgressStates";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("7a3a8162-4be1-46b5-bd50-b3efc2df6d2e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,85,143,205,106,195,48,16,132,207,49,248,29,244,0,37,126,129,82,48,161,189,21,74,212,208,179,42,79,93,81,89,82,181,43,232,15,121,247,174,85,25,146,227,206,204,55,187,27,204,2,74,198,66,29,50,12,187,184,63,196,228,124,228,190,251,237,187,221,48,12,234,150,202,178,152,252,125,215,230,35,82,6,33,48,41,126,135,34,54,12,21,223,234,208,96,69,32,114,49,168,148,227,44,97,218,111,93,195,69,89,42,175,222,89,133,80,150,13,212,255,220,83,195,244,218,77,18,93,111,217,189,24,199,46,204,15,49,159,8,249,81,124,51,227,230,218,25,133,151,139,2,95,218,247,95,176,101,13,140,86,62,12,85,211,31,206,123,13,15,203,152,170,50,206,242,211,149,242,236,216,227,148,38,179,9,21,106,205,186,62,226,126,154,117,196,103,1,9,31,38,89,36,202,185,239,206,234,15,63,232,88,123,95,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("c97cec0c-44eb-481a-82a1-0e11ea2c192d"));
		}

		#endregion

	}

	#endregion

}

