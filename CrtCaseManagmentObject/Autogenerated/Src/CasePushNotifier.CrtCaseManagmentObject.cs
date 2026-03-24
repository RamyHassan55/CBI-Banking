namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using global::Common.Logging;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: CasePushNotifier

	/// <summary>
	/// Notifies to customer about actions in Case.
	/// </summary>
	[DefaultBinding(typeof(ICasePushNotifier))]
	public class CasePushNotifier : ICasePushNotifier
	{

		private class CaseInfo
		{
			public Guid Id {
				get;
				internal set;
			}
			public string Number {
				get;
				internal set;
			}
			public Guid StatusId {
				get;
				internal set;
			}
			public Guid ContactUserId {
				get;
				internal set;
			}
			public Guid ContactCultureId {
				get;
				internal set;
			}
			public string ContactCultureName {
				get;
				internal set;
			}
		}

		#region Constants: Private

		private const string caseSchemaName = "Case";
		private const int pushNotificationMessageLimit = 1000;
		private static readonly ILog _log = LogManager.GetLogger("CasePushNotifier");

		#endregion

		#region Constructors: Public

		public CasePushNotifier(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Methods: Private
			
		private string GetPushNotificationTitle(string Number, string cultureName) {
			string titleTemplate = GetLocalizableValue("PushTitleTemplate", cultureName);
			return string.Format(titleTemplate, Number);
		}

		private string GetLocalizableValue(string localizableStringName, string cultureName) {
			LocalizableString localizableString = new LocalizableString(_userConnection.Workspace.ResourceStorage,
				"CasePushNotifier", string.Format("LocalizableStrings.{0}.Value", localizableStringName));
			var culture = CultureInfo.GetCultureInfo(cultureName);
			return localizableString.GetCultureValueWithFallback(culture, false);
		}

		private void SendPush(Guid userSysAdminUnitId, Guid caseId, string title, string body) {
			Dictionary<string, string> additionalData = new Dictionary<string, string>();
			additionalData.Add("entityName", caseSchemaName);
			additionalData.Add("recordId", caseId.ToString());
			var pushNotification = new PushNotification(_userConnection);
			try {
				pushNotification.Send(userSysAdminUnitId, title, body, additionalData);
			} catch (Exception ex) {
				_log.Error(ex);
			}
		}

		private string GetStatusLocalizableValue(Guid cultureId, Guid statusId) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "CaseStatus") {
				CanReadUncommitedData = true,
			};
			esq.SetLocalizationCultureId(cultureId);
			esq.AddColumn("Name");
			var status = esq.GetEntity(_userConnection, statusId);
			if (status != null) {
				return status.GetTypedColumnValue<string>("Name");
			} else {
				return "";
			}
		}

		private CaseInfo GetCaseInfo(Guid caseId) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, caseSchemaName) {
				IgnoreDisplayValues = true,
				CanReadUncommitedData = true,
			};
			var statusColumn = esq.AddColumn("Status.Id");
			var numberColumn = esq.AddColumn("Number");
			var sysAdminUnitIdColumn = esq.AddColumn("[SysAdminUnit:Contact:Contact].Id");
			var connectionTypeColumn = esq.AddColumn("[SysAdminUnit:Contact:Contact].ConnectionType");
			var sysCultureIdColumn = esq.AddColumn("[SysAdminUnit:Contact:Contact].SysCulture.Id");
			var sysCultureNameColumn = esq.AddColumn("[SysAdminUnit:Contact:Contact].SysCulture.Name");
			var idFilter = esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", caseId);
			esq.Filters.Add(idFilter);
			var userCollection = esq.GetEntityCollection(_userConnection);
			if (userCollection.Count > 0) {
				Entity result = userCollection[0];
				Guid contactUserId = result.GetTypedColumnValue<Guid>(sysAdminUnitIdColumn.Name);
				int userType = result.GetTypedColumnValue<int>(connectionTypeColumn.Name);
				if (contactUserId == Guid.Empty || userType != (int)UserType.SSP ||
						_userConnection.CurrentUser.Id == contactUserId) {
					return null;
				}
				return new CaseInfo {
					Id = caseId,
					StatusId = result.GetTypedColumnValue<Guid>(statusColumn.Name),
					Number = result.GetTypedColumnValue<string>(numberColumn.Name),
					ContactUserId = contactUserId,
					ContactCultureId = result.GetTypedColumnValue<Guid>(sysCultureIdColumn.Name),
					ContactCultureName = result.GetTypedColumnValue<string>(sysCultureNameColumn.Name),
				};
			} else {
				return null;
			}
		}

		private bool IsPushNotificationsEnabled() {
			return _userConnection.GetIsFeatureEnabled("UseMobileCasePushNotifications");
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		public virtual void NotifyNewStatus(Guid caseId) {
			if (!IsPushNotificationsEnabled()) {
				return;
			}
			CaseInfo info = GetCaseInfo(caseId);
			if (info == null) {
				return;
			}
			string cultureName = info.ContactCultureName;
			string title = GetPushNotificationTitle(info.Number, cultureName);
			string bodyTemplate = GetLocalizableValue("NewStatusNotificationTemplate", cultureName);
			string status = GetStatusLocalizableValue(info.ContactCultureId, info.StatusId);
			string body = string.Format(bodyTemplate, status);
			SendPush(info.ContactUserId, caseId, title, body);
		}

		/// <inheritdoc />
		public virtual void NotifyNewMessage(Guid caseId, string message) {
			if (!IsPushNotificationsEnabled()) {
				return;
			}
			CaseInfo info = GetCaseInfo(caseId);
			if (info == null) {
				return;
			}
			string cultureName = info.ContactCultureName;
			string title = GetPushNotificationTitle(info.Number, cultureName);
			string body;
			if (message != null) {
				body = Terrasoft.Common.StringUtilities.ConvertHtmlToPlainText(message);
				body = body.Length > pushNotificationMessageLimit ? body.Substring(0, pushNotificationMessageLimit) : body;
			} else {
				body = GetLocalizableValue("NewMessageNotificationTemplate", cultureName);
			}
			SendPush(info.ContactUserId, caseId, title, body);
		}

		#endregion

	}

	#endregion

	public interface ICasePushNotifier
	{
		/// <summary>
		/// Notifies the user about a change in the status of the case.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		void NotifyNewStatus(Guid caseId);

		/// <summary>
		/// Notifies the user about adding a new message on case.
		/// </summary>
		/// <param name="caseId">Case identifier.</param>
		void NotifyNewMessage(Guid caseId, string message);
	}
}
