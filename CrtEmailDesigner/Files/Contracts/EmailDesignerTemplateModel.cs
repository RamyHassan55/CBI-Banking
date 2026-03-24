namespace CrtEmailDesigner.Contracts
{
	using System;
	using System.Runtime.Serialization;
	using CrtContentDesigner.Models;

	#region Class: EmailDesignerTemplateModel

	/// <summary>
	/// Represents a resolved email template returned from <see cref="EmailBuilderTemplateService"/>.
	/// </summary>
	[DataContract]
	public class EmailDesignerTemplateModel : TemplateModel
	{

		#region Properties: Public

		/// <summary>
		/// Email ID
		/// </summary>
		[DataMember(Name = "isDefault")]
		public bool IsDefault { get; set; }
		
		/// <summary>
		/// Email ID
		/// </summary>
		[DataMember(Name = "emailId")]
		public Guid EmailId { get; set; }

		/// <summary>
		/// Language ID
		/// </summary>
		[DataMember(Name = "languageId")]
		public Guid LanguageId { get; set; }

		/// <summary>
		/// Language code (related to the language ID)
		/// </summary>
		[DataMember(Name = "languageCode")]
		public string LanguageCode { get; set; }

		#endregion

	}

	#endregion

}
