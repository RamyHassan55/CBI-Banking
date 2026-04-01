namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ITemplateHandlerSchema

	/// <exclude/>
	public class ITemplateHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ITemplateHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ITemplateHandlerSchema(ITemplateHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("ed5c3c82-7897-45fe-ada5-7cb0ef07c46a");
			Name = "ITemplateHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("d0843e01-a902-455d-9128-3897f691b1df");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,181,144,207,106,195,48,12,198,207,41,244,29,4,189,47,247,109,236,146,193,182,91,15,123,1,215,86,19,65,44,27,89,237,8,165,239,62,57,201,96,127,24,236,50,48,6,217,210,247,253,62,177,139,88,178,243,8,157,104,151,88,145,245,17,11,245,140,178,221,92,182,155,166,109,91,184,47,167,24,157,76,15,107,189,151,116,166,128,5,28,248,20,16,14,19,188,13,228,7,208,1,161,160,156,201,4,3,42,74,36,182,54,210,2,217,137,146,167,236,148,18,3,49,100,73,30,75,33,238,77,70,49,230,209,41,222,124,56,182,159,44,243,233,48,146,183,25,19,60,86,214,151,215,181,253,217,113,24,43,105,99,168,118,55,59,193,190,234,27,97,70,51,196,114,11,251,121,124,249,255,158,102,126,120,66,227,171,232,255,16,230,103,154,166,168,212,190,149,189,171,158,23,232,81,239,224,250,23,200,44,148,132,116,130,116,156,235,97,209,249,197,204,150,102,203,88,39,190,216,236,144,195,178,173,90,94,231,243,14,253,34,168,76,15,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("ed5c3c82-7897-45fe-ada5-7cb0ef07c46a"));
		}

		#endregion

	}

	#endregion

}

