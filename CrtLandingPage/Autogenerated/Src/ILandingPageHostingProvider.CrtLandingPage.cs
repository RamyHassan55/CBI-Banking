namespace CrtLandingPage
{
	using CrtLandingPage.Requests;

	#region Interface: ILandingPageHostingProvider

	public interface ILandingPageHostingProvider
	{

		#region Methods: Public

		/// <summary>
		/// Provides publishing of the landing page.
		/// </summary>
		/// <param name="request">Instance of the <see cref="LandingPagePublishRequest"/></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		LandingHostingActionResult Publish(LandingPagePublishRequest request);

		/// <summary>
		/// Provides unpublishing of the published landing page.
		/// </summary>
		/// <param name="request">Instance of the <see cref="LandingPageUnpublishRequest"/></param>
		/// <returns>Instance of the <see cref="LandingHostingActionResult"/></returns>
		LandingHostingActionResult Unpublish(LandingPageUnpublishRequest request);

		#endregion

	}

	#endregion

}
