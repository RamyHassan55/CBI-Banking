namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: FormSubmitEntityListener

	/// <summary>
	/// Listener for <see cref="WebFormData"/> entity events.
	/// </summary>
	/// <seealso cref="BaseEntityEventListener" />
	[EntityEventListener(SchemaName = "WebFormData")]
	public class FormSubmitEntityListener : BaseEntityEventListener
	{
		#region Methods: Public

		/// <summary>
		/// Handles <see cref="WebFormData"/> entity <see cref="OnUpdated"/> event.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">The <see cref="T:Terrasoft.Core.Entities.EntityAfterEventArgs" /> instance containing the
		/// event data.</param>
		public override void OnUpdated(object sender, EntityAfterEventArgs e) {
			base.OnUpdated(sender, e);
			var columnValue = e.ModifiedColumnValues.FindByName("ContactId");
			if (columnValue == null) {
				return;
			}
			var contactId = columnValue.Value as Guid?;
			var entity = (WebFormData)sender;
			entity.WebForm?.FetchFromDB("Id", entity.WebFormId, new[] { "ExternalURL" });
			if (string.IsNullOrWhiteSpace(entity.MatomoVisitorId) && string.IsNullOrWhiteSpace(entity.MatomoUserId)) {
				return;
			}
			var externalUrl = entity.WebForm?.ExternalURL;
			var message = new MatomoImportByFormSubmitMessage(contactId.Value, entity.MatomoVisitorId,
				entity.MatomoUserId, externalUrl);
			var queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", entity.UserConnection)
			);
			queueManager.Enqueue(new TouchQueueMessage[] { message });
		}

		#endregion

	}

	#endregion

}


