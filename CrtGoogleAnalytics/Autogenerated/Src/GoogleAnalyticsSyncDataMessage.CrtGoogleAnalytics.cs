namespace CrtGoogleAnalytics.Messages
{
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsSyncDataMessage

	/// <summary>
	/// Class to represent Google Analytics sync data message.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsSyncDataMessage : GoogleAnalyticsTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsSyncDataMessage"/>.
		/// </summary>
		public GoogleAnalyticsSyncDataMessage() {
			Type = TouchQueueMessageType.Sync;
			RequiresDeduplication = true;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.Synchronize();
		}

		#endregion

	}

	#endregion

}

