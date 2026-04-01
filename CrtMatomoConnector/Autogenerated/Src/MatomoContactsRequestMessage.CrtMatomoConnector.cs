namespace Terrasoft.Configuration
{
	using Terrasoft.Core;

	#region Class: MatomoContactsRequestMessage

	/// <summary>
	/// Class to represent matomo contacts to sync by userId request message.
	/// </summary>
	[TouchQueueMessage]
	public class MatomoContactsRequestMessage : MatomoTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoContactsRequestMessage"/>.
		/// </summary>
		public MatomoContactsRequestMessage() {
			Type = TouchQueueMessageType.Import;
			RequiresDeduplication = true;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Defines number of retries in case of crash or error.
		/// </summary>
		/// <returns>Number of retries.</returns>
		public override int GetMaxRetryCount() => 3;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = GetSyncManager(userConnection);
			syncManager.SynchronizeForContactsRequest();
		}

		#endregion

	}

	#endregion

}

