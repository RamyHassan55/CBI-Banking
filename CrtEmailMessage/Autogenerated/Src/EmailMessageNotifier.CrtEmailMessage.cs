namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.DB;
	using HttpUtility = System.Web.HttpUtility;

	#region Class: EmailMessageNotifier

	public class EmailMessageNotifier : BaseMessageNotifier
	{
		#region Constructor: Public

		public EmailMessageNotifier(Entity entity, UserConnection userConnection) {
			MessageInfo = new MessageInfo {
				Message = GetMessage(entity, userConnection),
				CreatedById = entity.GetTypedColumnValue<Guid>("MessageTypeId") == ActivityConsts.IncomingEmailTypeId ?
					entity.GetTypedColumnValue<Guid>("ContactId") : entity.GetTypedColumnValue<Guid>("CreatedById"),
				CreatedOn = entity.GetTypedColumnValue<DateTime>("CreatedOn"),
				MessageHistoryCreatedOn = entity.GetTypedColumnValue<DateTime>("SendDate"),
				HasAttachment = true,
				NotifierRecordId = entity.PrimaryColumnValue,
				SchemaUId = entity.Schema.UId,
				ListenersData = GetListenersData(entity, userConnection),
				EntityState = entity.ChangeType
			};
		}

		#endregion

		#region Methods: Private

		private Dictionary<Guid, Guid> GetListenersData(Entity entity, UserConnection userConnection) {
			var listenersData = new Dictionary<Guid, Guid>();
			var connectionColumnsSelect = new Select(userConnection)
				.Column("LN", "NotifierConnectionColumn")
				.From("ListenerByNotifier").As("LN")
				.LeftOuterJoin("MessageNotifier").As("MN").On("MN", "Id").IsEqual("LN", "MessageNotifierId")
				.Where("MN", "SchemaUId").IsEqual(new QueryParameter(entity.Schema.UId)) as Select;
			using (DBExecutor dbExecutor = userConnection.EnsureDBConnection()) {
				using (IDataReader dataReader = connectionColumnsSelect.ExecuteReader(dbExecutor)) {
					while (dataReader.Read()) {
						var notifierConnectionColumnName = dataReader.GetColumnValue<string>("NotifierConnectionColumn");
						if (!string.IsNullOrEmpty(notifierConnectionColumnName)) {
							var connectionColumnValue = entity.GetTypedColumnValue<Guid>(notifierConnectionColumnName + "Id");
							if (connectionColumnValue != Guid.Empty) {
								listenersData.Add(entity.Schema.Columns.GetByName(notifierConnectionColumnName).ReferenceSchemaUId,
									connectionColumnValue);
							}
						}
					}
				}
			}
			return listenersData;
		}

		private string GetMessage(Entity entity, UserConnection userConnection) {
			if (entity.GetTypedColumnValue<bool>("IsHtmlBody")) {
				return userConnection.GetIsFeatureEnabled("DisableEmailDecoding")
					? entity.GetTypedColumnValue<string>("Body")
					: HttpUtility.HtmlDecode(entity.GetTypedColumnValue<string>("Body"));
			}
			return StringUtilities.FormatForHtml(entity.GetTypedColumnValue<string>("Body"));
		}

		#endregion

	}

	#endregion

}
