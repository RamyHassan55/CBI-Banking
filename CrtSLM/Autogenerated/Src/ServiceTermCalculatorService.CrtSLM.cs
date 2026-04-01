namespace Terrasoft.Configuration.Calendars
{
	using Terrasoft.Configuration.ServiceTerm;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.ServiceModel.Web;
	using System.ServiceModel.Activation;
	using Terrasoft.Configuration.TermCalculationService;
	using Terrasoft.Web.Common;

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class ServiceTermCalculatorService : BaseService
	{

		#region Methods: Public

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		[return: MessageParameter(Name = "TermResponse")]
		public ServiceTermResponse CalculateTerms(Dictionary<string, object> conditions) {
			var calculator = new ServiceTermCalculator(conditions, UserConnection);
			var responseDate = calculator.CalculateResponseDate();
			var solutionDate = calculator.CalculateSolutionDate();
			return new ServiceTermResponse {
				ReactionTime = responseDate,
				SolutionTime = solutionDate
			};
		}
		
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		[return: MessageParameter(Name = "TermResponse")]
		public ServiceTermResponse CalculateSolutionDateAfterPause(Dictionary<string, object> conditions) {
			var calculator = new ServiceTermCalculator(conditions, UserConnection);
			var solutionDate = calculator.CalculateSolutionDate();
			return new ServiceTermResponse {
				SolutionTime = solutionDate
			};
		}

		#endregion
	}
}
