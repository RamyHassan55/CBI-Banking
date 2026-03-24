namespace CrtContentDesigner.Contracts
{
	using System;
	using System.Runtime.Serialization;
	using CrtContentDesigner.Models;

	#region Class: BaseTemplateRequest

	/// <summary>
	/// Represents a request to save a template.
	/// </summary>
	[DataContract]
	public class SaveTemplateRequest : BaseTemplateRequest
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the unique identifier of the record to be saved.
		/// </summary>
		[DataMember(Name = "recordId")]
		public string RecordId { get; set; }

		/// <summary>
		/// Language identifier associated with the template.
		/// </summary>
		[DataMember(Name = "languageId")]
		public string LanguageId { get; set; }

		/// <summary>
		/// Gets or sets the template model.
		/// </summary>
		[DataMember(Name = "content")]
		public TemplateModel Content { get; set; }

		#endregion

	}

	#endregion

}
