namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Terrasoft.Core.Entities;
	using Web.Common;

	#region Class: SenderObtainingService

	/// <summary>
	/// Service for sender mailbox obtaining.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class SenderObtainingService : BaseService
	{

		#region Class: SenderObtainingServiceResponse

		/// <summary>
		/// Represents sender mailbox data container.
		/// </summary>
		[DataContract]
		public class SenderObtainingServiceResponse : ConfigurationServiceResponse
		{
			/// <summary>
			/// Sender mailbox name.
			/// </summary>
			[DataMember(Name = "MailboxName")]
			public string MailboxName { get; set; }

			/// <summary>
			/// Sender mailbox id.
			/// </summary>
			[DataMember(Name = "MailboxId")]
			public Guid MailboxId { get; set; }
		}

		#endregion

		#region Methods: Private

		private Guid GetMailBoxId(string mailboxName) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "MailboxSyncSettings");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "MailboxName", mailboxName));
			EntityCollection entityCollection = esq.GetEntityCollection(UserConnection);
			return entityCollection.Any() ? entityCollection.First.Value.GetTypedColumnValue<Guid>("Id") : default(Guid);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Obtains sender mailbox by activity recipients, case contact and category.
		/// Uses <seealso cref="EmailSenderObtainerWrapper"/>.
		/// </summary>
		/// <param name="recipients">Email recipients, separated with semicolon.</param>
		/// <param name="contactId">Case contact entity identifier.</param>
		/// <param name="categoryId">Case category entity identifier.</param>
		/// <returns>Response with sender mailbox data.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public SenderObtainingServiceResponse ObtainSenderMailbox(string recipients, Guid contactId, Guid categoryId) {
			var wrapper = new EmailSenderObtainerWrapper(UserConnection);
			try {
				var mailboxName = wrapper.GetSupportMailBox(recipients, contactId, categoryId);
				return new SenderObtainingServiceResponse {
					Success = !string.IsNullOrEmpty(mailboxName),
					MailboxName = mailboxName,
				};
			} catch (Exception e) {
				return new SenderObtainingServiceResponse { Success = false, Exception = e };
			}
		}


		/// <summary>
		/// Obtains sender mailbox by activity recipients, case contact and category.
		/// Uses <seealso cref="EmailSenderObtainerWrapper"/>.
		/// </summary>
		/// <param name="recipients">Email recipients, separated with semicolon.</param>
		/// <param name="contactId">Case contact entity identifier.</param>
		/// <param name="categoryId">Case category entity identifier.</param>
		/// <returns>Response with sender mailbox data.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public SenderObtainingServiceResponse ObtainSenderMailboxByActivityId(Guid activityId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Activity");
			var recepientColumnName = esq.AddColumn("Recepient").Name;
			var copyRecepientColumnName = esq.AddColumn("CopyRecepient").Name;
			var caseContactColumnName = esq.AddColumn("Case.Contact.Id").Name;
			var caseCategoryNameColumnName = esq.AddColumn("Case.Category.Id").Name;
			var activty = esq.GetEntity(UserConnection, activityId);
			var wrapper = new EmailSenderObtainerWrapper(UserConnection);
			try
			{
				var mailboxName = wrapper.GetSupportMailBox(
					new List<string> {
						activty.GetTypedColumnValue<string>(recepientColumnName),
						activty.GetTypedColumnValue<string>(copyRecepientColumnName)
					}, 
					activty.GetTypedColumnValue<Guid>(caseContactColumnName),
					activty.GetTypedColumnValue<Guid>(caseCategoryNameColumnName));
				var mailboxId = GetMailBoxId(mailboxName);
				return new SenderObtainingServiceResponse {
					Success = !string.IsNullOrEmpty(mailboxName),
					MailboxName = mailboxName,
					MailboxId = mailboxId,
				};
			}
			catch (Exception e)	{
				return new SenderObtainingServiceResponse { Success = false, Exception = e };
			}
		}

		#endregion

		}

	#endregion

}
