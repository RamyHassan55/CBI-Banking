namespace CrtLandingPage
{
	using System;
	using System.Runtime.Serialization;
	using CrtLandingPage.Requests;

	#region Class: LandingHostingActionResult

	/// <summary>
	/// Represents the result of the landing hosting action.
	/// </summary>
	[Serializable]
	[DataContract]
	public class LandingHostingActionResult
	{

		#region Properties: Public

		/// <summary>
		/// Action result success flag.
		/// </summary>
		[DataMember(Name = "success")]
		public bool Success { get; set; } = true;

		/// <summary>
		/// Operation error message.
		/// </summary>
		[DataMember(Name = "errorMessage")]
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Exception object.
		/// </summary>
		[DataMember(Name = "exception")]
		public Exception Exception { get; set; }

		/// <summary>
		/// Hosting action detailed information.
		/// </summary>
		[DataMember(Name = "hostingInfo")]
		public LandingPageHostingInfo HostingInfo { get; set; }

		#endregion

	}

	#endregion

}
