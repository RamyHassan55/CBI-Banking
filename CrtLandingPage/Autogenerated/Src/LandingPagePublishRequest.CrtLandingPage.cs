namespace CrtLandingPage.Requests
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	#region Class: LandingPagePublishRequest

	[Serializable]
	[DataContract]
	public class LandingPagePublishRequest
	{

		#region Properties: Public

		/// <summary>
		/// Creatio unique identifier of the landing page.
		/// </summary>
		[DataMember(Name = "landingId")]
		public Guid LandingId { get; set; }

		/// <summary>
		/// Collection of the landing page variants.
		/// </summary>
		[DataMember(Name = "variants")]
		public IEnumerable<LandingPageHtml> Variants { get; set; }

		#endregion

	}

	#endregion

}
