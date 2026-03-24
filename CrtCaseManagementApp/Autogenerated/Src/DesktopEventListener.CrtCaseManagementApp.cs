namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Configuration;
	
	#region Class: DesktopEventListener

	/// <summary>
	/// Listener for 'BaseAddress' entity events.
	/// </summary>
	/// <seealso cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener"/>
	[EntityEventListener(SchemaName = "Desktop")]
	public class DesktopEventListener : BaseEntityEventListener
	{

		#region Fields: Private

		private readonly Guid _allExternalUsersId = new Guid("720B771C-E7A7-4F31-9CFB-52CD21C3739F");
		private readonly Guid _allEmployeesUsersId = new Guid("A29A3BA5-4B0D-DE11-9A51-005056C00008");
		private readonly string _entitySchemaName = "Desktop";

		#endregion

		#region Methods: Private

		private void AddExternalDesktopRights(Entity entity) {
			if(entity.GetTypedColumnValue<string>("DesktopSchemaName") == "ServiceExternalDesktop") {
				var externalDesktopId = entity.GetTypedColumnValue<Guid>("Id");
				entity.UserConnection.DBSecurityEngine.SetEntitySchemaRecordRightLevel(_allExternalUsersId,
						_entitySchemaName, externalDesktopId, SchemaRecordRightLevels.CanRead, false);
				entity.UserConnection.DBSecurityEngine.ForceDeleteEntitySchemaRecordRightLevel(_allEmployeesUsersId,
					EntitySchemaRecordRightOperation.Read, _entitySchemaName, externalDesktopId);				
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="BaseEntityEventListener.OnInserted"/>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			base.OnInserted(sender, e);
			AddExternalDesktopRights(sender as Entity);
		}

		#endregion

	}

	#endregion	
	
}
