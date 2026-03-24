namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: AutoSubmittedHandlerSchema

	/// <exclude/>
	public class AutoSubmittedHandlerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public AutoSubmittedHandlerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public AutoSubmittedHandlerSchema(AutoSubmittedHandlerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("4504d36d-abab-4979-82a1-10f52e7b8f0e");
			Name = "AutoSubmittedHandler";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("c3c90037-274c-4793-841e-197eb228d3cd");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,146,203,110,227,48,12,69,215,14,144,127,32,60,155,4,104,237,125,155,6,8,130,41,218,69,31,152,4,179,167,109,38,214,140,30,46,41,181,8,138,254,251,200,178,211,71,138,89,25,162,121,207,229,21,101,209,144,116,88,19,108,137,25,197,237,124,177,118,118,167,246,129,209,43,103,167,147,215,233,36,11,162,236,30,54,7,241,100,46,167,147,88,41,203,18,22,18,140,65,62,44,199,243,10,106,141,34,224,91,244,192,212,49,9,89,47,128,208,162,109,52,49,184,29,228,171,224,221,249,38,84,70,121,79,77,14,29,187,142,216,31,64,89,32,131,74,67,75,216,16,23,71,151,242,147,77,23,42,173,234,209,167,39,189,131,110,70,139,11,184,189,73,250,199,145,59,254,136,226,215,52,121,246,131,105,31,147,253,167,15,238,200,84,196,50,244,158,198,76,133,59,242,173,107,160,110,169,254,219,167,37,120,70,29,168,79,55,140,254,145,233,52,109,241,14,45,79,169,139,14,25,13,216,184,145,171,60,1,243,229,239,35,247,219,173,157,24,21,139,50,201,63,104,76,62,176,149,229,175,225,11,91,142,36,181,27,71,85,2,249,189,203,33,102,134,107,212,18,77,98,14,126,81,66,17,117,212,246,176,241,198,43,231,52,172,251,196,51,87,253,161,218,15,160,57,244,207,35,203,6,5,136,231,248,80,138,159,79,33,50,103,169,163,216,186,77,170,206,230,103,176,70,161,248,188,196,75,177,210,218,189,80,243,101,135,199,85,164,216,103,9,156,13,226,181,51,49,159,18,103,139,7,110,148,69,125,187,183,142,169,7,206,47,251,206,183,113,185,100,155,97,191,241,20,107,217,116,242,246,15,117,246,157,245,229,2,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("4504d36d-abab-4979-82a1-10f52e7b8f0e"));
		}

		#endregion

	}

	#endregion

}

