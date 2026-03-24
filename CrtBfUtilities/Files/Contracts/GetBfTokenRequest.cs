namespace Creatio.ContentDesigner
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents request to get token for BF
	/// </summary>
	[DataContract]
	internal class GetBfTokenRequest
	{

		#region Properties: Public

		/// <summary>
		/// Content designer instance name
		/// </summary>
		[DataMember(Name = "instanceName")]
		public string InstanceName { get; set; }

		/// <summary>
		/// Designer UId
		/// </summary>
		[DataMember(Name = "uid")]
		public string UId { get; set; }

		/// <summary>
		/// Application version
		/// </summary>
		[DataMember(Name = "appVersion")]
		public string AppVersion { get; set; }

		#endregion

	}
}
