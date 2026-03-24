namespace CrtLandingPageAnalytics
{
	using System;
	using CrtGoogleAnalytics.Messages;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsSyncPerLandingMessage

	/// <summary>
	/// Class to represent Google Analytics sync statistics per landing page message.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsSyncPerLandingMessage : GoogleAnalyticsTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsSyncPerLandingMessage"/>.
		/// </summary>
		public GoogleAnalyticsSyncPerLandingMessage(Guid landingPageId, Guid streamId, DateTime date) {
			Type = TouchQueueMessageType.Sync;
			LandingPageId = landingPageId;
			StreamId = streamId;
			StartDate = date;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Identifier of landing page
		/// </summary>
		public Guid LandingPageId { get; set; }

		/// <summary>
		/// Identifier of web analytics stream
		/// </summary>
		public Guid StreamId { get; set; }

		/// <summary>
		/// Start date to retreive data from GA.
		/// </summary>
		public DateTime StartDate { get; set; }

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override int GetPriority() => 6;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsStatsSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.SynchronizeForLanding(StreamId, LandingPageId);
		}

		#endregion

	}

	#endregion

}

