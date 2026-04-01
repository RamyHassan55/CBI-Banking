namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using CoreSettings = Core.Configuration.SysSettings;

	#region Class: MatomoTrackingWebhookEventHandler

	/// <summary>
	/// Class to represent matomo tracking webhook event handler.
	/// </summary>
	[TrackingWebhookEventHandler]
	public class MatomoTrackingWebhookEventHandler : BaseTrackingWebhookEventHandler
	{

		#region Fields: Private

		private static readonly Guid _matomoTrackingId = Guid.Parse("8BAF47F2-4DD9-4AB1-BB27-EF949D03E594");
		private readonly string _defaultApiKey = "anonymous";

		#endregion

		#region Properties: Protected

		/// <inheritdoc/>
		protected override IEnumerable<Guid> SupportedTrackings { get; set; } = new List<Guid> {
			Guid.Empty, _matomoTrackingId
		};

		#endregion

		#region Methods: Protected

		/// <inheritdoc/>
		protected override void InternalHandle(UserConnection userConnection,
				TrackingImportByWebhookMessage sourceMessage) {
			var message = new MatomoTrackingImportByWebhookMessage(sourceMessage);
			var queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", userConnection)
			);
			queueManager.Enqueue(new TouchQueueMessage[] { message });
		}

		/// <inheritdoc/>
		protected override bool IsConnected(UserConnection userConnection) {
			var url = CoreSettings.GetValue(userConnection, "MatomoAPIAddress", "");
			var apiKey = CoreSettings.GetValue(userConnection, "MatomoAPIKey", _defaultApiKey);
			if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(apiKey)
					|| apiKey.Equals(_defaultApiKey)) {
				return false;
			}
			return base.IsConnected(userConnection);
		}

		#endregion

	}

	#endregion

}

