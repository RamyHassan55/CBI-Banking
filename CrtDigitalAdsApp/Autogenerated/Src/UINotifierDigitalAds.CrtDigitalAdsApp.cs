namespace CrtDigitalAdsApp.Utilities
{
	using CrtCloudIntegration.Utilities;
	using Terrasoft.Core.Factories;

	#region Class: UINotifierDigitalAds

	/// <summary>
	/// Represents the implementation of UI notifier.
	/// </summary>
	/// <seealso cref="CrtCloudIntegration.Utilities.IUINotifier" />
	[DefaultBinding(typeof(IUINotifier), Name = "CrtDigitalAdsApp")]
	public class UINotifierDigitalAds: UINotifierBase
	{

		#region Properties: Protected

		protected override string SenderName => "CrtDigitalAdsApp";

		#endregion

	}

	#endregion

}

