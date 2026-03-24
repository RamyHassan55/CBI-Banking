namespace CrtContentDesigner
{
	using CrtContentDesigner.Contracts;

	/// <summary>
	/// Defines the methods required to save an email templates.
	/// </summary>
	public interface ITemplateSave : ITemplateHandler
	{

		#region Methods: Public

		/// <summary>
		/// Saves the email template with the specified save request details.
		/// </summary>
		/// <param name="saveRequest">The details of the save request.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		bool SaveTemplate(SaveTemplateRequest saveRequest);

		#endregion

	}
}
