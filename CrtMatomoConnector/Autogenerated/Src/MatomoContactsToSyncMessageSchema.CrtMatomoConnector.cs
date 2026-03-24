namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoContactsToSyncMessageSchema

	/// <exclude/>
	public class MatomoContactsToSyncMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoContactsToSyncMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoContactsToSyncMessageSchema(MatomoContactsToSyncMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd");
			Name = "MatomoContactsToSyncMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,85,75,79,219,64,16,62,131,196,127,24,165,151,68,66,206,173,135,230,33,181,1,81,42,165,162,74,218,11,226,176,172,199,206,74,246,174,187,143,208,52,226,191,119,118,189,142,157,16,82,78,92,44,123,222,223,204,55,99,201,74,52,21,227,8,75,212,154,25,149,217,100,166,100,38,114,167,153,21,74,94,156,111,47,206,207,156,17,50,135,197,198,88,44,71,7,223,100,95,20,200,189,177,73,110,80,162,22,188,181,249,142,79,150,20,62,238,55,163,100,171,232,230,211,72,114,210,124,208,152,83,24,152,21,204,152,79,48,103,86,149,138,202,177,140,91,179,84,139,141,228,115,52,134,229,24,204,135,195,33,140,141,43,75,166,55,211,248,29,92,193,42,208,88,105,52,40,45,148,33,12,24,242,134,148,89,6,44,20,11,101,29,10,50,165,193,84,200,69,38,48,5,30,211,37,77,130,97,39,195,253,82,57,190,250,225,208,97,44,228,129,164,149,123,44,4,7,30,82,159,40,26,26,72,47,162,80,144,109,128,212,182,128,154,102,181,227,86,105,234,196,93,200,80,91,28,162,174,97,183,230,1,207,216,32,2,215,152,77,122,39,10,234,13,167,201,46,228,240,48,230,184,98,154,149,32,137,34,147,94,108,203,109,106,122,211,24,11,68,26,58,237,27,155,140,135,193,250,184,179,177,76,219,43,102,177,55,245,207,198,9,50,173,202,255,120,10,139,33,39,61,64,101,187,89,146,248,120,238,56,139,19,160,251,183,215,210,149,168,217,99,129,227,27,39,210,41,180,224,46,193,23,184,20,37,194,174,232,75,31,247,236,172,235,38,164,157,66,172,109,0,219,96,176,220,84,8,19,120,49,91,47,79,124,254,81,48,155,237,114,145,113,155,184,86,46,234,144,164,137,193,163,184,41,197,43,154,247,160,122,142,172,65,153,214,196,217,103,209,157,86,21,106,43,240,109,28,106,246,216,119,154,239,15,57,44,78,232,246,113,194,220,251,237,142,233,54,253,43,204,152,43,236,47,86,56,252,202,100,90,248,141,159,192,49,113,114,155,75,58,0,131,135,206,244,94,78,168,211,181,45,228,104,71,96,252,227,249,4,156,200,152,249,1,99,200,185,6,19,200,247,206,96,2,111,154,33,191,21,72,152,190,175,57,44,78,38,10,139,186,89,4,143,36,129,207,69,1,184,166,91,103,224,73,216,85,109,155,107,100,222,146,174,1,254,118,172,32,21,153,61,98,152,226,74,43,41,254,98,250,30,13,216,109,84,75,227,35,208,95,163,240,28,237,74,165,111,225,47,213,35,36,13,185,210,66,105,97,55,129,197,78,235,240,15,168,119,49,129,184,148,177,81,43,145,175,208,216,214,165,233,81,165,21,39,75,250,31,100,66,27,251,74,155,34,64,181,166,127,153,72,17,104,188,112,131,246,46,70,235,15,96,50,133,143,163,78,205,66,174,232,255,104,83,197,135,71,35,172,149,72,225,250,15,114,103,177,255,211,160,38,226,203,184,148,110,239,179,57,59,107,166,195,68,231,76,18,46,77,115,161,10,22,173,160,127,224,86,95,148,142,71,184,77,145,15,95,54,62,39,145,179,63,235,156,196,72,216,203,118,128,131,19,199,167,150,238,11,73,246,15,68,89,87,190,106,8,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("a89c84b0-070b-428c-4979-14366327c04d"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd"),
				ModifiedInSchemaUId = new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("135b87ef-075b-19ea-9095-ecfb6acc3446"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd"),
				ModifiedInSchemaUId = new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("d7b976bd-072c-41be-8958-9e83f3f3b6dd"));
		}

		#endregion

	}

	#endregion

}

