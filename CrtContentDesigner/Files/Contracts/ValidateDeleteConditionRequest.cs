namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a request to validate a bulk email and its associated records.
	///	</summary>
	[DataContract]
	public class ValidateDeleteConditionRequest
	{
		/// <summary>
		/// The unique identifier of the bulk email to validate.
		/// </summary>
		[DataMember(Name = "bulkEmailId")]
		public string BulkEmailId { get; set; }

		/// <summary>
		/// An array of display condition identifiers to validate contains in bf email templates.
		/// </summary>
		[DataMember(Name = "bfDisplayConditionsIds")]
		public string[] BfDisplayConditionsIds { get; set; }
	}

}
