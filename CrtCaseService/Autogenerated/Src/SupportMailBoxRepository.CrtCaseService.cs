namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: SupportMailBoxRepository

	/// <summary>
	/// Provides obtaining of support mailboxes.
	/// </summary>
	public class SupportMailBoxRepository : ISupportMailBoxRepository
	{

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="SupportMailBoxRepository"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public SupportMailBoxRepository(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets support mailboxes with language.
		/// </summary>
		/// <returns>Support mailboxes with language.</returns>
		public IEnumerable<Entity> GetMailboxes() {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "MailboxForIncidentRegistration");
			esq.AddColumn("Category");
			esq.AddColumn("MailboxSyncSettings.MailboxName").Name = "MailboxName";
			esq.AddColumn("MailboxSyncSettings.MessageLanguage.Id").Name = "MessageLanguageId";
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"MailboxSyncSettings.SendEmailsViaThisAccount", true));
			return esq.GetEntityCollection(_userConnection);
		}

		/// <summary>
		/// Gets support mailbox from system setting.
		/// </summary>
		/// <returns>Support mailbox.</returns>
		public string GetMailboxFromSystemSetting() {
			return SystemSettings.GetValue<string>(_userConnection, "SupportServiceEmail", string.Empty);
		}

		#endregion

	}

	#endregion

}
