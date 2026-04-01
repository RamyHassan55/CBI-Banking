namespace Terrasoft.Configuration
{
	using System.Linq;
	using Terrasoft.Core;
	using GeneratedWebFormService;
	using Terrasoft.Core.Factories;

	#region Class: WebFormSubmitPreProcessHandler

	/// <summary>
	/// Handles pre process handling for form submit saving.
	/// </summary>
	/// <seealso cref="Terrasoft.Configuration.IGeneratedWebFormPreProcessHandler" />
	public class WebFormSubmitPreProcessHandler : IGeneratedWebFormPreProcessHandler
	{

		#region Fields: Private

		private readonly string _registerMethodColumn = "RegisterMethodId";
		private readonly string _landingRegisterMethodId = "4F685B03-B96F-4D1B-9CAF-E36A2DA53CBD";
		private readonly string _facebookRegisterMethodId = "E6EEF00A-A551-4D7E-AB21-BC9B5F694849";

		#endregion

		#region Properties: Protected

		protected UserConnection UserConnection { get; set; }

		protected FormData FormData { get; set; }
		protected IWebFormImportParamsGenerator ParamsGenerator { get; set; }

		private ITouchSource _sourceHelper;
		public ITouchSource SourceHelper {
			get => _sourceHelper ?? (_sourceHelper =
				ClassFactory.Get<ITouchSource>(new ConstructorArgument("userConnection", UserConnection)));
			set => _sourceHelper = value;
		}

		#endregion

		#region Methods: Protected

		protected void EditFormData() {
			var webFormId = FormData.GetWebFormId();
			FormData.AddFormFieldValue("WebFormId", webFormId.ToString());
			var defaultValues = ParamsGenerator.DefaultValueManager.GetValues(webFormId, UserConnection);
			if (!defaultValues.ContainsKey(_registerMethodColumn)) {
				var socialMetadata = FormData.formFieldsData.SingleOrDefault(f => f.name.Equals("SocialMetadata"));
				var registerMethodId = socialMetadata == null ? _landingRegisterMethodId : _facebookRegisterMethodId;
				FormData.AddFormFieldValue(_registerMethodColumn, registerMethodId);
			}
			var bpmHref = FormData.formFieldsData.SingleOrDefault(x => x.name == "BpmHref")?.value;
			var bpmRef = FormData.formFieldsData.SingleOrDefault(x => x.name == "BpmRef")?.value;
			if (bpmHref == null && bpmRef == null) {
				return;
			}
			FormData.AddFormFieldValue("LandingPageURL", bpmHref);
			FormData.AddFormFieldValue("Referrer", bpmRef);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Executes the the pre processing for specified landing page.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="formData">The form data.</param>
		/// <param name="paramsGenerator">The parameters generator.</param>
		/// <returns>
		/// Processed form data.
		/// </returns>
		public FormData Execute(UserConnection userConnection, FormData formData,
				IWebFormImportParamsGenerator paramsGenerator) {
			UserConnection = userConnection;
			FormData = formData;
			ParamsGenerator = paramsGenerator;
			EditFormData();
			return FormData;
		}

		#endregion

	}

	#endregion

}

