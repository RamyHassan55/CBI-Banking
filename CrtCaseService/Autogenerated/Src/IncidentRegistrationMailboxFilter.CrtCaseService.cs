namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: IncidentRegistrationMailboxFilter
	/// <summary>
	/// Filter client mailboxes using support mailboxes.
	/// </summary>
	public class IncindentRegistrationMailboxFilter
	{
		#region Constants: Private

		private const string ActivitySchemaName = "Activity";
		private const string MailboxForIncidentRegistrationSchemaName = "MailboxForIncidentRegistration";
		private const string Recepient = "Recepient";
		private const string CopyRecepient = "CopyRecepient";
		private const string BlindCopyRecepient = "BlindCopyRecepient";
		private const string EmailFromMailBoxSyncSettings = "MailboxSyncSettings.SenderEmailAddress";
		private const string AliasAddress = "AliasAddress";

		#endregion

		#region Properties: Public

		/// <summary>
		/// Dictionary, contains column name as key and column alias as value.
		/// </summary>
		public Dictionary<string, string> AlliasColumnMap {
			get; private set;
		}

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get; set;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="IncindentRegistrationMailboxFilter"/> class.
		/// </summary>
		public IncindentRegistrationMailboxFilter() {
			AlliasColumnMap = new Dictionary<string, string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IncindentRegistrationMailboxFilter"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public IncindentRegistrationMailboxFilter(UserConnection userConnection)
			: this() {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private void AddColumnMap(EntitySchemaQuery esq, IEnumerable<string> columns) {
			foreach (string column in columns) {
				AlliasColumnMap[column] = esq.AddColumn(column).Name;
			}
		}

		private EntitySchemaQuery GetEsqForSchema(string schema, IEnumerable<string> columns) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, schema);
			AddColumnMap(esq, columns);
			return esq;
		}

		private string[] GetEmails(Entity activityEntity) {
			string recipientEmail = activityEntity.GetTypedColumnValue<string>(AlliasColumnMap[Recepient]);
			string ccEmail = activityEntity.GetTypedColumnValue<string>(AlliasColumnMap[CopyRecepient]);
			string bccEmail = activityEntity.GetTypedColumnValue<string>(AlliasColumnMap[BlindCopyRecepient]);
			var emails = string.Join(";", recipientEmail, ccEmail, bccEmail);
			return emails.ParseEmailAddress().ToArray();
		}

		private string[] GetActivityRecipientsEmails(Guid activityId) {
			var esq = GetEsqForSchema(ActivitySchemaName, new[] {Recepient, CopyRecepient, BlindCopyRecepient});
			Entity activityEntity = esq.GetEntity(UserConnection, activityId);			
			return GetEmails(activityEntity);
		}

		private int GetIncidentRegistrationMailboxesCount(string[] activityParticipants) {
			if (activityParticipants.IsNullOrEmpty()) {
				return 0;
			}
			var esq = GetEsqForSchema(MailboxForIncidentRegistrationSchemaName, 
				new []{ EmailFromMailBoxSyncSettings });
			esq.Filters.LogicalOperation = LogicalOperationStrict.Or;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				EmailFromMailBoxSyncSettings, activityParticipants));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				AliasAddress, activityParticipants));
			EntityCollection incidentRegistrationMailboxes = esq.GetEntityCollection(UserConnection);
			return incidentRegistrationMailboxes.Count;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets filtered emails without support emails.
		/// </summary>
		/// <param name="emails">Emails to be filtered.</param>
		/// <returns>Filtered emails.</returns>
		public string GetRecipientWithoutIncindent(string emails) {
			var esq = GetEsqForSchema(MailboxForIncidentRegistrationSchemaName, 
				new[] { EmailFromMailBoxSyncSettings, AliasAddress });
			var mailboxes = esq.GetEntityCollection(UserConnection);
			var emailList = emails.ParseEmailAddress();
			foreach (var mailBox in mailboxes) {
				string senderEmailAddress = 
					mailBox.GetTypedColumnValue<string>(AlliasColumnMap[EmailFromMailBoxSyncSettings]);
				string aliasEmailAddress = 
					mailBox.GetTypedColumnValue<string>(AlliasColumnMap[AliasAddress]);
				if (!senderEmailAddress.IsNullOrEmpty()) {
					emailList.Remove(emailList.Where(s => s.Equals(senderEmailAddress, 
						StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
				}
				if (!aliasEmailAddress.IsNullOrEmpty()) {
					emailList.Remove(emailList.Where(s => s.Equals(aliasEmailAddress, 
						StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
				}
			}
			return string.Join(";", emailList.ToArray());
		}

		/// <summary>
		/// Indicates, that emails for incident registration are present in activity recipients.
		/// </summary>
		/// <param name="activityId">Activity identifier.</param>
		/// <returns>False if emails for incident registration aren't present in activity.</returns>
		public bool IsPresentRegistrationEmailsInActivity(Guid activityId) {
			string[] allRecipients = GetActivityRecipientsEmails(activityId);
			int registrationEmailsCount = GetIncidentRegistrationMailboxesCount(allRecipients);
			if (registrationEmailsCount == 0) {
				return false;
			}
			return true;
		}

		#endregion

	}

	#endregion
}
