namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoContactsRequestMessageSchema

	/// <exclude/>
	public class MatomoContactsRequestMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoContactsRequestMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoContactsRequestMessageSchema(MatomoContactsRequestMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca");
			Name = "MatomoContactsRequestMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,61,111,219,48,16,157,109,192,255,225,144,46,206,34,13,221,226,143,197,73,138,12,42,210,212,153,130,14,52,117,178,8,68,60,245,72,26,81,131,254,247,158,40,43,150,157,198,200,192,129,143,239,30,239,222,59,171,42,116,181,210,8,107,100,86,142,10,159,172,200,22,102,27,88,121,67,118,50,126,157,140,71,193,25,187,61,162,48,206,38,99,121,249,194,184,21,26,172,158,149,115,87,144,41,79,21,137,130,87,218,187,7,252,29,208,249,12,157,83,91,140,252,52,77,97,238,66,85,41,110,150,251,123,172,5,79,192,88,51,58,180,30,170,168,3,122,47,212,62,186,198,106,216,52,16,28,242,93,46,220,168,13,85,39,158,244,218,233,64,252,105,77,65,151,63,2,6,220,247,240,75,208,58,108,158,141,6,29,127,61,215,48,244,243,188,147,17,149,215,56,206,97,126,178,206,115,208,158,88,108,184,143,95,116,140,211,137,187,145,15,116,40,228,204,29,34,104,198,98,113,113,174,163,139,116,153,188,105,14,39,237,135,58,87,60,189,132,54,203,209,104,221,212,8,11,120,55,85,139,39,119,85,77,236,103,145,216,214,27,73,228,26,243,80,139,124,220,8,169,148,206,49,50,254,238,77,64,155,119,62,28,155,146,161,47,41,255,140,31,215,88,24,139,14,108,168,54,200,64,133,228,235,217,8,98,44,104,229,176,133,180,108,95,9,226,150,236,33,241,7,70,68,68,138,3,91,183,252,126,42,151,204,211,254,109,224,26,237,68,209,228,40,159,121,248,134,62,83,47,15,194,111,86,20,172,23,219,22,75,248,58,27,116,111,108,137,108,124,78,58,253,175,204,142,76,14,55,47,168,131,199,233,163,236,171,4,98,81,71,247,194,209,181,143,100,167,56,46,120,166,172,4,193,226,177,180,241,243,0,76,79,202,186,124,6,21,73,75,46,153,172,249,131,183,196,39,27,48,189,60,19,87,135,30,131,130,253,3,146,20,2,27,27,4,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("5301d476-7fad-f422-6cc1-af9010fb0c89"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca"),
				ModifiedInSchemaUId = new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("1c577ef4-80ae-df02-f56c-a0fd421199f0"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca"),
				ModifiedInSchemaUId = new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("5dfa5bae-0220-4b3b-8719-21bfb00da2ca"));
		}

		#endregion

	}

	#endregion

}

