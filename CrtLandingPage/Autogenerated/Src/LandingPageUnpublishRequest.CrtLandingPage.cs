namespace CrtLandingPage.Requests
{
	using System;
	using System.Collections.Generic;

	#region Class: LandingPageUnpublishRequest

	/// <summary>
	/// Request to unpublish the landing page.
	/// </summary>
	public class LandingPageUnpublishRequest
	{
		#region Properties: Public

		/// <summary>
		/// Unique identifier of the landing page
		/// </summary>
		public Guid LandingId { get; set; }
		
		/// <summary>
		/// Flag that indicates whether to remove all landing variants, disregarding <see cref="LandingPageUnpublishRequest.VariantIds"/>>.
		/// </summary>
		public bool RemoveAll { get; set; }

        /// <summary>
        /// Identifiers of landing variants to unpublish. Ignored when RemoveAll is true.
        /// </summary>
		public List<Guid> VariantIds {
			get; set;
		}

		#endregion

	}

	#endregion

}
