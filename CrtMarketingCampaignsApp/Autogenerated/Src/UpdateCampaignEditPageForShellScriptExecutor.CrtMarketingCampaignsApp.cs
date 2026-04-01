namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	public class UpdateCampaignEditPageForShellScriptExecutor : IInstallScriptExecutor
	{
		private readonly string _entitySchemaName = "Campaign";
		private readonly Guid _freedomUI = Guid.Parse("F4D757E1-2AEB-496F-B751-3D5B39708EA3");

		private void UpdateCampaignEditPage(UserConnection userConnection) {
			EntitySchemaManager entitySchemaManager = userConnection.EntitySchemaManager;
			Entity editPagesUIType = entitySchemaManager.GetEntityByName("EntityEditPagesUITypes", userConnection);
			var conditions = new Dictionary<string, object> {
				{ "EntitySchemaName", _entitySchemaName }
			};
			if (!editPagesUIType.FetchFromDB(conditions)) {
				return;
			}
			editPagesUIType.SetColumnValue("FreedomId", _freedomUI);
			editPagesUIType.Save();
		}

		public void Execute(UserConnection userConnection) {
			UpdateCampaignEditPage(userConnection);
		}
	}
}

