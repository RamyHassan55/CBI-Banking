namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: DefaultGAOAuthHandlerSchema

	/// <exclude/>
	public class DefaultGAOAuthHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public DefaultGAOAuthHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public DefaultGAOAuthHandlerSchema(DefaultGAOAuthHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("0282b78a-e6dd-494c-846a-044c7b419c16");
			Name = "DefaultGAOAuthHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("f3e00ac6-0422-4cac-8e36-b64e7b099372");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,125,82,77,79,2,49,16,61,67,194,127,152,192,5,18,93,226,65,141,32,36,8,17,185,0,1,110,198,144,178,59,187,52,150,118,109,103,33,132,240,223,157,93,62,178,42,114,107,167,111,222,123,243,58,90,172,208,197,194,71,232,90,234,27,19,41,236,104,161,182,36,125,231,189,9,29,40,180,174,84,220,149,138,133,196,73,29,165,176,174,50,73,48,208,132,145,21,36,141,62,227,154,87,81,233,217,134,44,149,195,205,208,90,225,76,72,94,215,88,244,94,133,79,198,202,12,193,152,122,189,14,207,46,89,173,132,221,182,143,247,131,150,3,145,208,18,44,126,37,232,8,200,64,74,188,48,230,19,2,25,73,18,10,68,16,48,42,142,189,19,81,61,199,244,222,195,80,36,138,94,164,14,216,71,149,182,49,154,176,58,152,26,95,10,53,234,48,249,113,168,218,13,12,57,35,104,65,249,216,211,239,228,223,203,181,15,230,139,147,133,146,62,248,74,56,7,23,113,208,56,213,243,213,27,184,32,201,124,187,108,254,66,197,98,196,209,193,216,154,24,45,113,48,13,24,103,82,135,247,223,1,101,133,78,28,51,32,203,28,52,91,247,206,200,124,2,39,203,102,205,95,32,3,4,71,54,253,145,124,119,171,13,229,13,46,230,226,180,17,229,230,21,221,217,18,129,137,52,73,218,130,243,217,241,63,202,41,118,212,27,53,96,50,236,221,222,63,60,62,221,193,132,231,116,188,30,96,54,250,208,11,82,3,253,96,68,187,150,188,60,222,21,243,211,172,243,143,109,239,216,235,133,137,82,115,225,51,203,121,148,10,234,224,144,114,118,223,151,138,251,111,73,165,91,198,20,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("0282b78a-e6dd-494c-846a-044c7b419c16"));
		}

		#endregion

	}

	#endregion

}

