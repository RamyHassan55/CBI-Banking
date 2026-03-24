namespace CrtEmailDesignerInEmailMarketing.Strategy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.DynamicContent;
	using Terrasoft.Configuration.DynamicContent.Models;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: EmailMarketingTemplatePostDelete

    /// <summary>
    /// Bulk email template delete strategy.
    /// Extends the default email template delete strategy to delete dynamic content templates
    /// after deleting the email template.
    /// </summary>
    [DefaultBinding(typeof(IEmailTemplatePostDelete), Name = "BulkEmail")]
	internal class EmailMarketingTemplatePostDelete : IEmailTemplatePostDelete
	{

		#region Fields: Private

		private readonly BulkEmailHyperlinkRepository _bulkEmailHyperlinkRepository;

		private readonly DCTemplateRepository<DCTemplateModel> _dcTemplateRepository;

		#endregion

		#region Constructors: Public

		public EmailMarketingTemplatePostDelete(UserConnection userConnection) {
			_bulkEmailHyperlinkRepository = ClassFactory.Get<BulkEmailHyperlinkRepository>(
				new ConstructorArgument("userConnection", userConnection));
			_dcTemplateRepository = new DCTemplateRepository<DCTemplateModel>(userConnection);
		}

		#endregion

		#region Methods: Private

		private static DCRepositoryReadOptions<DCTemplateModel, DCReplicaModel> GetDcTemplateReadOptions() {
			return new DCRepositoryReadOptions<DCTemplateModel, DCReplicaModel> {
				TemplateReadOptions = DCTemplateReadOption.ExcludeReplicaHtmlContent
			};
		}

		private DCTemplateModel FetchDcTemplateForRecordAndLanguage(Guid recordId, Guid languageId) {
			IEnumerable<DCTemplateModel> dcTemplates =
				_dcTemplateRepository.GetDcTemplateModels(recordId, GetDcTemplateReadOptions()).ToList();
			return dcTemplates.FirstOrDefault(x =>
				x.LanguageId.HasValue && x.LanguageId != Guid.Empty && x.LanguageId.Value == languageId);
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IEmailTemplatePostDelete.Execute"/>
		public void Execute(EmailDesignerTemplateModel model) {
			DCTemplateModel dcTemplate = FetchDcTemplateForRecordAndLanguage(model.EmailId, model.LanguageId);
			if (dcTemplate == null) {
				return;
			}
			bool dcTemplateDeleted = _dcTemplateRepository.Delete(dcTemplate.Id);
			if (dcTemplateDeleted) {
				_bulkEmailHyperlinkRepository.ClearDynamicTemplateHyperlinks(dcTemplate.RecordId, dcTemplate.Id);
			}
		}

		#endregion

	}

	#endregion

}
