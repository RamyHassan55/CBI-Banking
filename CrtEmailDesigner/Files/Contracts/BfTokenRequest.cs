namespace CrtEmailDesigner.Contracts
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a request to get access token.
	/// </summary>
	[DataContract]
	[Obsolete("Class will be removed in upcoming releases")]
	public class BfTokenRequest
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the designer code for token request.
		/// </summary>
		[DataMember(Name = "designerCode")]
		public string DesignerCode { get; set; }

		#endregion

	}
}