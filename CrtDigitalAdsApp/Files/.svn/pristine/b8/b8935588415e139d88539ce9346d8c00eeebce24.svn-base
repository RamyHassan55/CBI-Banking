namespace CrtDigitalAdsApp.Handlers
{
	using CrtCloudIntegration.Handlers;
	using CrtCloudIntegration.Interfaces;
	using Terrasoft.Core.Factories;

	#region Class: DefaultAdsOAuthHandler

	/// <summary>
	/// Handles auth request to facebook digital adds app.
	/// </summary>
	[DefaultBinding(typeof(ISocialOAuthHandler), Name = "DefaultAdsOAuthHandler")]
	public class DefaultAdsOAuthHandler : DefaultOAuthHandler, ISocialOAuthHandler
	{

		#region Properties: Public

		/// <summary>
		/// Application name.
		/// </summary>
		public override string Application => "digital_ads";

		/// <summary>
		/// The identity scope.
		/// </summary>
		public override string Scope => "digital_ads.service.full_access";

		#endregion

	}

	#endregion

}
