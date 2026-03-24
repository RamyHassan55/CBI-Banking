namespace CrtEmailDesigner.Strategy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: EmailDesignerTemplateCreateStrategy

	[DefaultBinding(typeof(IEmailTemplateCreateStrategy))]
	public class EmailDesignerTemplateCreateStrategy : IEmailTemplateCreateStrategy
	{

		#region Fields: Private

		private readonly IBfTemplateLanguageService _bfTemplateLanguageService;
		private readonly IBfEmailTemplateRepository _bfTemplateRepository;
		private readonly ILog _logger;

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		public EmailDesignerTemplateCreateStrategy(UserConnection userConnection) {
			_userConnection = userConnection;
			_bfTemplateRepository = ClassFactory.Get<IBfEmailTemplateRepository>(
				new ConstructorArgument("userConnection", userConnection));
			_bfTemplateLanguageService = ClassFactory.Get<IBfTemplateLanguageService>(
				new ConstructorArgument("userConnection", userConnection));
			_logger = LogManager.GetLogger(nameof(EmailDesignerTemplateCreateStrategy));
		}

		#endregion

		#region Methods: Private

		private void OnPostCreate(IList<EmailDesignerTemplateModel> models, IList<string> handlerCodes) {
			if (!models.Any() || !handlerCodes.Any()) {
				return;
			}
			var handlers = new List<IEmailTemplatePostCreate>();
			foreach (string handlerCode in handlerCodes.Distinct()) {
				if (ClassFactory.TryGet(handlerCode, out IEmailTemplatePostCreate handler,
						new ConstructorArgument("userConnection", _userConnection))) {
					handlers.Add(handler);
				} else {
					_logger.Warn($"Could not find handler {nameof(IEmailTemplatePostCreate)} for code: {handlerCode}");
				}
			}
			if (!handlers.Any()) {
				return;
			}
			foreach (EmailDesignerTemplateModel model in models) {
				foreach (IEmailTemplatePostCreate handler in handlers) {
					handler.Execute(model);
				}
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IEmailTemplateCreateStrategy.CreateMany"/>
		public IList<EmailDesignerTemplateModel> CreateMany(Guid emailId, Guid[] languageIds, IList<string> handlerCodes) {
			if (!languageIds.Any()) {
				return new List<EmailDesignerTemplateModel>();
			}
			EmailDesignerTemplateModel defaultTemplate = _bfTemplateLanguageService.FetchDefaultTemplate(emailId);
			var modelsToCreate = languageIds.Select(languageId => new EmailDesignerTemplateModel {
				EmailId = emailId,
				LanguageId = languageId,
				AmpHtml = defaultTemplate.AmpHtml,
				PageJson = defaultTemplate.PageJson,
				PageHtml = defaultTemplate.PageHtml
			}).ToList();
			if (!modelsToCreate.Any()) {
				return modelsToCreate;
			}
			try {
				var result = _bfTemplateRepository.CreateMany(modelsToCreate);
				OnPostCreate(result, handlerCodes);
				return result;
			} catch (Exception exception) {
				_logger.Error($"Error while creating email templates for emailId: {emailId}", exception);
				throw;
			}
		}

		/// <summary>
		/// Executes the strategy to create default email template.
		/// </summary>
		/// <param name="emailId"></param>
		/// <param name="handlerCodes"></param>
		/// <returns></returns>
		public EmailDesignerTemplateModel CreateDefault(Guid emailId, IList<string> handlerCodes) {
			Guid defaultLanguageId = _bfTemplateLanguageService.GetDefaultLanguageId();
			var model = new EmailDesignerTemplateModel {
				EmailId = emailId,
				LanguageId = defaultLanguageId,
				IsDefault = true
			};
			try {
				EmailDesignerTemplateModel result = _bfTemplateRepository.Create(model);
				OnPostCreate(new[] { result }, handlerCodes);
				return result;
			} catch (Exception exception) {
				_logger.Error($"Error while creating email templates for emailId: {emailId}", exception);
				throw;
			}
		}

		#endregion

	}

	#endregion

}
