namespace Terrasoft.Configuration
{
	using System;

	#region Class: EmailRegistrationData
	/// <summary>
	/// Container for email messages.
	/// </summary>
	public class EmailRegistrationData
	{
		#region Properties: Public

		/// <summary>
		/// Identifier of created Activity.
		/// </summary>
		public Guid ActivityId	{ get; private set;	}

		/// <summary>
		/// Identifier of EmailMessageData.
		/// </summary>
		public Guid Id	{ get; private set; }

		/// <summary>
		/// Identifier of parent EmailMessageData.
		/// </summary>
		public Guid ParentMessageId { get; private set;	}

		/// <summary>
		/// Identifier of Conversation.
		/// </summary>
		public Guid ConversationId {
			get; set;
		}

		/// <summary>
		/// Identifier of created case.
		/// </summary>
		public Guid CaseId { get; set; }

		#endregion


		#region Constructors: Public

		/// <summary>
		/// Instance of <see cref="EmailRegistrationData"/> type.
		/// <param name="acitvityId">Identifier of Activity.</param>
		/// <param name="id">Identifier of EmailMessageData.</param>
		/// <param name="parentMessageId">Identifier of parent EmailMessageData.</param>
		/// </summary>
		public EmailRegistrationData(Guid acitvityId, Guid id, Guid parentMessageId) {
			ActivityId = acitvityId;
			Id = id;
			ParentMessageId = parentMessageId;
		}

		#endregion
	}

	#endregion
}
