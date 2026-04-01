namespace CrtLandingPageAnalytics
{
	using System.Collections.Generic;
	using CrtGoogleAnalytics.Messages;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;


	#region Class: GoogleAnalyticsSyncStatsBatchMessage
	/// <summary>
	/// Class to represent Google Analytics sync statistics per batch message.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsSyncStatsBatchMessage : GoogleAnalyticsTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsSyncStatsBatchMessage"/>.
		/// </summary>
		public GoogleAnalyticsSyncStatsBatchMessage(IEnumerable<GoogleAnalyticsLandingStreamDto> streams) {
			Type = TouchQueueMessageType.Sync;
			Streams = streams;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Batch of visitors to sync data.
		/// </summary>
		public IEnumerable<GoogleAnalyticsLandingStreamDto> Streams { get; set; }

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override int GetPriority() => 6;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsStatsSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.Synchronize(Streams);
		}

		#endregion

	}

	#endregion

}

