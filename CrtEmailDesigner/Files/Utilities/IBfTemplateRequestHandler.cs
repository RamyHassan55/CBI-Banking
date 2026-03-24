namespace CrtEmailDesigner.Utilities
{
	using CrtEmailDesigner.Contracts;
	using System;

	/// <summary>
	/// Provides a handler for the handling requests from a designer.
	/// </summary>
	[Obsolete("Use ITemplateRequestHandler instead.")]
	public interface IBfTemplateRequestHandler
	{

		#region Methods: Public

		/// <summary>
		/// Loads the Bf email template.
		/// </summary>
		/// <param name="loadRequest">The load request containing the template ID.</param>
		/// <returns>The loaded Bf email template.</returns>
		BfEmailTemplateResponse LoadTemplate(LoadTemplateRequest loadRequest);

		/// <summary>
		/// Saves the template.
		/// </summary>
		/// <param name="templateRequest">The save request containing the template details.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		BaseEmailTemplateResponse SaveTemplate(SaveBfEmailTemplateRequest templateRequest);

		#endregion

	}
}