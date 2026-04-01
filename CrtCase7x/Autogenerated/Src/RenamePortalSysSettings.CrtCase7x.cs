namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	public class RenamePortalSysSettings : IInstallScriptExecutor
	{
		#region ExternalSettingsValue

		class ExternalSettingsValue
		{
			/// <summary>
			/// Settings culture.
			/// </summary>
			public CultureInfo Culture { get; set; }

			/// <summary>
			/// Settings value.
			/// </summary>
			public string Value { get; set; }
		}

		#endregion

		#region Fields: Private

		private UserConnection _userConnection;
		private EntitySchemaManager _entitySchemaManager;

		private static Dictionary<string, List<ExternalSettingsValue>> _settings = new Dictionary<string, List<ExternalSettingsValue>>() {
			{
				"PortalSiteUrl",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Hostname for external users" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Адрес сайта для external пользователей" }
				}
			},
			{
				"PortalRecoveryPasswordEmailTemplate",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Password reset email template for external user" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Шаблон email-сообщения для восстановления пароля external пользователей" }
				}
			},
			{
				"PortalRegistrationEmailTemplate",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Confirmation email template for external user registration" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Шаблон email-сообщения, подтверждающего регистрацию external пользователей" }
				}
			},
			{
				"GrantCasePermissionsForPortalOrganization",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Grant case permissions for external user organization" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Раздавать права на обращение организации external пользователя" }
				}
			},
			{
				"PortalInvitationEmailTemplate",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Invite email template for external user" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Шаблон приглашения external пользователей" }
				}
			},
			{
				"SSPLogoImage",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Logo on login page for external users" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Логотип на странице логина для external пользователей" }
				}
			},
			{
				"SSPRegistrationMailbox",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Mailbox for registration and password recovery" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Почтовый ящик для регистрации и восстановления пароля" }
				}
			},
			{
				"SSPMainPage",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Start page for external users" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Стартовая страница для external пользователей" }
				}
			},
			{
				"EnableRightsOnServiceObjects",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Enable rights on service agreements and conf items for external users" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Включать права доступа на сервисные договоры и конфигурационные единицы для external пользователей" }
				}
			},
			{
				"CheckTermsAndConditions",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Request consent to privacy terms and conditions from the external users"
					} ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Запрашивать согласие с условиями для external пользователей" }
				}
			},
			{
				"EnableComplainFunction",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "Enable complain function on case for external users"
					} ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Включить возможность пожаловаться на обращение для external пользователей" }
				}
			}
		};

		private static Dictionary<string, List<ExternalSettingsValue>> _settingsGroup = new Dictionary<string, List<ExternalSettingsValue>>() {
			{
				"0FF76E29-A66C-49BE-91CC-E92AC1019F5E",
				new List<ExternalSettingsValue>() {
					new ExternalSettingsValue() {
						Culture = new CultureInfo("en-US"),
						Value = "External users settings" } ,
					new ExternalSettingsValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Настройки для external пользователей" }
				}
			}
		};

		#endregion

		#region Methods: Private

		private void RenameSysSettingGroup(KeyValuePair<string, List<ExternalSettingsValue>> setting) {
			Entity sysSettingsFolderEntity = _entitySchemaManager.GetEntityByName("SysSettingsFolder", _userConnection);
			var sysSettingsFolderCondition = new Dictionary<string, object> {
				{ "Id", Guid.Parse(setting.Key)}
			};
			if (!sysSettingsFolderEntity.FetchFromDB(sysSettingsFolderCondition)) {
				return;
			}
			var sysSettingsFolderString = new LocalizableString();
			foreach (ExternalSettingsValue settingsLczValue in setting.Value) {
				sysSettingsFolderString.SetCultureValue(settingsLczValue.Culture, settingsLczValue.Value);
			}
			sysSettingsFolderEntity.SetColumnValue("Name", sysSettingsFolderString);
			sysSettingsFolderEntity.Save(false);
		}

		private void RenameSetting(KeyValuePair<string, List<ExternalSettingsValue>> setting) {
			Entity sysSettingsEntity = _entitySchemaManager.GetEntityByName("SysSettings", _userConnection);
			var sysSettingsCodeCondition = new Dictionary<string, object> {
				{ "Code", setting.Key}
			};
			if (!sysSettingsEntity.FetchFromDB(sysSettingsCodeCondition)) {
				return;
			}
			var sysSettingsString = new LocalizableString();
			foreach (ExternalSettingsValue settingsLczValue in setting.Value) {
				sysSettingsString.SetCultureValue(settingsLczValue.Culture, settingsLczValue.Value);
			}
			sysSettingsEntity.SetColumnValue("Name", sysSettingsString);
			sysSettingsEntity.Save(false);
		}


		#endregion

		#region Methods: Public
		public void Execute(UserConnection userConnection) {
			_userConnection = userConnection;
			_entitySchemaManager = userConnection.EntitySchemaManager;
			foreach (var setting in _settings) {
				RenameSetting(setting);
			}
			foreach (var settGroup in _settingsGroup) {
				RenameSysSettingGroup(settGroup);
			}
		}

		#endregion

	}
}
