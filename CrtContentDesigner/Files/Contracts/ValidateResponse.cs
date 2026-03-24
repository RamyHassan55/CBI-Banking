namespace CrtContentDesigner.Contracts
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	/// <summary>
	/// Response for the ValidateTemplates method.
	/// </summary>
	[DataContract]
	public class ValidateResponse
	{
		/// <summary>
		/// Gets or sets the validation result message.
		/// </summary>
		[DataMember(Name = "isTemplateValid")]
		public bool IsTemplateValid { get; set; }

		/// <summary>
		/// Gets or sets the list of template languages used in the templates.
		/// </summary>
		[DataMember(Name = "templateLanguages")]
		public List<string> TemplateLanguages { get; set; } = new List<string>();
	}
}
