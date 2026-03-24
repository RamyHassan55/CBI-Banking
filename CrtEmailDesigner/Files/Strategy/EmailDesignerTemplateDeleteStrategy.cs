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

	#region Class: EmailDesignerTemplateDeleteStrategy

	/// <summary>
	/// Default strategy to delete email templates in CrtEmailDesigner.
	/// </summary>
	[DefaultBinding(typeof(IEmailTemplateDeleteStrategy))]
	internal class EmailDesignerTemplateDeleteStrategy : IEmailTemplateDeleteStrategy
	{

		#region Fields: Private

		private readonly IBfEmailTemplateRepository _bfTemplateRepository;
		private readonly ILog _logger;

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		public EmailDesignerTemplateDeleteStrategy(UserConnection userConnection) {
			_userConnection = userConnection;
			_bfTemplateRepository =
				ClassFactory.Get<IBfEmailTemplateRepository>(new ConstructorArgument("userConnection", userConnection));
			_logger = LogManager.GetLogger(nameof(EmailDesignerTemplateDeleteStrategy));
		}

		#endregion

		#region Methods: Private

		private void OnPostDelete(IList<EmailDesignerTemplateModel> models, IList<string> handlerCodes) {
			if (!models.Any() || !handlerCodes.Any()) {
				return;
			}
			var handlers = new List<IEmailTemplatePostDelete>();
			foreach (string handlerCode in handlerCodes.Distinct()) {
				if (ClassFactory.TryGet(handlerCode, out IEmailTemplatePostDelete handler,
						new ConstructorArgument("userConnection", _userConnection))) {
					handlers.Add(handler);
				} else {
					_logger.Warn($"Could not find handler {nameof(IEmailTemplatePostDelete)} for code: {handlerCode}");
				}
			}
			if (!handlers.Any()) {
				return;
			}
			foreach (EmailDesignerTemplateModel model in models) {
				foreach (IEmailTemplatePostDelete handler in handlers) {
					handler.Execute(model);
				}
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IEmailTemplateDeleteStrategy.DeleteMany"/>
		public IList<EmailDesignerTemplateModel> DeleteMany(Guid emailId, Guid[] languageIds, IList<string> handlerCodes) {
			var result = new List<EmailDesignerTemplateModel>();
			var templateIds = _bfTemplateRepository.FetchAllByEmailId(emailId)
				.Where(x => languageIds.Contains(x.LanguageId)).Select(x => Guid.Parse(x.Id)).ToArray();
			if (!templateIds.Any()) {
				return result;
			}
			try {
				var deletedModels = _bfTemplateRepository.DeleteMany(templateIds);
				result.AddRange(deletedModels);
				OnPostDelete(result, handlerCodes);
				return result;
			} catch (Exception exception) {
				_logger.Error($"Error deleting email templates for emailId: {emailId}", exception);
				throw;
			}
		}

		#endregion

	}

	#endregion

}
