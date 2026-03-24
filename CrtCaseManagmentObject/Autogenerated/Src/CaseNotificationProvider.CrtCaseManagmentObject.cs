namespace Terrasoft.Configuration
{
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Configuration;
	using System.Collections.Generic;
	using Terrasoft.Core;
	using Terrasoft.Common;
	using System;

	#region Class: CaseNotificationProvider

	/// <summary>
	/// Case notification provider.
	/// </summary>
	public class CaseNotificationProvider : BaseNotificationProvider, INotification
	{
		#region Constants: Private

		private const string GROUP_NAME = "Notification";
		private const string NAME = "Case";

		#endregion

		#region Constructors: Public

		public CaseNotificationProvider(UserConnection userConnection)
			: base(userConnection) {
		}

		/// <summary>
		/// Initializes an instance of the provider <see cref="CaseNotificationProvider"/> with parameters.
		/// </summary>
		/// <param name="parameters">parameters.</param>
		public CaseNotificationProvider(Dictionary<string, object> parameters)
			: base(parameters) {
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Group name.
		/// </summary>
		public string Group {
			get {
				return GROUP_NAME;
			}
		}

		/// <summary>
		/// Provider name.
		/// </summary>
		public string Name {
			get {
				return NAME;
			}
		}

		#endregion

		#region Methods: Protected

		protected override void AddColumns(Select select) {
			select
				.Column("Reminding", "Id").As("Id")
				.Column("Reminding", "RemindTime").As("RemindTime")
				.Column("Reminding", "ContactId").As("RemindingContactId")
				.Column("Reminding", "Description").As("Description")
				.Column("Case", "Id").As("CaseId")
				.Column("Case", "Subject").As("Title")
				.Column("SysSchema", "Name").As("EntitySchemaName")
				.Column("NotificationsSettings", "SysImageId").As("ImageId");
		}

		protected override void JoinTables(Select select) {
			select
				.InnerJoin("SysAdminUnit").On("Reminding", "ContactId").IsEqual("SysAdminUnit", "ContactId")
				.InnerJoin("Case").On("Reminding", "SubjectId").IsEqual("Case", "Id")
				.InnerJoin("SysSchema").On("Reminding", "SysEntitySchemaId").IsEqual("SysSchema", "UId")
				.LeftOuterJoin("NotificationsSettings")
						.On("Reminding", "SysEntitySchemaId").IsEqual("NotificationsSettings", "SysEntitySchemaUId");
		}

		protected override void AddConditions(Select select) {
			base.AddConditions(select);
			select.And("Reminding", "NotificationTypeId")
					.IsEqual(Column.Parameter(RemindingConsts.NotificationTypeNotificationId));
		}

		protected override INotificationInfo GetRecordNotificationInfo(
						Dictionary<string, string> dictionaryColumnValues) {
			Guid imageId;
			Guid.TryParse(dictionaryColumnValues["ImageId"], out imageId);
			return new NotificationInfo() {
				Title = dictionaryColumnValues["Title"],
				Body = dictionaryColumnValues["Description"],
				ImageId = imageId,
				EntityId = new Guid(dictionaryColumnValues["CaseId"]),
				EntitySchemaName = dictionaryColumnValues["EntitySchemaName"],
				MessageId = new Guid(dictionaryColumnValues["Id"]),
				SysAdminUnit = new Guid(dictionaryColumnValues["SysAdminUnitId"]),
				GroupName = Group
			};
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get count of actual cases.
		/// </summary>
		public new int GetCount() {
			int result = 0;
			Guid sysAdminUnitId;
			if (!Guid.TryParse(parameters["sysAdminUnitId"].ToString(), out sysAdminUnitId)) {
				return result;
			}
			DateTime date;
			if (!DateTime.TryParse(parameters["dueDate"].ToString(), out date)) {
				return result;
			}
			var countSelect = new Select(UserConnection)
				.Column(Func.Count("r", "Id"))
					.Distinct()
				.From("Reminding").As("r")
					.LeftOuterJoin("SysAdminUnit").As("sau")
						.On("sau", "ContactId").IsEqual("r", "ContactId")
					.InnerJoin("Case").As("case")
						.On("case", "Id").IsEqual("r", "SubjectId")
				.Where("r", "RemindTime").IsLessOrEqual(Column.Const(date))
				.And("r", "IsRead").IsEqual(Column.Parameter(false))
				.And("sau", "Id").IsEqual(Column.Parameter(sysAdminUnitId)) as Select;
			result = countSelect.ExecuteScalar<int>();
			return result;
		}

		public override void SetColumns(List<string> columns) {
		}

		public override string GetRecordResult(Dictionary<string, string> dictionaryColumnValues) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns notification.
		/// </summary>
		/// <returns>Notification.</returns>
		public IEnumerable<INotificationInfo> GetNotifications() {
			return GetNotificationsInfos();
		}

		/// <summary>
		/// Returns null to support the implementation of the old.
		/// </summary>
		/// <returns>null.</returns>
		public override Select GetEntitiesSelect() {
			return null;
		}

		#endregion
	}

	#endregion
}

