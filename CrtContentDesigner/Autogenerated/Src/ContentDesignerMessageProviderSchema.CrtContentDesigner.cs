namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: ContentDesignerMessageProviderSchema

	/// <exclude/>
	public class ContentDesignerMessageProviderSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public ContentDesignerMessageProviderSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public ContentDesignerMessageProviderSchema(ContentDesignerMessageProviderSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("aa5149e5-5916-45c6-9e68-a361015700ac");
			Name = "ContentDesignerMessageProvider";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("49249a60-f457-4989-b338-2472f16140e6");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,133,82,77,75,195,64,16,61,71,240,63,12,209,67,11,37,185,215,182,7,43,74,65,69,234,199,69,60,172,155,73,93,76,118,195,204,70,169,197,255,238,100,155,182,164,22,61,206,155,55,111,222,124,128,85,37,114,165,52,66,52,37,63,117,214,163,245,23,200,102,97,145,142,143,86,199,71,81,205,198,46,224,1,137,20,187,220,39,83,71,120,118,16,47,75,103,15,103,8,147,75,165,189,35,131,188,99,252,238,152,204,36,164,92,252,52,52,33,158,16,46,140,179,48,45,20,243,16,246,232,55,200,172,22,120,71,238,195,100,141,95,169,120,190,192,92,213,133,63,55,54,147,38,61,191,172,208,229,189,217,204,230,110,143,223,31,192,173,204,15,99,136,255,22,142,251,47,162,92,213,175,133,209,160,27,43,255,56,129,33,28,106,40,34,171,96,114,59,215,13,250,55,151,201,100,119,65,124,157,76,211,20,70,92,151,165,162,229,100,3,204,209,215,100,25,10,167,85,97,190,212,107,129,192,158,154,61,126,168,162,70,200,29,1,87,168,77,110,48,131,114,221,57,217,234,165,251,130,163,74,145,42,161,249,128,113,92,51,146,140,100,81,123,177,21,79,30,59,49,24,203,94,89,141,201,40,13,85,135,69,222,113,25,79,218,145,65,130,14,187,221,94,107,249,10,125,75,236,237,181,234,58,25,108,10,68,174,15,205,59,70,81,139,180,19,202,245,44,126,194,245,110,45,247,33,223,235,10,37,115,100,87,147,150,172,35,41,27,4,169,168,49,46,223,241,247,53,229,79,78,227,95,13,56,89,137,169,239,228,169,217,126,220,63,11,130,20,174,180,241,22,176,239,246,226,104,179,245,209,67,188,70,187,160,96,63,28,81,82,188,145,3,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreatePageConfigLengthLimitErrorLocalizableString());
			LocalizableStrings.Add(CreateEmailConfigLengthLimitErrorLocalizableString());
		}

		protected virtual SchemaLocalizableString CreatePageConfigLengthLimitErrorLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("ea09ab72-03c8-4668-1eb4-c652dbb67c69"),
				Name = "PageConfigLengthLimitError",
				CreatedInPackageId = new Guid("49249a60-f457-4989-b338-2472f16140e6"),
				CreatedInSchemaUId = new Guid("aa5149e5-5916-45c6-9e68-a361015700ac"),
				ModifiedInSchemaUId = new Guid("aa5149e5-5916-45c6-9e68-a361015700ac")
			};
			return localizableString;
		}

		protected virtual SchemaLocalizableString CreateEmailConfigLengthLimitErrorLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("c4596537-605f-e138-d315-ab9bb9c2de36"),
				Name = "EmailConfigLengthLimitError",
				CreatedInPackageId = new Guid("667c7ad5-6764-46f4-9da1-16cff5a23853"),
				CreatedInSchemaUId = new Guid("aa5149e5-5916-45c6-9e68-a361015700ac"),
				ModifiedInSchemaUId = new Guid("aa5149e5-5916-45c6-9e68-a361015700ac")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("aa5149e5-5916-45c6-9e68-a361015700ac"));
		}

		#endregion

	}

	#endregion

}

