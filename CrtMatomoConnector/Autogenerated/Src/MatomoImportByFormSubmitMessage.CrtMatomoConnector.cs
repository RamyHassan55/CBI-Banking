namespace Terrasoft.Configuration
{
	using System;
	using Newtonsoft.Json;
	using Terrasoft.Core;

	#region Class: MatomoImportByFormSubmitMessage

	/// <summary>
	/// Class to represent matomo data import by form submit action message.
	/// </summary>
	[TouchQueueMessage]
	public class MatomoImportByFormSubmitMessage : MatomoTouchQueueMessage
	{

		#region Constructors: Public

		/// <summary>
		/// Consrtuctor fro <see cref="MatomoImportByFormSubmitMessage"/>.
		/// </summary>
		/// <param name="contactId">Contact unique identifier from Creatio.</param>
		/// <param name="visitorId">Matomo visitor id.</param>
		/// <param name="userId">Matomo user id.</param>
		/// <param name="externalUrl">Form submit url.</param>
		public MatomoImportByFormSubmitMessage(Guid contactId, string visitorId, string userId,
				string externalUrl) : base() {
			Type = TouchQueueMessageType.Import;
			ContactId = contactId;
			MatomoVisitorId = visitorId;
			MatomoUserId = userId;
			ExternalUrl = externalUrl;
			RequiresDeduplication = true;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Contains contact id or <see cref="Guid.Empty"/>.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Guid ContactId { get; set; }

		/// <summary>
		/// Contains matomo visitor id or <see cref="Guid.Empty"/>.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string MatomoVisitorId { get; set; }

		/// <summary>
		/// Contains user id.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string MatomoUserId { get; set; }

		/// <summary>
		/// Contains web form Url.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string ExternalUrl { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Defines priority of current message. Messages with highest priority will be processed first.
		/// </summary>
		public override int GetPriority() => 12;

		/// <inheritdoc/>
		public override void Execute(UserConnection userConnection) {
			var syncManager = GetSyncManager(userConnection);
			syncManager.SynchronizeByFormSubmit(ContactId, MatomoVisitorId, MatomoUserId, ExternalUrl);
		}

		#endregion

	}
	#endregion

}

