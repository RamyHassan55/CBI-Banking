namespace CrtEmailDesignerInEmailMarketing
{
	using CrtContentDesigner.Contracts;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using System.Text.RegularExpressions;
	using Terrasoft.Configuration;
	using Terrasoft.Core.Entities;
	using Terrasoft.Web.Common;

	#region Class: BfDynamicContentService

	/// <summary>
	/// Service for validating templates in bulk email.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class BfDynamicContentService : BaseService
	{

		#region Methods: Private

		private void ChangeConditionInTemplates(EditConditionRequest validateRequest) {
			EntityCollection templates = GetTemplates(validateRequest.EmailId);
			foreach (Entity template in templates) {
				foreach (DisplayConditionRequest condition in validateRequest.DisplayConditions) {
					var previousPageJson = template.GetTypedColumnValue<string>("PageJson");
					string updatedPageJson = BFTemplateTransformerService.ReplaceAllLabelsAndDescriptionsByConditionId(
						previousPageJson, condition.ConditionId, condition.ConditionName,
						condition.ConditionDescription);
					template.SetColumnValue("PageJson", updatedPageJson);
					template.Save();
				}
			}
		}

		private EntityCollection GetTemplates(string bulkEmailId) {
			var displayConditionQuery = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "BfEmailTemplate") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			displayConditionQuery.Filters.Add(
				displayConditionQuery.CreateFilterWithParameters(FilterComparisonType.Equal, "EmailId", bulkEmailId));
			displayConditionQuery.AddColumn("PageHtml");
			displayConditionQuery.AddColumn("PageJson");
			displayConditionQuery.AddColumn("TemplateLanguage.Code");
			return displayConditionQuery.GetEntityCollection(UserConnection);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Edits display conditions for templates in a bulk email.
		/// </summary>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public EditConditionResponse EditDisplayConditions(EditConditionRequest editConditionRequest) {
			if (editConditionRequest.DisplayConditions.Any()) {
				ChangeConditionInTemplates(editConditionRequest);
			}
			return new EditConditionResponse {
				IsEditSuccess = true
			};
		}

		/// <summary>
		/// Validates templates in a bulk email against specified record IDs.
		/// </summary>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public ValidateResponse ValidateTemplates(ValidateDeleteConditionRequest validateDeleteConditionRequest) {
			EntityCollection templates = GetTemplates(validateDeleteConditionRequest.BulkEmailId);
			var invalidTemplates = templates
				.Where(template => validateDeleteConditionRequest.BfDisplayConditionsIds
					.Any(id => template.GetTypedColumnValue<string>("PageHtml")?.Contains(id) == true))
				.ToList();
			var invalidTemplateLanguages = invalidTemplates
				.Select(template => template.GetTypedColumnValue<string>("TemplateLanguage_Code"))
				.Where(lang => !string.IsNullOrEmpty(lang))
				.Distinct()
				.ToList();
			bool isValid = !invalidTemplates.Any();

			return new ValidateResponse {
				IsTemplateValid = isValid,
				TemplateLanguages = invalidTemplateLanguages
			};
		}

		#endregion

	}

	#endregion

}

