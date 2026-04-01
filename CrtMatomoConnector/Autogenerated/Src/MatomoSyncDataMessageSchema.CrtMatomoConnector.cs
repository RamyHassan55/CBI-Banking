namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: MatomoSyncDataMessageSchema

	/// <exclude/>
	public class MatomoSyncDataMessageSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public MatomoSyncDataMessageSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public MatomoSyncDataMessageSchema(MatomoSyncDataMessageSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec");
			Name = "MatomoSyncDataMessage";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,141,82,61,143,211,64,16,173,19,41,255,97,116,52,161,177,11,58,242,209,36,136,202,8,238,66,133,40,246,214,227,120,165,243,140,153,221,141,46,156,248,239,140,215,49,113,114,1,81,184,240,219,121,111,223,155,183,100,26,244,173,177,8,59,20,49,158,171,144,109,152,42,183,143,98,130,99,154,77,95,102,211,73,244,142,246,23,35,130,139,217,84,79,222,8,238,117,12,54,79,198,251,247,80,152,192,13,63,28,201,110,77,48,5,122,111,246,152,6,243,60,135,165,143,77,99,228,184,62,253,39,18,4,6,193,86,208,35,5,104,146,0,120,85,128,82,37,192,216,206,6,52,189,84,54,40,229,35,169,111,59,142,182,254,18,49,226,233,198,239,138,182,241,241,201,89,176,233,142,155,190,96,240,251,138,175,244,151,228,250,156,143,201,7,137,54,176,104,204,207,73,187,159,184,14,214,39,59,143,67,165,223,210,35,130,21,172,86,119,55,173,220,229,235,236,143,216,56,219,16,227,38,107,254,22,186,118,38,147,221,177,69,88,193,171,28,29,158,117,164,69,26,187,199,31,209,233,162,183,88,198,86,85,83,195,202,83,167,152,38,126,157,66,35,149,125,238,203,37,20,24,106,46,255,39,255,22,43,71,232,129,98,243,136,2,92,105,197,65,156,34,142,192,26,143,29,100,245,53,213,160,219,209,119,197,242,151,252,9,81,114,20,242,235,79,215,114,217,50,31,206,70,203,226,131,42,186,18,245,178,0,31,49,20,230,249,94,231,143,27,142,20,116,105,171,53,188,91,140,220,59,170,81,92,40,217,230,55,101,14,236,74,248,240,140,54,6,156,127,245,40,90,47,97,255,48,227,197,239,80,200,193,72,122,195,133,33,173,65,116,199,106,227,225,12,204,175,104,125,63,35,70,106,173,22,38,247,83,107,254,71,57,61,122,9,42,246,27,181,166,23,35,217,3,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateLogEventNameLocalizableString());
			LocalizableStrings.Add(CreateLogEventDescriptionLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateLogEventNameLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("9fcd1454-9111-4bd8-1172-03341e16d756"),
				Name = "LogEventName",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec"),
				ModifiedInSchemaUId = new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateLogEventDescriptionLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("c3de1b6f-2793-89f3-a694-65861d1a02fb"),
				Name = "LogEventDescription",
				CreatedInPackageId = new Guid("992bffc1-0ec1-4459-b82e-3bbf78f3508e"),
				CreatedInSchemaUId = new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec"),
				ModifiedInSchemaUId = new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("0819bdf9-1032-4f16-9e3f-1dc2ab1075ec"));
		}

		#endregion

	}

	#endregion

}

