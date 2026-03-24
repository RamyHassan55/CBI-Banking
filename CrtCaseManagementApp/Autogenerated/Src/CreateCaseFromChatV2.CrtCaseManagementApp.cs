namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration.Omnichannel.Messaging;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: CreateCaseFromChatV2MethodsWrapper

	/// <exclude/>
	public class CreateCaseFromChatV2MethodsWrapper : ProcessModel
	{

		public CreateCaseFromChatV2MethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			UserConnection userConnection = Get<UserConnection>("UserConnection");
			Guid chatId = Get<Guid>("ChatId");
			var chatProvider = new OmnichannelChatProvider(userConnection);
			var messages = chatProvider.GetChatMessages(chatId).ToList();
			var firstMessage = messages.First();
			var messagesBeforeAnswer = messages.TakeWhile((m) => m.Direction == firstMessage.Direction);
			Set("CaseSubject", firstMessage.Text);
			Set("CaseDescription", string.Join("\n", messagesBeforeAnswer.Select((m) => m.Text)));
			return true;
		}

		#endregion

	}

	#endregion

}

