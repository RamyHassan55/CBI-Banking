namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents the request to edit display conditions for templates in a bulk email.
	/// </summary>
	[DataContract]
	public class EditConditionResponse
	{
		/// <summary>
		/// Gets or sets the result of the condition editing.
		/// </summary>
		[DataMember(Name = "isEditSuccess")]
		public bool IsEditSuccess { get; set; }
	}
}
