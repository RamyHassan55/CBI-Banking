namespace Terrasoft.Configuration
{
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	public class HideCampaignFromAppHubInstallScriptExecutor : IInstallScriptExecutor
	{
		private readonly string _appCode = "CrtMarketingCampaignsApp";

		private void HideApp(UserConnection userConnection) {
			EntitySchemaManager entitySchemaManager = userConnection.EntitySchemaManager;
			Entity sysModuleEdit = entitySchemaManager.GetEntityByName("SysInstalledApp", userConnection);
			var conditions = new Dictionary<string, object> {
				{ "Code", _appCode }
			};
			if (!sysModuleEdit.FetchFromDB(conditions)) {
				return;
			}
			sysModuleEdit.SetColumnValue("IsHidden", true);
			sysModuleEdit.Save();
		}

		public void Execute(UserConnection userConnection) {
			HideApp(userConnection);
		}
	}
}

