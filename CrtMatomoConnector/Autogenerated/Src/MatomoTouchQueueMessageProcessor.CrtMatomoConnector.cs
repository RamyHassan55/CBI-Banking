namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;

	#region Class: MatomoTouchQueueMessageProcessor

	/// <summary>
	/// Processes single typed message from <see cref="TouchQueue"/>.
	/// </summary>
	[Obsolete("8.1.3 | Use TouchQueueMessage.Execute() implementations.")]
	[TouchQueueProcessor]
	public class MatomoTouchQueueMessageProcessor : BaseTouchQueueMessageProcessor
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="MatomoTouchQueueMessageProcessor"/> class.
		/// </summary>
		/// <param name="userConnection">Current user connection.</param>
		public MatomoTouchQueueMessageProcessor(UserConnection userConnection) : base(userConnection) { }

		#endregion

		#region Properties: Public

		private MatomoDataSyncManager _matomoDataSyncManager;
		public MatomoDataSyncManager MatomoDataSyncManager {
			get => _matomoDataSyncManager ?? (_matomoDataSyncManager = new MatomoDataSyncManager(UserConnection));
			set => _matomoDataSyncManager = value;
		}

		#endregion

		#region Methods: Protected

		///<inheritdoc />
		protected override string GetOperationSuccessText(BaseTaskQueueMessage message, int count) {
			var text = UserConnection.GetLocalizableString(nameof(MatomoTouchQueueMessageProcessor),
				"ImportMatomoSessionSuccessText");
			return string.Format(text, count);
		}

		///<inheritdoc />
		protected override string GetOperationFailedText(BaseTaskQueueMessage message) {
			return UserConnection.GetLocalizableString(nameof(MatomoTouchQueueMessageProcessor),
				"ImportMatomoSessionFailedText");
		}

		///<inheritdoc />
		protected override bool CanNotify => false;

		/// <inheritdoc cref="BaseTouchQueueMessageProcessor"/>
		/// <summary>
		/// Initializes strategy for adding audience messages' processing.
		/// </summary>
		protected override void InitStrategy() {
			Strategy = new Dictionary<Type, Func<TouchQueueMessage, int>> {
				[typeof(TrackingImportByWebhookMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as TrackingImportByWebhookMessage;
					return MatomoDataSyncManager.SynchronizeByWebhook(message.ContactId, message.TrackingUserId, 
						message.PageUrl);
				}),
				[typeof(MatomoImportByFormSubmitMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as MatomoImportByFormSubmitMessage;
					return MatomoDataSyncManager.SynchronizeByFormSubmit(message.ContactId, message.MatomoVisitorId,
						message.MatomoUserId, message.ExternalUrl);
				}),
				[typeof(MatomoSyncDataMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as MatomoSyncDataMessage;
					return MatomoDataSyncManager.Synchronize();
				}),
				[typeof(MatomoSyncVisitorsMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as MatomoSyncVisitorsMessage;
					return MatomoDataSyncManager.Synchronize(message.Visitors, message.SiteIds, message.StartDate);
				}),
				[typeof(MatomoContactsRequestMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as MatomoContactsRequestMessage;
					return MatomoDataSyncManager.SynchronizeForContactsRequest();
				}),
				[typeof(MatomoContactsToSyncMessage)] = new Func<TouchQueueMessage, int>((baseMessage) => {
					var message = baseMessage as MatomoContactsToSyncMessage;
					return MatomoDataSyncManager.SynchronizeByUserIds(message.ContactIds,
						message.SiteIds, message.StartDate);
				})
			};
		}

		///<inheritdoc />
		protected override string GetEventName(TouchQueueMessage message) =>
			UserConnection.GetLocalizableString(message.GetType().Name, "LogEventName");

		///<inheritdoc />
		protected override string GetEventDescription(TouchQueueMessage message, int result) {
			return UserConnection.GetLocalizableString(message.GetType().Name, "LogEventDescription");
		}

		#endregion

	}

	#endregion

}

