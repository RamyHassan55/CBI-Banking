namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: MatomoTrackingImportByWebhookMessage

	/// <summary>
	/// Class to represent matomo data import by tracking webhook event.
	/// </summary>
	[TouchQueueMessage]
	public class MatomoTrackingImportByWebhookMessage : TrackingImportByWebhookMessage
	{

		#region Constants: Private

		private const string TrackingSystemId = "8BAF47F2-4DD9-4AB1-BB27-EF949D03E594";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoTrackingImportByWebhookMessage"/>.
		/// </summary>
		/// <param name="message">Instance of the <see cref="TrackingImportByWebhookMessage"/> message.</param>
		public MatomoTrackingImportByWebhookMessage(TrackingImportByWebhookMessage message) 
			: base(message.ContactId, message.TrackingUserId, message.PageUrl) { }

		/// <summary>
		/// Constructor for <see cref="MatomoTrackingImportByWebhookMessage"/>.
		/// </summary>
		public MatomoTrackingImportByWebhookMessage() { }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns identifier of tracking system <see cref="TouchEventTracking"/>.
		/// </summary>
		/// <returns>Identifier of Tracking system. <see cref="Guid.Empty"/> by default.</returns>
		public override Guid GetTracking() => Guid.Parse(TrackingSystemId);

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<MatomoDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection)
			);
			syncManager.SynchronizeByWebhook(ContactId, TrackingUserId, PageUrl);
		}

		#endregion

	}
	#endregion

}

