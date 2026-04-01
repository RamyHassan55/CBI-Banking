namespace CrtGoogleAnalytics.Handlers
{
	using CrtCloudIntegration.Handlers;
	using CrtCloudIntegration.Interfaces;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Handles auth request to facebook digital adds app.
	/// </summary>
	[DefaultBinding(typeof(ISocialOAuthHandler), Name = "DefaultGAOAuthHandler")]
	public class DefaultGAOAuthHandler : DefaultOAuthHandler, ISocialOAuthHandler
	{

		#region Properties: Public

		/// <summary>
		/// Application name.
		/// </summary>
		public override string Application => "web_analytics";

		/// <summary>
		/// The identity scope.
		/// </summary>
		// TODO: RND-56791 Register own scope in the identity services.
		public override string Scope => "web_analytics.service.full_access";

		#endregion

	}
}
