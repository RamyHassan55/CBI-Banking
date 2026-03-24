namespace CrtLandingPageAnalytics
{
	using CrtGoogleAnalytics.Messages;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsSyncStatsMessage

	/// <summary>
	/// Class to represent Google Analytics sync statistics message.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsSyncStatsMessage : GoogleAnalyticsTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsSyncStatsMessage"/>.
		/// </summary>
		public GoogleAnalyticsSyncStatsMessage() {
			Type = TouchQueueMessageType.Sync;
			RequiresDeduplication = true;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsStatsSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.Synchronize();
		}

		#endregion

	}

	#endregion

}

