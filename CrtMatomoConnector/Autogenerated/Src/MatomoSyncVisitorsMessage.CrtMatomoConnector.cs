namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Terrasoft.Core;

	#region Class: MatomoSyncVisitorsMessage

	/// <summary>
	/// Class to represent matomo sync data action message for visitors specified.
	/// </summary>
	[TouchQueueMessage]
	public class MatomoSyncVisitorsMessage : MatomoTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="MatomoSyncVisitorsMessage"/>.
		/// </summary>
		/// <param name="visitors">Models of visitors to sync.</param>
		/// <param name="startDate">Date to sync from.</param>
		/// <param name="siteIds">Ids of matomo sites to sync.</param>
		public MatomoSyncVisitorsMessage(IEnumerable<TouchModel> visitors, DateTime startDate,
				IEnumerable<int> siteIds) {
			Type = TouchQueueMessageType.Sync;
			Visitors = visitors;
			SiteIds = siteIds;
			StartDate = startDate;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<TouchModel> Visitors { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<int> SiteIds { get; set; }

		/// <summary>
		/// 
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
			syncManager.Synchronize(Visitors, SiteIds, StartDate);
		}

		#endregion
	}

	#endregion

}

