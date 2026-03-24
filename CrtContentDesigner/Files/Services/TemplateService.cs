namespace CrtContentDesigner.Services
{
	using System.ServiceModel.Activation;
	using System.ServiceModel;
	using Terrasoft.Web.Common;
	using System.ServiceModel.Web;
	using CrtContentDesigner.Utilities;
	using CrtContentDesigner.Contracts;
	using Terrasoft.Core.Factories;

	#region Class: TemplateService
	/// <summary>
	/// Provides services for managing templates.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class TemplateService : BaseService
	{

		#region Fields: Private

		private ITemplateRequestHandler _requestHandler;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the handlers repository.
		/// </summary>
		internal ITemplateRequestHandler RequestHandler {
			get =>
				_requestHandler ?? (_requestHandler = ClassFactory.Get<ITemplateRequestHandler>(new ConstructorArgument("userConnection", UserConnection)));
			set => _requestHandler = value;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Loads the template.
		/// </summary>
		/// <param name="templateRequest">The load request containing the template ID.</param>
		/// <returns>The loaded template.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public LoadTemplateResponse TemplateLoad(LoadTemplateRequest templateRequest) {
			return RequestHandler.LoadTemplate(templateRequest);
		}

		/// <summary>
		/// Saves the template.
		/// </summary>
		/// <param name="templateRequest">The save request containing the template details.</param>
		/// <returns><see cref="SaveTemplateResponse"/></returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public SaveTemplateResponse TemplateSave(SaveTemplateRequest templateRequest) {
			return RequestHandler.SaveTemplate(templateRequest);
		}

		#endregion

	}

	#endregion
}