namespace CrtEmailDesignerInEmailMarketing.Services
{
	using Terrasoft.Configuration;

	/// <summary>
	/// Provides methods for managing and manipulating hyperlinks in HTML content.
	/// </summary>
	internal interface IBulkEmailHyperlinkCorrector
	{

		#region Methods: Public

		/// <summary>
		/// Adds unique replica and trackId parameters to each hyperlink in the HTML content.
		/// Overrides existing parameters if they are present.
		/// </summary>
		/// <param name="htmlContent">Valid HTML content</param>
		/// <param name="replicaIndex">The replica index to add to the links.</param>
		/// <param name="startTrackId">Track id start point for current content. Incrementally applied to each hyperlink.</param>
		/// <returns>The HTML content with updated links.</returns>
		string ApplyBpmParametersToHyperlinks(string htmlContent, int replicaIndex, int startTrackId);

		/// <summary>
		/// Extracts all unique hyperlinks from the HTML content.
		/// </summary>
		/// <param name="htmlContent">The HTML content containing the links.</param>
		/// <returns>A list of unique hyperlinks.</returns>
		HyperlinkData[] ExtractHyperlinks(string htmlContent);

		#endregion

	}
}