
namespace CrtLandingPage
{
	using System;
	using System.Collections.Generic;
	using CrtLandingPage.Dto;

	/// <summary>
	/// Provides a code by which the client scripts are built or retrieved for landing page.
	/// </summary>
	public interface IClientScriptProvider
	{
		/// <summary>
		/// Returns client scripts for landing page.
		/// </summary>
		/// <param name="landingId">The identifier of landing page.</param>
		/// <returns>The list of client script dtos.</returns>
		IEnumerable<LandingClientScript> GetScripts(Guid landingId);
	}
}

