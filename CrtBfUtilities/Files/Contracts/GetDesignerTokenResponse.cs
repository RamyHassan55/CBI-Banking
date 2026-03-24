namespace Creatio.ContentDesigner
{
	using CrtBfUtilities.Contracts;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents response from service, which contains access token for designer to load.
	/// </summary>
	[DataContract]
	public class GetDesignerTokenResponse
	{

		#region Properties: Public

		[DataMember(Name = "access_token")]
		public string AccessToken { get; set; } = string.Empty;

		[DataMember(Name = "error")]
		public string Error { get; set; } = string.Empty;

		[DataMember(Name = "isSuccess")]
		public bool IsSuccess { get; set; }

		[DataMember(Name = "overrideProperties")]
		public List<ConfigVersionedProperty> OverrideProperties { get; set; }

		#endregion

	}
}
