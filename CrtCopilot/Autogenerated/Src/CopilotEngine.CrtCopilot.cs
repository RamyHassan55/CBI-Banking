namespace Creatio.Copilot
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Security;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Creatio.Copilot.Actions;
	using Creatio.Copilot.Metadata;
	using Creatio.FeatureToggling;
	using global::Common.Logging;
	using Newtonsoft.Json;
	using Terrasoft.Common;
	using Terrasoft.Common.Threading;
	using Terrasoft.Configuration.GenAI;
	using Terrasoft.Core;
	using Terrasoft.Core.Applications.GenAI;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Security;
	using Terrasoft.Enrichment.Interfaces.ChatCompletion;
	using Terrasoft.Enrichment.Interfaces.ChatCompletion.Requests;
	using Terrasoft.Enrichment.Interfaces.ChatCompletion.Responses;

	[DefaultBinding(typeof(ICopilotEngine))]
	internal class CopilotEngine : ICopilotEngine
	{

		#region Constants: Private

		private const string CanDevelopCopilotIntentsOperation = "CanDevelopAISkills";
		private const string CanRunCopilotOperation = "CanRunCreatioAI";
		private const string CanRunCopilotApiOperation = "CanRunCreatioAIApi";
		private const int DocumentMsgInsertPosition = 2;

		#endregion

		#region Fields: Private

		private readonly ILog _log = LogManager.GetLogger("Copilot");
		private readonly UserConnection _userConnection;
		private readonly IGenAICompletionServiceProxy _completionService;
		private readonly ICopilotSessionManager _sessionManager;
		private readonly ICopilotRequestLogger _requestLogger;
		private readonly ICopilotOutputParametersHandler _outputParametersHandler;
		private readonly ICopilotMsgChannelSender _copilotMsgChannelSender;
		private readonly ICopilotContextBuilder _contextBuilder;
		private readonly ICopilotToolProcessor _toolProcessor;
		private readonly ICopilotHyperlinkUtils _hyperlinkUtils;
		private readonly IDocumentTool _documentTool;
		private readonly ICopilotSessionResponseDispatcher _sessionDispatcher;
		private readonly IIntentToolExecutorFactory _intentToolExecutorFactory;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CopilotEngine"/>
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="completionService">GenAI Completion service.</param>
		/// <param name="copilotSessionManager">Copilot session manager.</param>
		/// <param name="copilotMsgChannelSender">Copilot message sender.</param>
		/// <param name="contextBuilder">Copilot context builder.</param>
		/// <param name="toolProcessor">Copilot tool processor.</param>
		/// <param name="requestLogger">Copilot request logger.</param>
		/// <param name="outputParametersHandler">Copilot output parameters handler.</param>
		/// <param name="hyperlinkUtils">An instance of <see cref="ICopilotHyperlinkUtils"/>
		/// used for handling hyperlink-related utilities.</param>
		/// <param name="documentTool">An instance of <see cref="IDocumentTool"/></param>
		/// <param name="sessionDispatcher">An instance of <see cref="ICopilotSessionResponseDispatcher"/></param>
		/// <param name="intentToolExecutorFactory">An instance of <see cref="IIntentToolExecutorFactory"/></param>
		public CopilotEngine(UserConnection userConnection, IGenAICompletionServiceProxy completionService,
				ICopilotSessionManager copilotSessionManager, ICopilotMsgChannelSender copilotMsgChannelSender,
				ICopilotContextBuilder contextBuilder, ICopilotToolProcessor toolProcessor,
				ICopilotRequestLogger requestLogger, ICopilotOutputParametersHandler outputParametersHandler,
				ICopilotHyperlinkUtils hyperlinkUtils, IDocumentTool documentTool,
				ICopilotSessionResponseDispatcher sessionDispatcher, IIntentToolExecutorFactory intentToolExecutorFactory) {
			_userConnection = userConnection;
			_completionService = completionService;
			_sessionManager = copilotSessionManager;
			_copilotMsgChannelSender = copilotMsgChannelSender;
			_contextBuilder = contextBuilder;
			_toolProcessor = toolProcessor;
			_requestLogger = requestLogger;
			_outputParametersHandler = outputParametersHandler;
			_hyperlinkUtils = hyperlinkUtils;
			_documentTool = documentTool;
			_sessionDispatcher = sessionDispatcher;
			_intentToolExecutorFactory = intentToolExecutorFactory;
		}

		#endregion

		#region Properties: Private

		/// <summary>
		/// All Default skills.
		/// </summary>
		private IEnumerable<CopilotIntentSchema> _systemIntents;
		private IEnumerable<CopilotIntentSchema> SystemIntents => _systemIntents ??
			(_systemIntents = IntentSchemaService.FindSystemIntents()
				.Where(x => x.Status != CopilotIntentStatus.Deactivated)
				.ToList());

		private string PromptToRemoveInvalidLinks => "Rewrite the provided text, removing invalid links that " +
			"literally and strictly look like \"" + _hyperlinkUtils.InvalidLinkMarker + "\". If the message " +
			"does not contain other links except \"" + _hyperlinkUtils.InvalidLinkMarker + "\" please tell " +
			"that without mentioning the \"" + _hyperlinkUtils.InvalidLinkMarker + "\". All other links are " +
			"valid. Remove invalid links naturally, without additional comments, but save the idea and structure. " +
			"If the invalid link contains alternative text or markdown format remove them as well.";

		private string ApiPromptToRemoveInvalidLinks => "Rewrite the provided text, removing invalid links that " +
			"literally and strictly look like \"" + _hyperlinkUtils.InvalidLinkMarker + "\". Keep the formatting and " +
			"structure when possible. Remove any alternative text or markdown format related to invalid link. When" +
			" working with JSON, keep the name-value structure without modifying the names. Consider other links " +
			"as valid. If the value contains only \"" + _hyperlinkUtils.InvalidLinkMarker + "\", replace it with an" +
			" empty string.";

		#endregion

		#region Properties: Public

		private IIntentSchemaService _intentSchemaService;

		/// <summary>
		/// Gets or sets the instance of <see cref="IIntentSchemaService"/>.
		/// </summary>
		/// <value>
		/// The <see cref="IIntentSchemaService"/> instance used to interact with skill schema service.
		/// </value>
		internal IIntentSchemaService IntentSchemaService {
			get {
				if (_intentSchemaService != null) {
					return _intentSchemaService;
				}
				return _intentSchemaService = _userConnection.GetIntentSchemaService();
			}
			set => _intentSchemaService = value;
		}

		#endregion

		#region Methods: Private

		private void AdjustSessionSystemIntentPrompt(CopilotSession copilotSession) {
			var intentId = CanUseCopilotAgents() ? copilotSession.RootIntentId : copilotSession.CurrentIntentId;
			if (intentId.IsNullOrEmpty()) {
				return;
			}
			if (copilotSession.Messages.Any(copilotMessage => copilotMessage.IsFromSystemIntent)) {
				return;
			}
			string baseApplicationUrl = _hyperlinkUtils.GetBaseApplicationUrl();
			var parameters = new Dictionary<string, object> {
				{ "BaseAppUrl", baseApplicationUrl }
			};
			string systemIntentPrompt = GenerateIntentPrompt(SystemIntents, parameters);
			CopilotMessage message = CopilotMessage.FromSystem(systemIntentPrompt);
			message.IsFromSystemIntent = true;
			copilotSession.AddMessage(message);
		}

		private Guid SaveRequestInfo(DateTime? start, DateTime? end, UsageResponse usage, string error, bool isFailed) {
			start = start ?? DateTime.Now;
			end = end ?? DateTime.Now;
			var duration = (long)(end - start).Value.TotalMilliseconds;
			var requestInfo = new CopilotRequestInfo {
				StartDate = start.Value,
				Error = error,
				TotalTokens = usage?.TotalTokens ?? 0,
				PromptTokens = usage?.PromptTokens ?? 0,
				CompletionTokens = usage?.CompletionTokens,
				Duration = duration,
				IsFailed = isFailed
			};
			return _requestLogger.SaveCopilotRequest(requestInfo);
		}

		private string RemoveInvalidLinks(string content, bool isApi) {
			try {
				string prompt = isApi ? ApiPromptToRemoveInvalidLinks : PromptToRemoveInvalidLinks;
				ChatCompletionRequest rewriteRequest = CreateSimpleCompletionRequest(prompt, content);
				var (_, _, response) = ProcessCompletionRequest(rewriteRequest)
					.GetAwaiter()
					.GetResult();
				List<CopilotMessage> responseMessages = GetAssistantMessagesWithoutToolCalls(response);
				return responseMessages.Select(x => x.Content).ConcatIfNotEmpty(string.Empty);
			} catch (Exception e) {
				_log.Error("Error occurred while removing invalid links", e);
				return content;
			}
		}

		private void HandleCompletionResponse(ChatCompletionResponse completionResponse, CopilotSession session) {
			if (completionResponse?.Choices == null) {
				return;
			}
			List<CopilotMessage> assistantMessages = GetAssistantMessagesWithoutToolCalls(completionResponse);
			assistantMessages.ForEach(message => {
				if (!_hyperlinkUtils.TryMarkInvalidLinks(session, message.Content, out string markedContent)) {
					return;
				}
				message.Content = RemoveInvalidLinks(markedContent, isApi: false);
			});
			session.AddMessages(assistantMessages);
			SendMessagesToClient(session);
		}

		private List<BaseCopilotMessage> GetMessagesToSend(CopilotSession copilotSession) {
			return copilotSession.Messages
				.Where(message => !message.IsSentToClient && !message.IsFromSystemPrompt && !message.TruncateOnSave)
				.Cast<BaseCopilotMessage>().ToList();
		}

		private void SendMessagesToClient(CopilotSession copilotSession) {
			var messagesToSend = GetMessagesToSend(copilotSession);
			if (messagesToSend.Count == 0) {
				return;
			}
			messagesToSend.ForEach(message => message.RootIntentCaption = GetIntentCaptionByIntentId(message.RootIntentId));
			_copilotMsgChannelSender.SendMessages(new CopilotChatPart(messagesToSend, new BaseCopilotSession(copilotSession)));
			messagesToSend.ForEach(message => message.IsSentToClient = true);
		}

		private string GetIntentCaptionByIntentId(Guid? rootIntentId) {
			if (!rootIntentId.HasValue || rootIntentId == Guid.Empty) {
				return string.Empty;
			}
			return IntentSchemaService.FindSchemaByUId(rootIntentId.Value)?.Caption ?? string.Empty;
		}

		private static List<CopilotMessage> GetAssistantMessagesWithoutToolCalls(
				ChatCompletionResponse completionResponse) {
			List<CopilotMessage> assistantMessages = GetResponsesWithMessage(completionResponse)
				.Select(response => new CopilotMessage(response.Message, skipToolCalls: true))
				.ToList();
			return assistantMessages;
		}

		private static void SetErrorResponse(CopilotIntentCallResult response, string errorMessage,
				IntentCallStatus status = IntentCallStatus.FailedToExecute) {
			response.ErrorMessage = errorMessage;
			response.Status = status;
		}

		private static IEnumerable<ChatChoiceResponse> GetResponsesWithMessage(
				ChatCompletionResponse completionResponse) {
			return completionResponse.Choices.Where(response => response.Message.Content.IsNotNullOrEmpty());
		}

		private static IEnumerable<string> FilterActiveIntents(string[] names,
				IEnumerable<CopilotIntentSchema> activeIntents) {
			HashSet<string> allActiveIntentNames = activeIntents.Select(intent => intent.Name.ToLower()).ToHashSet();
			return names.Where(name => allActiveIntentNames.Contains(name.ToLower()));
		}

		private void HandleToolCallsCompleted(List<CopilotMessage> toolMessages, CopilotSession session,
				CopilotContext copilotContext = null) {
			if (toolMessages.IsNullOrEmpty()) {
				return;
			}
			AdjustSessionSystemIntentPrompt(session);
			session.AddMessages(toolMessages);
			if (HandleAllToolMessagesShouldOmitAssistantResponse(toolMessages, session)) {
				return;
			}
			SendSession(session, copilotContext);
		}

		private bool HandleAllToolMessagesShouldOmitAssistantResponse(List<CopilotMessage> toolMessages,
				CopilotSession session) {
			if (toolMessages.Where(msg => msg.Role == CopilotMessageRole.Tool).All(msg => msg.OmitAssistantResponse)) {
				SendMessagesToClient(session);
				_sessionManager.Update(session, null);
				return true;
			}
			return false;
		}

		private async Task HandleApiToolCallsCompletedAsync(List<CopilotMessage> toolMessages, CopilotSession session,
				CopilotIntentCallResult intentResponse, JsonSchema jsonSchema, CancellationToken token) {
			if (toolMessages.IsNullOrEmpty()) {
				return;
			}
			AdjustSessionSystemIntentPrompt(session);
			session.AddMessages(toolMessages);
			await CompleteIntentAsync(session, intentResponse, jsonSchema, token);
		}

		private bool CanRunCopilotApi() {
			if (Features.GetIsDisabled<GenAIFeatures.EnableStandaloneApi>()) {
				return false;
			}
			return _userConnection.DBSecurityEngine.GetCanExecuteOperation(CanRunCopilotApiOperation);
		}

		private bool CanUseCopilotChat() {
			return _userConnection.DBSecurityEngine.GetCanExecuteOperation(CanRunCopilotOperation);
		}

		private bool CanUseCopilotAgents() {
			return Features.GetIsEnabled<Terrasoft.Configuration.GenAI.GenAIFeatures.UseCopilotAgents>();
		}

		private IEnumerable<CopilotIntentSchema> FindAgentsForChat(Guid? excludeIntentId) {
			IEnumerable<CopilotIntentSchema> items = IntentSchemaService.FindAgents()
				.Where(agent => !agent.Behavior.SkipForChat);
			if (excludeIntentId != null) {
				items = items.Where(x => x.UId != excludeIntentId);
			}
			return items;
		}

		private IEnumerable<CopilotIntentSchema> GetSubIntentsForChat(Guid? intentId, Guid? excludeIntentId) {
			if (!intentId.HasValue) {
				return Enumerable.Empty<CopilotIntentSchema>();
			}
			IEnumerable<CopilotIntentSchema> items = IntentSchemaService.GetSubIntents(intentId.Value)
				.Where(intent => !intent.Behavior.SkipForChat);
			if (excludeIntentId != null) {
				items = items.Where(x => x.UId != excludeIntentId);
			}
			return items;
		}

		private IEnumerable<CopilotIntentSchema> FindSkillsForChat(Guid? excludeIntentId) {
			IEnumerable<CopilotIntentSchema> items = IntentSchemaService.FindSkills()
				.Where(intent => !intent.Behavior.SkipForChat);
			if (excludeIntentId != null) {
				items = items.Where(x => x.UId != excludeIntentId);
			}
			return items;
		}

		private IEnumerable<CopilotIntentSchema> FindSkillsForApi() {
			return IntentSchemaService.FindSkills().Where(intent => intent.Behavior.SkipForChat);
		}

		private bool ValidateApiSkillForExecution(CopilotIntentCall call, CopilotIntentCallResult response,
			out CopilotIntentSchema intent) {
			intent = null;
			try {
				_userConnection.DBSecurityEngine.CheckCanExecuteOperation(CanRunCopilotApiOperation);
			} catch (SecurityException e) {
				SetErrorResponse(response, e.GetFullMessage(), IntentCallStatus.InsufficientPermissions);
				return false;
			}
			if (Features.GetIsDisabled<GenAIFeatures.EnableStandaloneApi>()) {
				SetErrorResponse(response, GetLocalizableString("StandaloneApiFeatureOff"),
					IntentCallStatus.InsufficientPermissions);
				return false;
			}
			try {
				intent = IntentSchemaService.FindSchemaByName(call.IntentName);
			} catch (SecurityException) {
				LocalizableString ls = GetLocalizableString("NoSkillReadRight");
				SetErrorResponse(response, ls.Format(call.IntentName),
					IntentCallStatus.InsufficientPermissions);
				intent = null;
				return false;
			}
			if (intent == null) {
				LocalizableString ls = GetLocalizableString("IntentNotFound");
				SetErrorResponse(response, ls.Format(call.IntentName), IntentCallStatus.IntentNotFound);
				return false;
			}
			if (!IntentSchemaService.HasOperationPermitted(UserSchemaOperationRights.Execute, intent.UId)) {
				LocalizableString ls = GetLocalizableString("NoIntentExecutionRight");
				SetErrorResponse(response, ls.Format(call.IntentName),
					IntentCallStatus.InsufficientPermissions);
				return false;
			}
			if (!IsActiveIntent(intent)) {
				LocalizableString ls = GetLocalizableString("InactiveIntent");
				SetErrorResponse(response, ls.Format(call.IntentName), IntentCallStatus.InactiveIntent);
				return false;
			}
			if (!intent.Behavior.SkipForChat) {
				LocalizableString ls = GetLocalizableString("WrongIntentMode");
				SetErrorResponse(response, ls.Format(call.IntentName, CopilotIntentMode.Chat),
					IntentCallStatus.WrongIntentMode);
				return false;
			}
			return true;
		}

		private CopilotSession CreateApiSkillSession(CopilotIntentCall call, CopilotIntentSchema intent,
				CopilotIntentCallResult response, bool useJsonSchema) {
			string prompt = GenerateIntentPrompt(call.Parameters, call.AdditionalPromptText, intent,
				response.Warnings, !useJsonSchema);
			var session = _sessionManager.CreateSession(CopilotSessionType.Api);
			session.CurrentIntentId = intent.UId;
			session.RootIntentId = intent.UId;
			session.AddMessage(CopilotMessage.FromUser(prompt));
			AddIntentDocuments(session);
			AddCallDocuments(session, call);
			_sessionManager.Add(session);
			return session;
		}

		private LocalizableString GetLocalizableString(string localizableStringName) {
			string lsv = "LocalizableStrings." + localizableStringName + ".Value";
			return new LocalizableString(_userConnection.Workspace.ResourceStorage, nameof(CopilotEngine), lsv);
		}

		private IEnumerable<CopilotActionMetaItem> GetActionsMetaItemsByIntent(Guid? intentId) {
			if (intentId.HasValue) {
				return GetCurrentIntentActionsMetaItems(intentId);
			}
			return Enumerable.Empty<CopilotActionMetaItem>();
		}

		private IEnumerable<CopilotActionMetaItem> GetActionsMetaItems(Guid? currentIntentId) {
			var actionMetaItems = new List<CopilotActionMetaItem>();
			if (!currentIntentId.HasValue) {
				return actionMetaItems;
			}
			if (!IsSystemIntent(currentIntentId.Value)) {
				actionMetaItems.AddRange(GetCurrentIntentActionsMetaItems(currentIntentId));
			}
			IEnumerable<CopilotActionMetaItem> systemIntentActionsMetaItems = GetSystemIntentActionsMetaItems();
			actionMetaItems.AddRange(systemIntentActionsMetaItems);
			return actionMetaItems;
		}

		private bool IsSystemIntent(Guid intentId) {
			return SystemIntents.Any(x => x.UId == intentId);
		}

		private IEnumerable<CopilotActionMetaItem> GetSystemIntentActionsMetaItems() {
			List<CopilotActionMetaItem> actionMetaItems = new List<CopilotActionMetaItem>();
			foreach (var intent in SystemIntents) {
				if (intent.Actions != null && intent.Actions.IsNotNullOrEmpty()) {
					actionMetaItems.AddRange(intent.Actions);
				}
			}
			return actionMetaItems;
		}

		private IEnumerable<CopilotActionMetaItem> GetCurrentIntentActionsMetaItems(Guid? intentId) {
			if (intentId.IsNullOrEmpty()) {
				return new List<CopilotActionMetaItem>();
			}
			CopilotIntentSchema intent = IntentSchemaService.FindSchemaByUId(intentId.Value);
			if (intent?.Type != CopilotIntentType.System && !IntentSchemaService.HasOperationPermitted(
					UserSchemaOperationRights.Execute, intent.UId)) {
				LocalizableString ls = GetLocalizableString("NoIntentExecutionRight");
				throw new SecurityException(ls.Format(intent.Name));
			}
			if (intent != null && intent.Behavior.SkipForChat) {
				return new List<CopilotActionMetaItem>();
			}
			List<CopilotActionMetaItem> actions = intent?.Actions?.ToList();
			return actions ?? new List<CopilotActionMetaItem>();
		}

		private CopilotToolContext GetApiSkillToolDefinitions() {
			IEnumerable<CopilotActionMetaItem> systemActions = GetSystemIntentActionsMetaItems();
			var toolDefinitions = _toolProcessor.GetToolDefinitions(systemActions);
			return new CopilotToolContext(toolDefinitions);
		}

		private CopilotToolContext GetToolDefinitionsWithAgents(CopilotSession session) {
			Guid? rootIntentId = session.RootIntentId;
			Guid? currentIntentId = session.CurrentIntentId;
			List<CopilotIntentSchema> intents = new List<CopilotIntentSchema>();
			IEnumerable<CopilotIntentSchema> agents = FindAgentsForChat(rootIntentId);
			intents.AddRange(agents);
			var actionMetaItems = new HashSet<CopilotActionMetaItem>();
			if (rootIntentId.HasValue) {
				IEnumerable<CopilotIntentSchema> skills = GetSubIntentsForChat(rootIntentId, currentIntentId);
				intents.AddRange(skills);
				IEnumerable<CopilotActionMetaItem> agentActions = GetActionsMetaItemsByIntent(rootIntentId);
				IEnumerable<CopilotActionMetaItem> skillActions = currentIntentId != rootIntentId
					? GetActionsMetaItemsByIntent(currentIntentId)
					: new List<CopilotActionMetaItem>();
				IEnumerable<CopilotActionMetaItem> systemActions = GetSystemIntentActionsMetaItems();
				actionMetaItems.AddRange(agentActions);
				actionMetaItems.AddRange(skillActions);
				actionMetaItems.AddRange(systemActions);
			}
			(List<ToolDefinition> tools, Dictionary<string, IToolExecutor> toolMapping) toolDefinitions =
				_toolProcessor.GetToolDefinitions(actionMetaItems, intents);
			return new CopilotToolContext(toolDefinitions.tools, toolDefinitions.toolMapping, intents);
		}

		private CopilotToolContext GetToolDefinitionsWithoutAgents(CopilotSession session) {
			Guid? currentIntentId = session.CurrentIntentId;
			var intents = FindSkillsForChat(currentIntentId);
			var actionMetaItems = GetActionsMetaItems(currentIntentId);
			(List<ToolDefinition> tools, Dictionary<string, IToolExecutor> toolMapping) toolDefinitions =
				_toolProcessor.GetToolDefinitions(actionMetaItems, intents);
			return new CopilotToolContext(toolDefinitions.tools, toolDefinitions.toolMapping, intents);
		}

		private CopilotToolContext GetChatSessionToolDefinitions(CopilotSession session) {
			if (CanUseCopilotAgents()) {
				return GetToolDefinitionsWithAgents(session);
			}
			return GetToolDefinitionsWithoutAgents(session);
		}

		private ChatCompletionRequest ApplyLlmModelToRequest(
				ChatCompletionRequest completionRequest, CopilotSession session) {
			if (Features.GetIsDisabled<Terrasoft.Configuration.GenAI.GenAIFeatures.MultiLlmSupport>()) {
				return completionRequest;
			}
			var intentId = session.CurrentIntentId ?? session.RootIntentId;
			string llmModelName = GetLlmModelFromIntent(intentId);
			if (!string.IsNullOrEmpty(llmModelName)) {
				completionRequest.Model = llmModelName;
			}
			return completionRequest;
		}

		private ChatCompletionRequest CreateCompletionRequest(CopilotSession session,
				CopilotToolContext copilotToolContext, CopilotContext copilotContext = null) {
			List<ChatMessage> messages = CreateCompletionMessages(session, copilotToolContext, copilotContext);
			var completionRequest = new ChatCompletionRequest {
				Messages = messages,
				Tools = copilotToolContext?.Tools ?? Array.Empty<ToolDefinition>(),
				Temperature = null
			};
			return ApplyLlmModelToRequest(completionRequest, session);
		}

		private string GetLlmModelFromIntent(Guid? intentId) {
			if (!intentId.HasValue || intentId.Value == Guid.Empty) {
				return null;
			}
			CopilotIntentSchema intent = IntentSchemaService.FindSchemaByUId(intentId.Value);
			if (intent == null) {
				return null;
			}
			return intent.LlmModel;
		}

		private ChatCompletionRequest CreateApiCompletionRequest(CopilotSession session,
				CopilotToolContext copilotToolContext, JsonSchema jsonSchema) {
			var request = CreateCompletionRequest(session, copilotToolContext);
			if (jsonSchema != null) {
				request.ResponseFormat = new ResponseFormat {
					Type = CompletionStatics.ResponseFormat.JsonSchema,
					JsonSchema = jsonSchema
				};
			}
			return request;
		}

		private List<ChatMessage> CreateCompletionMessages(CopilotSession session,
				CopilotToolContext copilotToolContext, CopilotContext copilotContext) {
			List<ChatMessage> messages = session.GetMergedMessages()
				.Select(msg => msg.ToCompletionApiMessage())
				.ToList();
			AddContextCompletionMessage(session, messages, copilotContext);
			AddAgentsDescriptionCompletionMessage(copilotToolContext, messages);
			AddDocumentsCompletionMessages(session, messages);
			return messages;
		}

		private void AddDocumentsCompletionMessages(CopilotSession session, List<ChatMessage> messages) {
			if (!Features.GetIsEnabled<GenAIFeatures.UseFileHandling>()) {
				return;
			}
			ChatMessage[] completionMessages = _documentTool.GetDocumentMessagesForCompletion(
					session.Documents, session.IsTransient)
				.Select(msg => msg.ToCompletionApiMessage())
				.ToArray();
			if (completionMessages.Length > 0) {
				int index = messages.Count > DocumentMsgInsertPosition ? DocumentMsgInsertPosition : messages.Count;
				messages.InsertRange(index, completionMessages);
			}
		}

		private void AddAgentsDescriptionCompletionMessage(CopilotToolContext copilotToolContext,
				List<ChatMessage> messages) {
			if (!CanUseCopilotAgents() || copilotToolContext == null) {
				return;
			}
			CopilotMessage agentsDescriptionMessage = GetAgentsDescriptionMessage(copilotToolContext.Intents);
			if (agentsDescriptionMessage != null) {
				messages.Insert(1, agentsDescriptionMessage.ToCompletionApiMessage());
			}
		}

		private void AddContextCompletionMessage(CopilotSession session, List<ChatMessage> messages,
				CopilotContext copilotContext) {
			UpdateContext(copilotContext, session);
			CopilotContext newCopilotContext = session.CurrentContext;
			if (newCopilotContext == null || newCopilotContext.Parts.Count == 0) {
				return;
			}
			string contextContent = _contextBuilder.BuildMessageContent(newCopilotContext);
			if (contextContent.IsNullOrEmpty()) {
				return;
			}
			var contextMessage = CopilotMessage.FromSystem(contextContent);
			var completionMessage = contextMessage.ToCompletionApiMessage();
			var lastUserMessageIndex = messages.FindLastIndex(m => m.Role == CopilotMessageRole.User);
			messages.Insert(lastUserMessageIndex == -1 ? 0 : lastUserMessageIndex, completionMessage);
		}

		private ChatCompletionRequest CreateSimpleCompletionRequest(string systemPrompt, string userMessageContent) {
			CopilotMessage systemMessage = CopilotMessage.FromSystem(systemPrompt);
			CopilotMessage userMessage = CopilotMessage.FromUser(userMessageContent);
			var completionRequest = new ChatCompletionRequest {
				Messages = new List<ChatMessage> {
					systemMessage.ToCompletionApiMessage(),
					userMessage.ToCompletionApiMessage()
				}
			};
			return completionRequest;
		}

		private bool IsActiveIntent(CopilotIntentSchema intent) {
			bool canDevelopIntents = _userConnection.DBSecurityEngine.GetCanExecuteOperation(
				CanDevelopCopilotIntentsOperation, new GetCanExecuteOperationOptions {
					ThrowExceptionIfNotFound = false
				});
			return intent.Status != CopilotIntentStatus.Deactivated &&
				(canDevelopIntents || intent.Status != CopilotIntentStatus.InDevelopment);
		}

		private async Task<(DateTime? start, DateTime? end, ChatCompletionResponse response)> ProcessCompletionRequest(
				ChatCompletionRequest request, bool sync = true, CancellationToken token = default) {
			DateTime? start = DateTime.Now;
			ChatCompletionResponse response;
			if (sync) {
				response = _completionService.ChatCompletion(request);
			} else {
				response = await _completionService.ChatCompletionAsync(request, token).ConfigureAwait(false);
			}
			DateTime? end = DateTime.Now;
			return (start, end, response);
		}

		private void HandleToolCalls(CopilotSession session, ChatCompletionResponse response,
				Dictionary<string, IToolExecutor> mapping, CopilotContext copilotContext) {
			List<CopilotMessage> messages = _toolProcessor.HandleToolCalls(response, session, mapping);
			HandleToolCallsCompleted(messages, session, copilotContext);
		}

		private async Task HandleApiToolCallsAsync(CopilotSession session, ChatCompletionResponse response,
				Dictionary<string, IToolExecutor> mapping, CopilotIntentCallResult intentResponse,
				JsonSchema jsonSchema, CancellationToken token) {
			List<CopilotMessage> messages = _toolProcessor.HandleToolCalls(response, session, mapping);
			await HandleApiToolCallsCompletedAsync(messages, session, intentResponse, jsonSchema, token);
		}

		private static (string errorMessage, string errorCode) GetErrorInfo(Exception e) {
			if (e is GenAIException genAiException) {
				return (genAiException.RawError, genAiException.ErrorCode);
			}
			if (e is SecurityException) {
				return (e.Message, CopilotServiceErrorCode.InsufficientPermissions);
			}
			return (e.Message, CopilotServiceErrorCode.UnknownError);
		}

		private CopilotMessage GetAgentsDescriptionMessage(IEnumerable<CopilotIntentSchema> intents) {
			if (intents == null) {
				return null;
			}
			var agentsWithSubIntents = intents
				.Where(i => i.Type == CopilotIntentType.Agent)
				.Select(agent => {
					var subIntents = IntentSchemaService.GetSubIntents(agent.UId).Select(i => new {
						Caption = i.Caption.Value,
						i.Name
					}).ToList();
					return new {
						agent.Name,
						SubIntents = subIntents
					};
				})
				.Where(x => x.SubIntents.Any())
				.ToDictionary(x => x.Name, x => x.SubIntents);
			if (agentsWithSubIntents.Any()) {
				string message = JsonConvert.SerializeObject(agentsWithSubIntents, Formatting.None);
				return new CopilotMessage(message, CopilotMessageRole.System);
			}
			return null;
		}

		private void SendSession(CopilotSession session, CopilotContext copilotContext = null) {
			lock (session) {
				DateTime? startDate = null, endDate = null;
				ChatCompletionResponse completionResponse = null;
				var errorMessage = string.Empty;
				var copilotToolContext = GetChatSessionToolDefinitions(session);
				var completionRequest = CreateCompletionRequest(session, copilotToolContext, copilotContext);
				var isFailed = false;
				try {
					SendMessagesToClient(session);
					_copilotMsgChannelSender.SendSessionProgress(CopilotSessionProgress.Create(_userConnection, session,
						CopilotSessionProgressStates.WaitingForAssistantMessage), session.UserId);
					LogCompletionRequest(session, completionRequest);
					(startDate, endDate, completionResponse) = ProcessCompletionRequest(completionRequest)
						.GetAwaiter()
						.GetResult();
					HandleCompletionResponse(completionResponse, session);
				} catch (Exception e) {
					(errorMessage, _) = GetErrorInfo(e);
					isFailed = true;
					throw;
				} finally {
					var usage = completionResponse?.Usage;
					var requestId = SaveRequestInfo(startDate, endDate, usage, errorMessage, isFailed);
					_sessionManager.Update(session, requestId);
				}
				_sessionDispatcher.DispatchAsync(session).GetAwaiter().GetResult();
				HandleToolCalls(session, completionResponse, copilotToolContext.Mapping, copilotContext);
			}
		}

		private void LogCompletionRequest(CopilotSession session, ChatCompletionRequest completionRequest) {
			if (!Features.GetIsEnabled<Terrasoft.Configuration.GenAI.GenAIFeatures.LogCreatioAIRequest>()) {
				return;
			}
			string requestJson = JsonConvert.SerializeObject(completionRequest, Formatting.Indented);
			_copilotMsgChannelSender.SendSessionProgress(CopilotSessionProgress.Create(_userConnection, session,
				CopilotSessionProgressStates.RequestSending, requestJson), _userConnection.CurrentUser.Id);
			_log.Debug($"Request sent to GenAI service: {requestJson}");
		}

		private bool TryFindTriggeredIntent(CopilotSession session, List<Guid> rootIntentIds, out IToolExecutionTrigger toolExecutionTrigger) {
			toolExecutionTrigger = null;

			if (rootIntentIds == null) {
				return false;
			}

			if (rootIntentIds.Count != 1) {
				return false;
			}

			var intentId = rootIntentIds[0];
			if (session.CurrentIntentId == intentId || session.RootIntentId == intentId) {
				return false;
			}

			var agents = IntentSchemaService.FindAgents();
			var agent = agents.FirstOrDefault(a => a.UId == intentId);
			if (agent == null) {
				return false;
			}

			toolExecutionTrigger = _intentToolExecutorFactory.Create(agent);
			return true;
		}

		private async Task<Guid> CompleteIntentAsync(CopilotSession session, CopilotIntentCallResult response,
				JsonSchema jsonSchema, CancellationToken token) {
			DateTime? startDate = null;
			DateTime? endDate = null;
			ChatCompletionResponse completionResponse = null;
			Guid requestId;
			bool isToolCallsEnabled = Features.GetIsEnabled<GenAIFeatures.EnableApiToolCalls>();
			CopilotToolContext copilotToolContext = null;
			if (isToolCallsEnabled) {
				copilotToolContext = GetApiSkillToolDefinitions();
			}
			ChatCompletionRequest completionRequest = CreateApiCompletionRequest(session, copilotToolContext, jsonSchema);
			try {
				(startDate, endDate, completionResponse) = await ProcessCompletionRequest(completionRequest,
						false, token)
					.ConfigureAwait(true);
				MapSkillResponse(completionResponse, response, session);
			} catch (Exception e) {
				(response.ErrorMessage, _) = GetErrorInfo(e);
				response.Status = IntentCallStatus.FailedToExecute;
			} finally {
				UsageResponse usage = completionResponse?.Usage;
				requestId = SaveRequestInfo(startDate, endDate, usage, response.ErrorMessage, !response.IsSuccess);
			}
			if (isToolCallsEnabled) {
				await HandleApiToolCallsAsync(session, completionResponse, copilotToolContext.Mapping, response,
					jsonSchema, token);
			}
			return requestId;
		}

		private void MapSkillResponse(ChatCompletionResponse completionResponse,
				CopilotIntentCallResult skillResponse, CopilotSession session) {
			if (completionResponse?.Choices == null) {
				skillResponse.Status = IntentCallStatus.CantGenerateGoodResponse;
				return;
			}
			List<CopilotMessage> assistantMessages = GetAssistantMessagesWithoutToolCalls(completionResponse);
			if (Features.GetIsEnabled<GenAIFeatures.EnableApiLinkValidation>()) {
				assistantMessages.ForEach(message => {
					if (!_hyperlinkUtils.TryMarkInvalidLinks(session, message.Content,
							out string markedContent)) {
						return;
					}
					message.Content = RemoveInvalidLinks(markedContent, isApi: true);
				});
			}
			session.AddMessages(assistantMessages);
			skillResponse.Status = IntentCallStatus.ExecutedSuccessfully;
			IEnumerable<string> messages = assistantMessages
				.Select(message => message.Content);
			skillResponse.Content = string.Join(" ", messages);
		}

		private string GenerateIntentPrompt(IDictionary<string, object> parameterValues, string additionalPromptText,
			CopilotIntentSchema intent, IList<string> warnings, bool includeOutputParametersInPrompt) {
			if (parameterValues == null) {
				parameterValues = new Dictionary<string, object>();
			}
			bool shouldInline = Features.GetIsEnabled<GenAIFeatures.UseInlineTemplateParameters>();
			List<string> intentInputParameters = intent.InputParameters
				.Select(x => x.Name)
				.ToList();
			HashSet<string> notSpecifiedKeys = GetNotSpecifiedParameters(parameterValues, intentInputParameters, warnings);
			GetExtraParameterNames(parameterValues, warnings, intentInputParameters);
			var inputParameters = new Dictionary<string, object>(parameterValues);
			var prompt = new StringBuilder();
			if (shouldInline) {
				prompt.Append(GetFormattedPrompt(parameterValues, intent, notSpecifiedKeys));
				inputParameters = inputParameters
					.Where(x => !intentInputParameters.Contains(x.Key))
					.ToDictionary(x => x.Key, x => x.Value);
			} else {
				prompt.Append(intent.FullPrompt);
				foreach (string key in notSpecifiedKeys) {
					inputParameters[key] = null;
				}
			}
			if (!string.IsNullOrWhiteSpace(additionalPromptText)) {
				prompt.Append(Environment.NewLine);
				prompt.Append(additionalPromptText);
			}
			AppendParameters(inputParameters, intent, prompt, includeOutputParametersInPrompt);
			return prompt.ToString();
		}

		private static void AppendParameters(Dictionary<string, object> inputParameterValues,
			CopilotIntentSchema intent, StringBuilder prompt, bool includeOutputParametersInPrompt) {
			var parameterSectionFormatter = new IntentParametersSectionFormatter();
			if (inputParameterValues.Count > 0) {
				prompt.Append(Environment.NewLine);
				prompt.Append(parameterSectionFormatter.FormatInputParameters(intent.InputParameters, inputParameterValues));
			}
			if (intent.OutputParameters.Count > 0 && includeOutputParametersInPrompt) {
				prompt.Append(Environment.NewLine);
				prompt.Append(parameterSectionFormatter.FormatOutputParameters(intent.OutputParameters));
			}
		}

		private string GetFormattedPrompt(IDictionary<string, object> parameterValues, CopilotIntentSchema intent,
				HashSet<string> notSpecifiedKeys) {
			var inlineParameters = new Dictionary<string, object>(parameterValues);
			inlineParameters.AddRange(notSpecifiedKeys.ToDictionary(x => x, x => (object)string.Empty));
			return GenerateIntentPrompt(intent, inlineParameters);
		}

		private void GetExtraParameterNames(IDictionary<string, object> inputParameters,
				IList<string> warnings, List<string> inputParameterNames) {
			HashSet<string> extraParameterNames = inputParameters.Keys.ToHashSet();
			extraParameterNames.ExceptWith(inputParameterNames);
			if (extraParameterNames.Any()) {
				string warning = GetLocalizableString("WarningParameterNotExist")
					.Format(string.Join(",", extraParameterNames));
				warnings.Add(warning);
			}
		}

		private HashSet<string> GetNotSpecifiedParameters(IDictionary<string, object> inputParameters,
				List<string> inputParameterNames, IList<string> warnings) {
			var unSpecified = new HashSet<string>(inputParameterNames);
			unSpecified.ExceptWith(inputParameters.Keys);
			if (unSpecified.Any()) {
				string warning = GetLocalizableString("WarningParameterValueNotSpecified")
					.Format(string.Join(",", unSpecified));
				warnings.Add(warning);
			}
			return unSpecified;
		}

		private string GenerateIntentPrompt(CopilotIntentSchema intent, IDictionary<string, object> parameterValues) {
			var formattingContext = new PromptTemplateFormattingContext(_userConnection) {
				VariableValues = parameterValues
			};
			return intent.PromptTemplate.Format(formattingContext);
		}

		private string GenerateIntentPrompt(IEnumerable<CopilotIntentSchema> intents,
				IDictionary<string, object> parameterValues) {
			var formattingContext = new PromptTemplateFormattingContext(_userConnection) {
				VariableValues = parameterValues
			};
			var separator = Environment.NewLine + Environment.NewLine;
			return intents.Select(x => x.PromptTemplate.Format(formattingContext)).ConcatIfNotEmpty(separator);
		}

		private void UpdateContext(CopilotContext copilotContext, CopilotSession session) {
			if (session.CurrentIntentId.IsNullOrEmpty()) {
				return;
			}
			session.CurrentContext = copilotContext;
		}

		private void ParseAndHandleOutputParameters(CopilotIntentCallResult response, CopilotIntentSchema intent) {
			if (!response.IsSuccess) {
				return;
			}
			try {
				HandleOutputParameters(response, intent);
			} catch (Exception e) {
				response.Status = IntentCallStatus.ResponseParsingFailed;
				LocalizableString ls = GetLocalizableString("ParsingFailed");
				response.ErrorMessage = ls.Format(e.GetFullMessage());
			}
		}

		private void HandleOutputParameters(CopilotIntentCallResult response, CopilotIntentSchema intent) {
			if (response.Content.IsNullOrEmpty()) {
				throw new ArgumentNullOrEmptyException(nameof(response.Content));
			}
			Dictionary<string, string> outputParameters = ParseContent(response.Content);
			if (intent.OutputParameters.Count > 0) {
				response.ResultParameters = _outputParametersHandler.HandleOutputParameters(outputParameters,
					intent.OutputParameters);
			} else {
				response.Content = outputParameters["content"];
			}
		}

		private static Dictionary<string, string> ParseContent(string content) {
			if (content.StartsWith("```json")) {
				content = content.Substring(7, content.Length - 10);
			}
			var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
			return result;
		}

		private void AddIntentDocuments(CopilotSession session) {
			if (Features.GetIsDisabled<GenAIFeatures.UseFileHandling>()) {
				return;
			}
			IList<CreatioAIDocument> documents = _documentTool.GetDocuments(_userConnection,
				session.CurrentIntentId, session.RootIntentId);
			if (documents.IsNotNullOrEmpty()) {
				session.Documents.AddRange(documents);
			}
		}

		private void AddCallDocuments(CopilotSession session, CopilotIntentCall call) {
			IList<ICreatioAIDocument> documents = call.Documents;
			if (documents.IsNullOrEmpty()) {
				return;
			}
			Exception[] errors = _documentTool.ValidateDocuments(_userConnection, session.Id, documents).ToArray();
			if (errors.Length > 0) {
				throw new AggregateException(errors);
			}
			session.Documents.AddRange(documents);
		}

		private bool IsAdvancedOutputParametersMode(bool useJsonSchema, CopilotIntentSchema intent) {
			return useJsonSchema && !string.IsNullOrWhiteSpace(intent.ResponseFormatJsonSchema.PackageName) &&
				!string.IsNullOrWhiteSpace(intent.ResponseFormatJsonSchema.SchemaName);
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public CopilotChatPart SendUserMessage(string content, Guid? copilotSessionId = null,
				CopilotContext copilotContext = null, CopilotSendMessageOptions options = null) {
			_userConnection.DBSecurityEngine.CheckCanExecuteOperation(CanRunCopilotOperation);
			CopilotSession session = null;
			if (copilotSessionId.HasValue && copilotSessionId.Value.IsNotEmpty()) {
				session = _sessionManager.FindById(copilotSessionId.Value);
			}
			if (session == null) {
				session = _sessionManager.CreateSession(CopilotSessionType.Chat, copilotSessionId);
				_sessionManager.Add(session);
			}
			if (TryFindTriggeredIntent(session, options?.RootIntentIds, out var agentToolExecutor)) {
				agentToolExecutor.TriggerExecution(session);
			}
			var userMessage = CopilotMessage.FromUser(content);
			if (options?.MessageId != null && options.MessageId.Value != Guid.Empty) {
				userMessage.Id = options.MessageId.Value;
			}
			session.AddMessage(userMessage);
			var lastMessageDate = userMessage.Date;
			string errorMessage = null;
			string errorCode = null;
			try {
				SendSession(session, copilotContext);
			} catch (Exception e) {
				(errorMessage, errorCode) = GetErrorInfo(e);
				_log.Error(e);
			} finally {
				_copilotMsgChannelSender.SendSessionProgress(CopilotSessionProgress.Create(_userConnection, session,
					CopilotSessionProgressStates.WaitingForUserMessage), session.UserId);
			}
			List<BaseCopilotMessage> lastMessages = session.Messages
				.Where(message => message.Date >= lastMessageDate)
				.Cast<BaseCopilotMessage>().ToList();
			return new CopilotChatPart(lastMessages, session, errorMessage, errorCode);
		}

		/// <inheritdoc/>
		public void CompleteAction(Guid copilotSessionId, string actionInstanceUId,
				CopilotActionExecutionResult result) {
			CopilotSession session = _sessionManager.FindById(copilotSessionId);
			if (session?.State != CopilotSessionState.Active || session.IsTransient == true) {
				return;
			}
			string resultContent = result.Status == CopilotActionExecutionStatus.Completed
				? result.Response ?? "Ok"
				: result.ErrorMessage ?? "Unknown error occurred";
			List<CopilotMessage> toolMessages = session.CreateToolCallMessages(actionInstanceUId, resultContent);
			try {
				HandleToolCallsCompleted(toolMessages, session);
			} catch (Exception e) {
				_log.Error(e);
				throw;
			} finally {
				_copilotMsgChannelSender.SendSessionProgress(CopilotSessionProgress.Create(_userConnection, session,
					CopilotSessionProgressStates.WaitingForUserMessage), session.UserId);
			}
		}

		/// <inheritdoc/>
		public async Task<CopilotIntentCallResult> ExecuteIntentAsync(CopilotIntentCall call,
				CancellationToken token = default) {
			call.CheckArgumentNull(nameof(call));
			var response = new CopilotIntentCallResult {
				Warnings = new List<string>()
			};
			if (!ValidateApiSkillForExecution(call, response, out CopilotIntentSchema intent)) {
				return response;
			}
			bool useJsonSchema = Features.GetIsEnabled<GenAIFeatures.UseJsonSchemaForApiOutputParameters>();
			Guid requestId = Guid.Empty;
			CopilotSession session = null;
			try {
				session = CreateApiSkillSession(call, intent, response, useJsonSchema);
				JsonSchema jsonSchema = useJsonSchema
					? _outputParametersHandler.GetOutputParametersJsonSchema(intent)
					: null;
				requestId = await CompleteIntentAsync(session, response, jsonSchema, token).ConfigureAwait(false);
				if (!IsAdvancedOutputParametersMode(useJsonSchema, intent)) {
					ParseAndHandleOutputParameters(response, intent);
				}
			} catch (Exception e) {
				SetErrorResponse(response, e.GetFullMessage());
			} finally {
				if (session != null) {
					_sessionManager.CloseSession(session, requestId);
				}
			}
			return response;
		}

		[Obsolete]
		public IList<string> GetAvailableIntents(CopilotIntentMode mode, params string[] names) {
			return GetAvailableSkills(mode, names);
		}

		/// <inheritdoc/>
		public IList<string> GetAvailableSkills(CopilotIntentMode mode, params string[] names) {
			if ((mode == CopilotIntentMode.Api && !CanRunCopilotApi()) ||
					(mode == CopilotIntentMode.Chat && !CanUseCopilotChat())) {
				return new List<string>();
			}
			IEnumerable<CopilotIntentSchema> activeIntents = mode == CopilotIntentMode.Api ?
				FindSkillsForApi() :
				FindSkillsForChat(null);
			IList<string> availableIntentNames = names.Any() ?
				FilterActiveIntents(names, activeIntents).ToList() :
				activeIntents.Select(intent => intent.Name).ToList();
			return availableIntentNames;
		}

		/// <inheritdoc/>
		public CopilotIntentCallResult ExecuteIntent(CopilotIntentCall call) {
			return AsyncPump.Run(() => ExecuteIntentAsync(call, CancellationToken.None));
		}

		#endregion

	}

}

