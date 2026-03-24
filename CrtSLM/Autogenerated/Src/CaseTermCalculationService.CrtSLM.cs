namespace Terrasoft.Configuration.Calendars
{
	using Core;
	using Core.Factories;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using TermCalculationService;
	using Terrasoft.Core.Store;
	using Terrasoft.Web.Common;

	#region Class : CaseTermCalculationService

	/// <summary>
	/// A class-service for case term calculation.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class CaseTermCalculationService : BaseService
	{

		#region Class: ServiceTermStringResponse

		[Serializable]
		[DataContract]
		protected class ServiceTermStringResponse
		{
			#region Properties: Public

			[DataMember(Name = "ReactionTime")]
			public string ReactionTime {
				get;
				set;
			}

			[DataMember(Name = "SolutionTime")]
			public string SolutionTime {
				get;
				set;
			}

			#endregion

		}

		#endregion

		#region Fields : Private

		private static readonly string _dateFormat = "yyyy'-'MM'-'dd HH':'mm':'ss";

		#endregion

		#region Methods: Private

		/// <summary>
		/// Replaces strings in given dictionary by guids.
		/// </summary>
		/// <param name="oldDictionary">Initial dictionary.</param>
		/// <returns>Dictionary with guids.</returns>
		private Dictionary<string, object> GetDictionaryWithGuids(Dictionary<string, object> oldDictionary) {
			var resultDictionary = new Dictionary<string, object>();
			foreach (var element in oldDictionary) {
				Guid guid;
				var value = Guid.TryParse(element.Value.ToString(), out guid) ? guid : element.Value;
				resultDictionary.Add(element.Key, value);
			}
			return resultDictionary;
		}

		/// <summary>
		/// Returns case parameters needed by term calculation.
		/// </summary>
		/// <returns>Calculation parameters.</returns>
		private Dictionary<string, object> GetTermCalculationParams(IEnumerable<CaseCalculationParameterValue> calculationParameters) {
			return
				calculationParameters
				.Select(item => new KeyValuePair<string, object>(item.Column, item.Value))
				.ToDictionary(item => item.Key, item => item.Value);
		}

		private ServiceTermResponse InternalCalculateTerms(Dictionary<string, object> arguments, string registrationDate) {
			Dictionary<string, object> featuredDictionary = GetDictionaryWithGuids(arguments);
			var parsedRegistrationDate = DateTime.Parse(registrationDate);
			var userConnectionArg = new ConstructorArgument("userConnection", UserConnection);
			var entryPoint = ClassFactory.Get<CaseTermCalculateEntryPoint>(userConnectionArg);
			return entryPoint.CalculateTerms(featuredDictionary, parsedRegistrationDate);
		}

		private TermCalculationLogStore InitializeTermCalculationLogStore(CaseCalculationLoggerRequest arguments, CaseTermStates caseTermState) {
			var logStore = new TermCalculationLogStore(arguments, caseTermState, UserConnection.CurrentUser.Culture);
			TermCalculationLogStoreInitializer.InitializeLogStore(UserConnection, logStore);
			return logStore;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Counts reaction time and a solution time to Case.
		/// </summary>
		/// <param name="arguments">Json-formatted string conditions for terms counting.</param>
		/// <param name="registrationDate">Json-formatted string of case registration date.</param>
		/// <returns>An object that contains the reaction time and solution time.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public string CalculateTerms(Dictionary<string, object> arguments, string registrationDate) {
			var stringResponse = new ServiceTermStringResponse();
			try {
				ServiceTermResponse response = InternalCalculateTerms(arguments, registrationDate);
				if (response.ReactionTime != null) {
					stringResponse.ReactionTime = response.ReactionTime.Value.ToString(_dateFormat);
				}
				if (response.SolutionTime != null) {
					stringResponse.SolutionTime = response.SolutionTime.Value.ToString(_dateFormat);
				}
			} catch (Exception e) {
				return e.Message;
			}
			return JsonConvert.SerializeObject(stringResponse);
		}

		/// <summary>
		/// Logs information used by calculation of reaction time.
		/// </summary>
		/// <param name="request">Service request.</param>
		/// <returns>Logged calculation info.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
			ResponseFormat = WebMessageFormat.Json)]
		public TermCalculationLogStore LogCalculateTermsReactionTime(CaseCalculationLoggerRequest request) {
			TermCalculationLogStore logStore = InitializeTermCalculationLogStore(request, CaseTermStates.ContainsResponse);
			Dictionary<string, object> calculationParameters = GetTermCalculationParams(request.Parameters);
			calculationParameters.Add("CheckCaseResponseOverdue", true);
			ServiceTermResponse calculationResult = InternalCalculateTerms(calculationParameters, request.RegistrationDate);
			logStore.CalculatedTerm = calculationResult.ReactionTime != null ? calculationResult.ReactionTime.ToString() : DateTime.MinValue.ToString();
			TermCalculationLogStoreInitializer.ResetLogStore(UserConnection);
			return logStore;
		}

		/// <summary>
		/// Logs information used by calculation of solution time.
		/// </summary>
		/// <param name="request">Service request.</param>
		/// <returns>Logged calculation info.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
			ResponseFormat = WebMessageFormat.Json)]
		public TermCalculationLogStore LogCalculateTermsSolutionTime(CaseCalculationLoggerRequest request) {
			TermCalculationLogStore logStore = InitializeTermCalculationLogStore(request, CaseTermStates.ContainsResolve);
			Dictionary<string, object> calculationParameters = GetTermCalculationParams(request.Parameters);
			calculationParameters.Add("CheckCaseSolutionOverdue", true);
			ServiceTermResponse calculationResult = InternalCalculateTerms(calculationParameters, request.RegistrationDate);
			logStore.CalculatedTerm = calculationResult.SolutionTime != null ? calculationResult.SolutionTime.ToString() : DateTime.MinValue.ToString();
			TermCalculationLogStoreInitializer.ResetLogStore(UserConnection);
			return logStore;
		}

		#endregion

	}

	#endregion

}
