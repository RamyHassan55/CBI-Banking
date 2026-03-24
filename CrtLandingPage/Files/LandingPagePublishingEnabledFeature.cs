namespace Terrasoft.Configuration
{
	using Creatio.FeatureToggling;

	internal class LandingPagePublishingEnabled : FeatureMetadata
	{
		public LandingPagePublishingEnabled() {
			Description = "Turns on the possibility to publish landing page to a public address in the internet";
			IsEnabled = true;
		}
	}
}