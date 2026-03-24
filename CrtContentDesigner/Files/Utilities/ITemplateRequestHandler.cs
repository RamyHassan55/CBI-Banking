namespace CrtContentDesigner.Utilities
{
	using CrtContentDesigner.Contracts;

	/// <summary>
	/// Provides a handler for the handling requests from a designer.
	/// </summary>
	public interface ITemplateRequestHandler
	{

		#region Methods: Public

		/// <summary>
		/// Loads the template.
		/// </summary>
		/// <param name="loadRequest">The load request containing the template ID.</param>
		/// <returns>The loaded template.</returns>
		LoadTemplateResponse LoadTemplate(LoadTemplateRequest loadRequest);

		/// <summary>
		/// Saves the template.
		/// </summary>
		/// <param name="templateRequest">The save request containing the template details.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		SaveTemplateResponse SaveTemplate(SaveTemplateRequest templateRequest);

		#endregion

	}
}
