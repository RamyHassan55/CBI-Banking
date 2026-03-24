namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents the response from a validation request for a bulk email.
	/// </summary>
	[DataContract]
	public class EditConditionRequest
	{

		/// <summary>
		/// The unique identifier of the bulk email to validate.
		/// </summary>
		[DataMember(Name = "emailId")]
		public string EmailId { get; set; }

		/// <summary>
		/// An array of display conditions to change.
		/// </summary>
		[DataMember(Name = "displayConditions")]
		public DisplayConditionRequest[] DisplayConditions { get; set; }
	}
}
