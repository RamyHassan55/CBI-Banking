namespace Terrasoft.Configuration
{
	using System;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Web.Common;

	#region Class: InvokableMacrosHelperService

	/// <summary>
	/// Macros helper service which uses <see cref="InvokableMacrosHelper"/>.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class InvokableMacrosHelperService : BaseService
	{

		#region Class: InvokableMacrosContainer

		/// <summary>
		/// Represents container which stores data for macros processing.
		/// </summary>
		protected class InvokableMacrosContainer
		{

			#region Properties: Public

			/// <summary>
			/// Template text.
			/// </summary>
			public string TemplateText { get; set; }
			/// <summary>
			/// Template subject.
			/// </summary>
			public string TemplateSubject { get; set; }
			/// <summary>
			/// Entityr schema name.
			/// </summary>
			public string EntitySchemaName { get; set; }
			/// <summary>
			/// Entity identifier.
			/// </summary>
			public Guid EntityId { get; set; }
			/// <summary>
			/// Language identifier.
			/// </summary>
			public Guid LanguageId { get; set; }
			/// <summary>
			/// Language code.
			/// </summary>
			public string LanguageCode { get; set; }

			#endregion

		}

		#endregion

		#region Constants: Private

		/// <summary>
		/// Email template entity schema name.
		/// </summary>
		private const string EmailTemplateSchemaName = "EmailTemplate";
		private const string CaseSchemaName = "Case";

		#endregion

		#region Properties: Public

		private CaseTimeZoneMacrosConverter _converter;
		/// <summary>
		/// Case time zone converter.
		/// </summary>
		public CaseTimeZoneMacrosConverter TimeZoneConverter {
			get => _converter ?? (_converter = new CaseTimeZoneMacrosConverter(UserConnection, CaseSchemaName));
		}

		#endregion

		#region Methods: Protected

		[Obsolete("Use GetResponseWithHandledMacroses instead.")]
		protected MacrosHelperServiceResponse GetResponseUsingMacrosHelper(string textTemplate, string subject,
			string entitySchemaName, Guid recordId, Guid languageId = default(Guid), string languageCode = null) {
			var response = new MacrosHelperServiceResponse();
			var macrosHelper = new InvokableMacrosHelper { UserConnection = UserConnection };
			MacrosExtendedProperties extendedProperties = null;
			try {
				if (languageId.IsNotEmpty()) {
					extendedProperties = new MacrosExtendedProperties { LanguageId = languageId };
				}
				if (entitySchemaName == CaseSchemaName) {
					textTemplate = TimeZoneConverter.ReplaceDateTimeMacroses(recordId, textTemplate, languageCode);
				}
				textTemplate = macrosHelper.GetTextTemplate(textTemplate, entitySchemaName, recordId,
					extendedProperties);
				string processedSubject = macrosHelper.GetTextTemplate(subject, entitySchemaName, recordId,
					extendedProperties);
				response.TextTemplate = textTemplate;
				response.SubjectTemplate = processedSubject;
				response.Success = true;
			} catch (Exception e) {
				response.Exception = e;
				response.Success = false;
			}
			return response;
		}

		protected MacrosHelperServiceResponse GetResponseWithHandledMacroses(InvokableMacrosContainer container) {
			var response = new MacrosHelperServiceResponse();
			var macrosHelper = new InvokableMacrosHelper { UserConnection = UserConnection };
			MacrosExtendedProperties extendedProperties = null;
			string textTemplate = container.TemplateText;
			if (container.LanguageId.IsNotEmpty()) {
				extendedProperties = new MacrosExtendedProperties { LanguageId = container.LanguageId };
			}
			try {
				if (IsNeedReplaceMacroses(container)) {
					textTemplate = TimeZoneConverter.ReplaceDateTimeMacroses(container.EntityId, textTemplate,
						container.LanguageCode);
				}
				textTemplate = macrosHelper.GetTextTemplate(textTemplate, container.EntitySchemaName,
					container.EntityId, extendedProperties);
				string processedSubject = macrosHelper.GetTextTemplate(container.TemplateSubject,
					container.EntitySchemaName, container.EntityId, extendedProperties);
				response.TextTemplate = textTemplate;
				response.SubjectTemplate = processedSubject;
				response.Success = true;
			} catch (Exception e) {
				response.Exception = e;
				response.Success = false;
			}
			return response;
		}

		protected virtual bool IsNeedReplaceMacroses(InvokableMacrosContainer container) {
			return container.EntitySchemaName == CaseSchemaName;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets processed template text.
		/// </summary>
		/// <param name="entitySchemaName">Entity schema name.</param>
		/// <param name="recordId">Record identifier.</param>
		/// <param name="textTemplate">Text template.</param>
		/// <returns>Processed template text.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public MacrosHelperServiceResponse GetTextTemplate(string entitySchemaName,
			Guid recordId, string textTemplate) {
			return GetResponseWithHandledMacroses(new InvokableMacrosContainer {
				TemplateText = textTemplate,
				TemplateSubject = string.Empty,
				EntitySchemaName = entitySchemaName,
				EntityId = recordId
			});
		}

		/// <summary>
		/// Gets processed multilanguage text from template.
		/// </summary>
		/// <param name="request">Service request.</param>
		/// <returns>Processed template text.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public MacrosHelperServiceResponse GetMultiLanguageTextTemplate(MacrosHelperServiceRequest request) {
			var mLangContentFactory = new MLangContentFactory(UserConnection);
			var contentKit = mLangContentFactory.GetContentKit(request.EntityName, EmailTemplateSchemaName);
			Entity template = contentKit.GetContent(request.TemplateId, request.EntityId);
			var textTemplate = template.GetTypedColumnValue<string>("Body");
			var subjectTemplate = template.GetTypedColumnValue<string>("Subject");
			var container = new InvokableMacrosContainer {
				TemplateText = textTemplate,
				TemplateSubject = subjectTemplate,
				EntitySchemaName = request.EntityName,
				EntityId = request.EntityId,
				LanguageCode = template.GetTypedColumnValue<string>("LanguageCode")
			};
			if (Features.GetIsEnabled("UseMacrosAdditionalParameters")) {
				container.LanguageId = template.GetTypedColumnValue<Guid>("LanguageId");
				return GetResponseWithHandledMacroses(container);
			}
			return GetResponseWithHandledMacroses(container);
		}

		#endregion
	}

	#endregion

}
