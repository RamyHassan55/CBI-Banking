namespace CrtEmailDesigner.Services
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Common.Logging;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.ServiceModelContract;
	using Terrasoft.Web.Common;

	#region Class: EmailDesignerTemplateService

	/// <summary>
	/// Service for managing email designer templates.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class EmailDesignerTemplateService : BaseService
	{

		#region Fields: Private

		private static readonly ConcurrentDictionary<Guid, object> _emailLocks = new ConcurrentDictionary<Guid, object>();
		private IBfEmailTemplateRepository _bfEmailTemplateRepository;
		private IBfTemplateLanguageService _bfTemplateLanguageService;

		private IEmailTemplateCreateStrategy _createStrategy;
		private IEmailTemplateDeleteStrategy _deleteStrategy;

		private ILog _logger;

		#endregion

		#region Properties: Private

		private IEmailTemplateCreateStrategy CreateStrategy =>
			_createStrategy ?? (_createStrategy =
				ClassFactory.Get<IEmailTemplateCreateStrategy>(
					new ConstructorArgument("userConnection", UserConnection)));

		private IEmailTemplateDeleteStrategy DeleteStrategy =>
			_deleteStrategy ?? (_deleteStrategy =
				ClassFactory.Get<IEmailTemplateDeleteStrategy>(
					new ConstructorArgument("userConnection", UserConnection)));

		private IBfEmailTemplateRepository BfEmailTemplateRepository =>
			_bfEmailTemplateRepository ?? (_bfEmailTemplateRepository =
				ClassFactory.Get<IBfEmailTemplateRepository>(new ConstructorArgument("userConnection",
					UserConnection)));

		private IBfTemplateLanguageService BfTemplateLanguageService =>
			_bfTemplateLanguageService ?? (_bfTemplateLanguageService =
				ClassFactory.Get<IBfTemplateLanguageService>(new ConstructorArgument("userConnection",
					UserConnection)));

		private ILog Logger => _logger ?? (_logger = LogManager.GetLogger(nameof(EmailDesignerTemplateService)));

		#endregion

		#region Methods: Private

		private SaveEmailLanguageTemplateResponse PrepareSaveLanguageTemplatesDryRunResponse(
			SaveEmailLanguageTemplateRequest request) {
			var existingTemplates = BfEmailTemplateRepository.FetchAllByEmailId(request.EmailId);
			var templatesToDelete = existingTemplates.Where(x => !request.LanguageIds.Contains(x.LanguageId)).ToList();
			var languageIdsToCreate = request.LanguageIds
				.Where(x => existingTemplates.All(y => y.LanguageId != x)).ToArray();
			var templatesToCreate = BfTemplateLanguageService.FetchLanguageCodes(languageIdsToCreate).Select(x =>
				new EmailDesignerTemplateModel {
					LanguageId = x.Item1,
					LanguageCode = x.Item2
				}).ToList();
			return new SaveEmailLanguageTemplateResponse {
				TemplatesToDelete = templatesToDelete,
				TemplatesToCreate = templatesToCreate
			};
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets all templates for the specified email.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public IEnumerable<EmailDesignerTemplateModel> GetAllTemplates(EmailDesignerTemplateRequest request) {
			return BfEmailTemplateRepository.FetchAllByEmailId(request.EmailId);
		}

		/// <summary>
		/// Gets or creates the default template for the specified email.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public EmailDesignerTemplateModel GetOrCreateDefaultTemplate(EmailDesignerTemplateRequest request) {
			BfTemplateLanguageService.UpdateLegacyTemplateIfNeeded(request.EmailId);
			var resolvedTemplates = BfEmailTemplateRepository.FetchAllByEmailId(request.EmailId);
			EmailDesignerTemplateModel defaultTemplate = resolvedTemplates.FirstOrDefault(x => x.IsDefault) ??
				CreateStrategy.CreateDefault(request.EmailId, request.HandlerCodes);
			return defaultTemplate;
		}

		/// <summary>
		/// Saves and removes the language templates for the specified email based on provided language IDs.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
			ResponseFormat = WebMessageFormat.Json)]
		public SaveEmailLanguageTemplateResponse SaveLanguageTemplates(SaveEmailLanguageTemplateRequest request) {
			if (request.DryRun) {
				return PrepareSaveLanguageTemplatesDryRunResponse(request);
			}
			var lockObj = _emailLocks.GetOrAdd(request.EmailId, _ => new object());
			lock (lockObj) {
				SaveEmailLanguageTemplateResponse response = PrepareSaveLanguageTemplatesDryRunResponse(request);
				try {
					response.TemplatesToCreate = CreateStrategy.CreateMany(request.EmailId,
						response.TemplatesToCreate.Select(x => x.LanguageId).ToArray(), request.HandlerCodes.ToArray());
					DeleteStrategy.DeleteMany(request.EmailId,
						response.TemplatesToDelete.Select(x => x.LanguageId).ToArray(), request.HandlerCodes.ToArray());
					return response;
				} catch (Exception exception) {
					Logger.Error($"Failed to save languages for Record: {request.EmailId}", exception);
					return new SaveEmailLanguageTemplateResponse {
						Success = false,
						ErrorInfo = new ErrorInfo {
							Message = $"Failed to save language templates: {exception.Message}"
						}
					};
				} finally {
					_emailLocks.TryRemove(request.EmailId, out _);
				}
			}
		}

		#endregion

	}

	#endregion

}
