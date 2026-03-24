namespace CrtGoogleAnalytics.Messages
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: GoogleAnalyticsSyncDataBatchMessage

	/// <summary>
	/// Class to represent Google Analytics sync batch data message.
	/// </summary>
	[TouchQueueMessage]
	public class GoogleAnalyticsSyncDataBatchMessage : GoogleAnalyticsTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Constructor for <see cref="GoogleAnalyticsSyncDataBatchMessage"/>.
		/// </summary>
		public GoogleAnalyticsSyncDataBatchMessage(IEnumerable<GAVisitorModel> visitors, DateTime startDate) {
			Type = TouchQueueMessageType.Sync;
			Visitors = visitors;
			StartDate = startDate;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Batch of visitors to sync data.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<GAVisitorModel> Visitors { get; set; }

		/// <summary>
		/// Start date to retreive data from GA.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public DateTime StartDate { get; set; }

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override int GetPriority() => 6;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = ClassFactory.Get<GoogleAnalyticsDataSyncManager>(
				new ConstructorArgument("userConnection", userConnection));
			syncManager.Synchronize(Visitors, StartDate);
		}

		#endregion

	}

	#endregion

}

