namespace CrtContentDesigner.Contracts
{
	using System;
	using System.Runtime.Serialization;

	#region Class: BaseTemplateRequest

	/// <summary>
	/// Represents a request to load a template.
	/// </summary>
	[DataContract]
	public class LoadTemplateRequest : BaseTemplateRequest
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the unique identifier of the record to be loaded.
		/// </summary>
		[DataMember(Name = "recordId")]
		public Guid RecordId { get; set; }

		#endregion

	}

	#endregion

}
