namespace CrtEmailDesigner.Services
{
	using CrtEmailDesigner.Utilities;
	using System.ServiceModel.Activation;
	using System.ServiceModel;
	using Terrasoft.Web.Common;
	using System.ServiceModel.Web;
	using CrtEmailDesigner.Contracts;
	using CrtContentDesigner.Utilities;
	using System;
	using CrtContentDesigner.Contracts;
	using CrtEmailDesigner.Models;
	using LoadTemplateRequestOld = Contracts.LoadTemplateRequest;
	using LoadTemplateRequest = CrtContentDesigner.Contracts.LoadTemplateRequest;

	#region Class: BfTemplateService
	/// <summary>
	/// Provides services for managing Bf email templates.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[Obsolete("Use TemplateSave instead.")]
	public class BfTemplateService : BaseService
	{

		#region Fields: Private

		private ITemplateRequestHandler _bfTemplateRequestHandler;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the Bf template repository.
		/// </summary>
		internal ITemplateRequestHandler BfTemplateRequestHandler {
			get =>
				_bfTemplateRequestHandler ?? (_bfTemplateRequestHandler = new TemplateRequestHandler(UserConnection));
			set => _bfTemplateRequestHandler = value;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Saves the Bf email template.
		/// </summary>
		/// <param name="templateRequest">The save request containing the template details.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		[Obsolete("Use CrtContentDesigner.TemplateService.TemplateSave instead.")]
		public BaseEmailTemplateResponse SaveTemplate(SaveBfEmailTemplateRequest templateRequest) {
			var response = BfTemplateRequestHandler.SaveTemplate(templateRequest);
			return new BaseEmailTemplateResponse {
				Error = response.Error,
				Success = response.Success
			};
		}

		/// <summary>
		/// Loads the Bf email template.
		/// </summary>
		/// <param name="templateRequest">The load request containing the template ID.</param>
		/// <returns>The loaded Bf email template.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		[Obsolete("Use CrtContentDesigner.TemplateService.TemplateLoad instead.")]
		public BfEmailTemplateResponse LoadTemplate(LoadTemplateRequestOld templateRequest) {
			var response = BfTemplateRequestHandler.LoadTemplate(templateRequest);
			return new BfEmailTemplateResponse {
				BfEmailTemplate = new BfEmailTemplateModel(response.Content),
				Error = response.Error,
				Success = response.Success
			};
		}

		#endregion

	}

	#endregion
}