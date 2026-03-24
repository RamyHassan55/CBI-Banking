namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoImportByFormSubmitMessageSchema

	/// <exclude/>
	public class MatomoImportByFormSubmitMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoImportByFormSubmitMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoImportByFormSubmitMessageSchema(MatomoImportByFormSubmitMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("8760db92-f330-469c-a162-74020a454f25");
			Name = "MatomoImportByFormSubmitMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,197,86,75,111,218,64,16,62,39,82,255,195,136,94,64,66,88,237,177,4,14,37,143,166,18,85,218,60,46,81,14,139,61,134,149,236,93,103,118,151,196,69,249,239,29,239,26,48,132,132,228,146,94,162,120,30,223,126,51,243,237,44,74,228,104,10,17,35,92,33,145,48,58,181,189,145,86,169,156,58,18,86,106,245,233,112,241,233,240,192,25,169,166,112,89,26,139,121,127,245,253,11,31,172,86,62,231,167,209,106,237,104,98,17,178,157,61,159,9,167,140,7,163,76,24,243,13,198,194,234,92,159,231,133,38,251,189,60,213,148,95,186,73,46,237,24,141,17,83,244,41,81,20,193,145,113,121,46,168,28,214,223,62,29,172,6,194,130,208,160,178,144,123,40,72,132,21,32,61,32,76,74,72,25,18,140,199,4,17,87,165,64,30,176,123,75,232,168,129,125,123,165,93,60,251,237,208,97,77,225,142,173,133,155,100,50,134,216,31,186,135,50,44,139,122,134,196,64,11,95,208,186,9,220,54,75,46,182,154,184,23,23,254,148,16,177,93,115,40,154,195,201,250,112,72,73,115,0,34,196,132,233,160,181,135,84,43,26,246,86,176,209,54,238,81,33,72,228,160,88,4,131,86,172,149,229,70,157,39,173,225,40,252,11,78,201,123,135,32,19,110,179,76,37,250,211,115,24,17,86,218,232,29,69,62,127,55,220,92,26,201,124,43,184,192,17,106,11,195,189,158,233,12,54,211,170,207,189,57,248,104,145,148,200,174,41,107,13,79,27,163,119,148,109,100,214,19,221,211,182,246,153,147,9,172,58,210,5,158,86,37,236,85,81,43,75,32,219,173,160,15,14,106,91,131,76,135,85,49,17,6,219,29,88,248,152,171,178,64,24,192,51,141,84,246,94,224,211,247,129,163,229,225,28,189,34,18,92,129,252,205,146,11,7,172,120,53,3,174,61,53,246,6,142,193,117,178,230,198,158,6,211,224,254,131,247,78,242,189,58,198,196,21,220,40,191,3,56,144,197,138,62,226,169,22,50,170,36,104,121,83,216,23,164,11,36,43,241,141,178,182,66,42,179,172,143,167,12,172,143,134,186,171,49,244,78,242,194,150,47,11,249,182,218,61,245,185,101,251,24,83,225,50,123,35,50,135,63,132,74,178,106,32,3,216,101,238,157,79,21,175,167,206,93,67,23,126,238,235,214,47,96,138,182,15,166,250,243,244,150,74,242,109,169,255,239,130,106,73,110,75,230,221,133,45,111,225,71,83,174,69,252,110,190,15,56,9,79,0,75,251,3,73,55,175,215,14,206,47,93,155,49,218,153,78,222,114,103,152,143,84,104,160,32,169,73,218,18,116,10,177,35,242,15,97,253,190,65,189,83,184,11,210,206,96,38,167,51,52,118,157,242,32,179,12,38,200,6,29,115,36,38,144,74,50,246,133,54,213,5,234,57,63,234,252,18,128,228,147,206,208,94,212,104,188,217,6,67,248,242,181,223,32,45,213,12,217,151,232,56,218,9,49,215,124,49,78,30,49,118,22,219,213,132,121,108,10,195,35,237,54,62,151,107,115,46,8,76,169,226,177,80,92,24,241,96,152,194,229,218,208,222,74,11,219,172,145,209,171,130,103,164,149,252,139,205,133,223,30,173,151,252,214,37,233,110,72,176,219,156,109,231,149,93,248,84,253,216,217,48,177,229,31,101,2,67,194,104,9,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("1c165e98-e3f7-76ad-2cce-2cc180f66e8c"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("8760db92-f330-469c-a162-74020a454f25"),
				ModifiedInSchemaUId = new Guid("8760db92-f330-469c-a162-74020a454f25")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("83436f7d-b633-7b1b-1635-dd914b72a8de"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("8760db92-f330-469c-a162-74020a454f25"),
				ModifiedInSchemaUId = new Guid("8760db92-f330-469c-a162-74020a454f25")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("8760db92-f330-469c-a162-74020a454f25"));
		}

		#endregion

	}

	#endregion

}

