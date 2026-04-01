namespace CrtGoogleAnalytics.Messages
{
	using System;
	using CrtGoogleAnalytics.Dto;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsImportByWebhookMessage

	/// <summary>
	/// Class to represent Google Analytics data import by tracking webhook event.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsImportByWebhookMessage : TrackingImportByWebhookMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsImportByWebhookMessage"/>.
		/// </summary>
		/// <param name="message">Instance of the <see cref="TrackingImportByWebhookMessage"/> message.</param>
		public GoogleAnalyticsImportByWebhookMessage(TrackingImportByWebhookMessage message)
			: base(message.ContactId, message.TrackingUserId, message.PageUrl) {
			TrackingId = GoogleAnalyticsConsts.GoogleAnalyticsPlatformId;
		}

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsImportByWebhookMessage"/>.
		/// </summary>
		public GoogleAnalyticsImportByWebhookMessage() {
			TrackingId = GoogleAnalyticsConsts.GoogleAnalyticsPlatformId;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Google analytics measurement id.
		/// </summary>
		public string MeasurementId { get; set; }

		/// <summary>
		/// Google analytics property id.
		/// </summary>
		public string PropertyId { get; set; }

		/// <summary>
		/// Google analytics account id.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// Creatio web analytics visitor id.
		/// </summary>
		public Guid WebAnalyticsVisitorId { get; set; }

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.SynchronizeByWebhook(WebAnalyticsVisitorId, TrackingUserId, new GoogleAnalyticsStreamDto {
				AccountId = AccountId,
				PropertyId = PropertyId,
				MeasurementId = MeasurementId
			});
		}

		#endregion

	}

	#endregion

}

