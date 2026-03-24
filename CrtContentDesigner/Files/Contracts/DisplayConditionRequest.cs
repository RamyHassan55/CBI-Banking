namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a display condition for a bulk email.
	/// </summary>
	[DataContract]
	public class DisplayConditionRequest
	{
		/// <summary>
		/// The unique identifier of the display condition.
		/// </summary>
		[DataMember(Name = "conditionId")]
		public string ConditionId { get; set; }

		/// <summary>
		/// The name of the display condition.
		/// </summary>
		[DataMember(Name = "conditionName")]
		public string ConditionName { get; set; }

		/// <summary>
		/// The description of the display condition.
		/// </summary>
		[DataMember(Name = "conditionDescription")]
		public string ConditionDescription { get; set; }
	}
}
