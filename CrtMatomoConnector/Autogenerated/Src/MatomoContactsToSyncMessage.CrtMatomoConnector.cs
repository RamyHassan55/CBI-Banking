namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Terrasoft.Core;

	#region Class: MatomoContactsToSyncMessage

	/// <summary>
	/// Class to represent matomo sync data action message for specified contacts.
	/// </summary>
	[TouchQueueMessage]
	public class MatomoContactsToSyncMessage : MatomoTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoContactsToSyncMessage"/>.
		/// </summary>
		/// <param name="contactIds">Contact ids to sync.</param>
		/// <param name="startDate">Date to sync from.</param>
		/// <param name="siteIds">Ids of matomo sites to sync.</param>
		public MatomoContactsToSyncMessage(IEnumerable<Guid> contactIds, DateTime startDate,
				IEnumerable<int> siteIds) {
			Type = TouchQueueMessageType.Sync;
			ContactIds = contactIds;
			SiteIds = siteIds;
			StartDate = startDate;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Collection of contact ids to data sync.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<Guid> ContactIds { get; set; }

		/// <summary>
		/// Ids of Matomo sites to get data from.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<int> SiteIds { get; set; }

		/// <summary>
		/// Start date to filter matomo data. All events with date greater or equal will be synchronized.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public DateTime StartDate { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Defines priority of current message. Messages with highest priority will be processed first.
		/// </summary>
		public override int GetPriority() => 6;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = GetSyncManager(userConnection);
			syncManager.SynchronizeByUserIds(ContactIds, SiteIds, StartDate);
		}

		#endregion

	}

	#endregion

}

