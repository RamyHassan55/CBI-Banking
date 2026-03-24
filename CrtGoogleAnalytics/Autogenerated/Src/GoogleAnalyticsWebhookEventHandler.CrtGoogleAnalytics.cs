namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtGoogleAnalytics;
	using CrtGoogleAnalytics.Messages;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using global::Common.Logging;
	using Terrasoft.Common;

	#region Class: GoogleAnalyticsWebhookEventHandler

	/// <summary>
	/// Class to represent google analytics webhook event handler.
	/// </summary>
	[TrackingWebhookEventHandler]
	public class GoogleAnalyticsWebhookEventHandler : BaseTrackingWebhookEventHandler
	{

		#region Fields: Private

		private static readonly Guid _googleAnalyticsTrackingId = GoogleAnalyticsConsts.GoogleAnalyticsPlatformId;

		#endregion

		#region Properties: Protected

		/// <inheritdoc/>
		protected override IEnumerable<Guid> SupportedTrackings { get; set; } = new List<Guid> {
			Guid.Empty, _googleAnalyticsTrackingId
		};

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		private ILog _logger;
		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger("GoogleAnalytics"));
			set => _logger = value;
		}

		/// <summary>
		/// Unique sream identifier in the Creatio.
		/// </summary>
		public Guid StreamId { get; set; }

		#endregion

		#region Methods: Private

		private WebAnalyticsVisitorRepository GetVisitorRepository(UserConnection userConnection) =>
			ClassFactory.Get<WebAnalyticsVisitorRepository>(
				new ConstructorArgument("userConnection", userConnection),
				new ConstructorArgument("logger", Logger),
				new ConstructorArgument("analitycsPlatformId", GoogleAnalyticsConsts.GoogleAnalyticsPlatformId)
			);

		private GoogleAnalyticsAccountRepositoty GetAccountRepository(UserConnection userConnection) =>
			ClassFactory.Get<GoogleAnalyticsAccountRepositoty>(
				new ConstructorArgument("userConnection", userConnection),
				new ConstructorArgument("logger", Logger)
			);

		private bool SaveVisitor(UserConnection userConnection, GoogleAnalyticsImportByWebhookMessage message) {
			var model = new WebAnalyticsVisitorModel {
				ContactId = message.ContactId,
				StreamId = StreamId,
				VisitorId = message.TrackingUserId
			};
			var repository = GetVisitorRepository(userConnection);
			var visitorId = repository.FindVisitor(model);
			if (visitorId.IsNotEmpty()) {
				Logger.Info($"Skipped webhook. Existed visitor ({model.VisitorId})" +
					" - analytics data will be loaded by schedule.");
				return false;
			}
			message.WebAnalyticsVisitorId = repository.Save(model);
			if (message.WebAnalyticsVisitorId.IsEmpty()) {
				return false;
			}
			return true;
		}

		private void FillStreamData(UserConnection userConnection, GoogleAnalyticsImportByWebhookMessage message) {
			var parts = message.TrackingUserId.Split(':');
			if (parts.Count() < 2) {
				throw new Exception($"Incorrect visitorId format for GA: {message.TrackingUserId}");
			}
			message.TrackingUserId = parts[0];
			var measurementId = parts[1];
			var accountRepository = GetAccountRepository(userConnection);
			var streamDto = accountRepository.GetStreamData(measurementId)
				?? throw new Exception($"There is no stream for measurementId: {measurementId}");
			StreamId = streamDto.StreamId;
			message.MeasurementId = measurementId;
			message.PropertyId = streamDto.PropertyId;
			message.AccountId = streamDto.AccountId;
		}

		private GoogleAnalyticsImportByWebhookMessage GetGAWebhookMessage(
				TrackingImportByWebhookMessage sourceMessage, UserConnection userConnection) {
			var message = new GoogleAnalyticsImportByWebhookMessage(sourceMessage);
			FillStreamData(userConnection, message);
 			return message;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		protected override void InternalHandle(UserConnection userConnection,
				TrackingImportByWebhookMessage sourceMessage) {
			var message = GetGAWebhookMessage(sourceMessage, userConnection);
			if (!SaveVisitor(userConnection, message)) {
				return;
			}
			var queueManager = ClassFactory.Get<TouchQueueManager>(
				new ConstructorArgument("userConnection", userConnection)
			);
			queueManager.Enqueue(new TouchQueueMessage[] { message });
		}

		/// <inheritdoc/>
		protected override bool IsConnected(UserConnection userConnection) {
			var accountsRepository = ClassFactory.Get<GoogleAnalyticsAccountRepositoty>(
				new ConstructorArgument("userConnection", userConnection),
				new ConstructorArgument("logger", Logger),
				new ConstructorArgument("analitycsPlatformId", GoogleAnalyticsConsts.GoogleAnalyticsPlatformId));
			return accountsRepository.GetConnectedAccounts().Any() && base.IsConnected(userConnection);
		}

		#endregion

	}

	#endregion

}

