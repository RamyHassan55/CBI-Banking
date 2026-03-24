namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;

	#region Class: ServiceObjectEventListener

	/// <summary>
	/// Represents rights regulator for service entities.
	/// </summary>
	public class ServiceObjectsRightsRegulator
	{

		#region Fields: Private

		private readonly Guid _organizationSysAdminUnitId = new Guid("DF93DCB9-6BD7-DF11-9B2A-001D60E938C6");
		private readonly UserConnection _userConnection;
		private readonly string _schemaName;

		#endregion

		#region Contructors: Public

		/// <summary>
		/// Initializes new instance of <see cref="ServiceObjectsRightsRegulator"/>
		/// </summary>
		/// <param name="userConnection">User connection</param>
		/// <param name="schemaName">Schema name for regulating rights.</param>
		public ServiceObjectsRightsRegulator(UserConnection userConnection, string schemaName) {
			_userConnection = userConnection;
			_schemaName = schemaName;
		}

		#endregion

		#region Methods: Private

		private bool TryGetContactUserId(Guid contactId, out Guid sysAdminUnit) {
			sysAdminUnit = Guid.Empty;
			Select selectQuery = new Select(_userConnection)
										.Column("Id")
										.From("VwSysAdminUnit")
										.Where("ContactId").IsEqual(Column.Parameter(contactId))
										.And("SysAdminUnitTypeId")
										.IsEqual(Column.Const(BaseConsts.UserSysAdminUnitTypeId))
											as Select;
			selectQuery.ExecuteSingleRecord<Guid>(reader =>
				reader.GetColumnValue<Guid>("Id"), out sysAdminUnit);
			return sysAdminUnit != Guid.Empty;
		}

		private bool TryGetSspOrganizationId(Guid accountId, out Guid organizationId) {
			organizationId = Guid.Empty;
			Select selectQuery = new Select(_userConnection)
					.Column("Id")
					.From("VwSspAdminUnit")
					.Where("PortalAccountId").IsEqual(Column.Parameter(accountId))
					.And("SysAdminUnitTypeId")
					.IsEqual(Column.Const(_organizationSysAdminUnitId))
				as Select;
			selectQuery.ExecuteSingleRecord<Guid>(reader =>
				reader.GetColumnValue<Guid>("Id"), out organizationId);
			return organizationId != Guid.Empty;
		}

		private void ChangeReadRights(Guid entityId, Guid sysAdminUnitId, EntitySchemaRecordRightLevel rightLevel) {
			_userConnection.DBSecurityEngine.SetEntitySchemaRecordReadingRightLevel(sysAdminUnitId,
				_schemaName, entityId, rightLevel);
		}

		private List<Guid> GetAccountContacts(Guid accountId) {
			Select selectQuery = new Select(_userConnection)
					.Column("SAU", "Id")
					.From("VwSysAdminUnit", "SAU")
					.InnerJoin("Contact").As("C")
					.On("SAU", "ContactId").IsEqual("C", "Id")
					.Where("C", "AccountId").IsEqual(Column.Parameter(accountId))
				as Select;
			return selectQuery.ExecuteEnumerable(reader => reader.GetColumnValue<Guid>("Id")).ToList();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Sets given rights level on entity for account.
		/// </summary>
		/// <param name="accountId">Account identifier.</param>
		/// <param name="entityId">Entity identifier.</param>
		/// <param name="rightLevel">Entity rights level.</param>
		public void ChangeOrganizationReadRightsLevel(Guid accountId, Guid entityId, EntitySchemaRecordRightLevel rightLevel) {
			if (accountId == Guid.Empty) {
				return;
			}
			if (TryGetSspOrganizationId(accountId, out Guid orgnizationSysAdminUnit)) {
				ChangeReadRights(entityId, orgnizationSysAdminUnit, rightLevel);
			} else {
				IEnumerable<Guid> accountUserIds = GetAccountContacts(accountId);
				foreach (Guid sysAdminUnitId in accountUserIds) {
					ChangeReadRights(entityId, sysAdminUnitId, rightLevel);
				}
			}
		}

		/// <summary>
		/// Sets given rights level on entity for contact.
		/// </summary>
		/// <param name="contactId">Contact identifier.</param>
		/// <param name="entityId">Entity identifier.</param>
		/// <param name="rightLevel">Entity rights level.</param>
		public void ChangeContactReadRightsLevel(Guid contactId, Guid entityId, EntitySchemaRecordRightLevel rightLevel) {
			if (contactId == Guid.Empty) {
				return;
			}
			if (TryGetContactUserId(contactId, out Guid orgnizationSysAdminUnit)) {
				ChangeReadRights(entityId, orgnizationSysAdminUnit, rightLevel);
			}
		}

		#endregion

	}

	#endregion

}

