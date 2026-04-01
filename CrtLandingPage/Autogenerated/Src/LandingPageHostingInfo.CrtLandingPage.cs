namespace CrtLandingPage.Requests
{
	using System;
	using System.Runtime.Serialization;

	#region Class: LandingPageHostingInfo

	[Serializable]
	[DataContract]
	public class LandingPageHostingInfo
	{

		#region Properties: Public

		/// <summary>
		/// Landing page base URL.
		/// </summary>
		[DataMember(Name = "url")]
		public string Url {
			get; set;
		}

		#endregion

	}

	#endregion

}
