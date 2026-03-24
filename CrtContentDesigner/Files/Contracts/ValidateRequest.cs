namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a request to validate a bulk email and its associated records.
	///	</summary>
	[DataContract]
	public class ValidateRequest
	{
		/// <summary>
		/// The unique identifier of the bulk email to validate.
		/// </summary>
		[DataMember(Name = "bulkEmailId")]
		public string BulkEmailId { get; set; }

		/// <summary>
		/// An array of record identifiers to validate against the bulk email.
		/// </summary>
		[DataMember(Name = "recordIds")]
		public string[] RecordIds { get; set; }
	}
}
