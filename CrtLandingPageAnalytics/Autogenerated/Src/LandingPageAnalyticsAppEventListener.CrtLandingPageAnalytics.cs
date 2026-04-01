namespace CrtLandingPageAnalytics
{
	using CrtHTMLPageDesigner;
	using Terrasoft.Core.Factories;
	using Terrasoft.Web.Common;

	#region Class: LandingPageAnalyticsAppEventListener

	/// <summary>
	/// Application starts event listener for landing page analytics.
	/// </summary>
	public class LandingPageAnalyticsAppEventListener : AppEventListenerBase
	{

		#region Methods : Public

		/// <inheritdoc/>
		public override void OnAppStart(AppEventContext context) {
			base.OnAppStart(context);
			ClassFactory.ReBind<IHtmlPageHashUpdater, AnalyticsHtmlPageHashUpdater>();
		}

		#endregion

	}

	#endregion

}
