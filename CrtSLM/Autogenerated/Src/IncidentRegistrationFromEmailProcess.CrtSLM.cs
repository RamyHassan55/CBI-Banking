namespace Terrasoft.Core.Process
{

	using Creatio.FeatureToggling;
	using global::Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: IncidentRegistrationFromEmailProcessMethodsWrapper

	/// <exclude/>
	public class IncidentRegistrationFromEmailProcessMethodsWrapper : ProcessModel
	{

		public IncidentRegistrationFromEmailProcessMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("RegisterIncidentScriptTaskExecute", RegisterIncidentScriptTaskExecute);
		}

		#region Methods: Private

		private bool RegisterIncidentScriptTaskExecute(ProcessExecutingContext context) {
			try {
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]|Process was started", Get<Guid>("RecordId"));
				lock (_syncLock) {
				    Guid recordId = Get<Guid>("RecordId");
				    var userConnection = Get<UserConnection>("UserConnection");
				    var emailsList = GetActivityRecordsId(recordId);
					var activitySchema = userConnection.EntitySchemaManager.FindInstanceByName("Activity");
				    foreach (var email in emailsList) {
				        _log.InfoFormat(@"[EmailSyncSessionId:{1}]|Start register case for ActivityId:{0}", email.ActivityId, Get<Guid>("RecordId"));
				        try {
				        	if(email.CaseId == default(Guid)) {
								var CaseIdFromCurrentChain = emailsList.FirstOrDefault(e => 
									e.ConversationId != default(Guid) && e.ConversationId == email.ConversationId && e.CaseId != default(Guid));
								if (CaseIdFromCurrentChain != null && CaseIdFromCurrentChain.CaseId != default(Guid)) {
									email.CaseId = CaseIdFromCurrentChain.CaseId;
									Entity entity = activitySchema.CreateEntity(userConnection);
									if (entity.FetchFromDB(email.ActivityId)) {
										entity.SetColumnValue("CaseId", email.CaseId);
										_log.InfoFormat(@"[EmailSyncSessionId:{1}]| Case wasn't created during this process activity with ActivityId:{0},
						                    bound to CaseId:{2}", email.ActivityId, Get<Guid>("RecordId"), email.CaseId);
										entity.Save();
									}
								} else {
					        		email.CaseId = RegisterIncidentFromEmail(email.ActivityId);
								}
				        		if(email.CaseId != default(Guid)) {
					        		var parentEmails = emailsList.Where(emailRecord =>
				            			 emailRecord.ParentMessageId == email.Id);
					        		foreach(var parentEmail in parentEmails) {
						        		parentEmail.CaseId = email.CaseId;
						                Entity entity = activitySchema.CreateEntity(userConnection);
						                if (entity.FetchFromDB(parentEmail.ActivityId)) {
						                	 entity.SetColumnValue("CaseId", email.CaseId);
						                    _log.InfoFormat(@"[EmailSyncSessionId:{1}]| Case was created during this process activity with ActivityId:{0},
						                    set CaseId:{2}", parentEmail.ActivityId, Get<Guid>("RecordId"), parentEmail.CaseId);
						                    entity.Save(false);
						                }
					        		}
				        		}
				        	}
				        } catch (Exception ex) {
							_log.ErrorFormat(@"[EmailSyncSessionId:{0}]| Message: {1}, CallStack: {2}, InnerException: {3}",
							    Get<Guid>("RecordId"), ex.Message, ex.StackTrace, ex.InnerException);
						}
				        _log.InfoFormat(@"[EmailSyncSessionId:{1}]|End register case for ActivityId:{0}", email.ActivityId, Get<Guid>("RecordId"));
				    }
				}
				_log.InfoFormat(@"[EmailSyncSessionId:{0}]|Process was finished", Get<Guid>("RecordId"));
			} catch (Exception ex) {
			                _log.ErrorFormat(@"[EmailSyncSessionId:{0}]| Message: {1}, CallStack: {2}, InnerException: {3}",
			                    Get<Guid>("RecordId"), ex.Message, ex.StackTrace, ex.InnerException);
			            }
			return true;
		}

			private static readonly ILog _log = LogManager.GetLogger("IncidentRegistration");
			private static readonly object _syncLock = new object();
			
			private Select GetOldSelectWithFilterBySendStatus(Guid finishedSyncSessionId, DateTime lowerDate, List<Guid> activityParticipantRolesList) {
							var userConnection = Get<UserConnection>("UserConnection");
							return new Select(userConnection).Distinct()
																.Column("emd", "ActivityId")
																.Column("a", "CaseId")
																.Column("cs", "IsFinal")
																.Column("emd", "Id")
																.Column("pc", "Id").As("ParentCaseId")
																.Column("csp", "IsFinal").As("IsParentCaseFinal")
																.Column("emd", "ParentMessageId")
																.Column("emd", "ConversationId")
																.Column("a", "SendDate")
															.From("EmailMessageData").As("emd")
																.InnerJoin("FinishedSyncSession").As("fss")
																	.On("fss", "SyncSessionId").IsEqual("emd", "SyncSessionId")
																.InnerJoin("Activity").As("a")
																	.On("a", "Id").IsEqual("emd", "ActivityId")
																.InnerJoin("EmailSendStatus").As("ess")
																	.On("ess", "Id").IsEqual("a", "EmailSendStatusId")
																.InnerJoin("MailboxForIncidentRegistration").As("im")
																	.On("im", "MailboxSyncSettingsId").IsEqual("emd", "MailboxSyncSettings")
																.LeftOuterJoin("Case").As("c").
																	On("c", "Id").IsEqual("a", "CaseId")
																.LeftOuterJoin("CaseStatus").As("cs")
																	.On("c", "StatusId").IsEqual("cs", "Id")
																.LeftOuterJoin("Case").As("pc").
																	On("pc", "Id").IsEqual("c", "ParentCaseId")
																.LeftOuterJoin("CaseStatus").As("csp")
																	.On("pc", "StatusId").IsEqual("csp", "Id")
															.Where("fss", "Id").IsEqual(Column.Const(finishedSyncSessionId))
																.And("ess", "Code").IsEqual(Column.Const("Sended"))
																.And("emd", "RoleId").In(Column.Parameters(activityParticipantRolesList))
																.And("a", "ServiceProcessed").IsEqual(Column.Parameter(false))
																.And("a", "SendDate").IsGreaterOrEqual(Column.Const(lowerDate))
																.OrderByAsc("a", "SendDate") as Select;
			}
			
			protected IList<EmailRegistrationData> GetActivityRecordsId(Guid finishedSyncSessionId) {
				var userConnection = Get<UserConnection>("UserConnection");
				var maxSendDate = (int)Terrasoft.Core.Configuration.SysSettings.GetValue<int>(userConnection,
				"MaxEmailSendDateForCaseReg", 10);
				var lowerDate = DateTime.UtcNow.AddDays(-maxSendDate);
				var activityParticipantRolesList = new List<Guid> { CaseServiceConsts.ActivityParticipantRoleTo,
					CaseServiceConsts.ActivityParticipantRoleCc, CaseServiceConsts.ActivityParticipantRoleBcc };
				Select selectQuery = Features.GetIsEnabled("UseFilterBySendStatusInCaseRegistrationProcess") 
								? GetOldSelectWithFilterBySendStatus(finishedSyncSessionId, lowerDate, activityParticipantRolesList) 
								: new Select(userConnection).Distinct()
															.Column("emd", "ActivityId")
															.Column("a", "CaseId")
															.Column("cs", "IsFinal")
															.Column("emd", "Id")
															.Column("pc", "Id").As("ParentCaseId")
															.Column("csp", "IsFinal").As("IsParentCaseFinal")
															.Column("emd", "ParentMessageId")
															.Column("emd", "ConversationId")
															.Column("a", "SendDate")
														.From("EmailMessageData").As("emd")
															.InnerJoin("FinishedSyncSession").As("fss")
																.On("fss", "SyncSessionId").IsEqual("emd", "SyncSessionId")
															.InnerJoin("Activity").As("a")
																.On("a", "Id").IsEqual("emd", "ActivityId")
															.InnerJoin("MailboxForIncidentRegistration").As("im")
																.On("im", "MailboxSyncSettingsId").IsEqual("emd", "MailboxSyncSettings")
															.LeftOuterJoin("Case").As("c").
																On("c", "Id").IsEqual("a", "CaseId")
															.LeftOuterJoin("CaseStatus").As("cs")
																.On("c", "StatusId").IsEqual("cs", "Id")
															.LeftOuterJoin("Case").As("pc").
																On("pc", "Id").IsEqual("c", "ParentCaseId")
															.LeftOuterJoin("CaseStatus").As("csp")
																.On("pc", "StatusId").IsEqual("csp", "Id")
														.Where("fss", "Id").IsEqual(Column.Const(finishedSyncSessionId))
															.And("emd", "RoleId").In(Column.Parameters(activityParticipantRolesList))
															.And("a", "ServiceProcessed").IsEqual(Column.Parameter(false))
															.And("a", "SendDate").IsGreaterOrEqual(Column.Const(lowerDate))
															.OrderByAsc("a", "SendDate") as Select;
						IList<EmailRegistrationData> result = new List<EmailRegistrationData>();
				selectQuery.BuildParametersAsValue = true;
				_log.DebugFormat(@"[EmailSyncSessionId:{1}]|Select activity for registration Sql text {0}", selectQuery.GetSqlText(), Get<Guid>("RecordId"));
				using (DBExecutor dbExec = userConnection.EnsureDBConnection()) {
					var casesToReopen = new List<ActivityCaseValuePair>();
					using (IDataReader reader = selectQuery.ExecuteReader(dbExec)) {
						while (reader.Read()) {
							bool isCaseEmptyOrFinal = reader.GetColumnValue<Guid>("CaseId") == default(Guid) || reader.GetColumnValue<bool>("IsFinal") == true;
							bool isParentCaseEmptyOrFinal = reader.GetColumnValue<Guid>("ParentCaseId") == default(Guid) || reader.GetColumnValue<bool>("IsParentCaseFinal") == true;
							_log.InfoFormat(@"[EmailSyncSessionId:{0}]| isCaseEmptyOrFinal {1}, isParentCaseEmptyOrFinal {2}", Get<Guid>("RecordId"), isCaseEmptyOrFinal, isParentCaseEmptyOrFinal);
							if (isCaseEmptyOrFinal && isParentCaseEmptyOrFinal) {
								Guid activityId = reader.GetColumnValue<Guid>("ActivityId");
								var emailMessage = new EmailRegistrationData(activityId, reader.GetColumnValue<Guid>("Id"),
								 reader.GetColumnValue<Guid>("ParentMessageId"));
								 emailMessage.ConversationId = reader.GetColumnValue<Guid>("ConversationId");
								result.Add(emailMessage);
								_log.InfoFormat(@"[EmailSyncSessionId:{1}]|Add new activity with ActivityId:{0} in registration collection", activityId, Get<Guid>("RecordId"));
							} else {
								casesToReopen.Add(new ActivityCaseValuePair() {
									CaseId = isCaseEmptyOrFinal ? reader.GetColumnValue<Guid>("ParentCaseId") : reader.GetColumnValue<Guid>("CaseId"),
									ActivityId = reader.GetColumnValue<Guid>("ActivityId")
								});
							}
			
						}
					}
					foreach (var @case in casesToReopen) {
						var schema = userConnection.EntitySchemaManager.FindInstanceByName("Activity");
						Entity entity = schema.CreateEntity(userConnection);
						if (entity.FetchFromDB(@case.ActivityId)) {
							entity.SetColumnValue("CaseId", null);
							entity.SetColumnValue("CaseId", @case.CaseId);
							entity.Save(false);
							_log.InfoFormat(@"[EmailSyncSessionId:{0}]|Reopen case by CaseId = {1}. ActivityId = {2}", Get<Guid>("RecordId"), @case.CaseId, @case.ActivityId);
						}
					}
				}
				return result;
			}
			
			protected Guid RegisterIncidentFromEmail(Guid activityId) {
				Guid result  = default(Guid);
				var userConnection = Get<UserConnection>("UserConnection");
				bool isNeddToCheckRegistrationEmailsInActivityParticipants = !Features.GetIsEnabled("RegisterCaseFromAnyEmail");
				bool isCategoryFromMailBoxEnabled = Features.GetIsEnabled("CategoryFromMailBox");
				bool getCategoryFromEmailSysSettings = Terrasoft.Core.Configuration.SysSettings.GetValue<bool>(userConnection,
				            "DefineCaseCategoryFromMailBox", false);
				IncindentRegistrationMailboxFilter maiboxFilter = new IncindentRegistrationMailboxFilter(userConnection);
				if (isNeddToCheckRegistrationEmailsInActivityParticipants && !maiboxFilter.IsPresentRegistrationEmailsInActivity(activityId)) {
					_log.InfoFormat(@"[EmailSyncSessionId:{0}] | Incident registration mailboxes does not exists for ActivityId = {1}", Get<Guid>("RecordId"), activityId);
					return result;
				}
				var helper = ClassFactory.Get<IncidentRegistrationFromEmailHelper>(
				    new ConstructorArgument("userConnection", userConnection));
				if (isCategoryFromMailBoxEnabled && getCategoryFromEmailSysSettings) {
				    var supportMailBoxStore = ClassFactory.Get<CategoryFromSupportMailBox>(
				    new ConstructorArgument("userConnection", userConnection));
				    var categoryProvider = ClassFactory.Get<CategoryProvider>(
				    new ConstructorArgument("supportMailBoxRepository", supportMailBoxStore));
				    var categoryWrapper = ClassFactory.Get<CategoryProviderWrapper>(
				    new ConstructorArgument("userConnection", userConnection));
				    categoryWrapper.CategoryProvider = categoryProvider;
				    helper.CategoryProviderWrapper = categoryWrapper;
				}
				helper.EmailSyncSession = Get<Guid>("RecordId");
				result = helper.GetRegisterIncidentId(activityId);
				return result;
			}

		#endregion

	}

	#endregion

}

