namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IIntentToolExecutorFactorySchema

	/// <exclude/>
	public class IIntentToolExecutorFactorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IIntentToolExecutorFactorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IIntentToolExecutorFactorySchema(IIntentToolExecutorFactorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("07f57910-666c-4dbc-b33f-0000dc1dd45d");
			Name = "IIntentToolExecutorFactory";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("41e46e3a-7898-448f-b0fb-31200d6989e0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,144,77,78,195,48,16,133,247,145,114,135,81,87,176,137,15,64,200,166,128,212,53,92,96,48,99,106,41,30,91,254,17,84,85,239,142,107,167,180,180,198,242,194,26,205,251,222,123,102,52,20,28,74,130,181,39,140,218,14,107,235,244,108,99,223,237,251,14,242,17,66,192,24,146,49,232,119,211,121,244,68,74,51,5,64,80,40,163,245,59,208,28,201,171,35,74,89,15,178,224,248,51,143,67,68,150,121,213,42,216,228,29,142,111,214,206,207,223,36,83,214,13,23,46,226,175,141,75,239,179,150,23,220,205,173,252,165,154,87,193,146,248,159,212,167,113,41,90,146,51,125,253,198,107,167,131,20,142,29,226,150,32,56,146,90,105,250,128,229,135,234,246,171,220,146,193,225,202,89,180,173,71,135,30,13,112,254,244,199,149,188,197,172,166,133,93,74,115,132,80,233,163,40,194,107,154,167,152,60,135,105,20,167,215,121,161,209,165,22,191,107,196,135,70,150,251,135,10,59,244,93,190,63,168,111,191,47,40,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("07f57910-666c-4dbc-b33f-0000dc1dd45d"));
		}

		#endregion

	}

	#endregion

}

