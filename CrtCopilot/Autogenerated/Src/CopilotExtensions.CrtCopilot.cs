namespace Creatio.Copilot
{
	using Terrasoft.Core;

	internal static class CopilotExtensions
	{

		#region Methods: Public

		public static CopilotIntentSchemaManager GetIntentSchemaManager(this UserConnection userConnection) {
			return userConnection.Workspace.SchemaManagerProvider.GetManager<CopilotIntentSchemaManager>();
		}

		#endregion

	}

}

