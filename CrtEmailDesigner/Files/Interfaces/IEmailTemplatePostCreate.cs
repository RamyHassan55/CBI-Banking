namespace CrtEmailDesigner.Interfaces
{
	using CrtEmailDesigner.Contracts;

	/// <summary>
	/// Post create action for email template, executed in corresponding strategy.
	/// </summary>
	/// <remarks>
	/// To use is properly:
	/// 1. Implement this interface in a class.
	/// 2. Register the implementation in the DI container with given name
	/// [DefaultBinding(typeof(IEmailTemplatePostCreate), Name = "handler_code")]
	/// 3. Pass the handler code to the strategy <see cref="IEmailTemplateCreateStrategy"/>
	/// </remarks>
	public interface IEmailTemplatePostCreate
	{
		/// <summary>
		/// Executes the post create action for email template.
		/// </summary>
		/// <param name="model"></param>
		void Execute(EmailDesignerTemplateModel model);
	}
}
