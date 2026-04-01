namespace Creatio.Copilot
{
    using Terrasoft.Core;
    using Terrasoft.Core.Factories;

    [DefaultBinding(typeof(IIntentToolExecutorFactory))]
    public class IntentToolExecutorFactory : IIntentToolExecutorFactory
    {
        private readonly UserConnection _userConnection;
        private readonly ICopilotMsgChannelSender _copilotMsgChannelSender;
        private readonly IDocumentTool _documentTool;
        private readonly ICopilotSessionManager _sessionManager;


        public IntentToolExecutorFactory(UserConnection userConnection, ICopilotSessionManager copilotSessionManager,
                ICopilotMsgChannelSender copilotMsgChannelSender,IDocumentTool documentTool) {
            _userConnection = userConnection;
            _copilotMsgChannelSender = copilotMsgChannelSender;
            _documentTool = documentTool;
            _sessionManager = copilotSessionManager;
        }

        public IntentToolExecutor Create(CopilotIntentSchema copilotIntentSchema) {
            return new IntentToolExecutor(copilotIntentSchema, _userConnection, _copilotMsgChannelSender, _documentTool, _sessionManager);
        }
    }
}
