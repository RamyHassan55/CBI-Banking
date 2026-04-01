namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: CrtDigitalAdsAppConstsSchema

	/// <exclude/>
	public class CrtDigitalAdsAppConstsSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public CrtDigitalAdsAppConstsSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public CrtDigitalAdsAppConstsSchema(CrtDigitalAdsAppConstsSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("62cff4b7-8942-47cb-860d-697f230af8cf");
			Name = "CrtDigitalAdsAppConsts";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("abf715ae-fc66-421f-87dc-256931a52a49");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,203,75,204,77,45,46,72,76,78,85,112,46,42,113,201,76,207,44,73,204,113,76,41,118,44,40,224,229,170,230,229,226,44,40,77,202,201,76,86,40,46,73,44,1,82,201,57,137,197,197,24,42,157,243,243,138,75,138,129,138,129,26,128,36,154,158,162,212,196,148,252,188,156,74,32,191,40,51,47,93,33,212,211,47,191,36,51,45,51,181,40,56,53,47,37,181,200,15,232,2,5,91,5,37,116,83,149,172,193,166,213,242,114,1,17,0,239,105,251,4,167,0,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateFormerLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateFormerLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("b0312560-912f-2e9a-0bf4-b44b95b3226b"),
				Name = "Former",
				CreatedInPackageId = new Guid("a949c915-a12e-4960-b5df-cd5a028404f6"),
				CreatedInSchemaUId = new Guid("62cff4b7-8942-47cb-860d-697f230af8cf"),
				ModifiedInSchemaUId = new Guid("62cff4b7-8942-47cb-860d-697f230af8cf")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("62cff4b7-8942-47cb-860d-697f230af8cf"));
		}

		#endregion

	}

	#endregion

}

