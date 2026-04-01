namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using SystemSettings = Terrasoft.Core.Configuration.SysSettings;

	#region Class: ServiceObjectEventListener

	[EntityEventListener(SchemaName = "ServiceObject")]
	public class ServiceObjectEventListener : BaseEntityEventListener
	{

		#region Methods: Private

		private void ChangeReadRights(Entity entity, EntitySchemaRecordRightLevel rightLevel) {
			bool rightsEnabled = SystemSettings.GetValue(entity.UserConnection,	"EnableRightsOnServiceObjects", false);
			if (rightsEnabled) {
				var regulator = new ServiceObjectsRightsRegulator(entity.UserConnection, "ServicePact");
				Guid servicePactId = entity.GetTypedColumnValue<Guid>("ServicePactId");
				regulator.ChangeContactReadRightsLevel(entity.GetTypedColumnValue<Guid>("ContactId"), servicePactId, rightLevel);
				regulator.ChangeOrganizationReadRightsLevel(entity.GetTypedColumnValue<Guid>("AccountId"), servicePactId, rightLevel);
			}

		}

		private void FillColumnType(Entity entity) {
			var columnValue = entity.GetTypedColumnValue<Guid>("ContactId") != Guid.Empty
				? ServiceObjectConstants.ContactTypeId
				: entity.GetTypedColumnValue<Guid>("AccountId") != Guid.Empty
					? ServiceObjectConstants.AccountTypeId
					: Guid.Empty;
			if (columnValue == Guid.Empty) {
				entity.SetColumnValue("TypeId", null);
			} else {
				entity.SetColumnValue("TypeId", columnValue);
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnInserted"/>
		/// </summary>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			base.OnInserted(sender, e);
			Entity entity = (Entity)sender;
			ChangeReadRights(entity, EntitySchemaRecordRightLevel.Allow);
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnDeleted"/>
		/// </summary>
		public override void OnDeleted(object sender, EntityAfterEventArgs e) {
			base.OnDeleted(sender, e);
			Entity entity = (Entity)sender;
			ChangeReadRights(entity, EntitySchemaRecordRightLevel.Deny);
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnSaving"/>
		/// </summary>
		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			base.OnSaving(sender, e);
			FillColumnType((Entity)sender);
		}

		#endregion

	}

	#endregion

}

