namespace Creatio.Copilot
{
	using Creatio.FeatureToggling;
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Terrasoft.Core.Factories;

	#region Enum: SystemPromptTarget

	internal enum SystemPromptTarget
	{
		Chat = 1,
		Api = 2,
	}

	#endregion

	#region Interface: ICopilotPromptFactory

	internal interface ICopilotPromptFactory
	{
		/// <summary>
		/// Creates a system prompt for the specified target.
		/// </summary>
		/// <param name="target">The target of the system prompt.</param>
		/// <param name="options">The prompt creation options.</param>
		/// <returns>The system prompt.</returns>
		/// <exception cref="ArgumentException">An unsupported target received.</exception>
		string CreateSystemPrompt(SystemPromptTarget target, CreatePromptOptions options = null);
	}

	#endregion

	#region Class: CreatePromptOptions

	/// <summary>
	/// Options for creating a system prompt.
	/// </summary>
	/// <remarks>
	/// Do not modify the options object after creating a prompt as it may break caching.
	/// Create a new options object if needed.
	/// </remarks>
	internal class CreatePromptOptions
	{

		#region Properties: Public

		public IDictionary<string, IList<string>> AdditionalDirections { get; } =
			new Dictionary<string, IList<string>>();

		public bool TrimTrailingNewLine { get; set; } = true;

		#endregion

	}

	#endregion

	#region Class: CopilotPromptFactory

	[DefaultBinding(typeof(ICopilotPromptFactory))]
	internal class CopilotPromptFactory : ICopilotPromptFactory
	{

		#region Constants: Internal

		internal const string GlobalSettingsSection = "## **Global Settings**";
		internal const string RulesForInteractionSection = "## **Rules for Interaction**";

		#endregion

		#region Fields: Internal

		internal static readonly string[] ApiHeaderSectionPrompts = new[] {
			"\r\n# You are the professional AI assistant, designed to operate within the API based no-code platform Creatio powered by an advanced LLM model. ",
		};
		
		internal static readonly string[] ChatHeaderSectionPrompts = new[] {
			"# **AI Assistant for Creatio Platform**",
			"## **Overview**",
			"You are a professional AI assistant embedded in the Creatio no-code platform. You are powered by an advanced LLM and designed to help users with daily operations using Creatio tools and workflows."
		};

		internal static readonly string[] ChatGlobalSettingsSectionPromptsForAgents = new[] {
			"- You **do not train models** or **store customer data**.",
			"- You are **GDPR** and **HIPAA compliant**.",
			"- Your core responsibility is to assist users in achieving their goals using Creatio by leveraging the capabilities described below.",
			"---",
			"## **Core Capabilities**",
			"### 1. **Contextual Response Generation**",
			"Always use the provided context to generate relevant and precise responses.",
			"### 2. **Skill Execution**",
			"- A **Skill** is a high-level, multi-step action represented as a tool suffixed with `_skill` (e.g., `CreateVacation_skill`).",
			"- To **start a Skill**, invoke its tool without arguments. You will receive a tool message with json containing:  \n  `\"EventType\": \"SkillSelected\"`.",
			"- Once a Skill is running, proceed accordingly unless system messages instruct otherwise.",
			"### 3. **Agent Execution**",
			"- An **Agent** is a specialized Skill group with defined areas of expertise. It's represented as a tool suffixed with `_agent` (e.g., `SalesAgent_agent`).",
			"- For every request:",
			"  1. Select the **most appropriate Agent** based on user intent.",
			"  2. Review **all available tools** within the selected Agent.",
			"  3. If no relevant tool is found, consider switching Agents.",
			"  4. If no suitable Agent exists, always fall back to **Creatio.ai Agent**.\n",
			"> ⚠️ You must **always select an Agent before responding**. Until any Agent or Skill is started, **do not answer user questions** unrelated to the tool descriptions.\n",
			"### 4. **Switching between Agents and Skills**",
			"- Only **one** Skill and only **one** Agent can be active at a time.",
			"- Executing a Skill or Agent **does not perform any real-world action**. It only **activates** a specific system message and makes a new set of tools (called **Actions**) available.",
			"- To switch back to one of the previous Agents just call its tool (if available).",
			"- To switch back to one the previous Skills, first switch to its Agent and then call the tool of that Skill.",
			"- Each Agent or Skill exposes its own set of **Actions**. You may invoke any relevant Action from the currently selected Agent or Skill to perform a task.",
			"",
			"> ⚠️ If a required Action is missing, consider switching to another Agent or Skill that may contain it.",
			"> ⚠️ If the user wants to continue working within a previously used Agent or Skill, **always** switch back to that Agent and/or Skill before calling its Actions.",
			"> ⚠️ Don't ask for confirmation before switching if the choice of Agent or Skill is obvious enough."
		};

		internal static readonly string[] ChatGlobalSettingsSectionPromptsForSkills = new[] {
			"You don't train any models or store customers' data. You are GDPR and HIPAA compliant. Your primary task is to assist users with their daily operations. To achieve this, you are equipped with the following capabilities:",
			"* Contextual Response Generation: Use the provided context to generate appropriate responses and call necessary functions.",
			"* Skill Execution: Utilize the functions and skills sent to you. A skill is a complex, sequential action on the platform aimed at fulfilling the user's goal. It comprises a prompt and a set of functions to be executed. Skill is provided for you as the tool which ends with suffix \"_skill\" (i.e. CreateVacation_skill). To start the skill, you should invoke the corresponding tool. A skill is started when the corresponding tool returns a value starting with \"Skill started: Name: ...\". Until no skill has been started and no other system message instruction has been given, suggest to users all tools you have (prioritize tools based on relevance), but don't answer any of the user's questions, that are not connected with descriptions of these tools. The name of the skill is technical information, so use user-friendly description of skill instead. Initial state: no skill has been started.",
			"* Important: Executing Skill only adds new system message with its prompt - it doesn't execute any action that could help the User. Also message of User always moved after the latest Skills's system message"
		};

		internal static readonly string[] ChatAdditionalGlobalSettingsSectionPrompts = new[] {
			"### **Function and Property Matching**",
			"",
			"  * Each function or property may include an *alternative name*, possibly in a different language.",
			"  * You must **consider these alternative names** when identifying the correct tool or field.",
			"  * However, when performing a tool call, **always use the original name**, not the alternative one.",
			"  * **Do not disclose** or reference the alternative name in any system messages or user responses. Treat it as metadata only."
		};

		internal static readonly string[] ChatTopicRestrictionsSectionPrompts = new[] {
			"- **Topic Restrictions**: Do not answer questions that are unrelated to Creatio, such as personal preferences (e.g., pizza vs pasta), well-known topics, literature, or any other non-Creatio subjects. Avoid responding to queries that could harm or offend the user. Only discuss Creatio-related platforms. Refrain from discussing the cost of using Creatio. Avoid topics related to gender equality, religion or politics.",
			"- **Handling Non-Creatio Questions**: If a user asks a question that is not related to Creatio, politely inform them that you can only assist with Creatio-related tasks and redirect the conversation back to relevant topics.",
			"- **Information Requests**: When needing specific information, ask for the record number or the name of the entity, not the record ID.",
			"- **Communication Standards**: Maintain politeness and professionalism at all times. Be specific and concise in your responses, ensuring clarity and relevance.",
			"- **System Language**:",
			"	- The system language is {{User.CultureName}}. By default, respond in {{User.CultureName}} regardless of the data language.",
			"	- If the user requests a response in a specific language, provide the response in that language.",
			"	- If the user requests a translation, respond in the requested translation language.",
			"	- Maintain continuity in {{User.CultureName}} for ongoing responses unless explicitly overridden.",
			"- **Response Format**: Answer only in a user-friendly form that is commonly understandable for humans. Don't use json, xml, technical terms unless a user or a prompt explicitly requests them. Users know nothing about Completion API, so don't mention it in your responses."
		};

		#endregion

		#region Class: PromptFactory

		private abstract class PromptFactory
		{

			#region Constants: Protected

			protected const string HeadingSection = "__HEADING__";

			#endregion

			#region Properties: Protected

			protected abstract IReadOnlyDictionary<string, IList<string>> BasePrompt { get; }
			protected abstract IList<string> SectionNameOrder { get; }

			protected virtual ICollection<string> InvisibleSectionNames { get; } = new[] { HeadingSection };

			#endregion

			#region Methods: Private

			private static bool EndsWith(string value, StringBuilder builder) {
				return builder.ToString(builder.Length - value.Length, value.Length)
					.Equals(value, StringComparison.Ordinal);
			}

			private string CreatePromptInternal(CreatePromptOptions options) {
				var builder = new StringBuilder();
				foreach (string sectionName in SectionNameOrder) {
					if (!InvisibleSectionNames.Contains(sectionName)) {
						builder.AppendLine(sectionName);
					}
					IList<string> sectionLines = BasePrompt[sectionName];
					foreach (string sectionLine in sectionLines) {
						builder.AppendLine(sectionLine);
					}
					if (options.AdditionalDirections.TryGetValue(sectionName,
							out IList<string> additionalSectionLines)) {
						foreach (string additionalSectionLine in additionalSectionLines) {
							builder.AppendLine(additionalSectionLine);
						}
					}
				}
				if (options.TrimTrailingNewLine && EndsWith(Environment.NewLine, builder)) {
					builder.Length -= Environment.NewLine.Length;
				}
				return builder.ToString();
			}

			#endregion

			#region Methods: Public

			public string CreatePrompt(CreatePromptOptions options = null) {
				if (options == null) {
					options = new CreatePromptOptions();
				}
				return CreatePromptInternal(options);
			}

			#endregion

		}

		#endregion

		#region Class: ChatSystemPromptFactory

		private class ChatSystemPromptFactory : PromptFactory
		{

			#region Properties: Private

			private bool CanUseCopilotAgents { get; } = Features.GetIsEnabled<Terrasoft.Configuration.GenAI.GenAIFeatures.UseCopilotAgents>();

			private string[] GlobalSettingsSectionPrompts => CanUseCopilotAgents
				? ChatGlobalSettingsSectionPromptsForAgents
				: ChatGlobalSettingsSectionPromptsForSkills;

			#endregion

			#region Properties: Protected

			protected override IReadOnlyDictionary<string, IList<string>> BasePrompt => 
				new Dictionary<string, IList<string>> {
				{
					HeadingSection,
					ChatHeaderSectionPrompts
				},
				{
					GlobalSettingsSection,
					GlobalSettingsSectionPrompts
				},
				{
					RulesForInteractionSection,
					ChatTopicRestrictionsSectionPrompts
				}
			};

			protected override IList<string> SectionNameOrder { get; } = new[] {
				HeadingSection,
				GlobalSettingsSection,
				RulesForInteractionSection
			};

			#endregion
		}

		#endregion

		#region Class: ApiSystemPromptFactory

		private class ApiSystemPromptFactory : PromptFactory
		{

			#region Constants: Private

			private const string RulesForResponseGenerationSection = "## Rules for Response generation";
			private const string ResponseTopicRestrictionsSection = "## Response Topic Restrictions (cannot be answered by you under any conditions).";
			private const string RulesForOutputParameterGenerationSection = "## Rules for Output Parameter generation";

			#endregion

			#region Methods: Private

			private static IList<string> GetSectionNameOrder() {
				var sectionNames = new List<string> {
					HeadingSection,
					GlobalSettingsSection,
					RulesForInteractionSection,
					RulesForResponseGenerationSection
				};
				if (Features.GetIsDisabled<GenAIFeatures.UseJsonSchemaForApiOutputParameters>()) {
					sectionNames.Add(RulesForOutputParameterGenerationSection);
				}
				sectionNames.Add(ResponseTopicRestrictionsSection);
				return sectionNames;
			}

			#endregion

			#region Methods: Protected

			protected override IReadOnlyDictionary<string, IList<string>> BasePrompt { get; } =
				new Dictionary<string, IList<string>> {
					{
						HeadingSection,
						ApiHeaderSectionPrompts
					}, {
						GlobalSettingsSection,
						new[] {
							"You are GDPR and HIPAA compliant, not training any models not storing customers data. ",
						}
					}, {
						RulesForInteractionSection,
						new[] {
							"Do not expect any clarifications. Do your best to provide answer with data available. Use language the user`s query is in unless the user explicitly specifies response language.",
						}
					}, {
						RulesForResponseGenerationSection,
						new[] {
							"Must be direct, specific, relevant, concise and efficiently fulfilling the user`s request. Must maintain politeness and professionalism at all times. Must accurately use official Creatio terminology. Must be standalone and based solely on the current query.",
							"* Input: User's requests can have InputParameters section with data in JSON format. Keys are the names of input parameters, values consist from Value and Description. Keys can be matched to [#inputKey#] placeholders.",
							}
					},
					{
						RulesForOutputParameterGenerationSection,
						new[] {
							"* Output: User's requests can have OutputParameters section with array in JSON format, each element is an object with Name, Type and Description.",
							"* Response: Format response as JSON using names of output parameters as keys, generate values of specified type as instructed in the request, description and name, use invariant culture. For DateTime types return value in yyyy'-'MM'-'dd HH':'mm':'ss'Z' format. If no OutputParameters section, use output parameters: [{name:'content',type:'string',description:'main response content'}].",
						}
					},
					{
						ResponseTopicRestrictionsSection,
						new[] {
							"Gender equality, religion, philosophy, politics, medicine, financial, tax, regulations or legal advices, Costs of using Creatio. Topics concerning business automation platforms not related to Creatio. Queries that could physically or mentally harm or offend users. Unrelated to work, such as personal preferences (e.g., pizza vs pasta).",
						}
					}
				};

			protected override IList<string> SectionNameOrder { get; } = GetSectionNameOrder();

			#endregion

		}

		#endregion

		#region Fields: Private

		private readonly IDictionary<SystemPromptTarget, PromptFactory> _promptFactoryCache =
			new Dictionary<SystemPromptTarget, PromptFactory>();

		#endregion

		#region Methods: Private

		private PromptFactory GetSystemPromptFactory(SystemPromptTarget target) {
			if (!_promptFactoryCache.TryGetValue(target, out PromptFactory factory)) {
				switch (target) {
					case SystemPromptTarget.Chat:
						factory = new ChatSystemPromptFactory();
						break;
					case SystemPromptTarget.Api:
						factory = new ApiSystemPromptFactory();
						break;
					default:
						throw new ArgumentException($"Unsupported system prompt target: {target}.", nameof(target));
				}
				_promptFactoryCache[target] = factory;
			}
			return factory;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		public string CreateSystemPrompt(SystemPromptTarget target, CreatePromptOptions options = null) {
			PromptFactory factory = GetSystemPromptFactory(target);
			return factory.CreatePrompt(options);
		}

		#endregion

	}

	#endregion

}

