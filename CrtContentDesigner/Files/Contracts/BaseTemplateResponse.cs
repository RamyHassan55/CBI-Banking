namespace CrtContentDesigner.Contracts
{
	using System.Runtime.Serialization;

	#region Class: BaseTemplateResponse

	/// <summary>
	/// Represents a base response.
	/// </summary>
	[DataContract]
	public abstract class BaseTemplateResponse
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the error message, if any.
		/// </summary>
		[DataMember(Name = "error")]
		public string Error { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the response is successful.
		/// </summary>
		[DataMember(Name = "success")]
		public bool Success { get; set; } = true;

		#endregion

	}

	#endregion

}
