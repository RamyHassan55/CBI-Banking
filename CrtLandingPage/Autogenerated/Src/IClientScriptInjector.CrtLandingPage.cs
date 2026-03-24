namespace CrtLandingPage
{
	using System;

	/// <summary>
	/// Provides a code which allows the client scripts to be added or removed from landing page.
	/// </summary>
	public interface IClientScriptInjector
	{
		/// <summary>
		/// Injects client scripts in html markup.
		/// </summary>
		/// <param name="landingId">The identifier of landing page.</param>
		/// <param name="pageHtml">Html markup of the landing page.</param>
		/// <returns>The result html markup of the landing page.</returns>
		string Inject(Guid landingId, string pageHtml);
	}
}

