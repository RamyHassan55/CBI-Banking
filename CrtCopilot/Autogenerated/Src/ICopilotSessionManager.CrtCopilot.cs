namespace Creatio.Copilot
{
	using System;
	using System.Collections.Generic;

	#region Interface: ICopilotSessionManager

	/// <summary>
	/// Interface for managing Copilot sessions.
	/// </summary>
	public interface ICopilotSessionManager
	{

		#region Methods: Public

		/// <summary>
		/// Creates a new session instance. Does not add it to cache.
		/// </summary>
		/// <param name="sessionType">The session mode. Affects which system prompt is chosen.</param>
		/// <param name="sessionId">Optional session identifier. If null, a new identifier will be generated.</param>
		/// <returns>A new instance of <see cref="CopilotSession"/>.</returns>
		CopilotSession CreateSession(CopilotSessionType sessionType, Guid? sessionId = null);

		/// <summary>
		/// Attach session to active session list.
		/// </summary>
		CopilotSession Add(CopilotSession copilotSession);

		/// <summary>
		/// Updates copilot session.
		/// </summary>
		/// <param name="copilotSession">Copilot session to update.</param>
		/// <param name="requestId">Optional Completion API request identifier.</param>
		void Update(CopilotSession copilotSession, Guid? requestId);

		/// <summary>
		/// Finds copilot session by identifier.
		/// </summary>
		CopilotSession FindById(Guid copilotSessionId);

		/// <summary>
		/// Gets copilot session by identifier.
		/// </summary>
		CopilotSession GetById(Guid copilotSessionId);

		/// <summary>
		/// Gets active copilot sessions for the user.
		/// </summary>
		IEnumerable<CopilotSession> GetActiveSessions(Guid userId);

		/// <summary>
		/// Closes copilot session.
		/// </summary>
		void CloseSession(CopilotSession copilotSession, Guid? requestId);

		/// <summary>
		/// Renames copilot session.
		/// </summary>
		/// <param name="copilotSession">Session to rename.</param>
		/// <param name="sessionName">New session name.</param>
		void RenameSession(CopilotSession copilotSession, string sessionName);

		#endregion

	}

	#endregion

} 
