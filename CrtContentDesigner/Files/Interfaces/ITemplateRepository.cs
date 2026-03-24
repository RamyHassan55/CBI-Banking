namespace CrtContentDesigner.Repositories
{
	using CrtContentDesigner.Models;
	using System;

	/// <summary>
	/// Provides repository logic for Template.
	/// There's no reason to use this interface in any implementation.
	/// </summary>
	internal interface ITemplateRepository
	{

		#region Methods: Public

		/// <summary>
		/// Loads the email template with the specified load request details.
		/// </summary>
		/// <param name="templateId">The id of a template.</param> 
		TemplateModel LoadTemplate(Guid templateId);

		/// <summary>
		/// Saves the email template with the specified save request details.
		/// </summary>
		/// <param name="saveRequest">The details of the save request.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		bool SaveTemplate(TemplateModel saveRequest);

		#endregion

	}
}