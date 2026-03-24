namespace CrtEmailDesigner.Interfaces
{
	using CrtEmailDesigner.Contracts;

	/// <summary>
	/// Post delete action for email template, executed in corresponding strategy.
	/// </summary>
	/// <remarks>
	/// To use is properly:
	/// 1. Implement this interface in a class.
	/// 2. Register the implementation in the DI container with given name
	/// [DefaultBinding(typeof(IEmailTemplatePostDelete), Name = "handler_code")]
	/// 3. Pass the handler code to the strategy <see cref="IEmailTemplateDeleteStrategy"/>
	/// </remarks>
	public interface IEmailTemplatePostDelete
	{
		/// <summary>
		/// Executes the post create action for email template.
		/// </summary>
		/// <param name="model"></param>
		void Execute(EmailDesignerTemplateModel model);
	}
}
