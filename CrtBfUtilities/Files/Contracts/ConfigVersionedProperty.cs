using System.Runtime.Serialization;

namespace CrtBfUtilities.Contracts
{

	/// <summary>
	/// Represents a model with property config and value.
	/// </summary>
	[DataContract]
	public class ConfigVersionedProperty
	{

		#region Properties: Public

		/// <summary>
		/// Config property name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Config property value
		/// </summary>
		[DataMember(Name = "value")]
		public string Value { get; set; }

		#endregion

	}
}
