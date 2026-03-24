namespace Creatio.ContentDesigner
{
	/// <summary>
	/// Provides logic to work with BeeFree services
	/// </summary>
	public interface IBfContentServicesAPIClient
	{

		#region Methods: Public

		/// <summary>
		/// Transforms the content config JSON to HTML.
		/// </summary>
		/// <param name="contentConfig">Content config JSON</param>
		/// <param name="designerType">Designer type code</param>
		/// <returns>Content HTML</returns>
		string TransformConfigToHTML(string contentConfig, ContentDesignerType designerType);

		/// <summary>
		/// Transforms a simple JSON schema into a full JSON schema 
		/// that can be easily loaded as a template within the Beefree SDK builder.
		/// </summary>
		/// <param name="contentConfig">Content config of simple JSON</param>
		/// <param name="designerType">Designer type code</param>
		/// <returns>Full JSON</returns>
		string ConvertSimpleToFullJson(string contentConfig, ContentDesignerType designerType);

		#endregion

	}
}
