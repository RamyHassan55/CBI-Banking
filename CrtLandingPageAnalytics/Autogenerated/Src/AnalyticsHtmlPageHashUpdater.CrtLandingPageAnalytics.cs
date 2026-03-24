namespace CrtLandingPageAnalytics
{
	using System;
	using CrtHTMLPageDesigner;
	using CrtLandingPage;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: AnalyticsHtmlPageHashUpdater

	[DefaultBinding(typeof(IHtmlPageHashUpdater))]
	public class AnalyticsHtmlPageHashUpdater : HtmlPageHashUpdater
	{

		#region Methods: Protected

		protected override string BuildSourceString(Entity entity) {
			var source = base.BuildSourceString(entity);
			var landingPageRepository = ClassFactory.Get<ILandingPageRepository>();
			var landingId = entity.GetTypedColumnValue<Guid>("LandingPageId");
			if (landingId.IsEmpty()) {
				return source;
			}
			var landingPage = landingPageRepository.GetLandingPageById(landingId);
			if (landingPage != null) {
				source += landingPage.GetTypedColumnValue<Guid>("AnalyticsStreamId");
				source += Separator;
			}
			return source;
		}

		#endregion

	}

	#endregion

}
