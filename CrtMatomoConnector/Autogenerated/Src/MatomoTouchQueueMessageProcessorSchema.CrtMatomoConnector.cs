namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoTouchQueueMessageProcessorSchema

	/// <exclude/>
	public class MatomoTouchQueueMessageProcessorSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoTouchQueueMessageProcessorSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoTouchQueueMessageProcessorSchema(MatomoTouchQueueMessageProcessorSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967");
			Name = "MatomoTouchQueueMessageProcessor";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,87,91,111,219,54,20,126,86,129,254,135,3,237,97,50,96,200,24,246,50,52,182,11,52,151,34,64,211,102,179,179,61,20,197,64,75,199,54,17,137,212,72,202,141,151,229,191,239,144,162,124,145,29,43,110,0,191,36,246,33,121,190,239,124,231,66,90,176,28,117,193,18,132,49,42,197,180,156,154,248,92,138,41,159,149,138,25,46,197,219,55,143,111,223,4,165,230,98,6,163,165,54,152,159,53,190,211,254,44,195,196,110,214,241,71,20,168,120,178,222,179,233,86,33,217,105,229,39,133,51,218,13,231,25,211,250,29,220,48,35,115,57,150,101,50,255,189,196,18,111,80,107,54,195,91,37,19,250,36,149,59,211,235,245,160,175,203,60,103,106,57,244,223,253,14,212,96,161,50,4,179,44,48,133,188,58,15,83,37,115,58,131,8,137,194,233,32,92,35,132,189,97,92,251,236,109,56,253,250,101,162,101,134,6,163,240,183,248,151,248,87,248,15,238,52,73,211,164,22,95,62,96,82,210,182,14,240,188,200,48,71,97,88,37,64,216,249,102,29,173,143,172,194,176,246,162,156,100,60,129,196,6,222,26,55,188,131,15,76,227,33,97,130,71,39,206,90,81,162,96,84,153,24,169,72,216,91,135,86,237,104,234,231,12,215,130,27,206,50,254,47,73,200,64,224,119,224,116,158,9,42,7,57,5,51,199,77,249,218,232,146,168,85,96,241,10,175,215,4,236,23,76,177,28,4,85,221,32,44,53,42,34,44,170,218,9,135,231,165,82,36,36,88,59,36,171,133,184,223,115,167,156,19,47,96,27,151,232,110,203,55,108,67,117,72,217,9,41,27,53,205,143,240,228,245,68,145,86,146,110,235,75,0,5,42,195,177,161,110,161,248,130,25,244,188,46,152,97,163,165,72,110,152,32,82,10,254,206,247,153,207,118,226,105,158,219,111,181,253,24,4,51,52,48,24,62,227,26,222,191,135,232,153,165,129,203,243,94,215,13,213,58,29,71,49,208,7,161,6,176,96,89,137,110,107,139,122,55,104,230,50,181,210,41,105,8,3,211,85,109,246,185,152,211,224,48,169,76,160,87,101,186,222,3,114,65,51,132,167,8,84,218,118,164,124,68,243,133,178,224,26,110,84,38,54,227,99,124,48,145,107,22,166,239,55,43,162,30,6,93,170,108,67,69,85,10,211,241,10,46,152,2,67,231,40,130,237,184,105,136,153,79,50,177,125,193,38,25,142,28,106,100,107,86,78,163,182,202,235,116,157,243,32,188,206,11,169,76,181,125,68,75,219,100,67,175,173,66,83,42,225,35,139,175,164,34,145,35,203,170,235,201,110,234,250,10,157,174,24,207,48,109,149,169,214,198,211,58,145,46,107,114,225,15,197,59,145,50,131,115,38,62,75,195,167,75,91,171,83,150,105,127,217,84,115,103,195,77,53,205,14,79,214,176,55,124,209,208,36,165,169,237,103,75,152,210,184,102,105,106,101,103,101,202,209,142,80,175,169,254,25,138,202,175,77,241,254,217,184,39,168,133,228,169,195,26,121,140,168,78,78,109,240,173,124,193,93,118,200,85,127,76,247,95,23,174,74,145,244,119,98,115,29,48,28,122,31,193,87,123,87,82,222,198,138,37,247,68,172,74,203,135,229,95,56,153,75,121,239,15,117,190,121,144,67,62,163,200,14,211,250,132,149,223,99,184,22,171,111,227,1,108,236,2,166,225,48,242,153,119,225,43,113,239,188,138,237,231,185,146,130,114,177,58,31,121,64,251,140,49,44,49,215,105,183,230,16,215,144,182,174,173,221,99,4,245,250,45,253,185,83,153,239,205,224,169,46,218,90,172,138,69,77,216,118,235,168,156,228,220,156,64,173,22,232,227,229,90,187,56,164,88,229,232,79,174,57,61,41,104,161,169,88,181,94,235,89,91,47,31,12,42,193,178,118,45,45,37,75,243,100,10,54,0,143,213,45,122,65,60,94,45,125,210,152,26,160,71,199,85,167,174,246,179,78,230,136,27,188,78,55,13,134,41,67,190,176,69,11,95,78,250,15,252,167,68,125,186,38,217,143,123,172,34,212,31,13,71,109,185,175,183,143,93,66,78,30,238,22,236,241,243,160,234,97,189,59,12,244,78,211,191,164,34,220,255,167,215,189,94,46,23,244,115,224,51,189,47,162,29,221,214,15,150,129,243,19,188,228,169,82,211,165,69,123,83,70,157,216,58,239,66,248,73,206,86,88,238,9,242,10,190,23,168,19,197,11,203,226,121,218,213,115,84,161,46,51,243,3,111,174,214,64,54,72,108,63,169,26,79,243,202,186,109,36,219,255,103,64,63,133,22,16,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateImportMatomoSessionFailedTextLocalizableString());
			LocalizableStrings.Add(CreateImportMatomoSessionSuccessTextLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateImportMatomoSessionFailedTextLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("f96b58c3-bf12-9267-cdbe-281878d147a3"),
				Name = "ImportMatomoSessionFailedText",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967"),
				ModifiedInSchemaUId = new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateImportMatomoSessionSuccessTextLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("0f7bb024-1fd4-ca96-3d90-af75b4263a67"),
				Name = "ImportMatomoSessionSuccessText",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967"),
				ModifiedInSchemaUId = new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("2c6febba-600c-4d2f-85b5-2e3db920f967"));
		}

		#endregion

	}

	#endregion

}

