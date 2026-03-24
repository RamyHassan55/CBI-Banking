namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ISpecificationTermParametersSchema

	/// <exclude/>
	public class ISpecificationTermParametersSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ISpecificationTermParametersSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ISpecificationTermParametersSchema(ISpecificationTermParametersSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("7a9ce83e-fd98-4d67-b241-16d384aff5e3");
			Name = "ISpecificationTermParameters";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b11d550e-0087-4f53-ae17-fb00d41102cf");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,78,205,10,194,48,12,62,59,216,59,244,168,151,245,1,28,187,120,16,111,226,246,2,181,164,82,176,93,73,90,65,198,222,221,108,58,217,156,151,144,124,249,254,188,114,64,65,105,16,13,32,42,106,77,44,14,173,55,246,150,80,69,219,250,162,6,124,88,13,252,118,121,214,229,217,70,74,41,74,74,206,41,124,86,159,251,2,1,129,192,71,18,74,80,0,109,141,213,163,94,68,22,138,160,144,131,120,165,98,114,144,51,139,144,174,119,171,133,245,204,48,67,153,83,61,247,24,178,207,95,7,230,119,27,30,171,34,35,112,132,248,47,114,157,249,70,16,98,66,79,85,243,35,41,229,244,25,168,203,2,67,198,18,217,238,246,76,235,243,172,127,1,228,75,75,155,80,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("7a9ce83e-fd98-4d67-b241-16d384aff5e3"));
		}

		#endregion

	}

	#endregion

}

