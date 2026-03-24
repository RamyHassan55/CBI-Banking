namespace CrtContentDesigner.Models
{
	using System.Runtime.Serialization;

	#region Class: TemplateModel

	/// <summary>
	/// Represents a template in the system.
	/// </summary>
	[DataContract]
	public class TemplateModel
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the AMP HTML representation of the page content.
		/// </summary>
		[DataMember(Name = "ampHtml", EmitDefaultValue = false)]
		public string AmpHtml { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the template.
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the language of the template.
		/// </summary>
		[DataMember(Name = "language")]
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the HTML representation of the page content.
		/// </summary>
		[DataMember(Name = "pageHtml")]
		public string PageHtml { get; set; }

		/// <summary>
		/// Gets or sets the JSON representation of the page content.
		/// </summary>
		[DataMember(Name = "pageJson")]
		public string PageJson { get; set; }

		/// <summary>
		/// Gets or sets the version of the template.
		/// </summary>
		[DataMember(Name = "templateVersion")]
		public int TemplateVersion { get; set; }

		#endregion

	}

	#endregion
}