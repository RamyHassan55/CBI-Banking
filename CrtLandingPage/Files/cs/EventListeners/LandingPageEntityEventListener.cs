namespace Creatio.LandingPageDesigner.EventListeners
{
	using Creatio.LandingPageDesigner.Utils;
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;

	#region Class: LandingPageEntityEventListener

	[EntityEventListener(SchemaName = "LandingPage")]
	internal class LandingPageEntityEventListener : BaseEntityEventListener
	{
		private void CreateHtmlPage(Guid recordId, string name, UserConnection userConnection) {
			var schema = userConnection.EntitySchemaManager.GetInstanceByName("HTMLPage");
			var entity = schema.CreateEntity(userConnection);
			entity.SetDefColumnValues();
			string slug = SlugGenerator.GenerateSlug(name);
			entity.SetColumnValue("ContentSlug", slug);
			entity.SetColumnValue("LandingPageId", recordId);
			entity.Save(false);
		}

		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			var entity = (Entity)sender;
			var name = entity.GetTypedColumnValue<string>("Name");
			CreateHtmlPage(entity.PrimaryColumnValue, name, entity.UserConnection);
			base.OnInserted(sender, e);
		}
	}

	#endregion

}
