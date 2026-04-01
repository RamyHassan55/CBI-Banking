namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: MatomoTouchQueueMessage

	/// <summary>
	/// Class to represent base matomo touch queue action message. 
	/// </summary>
	public class MatomoTouchQueueMessage : TouchQueueMessage
	{

		#region Constants: Private

		/// <summary>
		/// Unique identifier of Matomo Analytics tracking system <see cref="TouchEventTracking"/> record.
		/// </summary>
		private const string TrackingSystemId = "8BAF47F2-4DD9-4AB1-BB27-EF949D03E594";

		#endregion

		#region Fields: Private

		private Guid _matomoTrackingSystemId;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoTouchQueueMessage"/>.
		/// </summary>
		public MatomoTouchQueueMessage() {
			_matomoTrackingSystemId = Guid.Parse(TrackingSystemId);
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Returns instance of the <see cref="MatomoDataSyncManager"/>.
		/// </summary>
		/// <param name="userConnection">Instance of the <see cref="UserConnection"/>.</param>
		/// <returns>Instance of the <see cref="MatomoDataSyncManager"/>.</returns>
		protected MatomoDataSyncManager GetSyncManager(UserConnection userConnection) =>
			ClassFactory.Get<MatomoDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection)
			);

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override Guid GetTracking() => _matomoTrackingSystemId;

		#endregion

	}

	#endregion

}

