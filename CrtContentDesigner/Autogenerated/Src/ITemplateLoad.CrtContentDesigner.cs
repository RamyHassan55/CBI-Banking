namespace CrtContentDesigner
{
	using CrtContentDesigner.Contracts;
	using CrtContentDesigner.Models;

	/// <summary>
	/// Defines the methods required to load an email templates.
	/// </summary>
	public interface ITemplateLoad : ITemplateHandler
	{

		#region Methods: Public

		/// <summary>
		/// Loads the email template with the specified load request details.
		/// </summary>
		/// <param name="loadRequest">The details of the load request.</param>
		/// <param name="emailTemplate">The email template to process.</param>
		TemplateModel LoadTemplate(LoadTemplateRequest loadRequest, TemplateModel template);

		#endregion

	}
}
