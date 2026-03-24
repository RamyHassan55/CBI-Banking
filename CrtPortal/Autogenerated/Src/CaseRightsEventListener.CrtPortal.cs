namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Common;
	using Terrasoft.Configuration.RightsService;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: CaseRightsEventListener

	/// <summary>
	/// Listener for case rights.
	/// </summary>
	[EntityEventListener(SchemaName = "Case")]
	public class CaseRightsEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private readonly Guid _organizationSysAdminUnitId = new Guid("DF93DCB9-6BD7-DF11-9B2A-001D60E938C6");

		#endregion

		#region Properties: Public

		public RightsHelper RightsHelper;

		#endregion

		#region Methods: Private		

		private bool TryGetSspOrganizationId(UserConnection userConnection, Guid contactId, out Guid organizationId) {
			organizationId = Guid.Empty;
			if (TryGetContactAccountId(userConnection, contactId, out Guid accountId)) {
				Select selectQuery = new Select(userConnection)
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
			return false;
		}

		private bool TryGetContactAccountId(UserConnection userConnection, Guid contactId, out Guid accountId) {
			accountId = Guid.Empty;
			Select selectQuery = new Select(userConnection)
					.Column("AccountId")
					.From("Contact")
					.Where("Id").IsEqual(Column.Parameter(contactId))
						as Select;
			selectQuery.ExecuteSingleRecord<Guid>(reader =>
				reader.GetColumnValue<Guid>("AccountId"), out accountId);
			return accountId != Guid.Empty;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnInserted"/>
		/// </summary>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			base.OnInserted(sender, e);
			Entity entity = (Entity)sender;
			Guid contactId = entity.GetTypedColumnValue<Guid>("ContactId");
			bool organizationCaseRightEnabled = SystemSettings.GetValue(entity.UserConnection, "GrantCasePermissionsForPortalOrganization", false);
			if (organizationCaseRightEnabled && contactId != Guid.Empty && TryGetSspOrganizationId(entity.UserConnection, contactId, out Guid organizationId)) {
				RightsHelper = RightsHelper ?? new RightsHelper(entity.UserConnection);
				if (RightsHelper.GetCanChangeReadSchemaRecordRight("Case", entity.PrimaryColumnValue)) {
					RightsHelper.SetRecordRight(organizationId, entity.SchemaName, entity.PrimaryColumnValue.ToString(), 0, 2);		
				}
			}
		}

		#endregion

	}

	#endregion

}

