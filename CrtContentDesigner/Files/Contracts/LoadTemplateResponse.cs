namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;
	using CrtContentDesigner.Models;

	#region Class: BaseTemplateResponse

	/// <summary>
	/// Represents a response of a template.
	/// </summary>
	[DataContract]
	public class LoadTemplateResponse : BaseTemplateResponse
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the template model.
		/// </summary>
		[DataMember(Name = "content")]
		public TemplateModel Content { get; set; }

		#endregion

	}

	#endregion

}
