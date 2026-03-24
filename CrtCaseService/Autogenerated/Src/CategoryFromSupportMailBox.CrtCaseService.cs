namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	#region Class: CategoryFromSupportMailBox

	/// <summary>
	/// Obtain support mail boxes.
	/// </summary>
	public class CategoryFromSupportMailBox : ICategoryFromSupportMailBox
	{
		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="CategoryFromSupportMailBox"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public CategoryFromSupportMailBox(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Generate select query for getting support mailboxes.
		/// </summary>
		/// <returns>Select query.</returns>
		private Select GetSelect() {
			var selectQuery = new Select(_userConnection)
									.Column("mss", "MailboxName")
									.Column("suppBox", "CategoryId")
									.Column("suppBox", "AliasAddress")
								.From("MailboxForIncidentRegistration").As("suppBox")
								.InnerJoin("MailboxSyncSettings").As("mss")
									.On("suppBox", "MailboxSyncSettingsId").IsEqual("mss", "Id")
								as Select;
			return selectQuery;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets support mailboxes with category.
		/// </summary>
		/// <returns>Support mailboxes with category.</returns>
		public Dictionary<string, Guid> GetMailBoxesWithCategories() {
			Dictionary<string, Guid> mailBoxes = new Dictionary<string, Guid>();
			Select selectQuery = GetSelect();
			using (DBExecutor dbExec = _userConnection.EnsureDBConnection()) {
				using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
					while (reader.Read()) {
						string email = reader.GetColumnValue<string>("AliasAddress");
						if (email.IsNullOrEmpty()) {
							email = reader.GetColumnValue<string>("MailboxName");
						}
						Guid categoryId = reader.GetColumnValue<Guid>("CategoryId");
						if (!mailBoxes.ContainsKey(email)) {
							mailBoxes.Add(email, categoryId);
						}
					}
				}
			}
			return mailBoxes;
		}

		#endregion

	}

	#endregion
}
