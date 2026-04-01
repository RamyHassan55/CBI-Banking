namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CalendarRemindCalculatorCustomerServiceSchema

	/// <exclude/>
	public class CalendarRemindCalculatorCustomerServiceSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CalendarRemindCalculatorCustomerServiceSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CalendarRemindCalculatorCustomerServiceSchema(CalendarRemindCalculatorCustomerServiceSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("edcd46c2-7ae7-4652-8fc1-4857d18db187");
			Name = "CalendarRemindCalculatorCustomerService";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("50529f8b-8504-4b8d-9a81-5bda32bd1be4");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,75,107,194,64,16,62,39,224,127,24,244,18,65,114,232,81,107,47,65,36,135,66,169,214,75,233,97,221,140,118,33,217,13,251,80,68,243,223,59,217,196,54,218,90,188,132,253,102,51,223,99,102,157,17,114,11,139,131,177,88,76,122,161,243,112,137,90,51,163,54,54,78,148,70,42,247,66,201,10,52,37,227,120,113,41,55,98,235,52,179,66,201,94,120,236,133,193,95,253,241,76,90,97,5,26,79,20,12,52,110,233,127,72,114,102,204,24,18,150,163,204,152,126,197,66,200,140,16,119,57,179,74,39,206,88,85,160,94,160,222,9,142,190,181,116,235,92,112,224,117,231,189,141,112,91,98,245,64,156,181,235,31,79,74,26,171,29,167,75,178,246,162,197,142,217,70,58,40,27,112,175,108,244,102,80,19,157,68,94,79,7,220,5,28,214,140,193,24,214,204,96,116,117,5,222,81,213,168,14,72,171,241,214,226,214,232,51,218,79,149,213,30,253,72,90,139,205,120,140,165,133,240,187,231,147,104,164,92,41,5,103,146,227,117,12,111,52,248,55,204,8,230,78,100,192,41,75,154,181,246,3,191,243,131,47,182,199,41,204,209,38,223,56,186,38,105,250,71,32,113,255,254,1,71,232,183,22,82,122,153,105,214,31,65,159,246,161,52,181,18,130,106,56,241,58,98,3,81,87,100,10,210,229,57,156,78,29,233,152,132,151,135,18,179,68,229,174,144,43,150,59,124,172,61,63,69,93,206,97,221,93,151,227,89,81,218,195,57,73,160,209,58,45,61,111,163,89,249,239,185,140,251,187,223,196,175,77,67,210,157,79,39,71,53,185,241,8,218,90,183,84,125,1,81,158,115,180,196,3,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("edcd46c2-7ae7-4652-8fc1-4857d18db187"));
		}

		#endregion

	}

	#endregion

}

