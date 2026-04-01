namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	/// <summary>
	/// Class, provides language, related with default support mailbox.
	/// </summary>
	public class DefSupportBoxLanguageRule : BaseLanguageRule
	{
		#region Constants: Private

		private const string DefaultSupportServiceEmailCode = "SupportServiceEmail";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="MailBoxLanguageRule"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public DefSupportBoxLanguageRule(UserConnection userConnection)
			:base(userConnection){}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Get language identifier from mailbox by that set in system setting.
		/// </summary>
		/// <param name="supportServiceEmail">System setting for incident registration value.</param>
		/// <returns>Language identifier.</returns>
		private Guid GetLanguageId(string supportServiceEmail) {
			EntitySchema mailboxSyncSettingsSchema =
				UserConnection.EntitySchemaManager.FindInstanceByName("MailboxSyncSettings");
			Entity entity = mailboxSyncSettingsSchema.CreateEntity(UserConnection);
			return entity.FetchFromDB("SenderEmailAddress", supportServiceEmail)
				? entity.GetTypedColumnValue<Guid>("MessageLanguageId")
				: Guid.Empty;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Provides language identifier from mailboxes for incident registration.
		/// </summary>
		/// <param name="caseEntityRecordId">Case entity identifier.</param>
		/// <inheritdoc />
		public override Guid GetLanguageId(Guid caseEntityRecordId) {
			string supportServiceEmail = SystemSettings.GetValue(UserConnection, 
				DefaultSupportServiceEmailCode, string.Empty);
			if (!string.IsNullOrEmpty(supportServiceEmail)) {
				Guid languageId = GetLanguageId(supportServiceEmail);
				return languageId;
			}
			return Guid.Empty;
		}

		#endregion

	}
}
