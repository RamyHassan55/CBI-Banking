 namespace Creatio.Copilot
{
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	
	#region Class: CopilotAgentQueryExecutor

	[DefaultBinding(typeof(IEntityQueryExecutor), Name = "CopilotAgentQueryExecutor")]
	public class CopilotAgentQueryExecutor : BaseCopilotIntentQueryExecutor
	{
		#region Constructors: Public

		public CopilotAgentQueryExecutor(UserConnection userConnection)
			: base(userConnection) { }

		#endregion

		#region Methods: Protected

		protected override IEnumerable<ISchemaManagerItem<CopilotIntentSchema>> GetIntentManagerItems() {
			IEnumerable<ISchemaManagerItem<CopilotIntentSchema>> items = base.GetIntentManagerItems();
			return items.Where(item => item.Instance.Type == CopilotIntentType.Agent);
		}

		#endregion

	}

	#endregion

}

