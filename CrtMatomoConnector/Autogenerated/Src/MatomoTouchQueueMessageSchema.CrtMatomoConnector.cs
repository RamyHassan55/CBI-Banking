namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoTouchQueueMessageSchema

	/// <exclude/>
	public class MatomoTouchQueueMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoTouchQueueMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoTouchQueueMessageSchema(MatomoTouchQueueMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4");
			Name = "MatomoTouchQueueMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,149,84,81,111,218,48,16,126,78,165,254,135,83,246,2,210,72,182,142,169,163,5,36,40,165,234,3,82,183,210,231,201,117,46,96,141,216,204,118,152,80,181,255,190,139,29,74,72,9,213,30,242,96,231,238,187,239,251,238,206,146,101,104,214,140,35,204,81,107,102,84,106,163,27,37,83,177,200,53,179,66,201,243,179,151,243,179,32,55,66,46,224,113,107,44,102,215,175,231,106,138,198,166,251,104,202,184,85,90,160,161,8,138,249,160,113,65,192,112,179,98,198,92,193,140,89,149,169,185,202,249,242,123,142,57,206,208,24,182,64,23,26,199,49,244,77,158,101,76,111,135,229,217,165,129,85,160,113,173,209,160,180,240,204,12,66,230,112,232,7,1,193,239,2,9,168,110,81,40,243,136,17,236,16,227,10,228,58,127,94,9,14,220,161,54,112,129,43,56,194,47,120,113,28,247,122,148,52,150,73,75,154,30,180,216,48,235,53,188,17,225,46,158,164,32,142,32,18,226,47,82,129,26,84,90,150,135,145,100,171,173,21,156,84,106,198,127,21,142,26,231,60,193,32,2,215,152,14,66,71,232,118,67,233,243,50,40,140,135,228,9,87,58,137,94,203,86,149,6,107,207,10,120,65,20,140,213,174,87,101,182,239,237,125,2,3,8,191,141,71,211,238,229,244,162,211,157,76,122,157,238,104,252,185,51,30,95,92,118,110,167,189,110,111,242,233,203,237,215,94,55,188,46,213,163,76,188,1,135,110,76,5,174,146,186,21,59,6,119,185,72,224,167,239,88,157,192,59,184,206,101,157,23,19,85,160,187,238,157,240,185,18,14,41,125,21,7,27,154,77,54,54,249,231,71,165,33,175,213,134,98,83,130,160,65,23,25,91,200,142,30,152,54,216,170,255,109,23,219,19,252,61,173,125,134,118,169,188,169,202,34,183,152,156,80,254,3,109,174,165,1,225,166,146,22,156,6,204,46,241,173,3,19,102,217,227,86,242,25,147,36,67,55,235,119,55,107,166,89,6,146,158,141,65,152,27,212,100,176,68,183,103,225,240,190,185,212,211,97,40,213,232,199,14,106,143,172,61,225,83,40,141,132,251,241,46,219,143,89,105,15,28,77,128,59,180,149,99,235,144,27,28,170,106,195,192,97,6,238,221,241,79,217,54,34,132,254,81,236,97,203,5,7,18,255,84,103,111,164,23,121,70,187,218,170,123,246,177,94,206,165,183,223,89,130,253,32,212,231,95,200,37,106,97,19,197,227,234,204,170,13,61,201,244,216,248,205,35,246,187,249,107,21,250,254,111,21,253,144,30,94,210,221,63,47,219,254,221,74,6,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("18e1b0ac-5d75-1d51-20ac-4733f64038f1"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4"),
				ModifiedInSchemaUId = new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("be1c70d6-2cc2-dcdb-93c6-69816a39f88a"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4"),
				ModifiedInSchemaUId = new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("6f1be96b-e617-4f38-98f2-c9ab7b7083f4"));
		}

		#endregion

	}

	#endregion

}

