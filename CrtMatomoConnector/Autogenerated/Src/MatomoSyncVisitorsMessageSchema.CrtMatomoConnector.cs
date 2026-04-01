namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoSyncVisitorsMessageSchema

	/// <exclude/>
	public class MatomoSyncVisitorsMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoSyncVisitorsMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoSyncVisitorsMessageSchema(MatomoSyncVisitorsMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd");
			Name = "MatomoSyncVisitorsMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,205,85,77,111,219,48,12,61,183,192,254,3,145,93,18,160,176,111,59,52,137,47,109,209,117,64,134,14,201,122,41,122,80,101,218,22,96,75,134,62,210,101,65,255,251,40,89,142,211,52,193,122,236,69,176,41,242,241,81,124,148,36,107,208,180,140,35,172,80,107,102,84,97,147,43,37,11,81,58,205,172,80,242,203,249,246,203,249,153,51,66,150,176,220,24,139,205,244,224,159,252,235,26,185,119,54,201,45,74,212,130,15,62,63,241,197,210,134,199,253,97,148,28,54,246,243,105,36,59,237,124,213,88,18,12,92,213,204,152,75,88,48,171,26,181,220,72,254,32,140,176,74,155,5,26,195,74,12,206,105,154,194,204,184,166,97,122,147,197,255,16,8,86,129,198,86,163,65,105,161,9,32,96,8,5,114,102,25,176,64,21,154,14,10,10,165,97,29,225,193,180,200,69,33,48,79,250,4,233,94,134,199,149,114,188,250,229,208,97,36,242,68,214,214,61,215,130,3,15,169,79,82,134,190,156,119,24,4,177,13,5,13,229,211,129,89,237,184,15,191,132,251,128,223,121,28,214,220,21,61,184,135,106,102,6,17,184,198,98,62,58,73,103,148,102,201,14,48,61,68,156,181,76,179,6,36,137,99,62,234,15,103,148,45,84,142,181,1,85,12,7,102,187,147,77,102,105,8,57,142,96,44,211,246,154,89,28,101,126,237,131,160,208,170,249,79,164,176,120,151,83,106,90,124,222,190,153,100,62,158,59,54,227,100,221,227,187,27,233,26,212,236,185,198,89,104,69,40,42,219,85,116,1,158,226,74,52,8,59,218,23,30,249,236,108,63,84,72,155,65,100,55,129,109,112,88,109,90,132,57,188,107,176,183,39,158,203,52,184,245,140,200,181,79,218,109,44,59,56,178,71,224,104,238,105,248,141,254,59,108,189,70,217,160,204,59,229,188,149,209,189,86,45,106,43,240,35,34,58,46,134,71,63,179,17,103,51,190,198,130,185,218,62,176,218,225,119,38,243,218,207,241,28,142,153,147,187,82,210,88,79,158,246,154,114,234,232,119,7,178,133,18,237,20,140,95,94,63,15,215,208,235,190,57,159,136,227,78,168,131,66,142,176,59,165,142,5,218,74,229,31,145,6,241,17,146,230,173,213,66,105,97,55,126,16,185,211,58,220,174,157,196,19,136,90,55,240,34,108,5,149,40,43,52,118,8,121,17,117,13,207,72,6,197,201,19,115,40,132,54,246,196,29,20,11,84,107,122,35,68,142,64,29,128,91,180,247,17,109,60,129,121,6,223,166,123,156,133,172,232,221,177,185,226,233,81,132,181,18,57,220,252,65,238,44,142,127,27,212,116,105,202,238,209,2,247,230,183,159,230,53,211,225,122,89,48,73,117,105,234,11,49,88,14,134,241,65,88,55,172,123,17,97,228,43,173,164,248,139,227,135,221,245,18,133,116,49,116,109,114,106,152,163,237,109,7,201,246,15,114,249,4,207,181,7,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("c1d5437e-f888-dc21-3319-d6a9fbfb09e1"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd"),
				ModifiedInSchemaUId = new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("9fe6ebe2-99ee-3a5c-c1c6-952230c33d0a"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd"),
				ModifiedInSchemaUId = new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("72210611-e9cf-47ba-b0c3-465f09cb4cdd"));
		}

		#endregion

	}

	#endregion

}

