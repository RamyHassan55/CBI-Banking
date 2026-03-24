namespace CrtEmailDesigner.Contracts
{
	using System;
	using System.Runtime.Serialization;

	#region Class: LoadTemplateRequest

	/// <summary>
	/// Represents a request to load an email template.
	/// </summary>
	[DataContract]
	[Obsolete("Use CrtContentDesigner.Contracts.LoadTemplateRequest instead.")]
	public class LoadTemplateRequest: CrtContentDesigner.Contracts.LoadTemplateRequest{

		/// <summary>
		/// Gets or sets the unique identifier of the template to be loaded.
		/// </summary>
		[DataMember(Name = "templateId")]
		[Obsolete("This property is obsolete. Use RecordId instead.")]
		public Guid TemplateId {
			get => RecordId;
			set => RecordId = value;
		}

	}
	
	#endregion
}