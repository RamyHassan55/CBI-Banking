namespace CrtLandingPageAnalytics
{
	using System;
	using CrtLandingPage;
	using Newtonsoft.Json.Linq;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: HtmlPageAnalyticsEventListener

	[EntityEventListener(SchemaName = "HTMLPage")]
	internal class HtmlPageAnalyticsEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private readonly Guid PublishedState = Guid.Parse("D121222C-0A28-4AE5-8EA9-ED4A64496AA0");

		#endregion

		#region Methods: Private

		private string GetPageTitle(string pageJson) {
			var jObject = JObject.Parse(pageJson);
			return jObject["page"]?["head"]?["meta"]?["title"]?.ToString();
		}

		private string GetLandingPageUrl(Entity htmlPage) {
			var landingId = htmlPage.GetTypedColumnValue<Guid>("LandingPageId");
			if (landingId.IsEmpty()) {
				return string.Empty;
			}
			var landingPageRepository = ClassFactory.Get<ILandingPageRepository>();
			var landingPage = landingPageRepository.GetLandingPageById(landingId);
			var streamId = landingPage.GetTypedColumnValue<Guid>("AnalyticsStreamId");
			if (landingPage == null || streamId.IsEmpty()) {
				return string.Empty;
			}
			return landingPage.GetTypedColumnValue<string>("Url");
		}

		private void ActualizeWebPage(Entity htmlPage) {
			var landingPageUrl = GetLandingPageUrl(htmlPage);
			if (string.IsNullOrEmpty(landingPageUrl)) {
				return;
			}
			var slug = htmlPage.GetTypedColumnValue<string>("ContentSlug");
			var pageJson = htmlPage.GetTypedColumnValue<string>("PageJson");
			var pageTitle = GetPageTitle(pageJson);
			var webPageRepository = ClassFactory.Get<HtmlWebPageRepository>(
				new ConstructorArgument("userConnection", htmlPage.UserConnection)
			);
			webPageRepository.CreateOrUpdatePage(pageTitle, $"{landingPageUrl}/{slug}", htmlPage.PrimaryColumnValue);
		}

		#endregion

		#region Methods: Public

		public override void OnSaved(object sender, EntityAfterEventArgs e) {
			var entity = (Entity)sender;
			var publicationState = entity.GetTypedColumnValue<Guid>("PublicationStateId");
			if (publicationState == PublishedState) {
				ActualizeWebPage(entity);
			}
			base.OnSaved(sender, e);
		}

		#endregion

	}

	#endregion

}

