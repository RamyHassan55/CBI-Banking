namespace CrtLandingPage.Requests
{
	using System;
	using System.Runtime.Serialization;

	#region Class: LandingPageHtml

	/// <summary>
	/// Represents a model of the landing page html.
	/// </summary>
	[Serializable]
	[DataContract]
	public class LandingPageHtml
	{

		#region Properties: Public

		/// <summary>
		/// Unique identifier of the html page for the landing.
		/// </summary>
		[DataMember(Name = "htmlPageId")]
		public Guid HtmlPageId {
			get; set;
		}

		/// <summary>
		/// Html content of the page.
		/// </summary>
		[DataMember(Name = "html")]
		public string Html {
			get; set;
		}

			/// <summary>
		/// Content slug of the html page.
		/// </summary>
		[DataMember(Name = "contentSlug")]
		public string ContentSlug {
			get; set;
		}

		#endregion

	}

	#endregion

}
