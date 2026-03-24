namespace Terrasoft.Configuration
{
	using global::Common.Logging;
	using System;
	using Terrasoft.Common;
	using Terrasoft.Configuration.RightsService;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: ActivityParticipantRightsEventListener

	/// <summary>
	/// Listener for activity rights.
	/// </summary>
	[EntityEventListener(SchemaName = "ActivityParticipant")]
	public class ActivityParticipantRightsEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private readonly Guid _emailTypeId = new Guid("E2831DEC-CFC0-DF11-B00F-001D60E938C6");
		private readonly Guid _organizationSysAdminUnitId = new Guid("DF93DCB9-6BD7-DF11-9B2A-001D60E938C6");
		private static readonly ILog _log = LogManager.GetLogger("Error");

		#endregion

		#region Methods: Private		

		private (Guid userId, Guid portalAccountId) GetActivityParticipantUserData(Entity activityParticipant) {
			Select selectQuery = new Select(activityParticipant.UserConnection)
					.Top(1)
					.Column("SAU", "Id").As("UserId")
					.Column("SAU", "PortalAccountId").As("PortalAccountId")
					.From("ActivityParticipant", "AP")
					.InnerJoin("VwSspAdminUnit").As("SAU")
					.On("AP", "ParticipantId").IsEqual("SAU", "ContactId")
					.InnerJoin("Activity").As("A")
					.On("AP", "ActivityId").IsEqual("A", "Id")
					.Where("AP", "ParticipantId").IsEqual(Column.Parameter(activityParticipant.GetTypedColumnValue<Guid>("ParticipantId")))
					.And("A", "TypeId").IsEqual(Column.Const(_emailTypeId))
					.And("SAU", "Active").IsEqual(Column.Const(true))
						as Select;
			selectQuery.ExecuteSingleRecord(r => (
				r.GetColumnValue<Guid>("UserId"),
				r.GetColumnValue<Guid>("PortalAccountId")
			), out var result);
			return result;
		}

		private bool TryGetSspOrganizationId(UserConnection userConnection, Guid accountId, out Guid organizationId) {
			organizationId = Guid.Empty;
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

		private bool GetIsSspContactUser(Entity activityParticipant) {
			Select selectQuery =
				new Select(activityParticipant.UserConnection)
					.Top(1)
					.Column("Id")
				.From("VwSspAdminUnit")
				.Where("ContactId")
					.IsEqual(Column.Parameter(activityParticipant.GetTypedColumnValue<Guid>("ParticipantId")))
				.And("SysAdminUnitTypeId")
					.IsEqual(Column.Const(BaseConsts.UserSysAdminUnitTypeId))
				as Select;
			selectQuery.ExecuteSingleRecord<Guid>(reader =>
				reader.GetColumnValue<Guid>("Id"), out Guid userId);
			return userId != Guid.Empty;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnInserted"/>
		/// </summary>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			base.OnInserted(sender, e);
			try {
				Entity activityParticipant = (Entity)sender;
				if (GetIsSspContactUser(activityParticipant)) {
					var userData = GetActivityParticipantUserData(activityParticipant);
					if (userData.userId != Guid.Empty) {
						RightsHelper rightsHelper = ClassFactory.Get<RightsHelper>(new ConstructorArgument("userConnection", activityParticipant.UserConnection));
						rightsHelper.SetRecordRight(userData.userId, "Activity", activityParticipant.GetTypedColumnValue<Guid>("ActivityId").ToString(), 0, 1);
						bool organizationCaseRightEnabled = SystemSettings.GetValue(activityParticipant.UserConnection, "GrantCasePermissionsForPortalOrganization", false);
						if (userData.portalAccountId != Guid.Empty && TryGetSspOrganizationId(activityParticipant.UserConnection, userData.portalAccountId, out Guid organizationId) && organizationCaseRightEnabled) {
							rightsHelper.SetRecordRight(organizationId, "Activity", activityParticipant.GetTypedColumnValue<Guid>("ActivityId").ToString(), 0, 1);
						}
					}
				}
			} catch (Exception ex) {
				_log.Error(ex.Message);
			}
		}

		#endregion

	}

	#endregion

}

