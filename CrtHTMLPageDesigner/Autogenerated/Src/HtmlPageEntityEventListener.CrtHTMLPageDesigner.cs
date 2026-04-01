namespace CrtHtmlPageDesigner
{
	using CrtHTMLPageDesigner;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: HtmlPageEntityEventListener

	[EntityEventListener(SchemaName = "HTMLPage")]
	internal class HtmlPageEntityEventListener : BaseEntityEventListener
	{

		#region Methods: Public

		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			var entity = (Entity)sender;
			var currentSlug = entity.GetTypedColumnValue<string>("ContentSlug");
			var previousSlug = entity.GetTypedOldColumnValue<string>("ContentSlug");
			var currentHtml = entity.GetTypedColumnValue<string>("PageHtml");
			var previousHtml = entity.GetTypedOldColumnValue<string>("PageHtml");
			if (currentSlug != previousSlug || currentHtml != previousHtml) {
				var hashUpdater = ClassFactory.Get<IHtmlPageHashUpdater>();
				hashUpdater.UpdateHash(entity);
			}
			base.OnSaving(sender, e);
		}

		#endregion

	}

	#endregion

}

