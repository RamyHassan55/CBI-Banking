namespace Creatio.Copilot
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using Creatio.FeatureToggling;
	using global::Common.Logging;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Store;
	using ItemNotFoundException = Terrasoft.Configuration.ItemNotFoundException;
	
	[DefaultBinding(typeof(ICopilotSessionManager))]
	internal class CopilotSessionManager: ICopilotSessionManager
	{

		#region Constants: Private

		private const string KeyPrefix = "copilotSession_";
		private const string SystemPromptCode = "System";
		private const string ApiSystemPromptCode = "ApiSystem";

		#endregion

		#region Fields: Private
		
		private static readonly IDictionary<Guid, CopilotSession> _sessions = 
			new ConcurrentDictionary<Guid, CopilotSession>();
		private static readonly ILog _log = LogManager.GetLogger("Copilot");
		private static bool _isStaticSessionCollectionInitialized;
		private readonly UserConnection _userConnection;
		private readonly ICopilotHistoryStorage _copilotHistoryStorage;
		private static readonly string _truncatedMarkerStart = "[Truncated]";
		private static readonly string _truncatedMarkerEnd = "...[Truncated]";
		private static readonly string _truncateLengthSysSettingCode = "CreatioAIMaxMessageContentLength";
		private readonly ICopilotPromptFactory _promptFactory;
		private readonly ICopilotPromptVariableResolver _variableResolver;
		private static readonly int _defaultTruncateLength = 200;
		private static readonly string _ttlSysSettingCode = "CreatioAISessionTtlHours";
		private static readonly int _defaultTtlHours = 24;
		private readonly int _sessionTtlHours;
		private static readonly CreatePromptOptions _alternativeNameInDescriptionChatPromptOptions =
			new CreatePromptOptions {
				AdditionalDirections = {
					{
						CopilotPromptFactory.GlobalSettingsSection,
						CopilotPromptFactory.ChatAdditionalGlobalSettingsSectionPrompts
					}
			}
		};

		#endregion

		#region Constructors: Public

		public CopilotSessionManager(UserConnection userConnection, ICopilotHistoryStorage copilotHistoryStorage,
				ICopilotPromptFactory promptFactory, ICopilotPromptVariableResolverFactory variableResolverFactory) {
			_userConnection = userConnection;
			_copilotHistoryStorage = copilotHistoryStorage;
			_promptFactory = promptFactory;
			_variableResolver = variableResolverFactory.Create();
			_sessionTtlHours = SysSettings.GetValue(_userConnection, _ttlSysSettingCode, _defaultTtlHours);
		}

		#endregion

		#region Properties: Private

		private static bool IsClusterMode => 
			Features.GetIsEnabled<Terrasoft.Configuration.GenAI.GenAIFeatures.CopilotEngineClusterMode>();

		#endregion

		#region Methods: Private

		private static CopilotSession FindById(Guid copilotSessionId, IDataStore dataStore) {
			string key = KeyPrefix + copilotSessionId;
			if (dataStore[key] is CopilotSession copilotSession) {
				return copilotSession;
			}
			dataStore.Remove(key);
			return null;
		}

		private CopilotSession LoadFromStorageById(Guid copilotSessionId) {
			CopilotSession copilotSession = _copilotHistoryStorage.LoadSession(copilotSessionId);
			if (copilotSession != null) {
				Add(copilotSession);
			}
			return copilotSession;
		}

		private void UnloadOldSessions() {
			DateTime lastDateOfLife = DateTime.UtcNow.AddHours(-_sessionTtlHours);
			List<CopilotSession> expiredSessions = GetSessions().Values
				.Where(s => s != null && s.LoadedOn < lastDateOfLife && s.Messages.All(m => m.Date < lastDateOfLife))
				.ToList();
			foreach (CopilotSession session in expiredSessions) {
				RemoveInDataStore(session.Id);
				_sessions.Remove(session.Id);
			}
		}

		private EntityCollection FetchCopilotPrompt(string code) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "CopilotPrompt") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				},
				Cache = _userConnection.SessionCache.WithLocalCaching("CopilotPromptCache"),
				CacheItemName = $"Copilot{code}PromptCacheItem",
				UseAdminRights = false
			};
			esq.AddColumn("Prompt");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Code", code));
			EntityCollection entityCollection = esq.GetEntityCollection(_userConnection);
			return entityCollection;
		}

		private string GetCopilotPrompt(string code, string fallbackPrompt) {
			EntityCollection entityCollection = FetchCopilotPrompt(code);
			string prompt = string.Join(Environment.NewLine,
				entityCollection?.Select(x => x?.GetTypedColumnValue<string>("Prompt") ?? string.Empty) ??
				Array.Empty<string>());
			return _variableResolver.Resolve(prompt.IsNotNullOrEmpty() ? prompt : fallbackPrompt);
		}

		private void UpdateInDataStore(CopilotSession session) {
			string key = KeyPrefix + session.Id;
			_userConnection.SessionData[key] = session;
			_userConnection.ApplicationData[key] = session;
		}

		private void RemoveInDataStore(Guid sessionId) {
			string key = KeyPrefix + sessionId;
			_userConnection.SessionData.Remove(key);
			_userConnection.ApplicationData.Remove(key);
		}

		private IDictionary<Guid, CopilotSession> GetSessions() {
			return IsClusterMode ? GetNonStaticCollection() : GetStaticCollection();
		}

		private IDictionary<Guid, CopilotSession> GetStaticCollection() {
			if (_isStaticSessionCollectionInitialized) {
				return _sessions;
			}
			Dictionary<Guid, CopilotSession> sessions = GetNonStaticCollection();
			_sessions.Clear();
			_sessions.AddRange(sessions);
			_isStaticSessionCollectionInitialized = true;
			return _sessions;
		}

		private Dictionary<Guid, CopilotSession> GetNonStaticCollection() {
			Dictionary<Guid, CopilotSession> sessions = LoadActiveSessionsFromDataStore(_userConnection.SessionData);
			sessions = sessions.IsNotNullOrEmpty()
				? sessions
				: LoadActiveSessionsFromDataStore(_userConnection.ApplicationData);
			return sessions;
		}

		private static Dictionary<Guid, CopilotSession> LoadActiveSessionsFromDataStore(IDataStore store) {
			var sessions = new Dictionary<Guid, CopilotSession>();
			IEnumerable<string> keys = store.Keys.Where(key => key.StartsWith(KeyPrefix));
			foreach (string key in keys) {
				var copilotSession = (CopilotSession)store[key];
				if (copilotSession == null || copilotSession.State == CopilotSessionState.Closed) {
					continue;
				}
				sessions[copilotSession.Id] = copilotSession;
			}
			return sessions;
		}

		private void Update(CopilotSession session) {
			if (session.State == CopilotSessionState.Closed) {
				RemoveInDataStore(session.Id);
			} else {
				UpdateInDataStore(session);
			}
			try {
				_copilotHistoryStorage.SaveSession(session);
			} catch (Exception e) {
				_log.Error($"Can't save session with id {session.Id}", e);
			}
		}

		private CopilotSession CreateSession(string systemPrompt) {
			CopilotMessage message = CopilotMessage.FromSystem(systemPrompt);
			message.IsFromSystemPrompt = true;
			var messages = new List<CopilotMessage> { message };
			return new CopilotSession(_userConnection.CurrentUser.Id, messages, null);
		}

		private string GetSystemPrompt(CopilotSessionType sessionType) {
			if (sessionType == CopilotSessionType.Api) {
				string apiPrompt = _promptFactory.CreateSystemPrompt(SystemPromptTarget.Api);
				return GetCopilotPrompt(ApiSystemPromptCode, apiPrompt);
			}
			bool shouldHaveAlternativeNameInstructions =
				Features.GetIsEnabled<Terrasoft.Configuration.GenAI.GenAIFeatures.AddCaptionToDescription>();
			CreatePromptOptions options = shouldHaveAlternativeNameInstructions
				? _alternativeNameInDescriptionChatPromptOptions
				: null;
			string chatPrompt = _promptFactory.CreateSystemPrompt(SystemPromptTarget.Chat, options);
			return GetCopilotPrompt(SystemPromptCode, chatPrompt);
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		public CopilotSession CreateSession(CopilotSessionType sessionType, Guid? sessionId = null) {
			string systemPrompt = GetSystemPrompt(sessionType);
			var session = CreateSession(systemPrompt);
			if (sessionId.HasValue) {
				session.Id = sessionId.Value;
			}
			session.IsTransient = sessionType == CopilotSessionType.Api;
			return session;
		}

		/// <inheritdoc />
		public CopilotSession Add(CopilotSession copilotSession) {
			if (!IsClusterMode) {
				IDictionary<Guid, CopilotSession> sessions = GetStaticCollection();
				sessions[copilotSession.Id] = copilotSession;
			}
			UpdateInDataStore(copilotSession);
			UnloadOldSessions();
			return copilotSession;
		}

		/// <inheritdoc />
		public void Update(CopilotSession copilotSession, Guid? requestId) {
			if (requestId.HasValue) {
				copilotSession.Messages.Where(msg => msg.CopilotRequestId.IsNullOrEmpty()).ForEach(msg => {
					msg.CopilotRequestId = requestId;
				});
			}
			int truncateLength = SysSettings.GetValue(_userConnection, _truncateLengthSysSettingCode,
				_defaultTruncateLength);
			copilotSession.Messages.Where(msg => msg.TruncateOnSave &&
					msg.Content?.Length >= truncateLength && !msg.Content.StartsWith(_truncatedMarkerStart))
				.ForEach(msg => {
					msg.Content = _truncatedMarkerStart + msg.Content.Substring(0, truncateLength)
						+ _truncatedMarkerEnd;
				});
			Update(copilotSession);
		}

		/// <inheritdoc />
		public CopilotSession FindById(Guid copilotSessionId) {
			CopilotSession copilotSession;
			if (!IsClusterMode) {
				IDictionary<Guid, CopilotSession> sessions = GetSessions();
				if (sessions.TryGetValue(copilotSessionId, out copilotSession)) {
					return copilotSession;
				}
			}
			copilotSession = FindById(copilotSessionId, _userConnection.SessionData) ??
				FindById(copilotSessionId, _userConnection.ApplicationData) ??
				LoadFromStorageById(copilotSessionId);
			return copilotSession;
		}

		/// <inheritdoc />
		public CopilotSession GetById(Guid copilotSessionId) {
			CopilotSession copilotSession = FindById(copilotSessionId);
			if (copilotSession == null) {
				throw new ItemNotFoundException(_userConnection, copilotSessionId.ToString(), nameof(CopilotSession));
			}
			return copilotSession;
		}

		/// <inheritdoc />
		public IEnumerable<CopilotSession> GetActiveSessions(Guid userId) {
			IDictionary<Guid, CopilotSession> sessions = GetSessions();
			return sessions.Values
				.Where(session => session != null && session.UserId == userId
					&& session.State == CopilotSessionState.Active)
				.OrderByDescending(session => session.StartDate);
		}

		/// <inheritdoc />
		public void CloseSession(CopilotSession copilotSession, Guid? requestId) {
			copilotSession.State = CopilotSessionState.Closed;
			copilotSession.EndDate = DateTime.UtcNow;
			Update(copilotSession, requestId);
		}

		/// <inheritdoc />
		public void RenameSession(CopilotSession copilotSession, string sessionName) {
			copilotSession.Title = sessionName;
			Update(copilotSession);
		}

		#endregion

	}

}


