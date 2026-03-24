namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ISupMailBoxLangProviderSchema

	/// <exclude/>
	public class ISupMailBoxLangProviderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ISupMailBoxLangProviderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ISupMailBoxLangProviderSchema(ISupMailBoxLangProviderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("15f7828f-b2b8-4927-9695-3e8e4374d7a6");
			Name = "ISupMailBoxLangProvider";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c3c90037-274c-4793-841e-197eb228d3cd");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,145,221,106,195,48,12,133,175,23,200,59,136,94,109,48,146,7,88,150,139,109,80,6,29,20,186,23,112,93,57,19,196,63,200,246,104,25,125,247,201,201,66,199,122,185,75,201,223,209,57,178,156,178,24,131,210,8,239,200,172,162,55,169,121,246,206,208,144,89,37,242,174,174,190,234,234,38,71,114,3,236,78,49,161,125,248,83,11,63,142,168,11,28,155,53,58,100,210,194,8,213,182,45,116,49,91,171,248,212,255,212,47,24,53,211,30,35,164,83,192,123,72,31,42,65,96,255,73,7,233,141,202,13,89,13,8,134,189,5,171,104,132,189,63,202,131,241,12,228,180,64,46,1,227,64,49,205,249,154,197,167,253,101,20,242,126,36,45,130,132,108,202,114,175,187,28,222,100,218,147,63,110,196,98,59,251,177,160,101,187,171,160,83,99,251,191,80,215,169,230,78,80,172,44,56,249,247,199,21,163,166,64,162,142,171,126,35,106,240,6,176,24,196,166,107,39,240,162,99,76,153,93,236,55,75,154,201,150,12,33,11,188,188,22,124,157,233,0,107,76,11,121,91,70,119,146,77,110,214,195,197,243,174,92,242,92,87,231,111,13,54,13,165,5,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("15f7828f-b2b8-4927-9695-3e8e4374d7a6"));
		}

		#endregion

	}

	#endregion

}

