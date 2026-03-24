namespace CrtEmailDesignerInEmailMarketing.Strategy
{
	using CrtContentDesigner;
	using CrtContentDesigner.Contracts;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	[DefaultBinding(typeof(IEmailTemplatePostCreate), Name = "BulkEmail")]
	public class EmailMarketingTemplatePostCreate : IEmailTemplatePostCreate
	{
		private readonly ITemplateSave _templateSave;

		public EmailMarketingTemplatePostCreate(UserConnection userConnection) {
			_templateSave = ClassFactory.Get<ITemplateSave>("DCTemplate",
				new ConstructorArgument("userConnection", userConnection));
		}
		
		/// <summary>
		/// Executes the post create action for email template.
		/// </summary>
		/// <param name="model"></param>
		public void Execute(EmailDesignerTemplateModel model) {
			_templateSave.SaveTemplate(new SaveTemplateRequest {
				RecordId = model.EmailId.ToString(),
				LanguageId = model.LanguageId.ToString(),
				Content = model
			});
		}
	}
}
