namespace Terrasoft.Configuration
{
	using System;
	using System.Data;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: ContactCommLanguage

	/// <summary>
	/// Provides Contact language.
	/// </summary>
	public class ContactCommLanguage : IContactCommLanguage
	{

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="ContactCommLanguage"/>.
		/// </summary>
		/// <param name="userConnection">User connection</param>
		public ContactCommLanguage(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get language identifier related with contact.
		/// </summary>
		/// <param name="contactId">Contact identifier.</param>
		/// <returns>Language identifier.</returns>
		public Guid Get(Guid contactId) {
			Guid languageId = Guid.Empty;
			Select selectQuery = GetSelect(contactId);
			if (selectQuery != null) {
				using (DBExecutor dbExec = _userConnection.EnsureDBConnection()) {
					using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
						while (reader.Read()) {
							languageId = reader.GetColumnValue<Guid>("LanguageId");
						}
					}
				}
			}
			return languageId;
		}

		/// <summary>
		/// Generate select query to Contact schema.
		/// </summary>
		/// <returns><see cref="Select"/> instance.</returns>
		public Select GetSelect(Guid contactId) {
			Select selectQuery = new Select(_userConnection)
				.Column("LanguageId")
				.From("Contact")
				.Where("Contact", "Id").IsEqual(Column.Const(contactId)) as Select;
			return selectQuery;
		}

		/// <summary>
		/// Gets default language from system setting.
		/// </summary>
		/// <returns>Default language identifier.</returns>
		public Guid GetDefault() {
			return SystemSettings.GetValue(_userConnection, "DefaultMessageLanguage", default(Guid));
		}

		#endregion

	}

	#endregion
}
