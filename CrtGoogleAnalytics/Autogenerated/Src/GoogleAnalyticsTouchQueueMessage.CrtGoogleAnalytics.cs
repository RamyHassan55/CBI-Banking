namespace CrtGoogleAnalytics.Messages
{
	using System;
	using CrtGoogleAnalytics;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsTouchQueueMessage

	/// <summary>
	/// Class to represent base GA touch queue action message. 
	/// </summary>
	public class GoogleAnalyticsTouchQueueMessage : TouchQueueMessage
	{

		#region Fields: Private

		private Guid _trackingSystemId;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsTouchQueueMessage"/>.
		/// </summary>
		public GoogleAnalyticsTouchQueueMessage() {
			_trackingSystemId = GoogleAnalyticsConsts.GoogleAnalyticsPlatformId;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Returns instance of the <see cref="GoogleAnalyticsDataSyncManager"/>.
		/// </summary>
		/// <param name="userConnection">Instance of the <see cref="UserConnection"/>.</param>
		/// <returns>Instance of the <see cref="GoogleAnalyticsDataSyncManager"/>.</returns>
		protected GoogleAnalyticsDataSyncManager GetSyncManager(UserConnection userConnection) =>
			ClassFactory.Get<GoogleAnalyticsDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection)
			);

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override Guid GetTracking() => _trackingSystemId;

		#endregion

	}

	#endregion

}

