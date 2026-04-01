namespace CrtLandingPageAnalytics
{
	using System;
	using CrtHTMLPageDesigner;
	using CrtLandingPage;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: LandingPageEntityEventListener

	[EntityEventListener(SchemaName = "LandingPage")]
	internal class LandingPageEntityEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private bool IsStreamValueChanged = false;

		#endregion

		#region Methods: Private

		private void UpdateHtmlPageHash(Entity landingPage) {
			var repository = ClassFactory.Get<ILandingPageRepository>();
			var hashUpdater = ClassFactory.Get<IHtmlPageHashUpdater>();
			var pages = repository.GetLandingHtmlPages(landingPage.PrimaryColumnValue);
			foreach (var page in pages) {
				hashUpdater.UpdateHash(page);
				page.Save(false);
			}
		}

		#endregion

		#region Methods: Public

		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			var entity = (Entity)sender;
			var currentStream = entity.GetTypedColumnValue<Guid>("AnalyticsStreamId");
			var previousStream = entity.GetTypedOldColumnValue<Guid>("AnalyticsStreamId");
			IsStreamValueChanged = currentStream != previousStream;
			base.OnSaving(sender, e);
		}

		public override void OnSaved(object sender, EntityAfterEventArgs e) {
			var entity = (Entity)sender;
			if (IsStreamValueChanged) {
				UpdateHtmlPageHash(entity);
			}
			base.OnSaved(sender, e);
		}

		#endregion

	}

	#endregion

}

