namespace CrtEmailDesigner.Models
{
	using CrtContentDesigner.Models;
	using System;
	using System.Runtime.Serialization;

	#region Class: BfEmailTemplateModel

	/// <summary>
	/// Represents a bf email template in the system.
	/// </summary>
	[DataContract]
	[Obsolete("This class is obsolete. Use TemplateModel instead.")]
	public class BfEmailTemplateModel: TemplateModel
	{
		#region Constructor: Public

		public BfEmailTemplateModel() { }
		public BfEmailTemplateModel(TemplateModel template) {
			Id = template.Id;
			Language = template.Language;
			PageHtml = template.PageHtml;
			PageJson = template.PageJson;
			TemplateVersion = template.TemplateVersion;
			AmpHtml = template.AmpHtml;
		}

		#endregion

	}

	#endregion
}