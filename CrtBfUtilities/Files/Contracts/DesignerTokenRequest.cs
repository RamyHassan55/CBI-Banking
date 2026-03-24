namespace Creatio.ContentDesigner
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a request to get access token.
	/// </summary>
	[DataContract]
	public class DesignerTokenRequest
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the designer code for token request.
		/// </summary>
		[DataMember(Name = "designerCode")]
		public string DesignerCode { get; set; }

		/// <summary>
		/// Application version.
		/// </summary>
		[DataMember(Name = "appVersion")]
		public string AppVersion { get; set; }

		#endregion

	}
}
