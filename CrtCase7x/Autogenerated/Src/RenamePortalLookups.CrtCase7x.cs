namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	public class RenamePortalLookups : IInstallScriptExecutor
	{

		#region ExternalLookupValue

		public class ExternalLookupValue
		{
			public CultureInfo Culture { get; set; }
			public string Value { get; set; }
		}

		#endregion

		#region Fields: Private

		private UserConnection _userConnection;
		private EntitySchemaManager _entitySchemaManager;

		private static Dictionary<string, List<ExternalLookupValue>> _lookups = new Dictionary<string, List<ExternalLookupValue>>() {
			{
				"2EE7A27A-E0FD-4B0F-95AD-5C886420A3C9",
				new List<ExternalLookupValue>() {
					new ExternalLookupValue() {
						Culture = new CultureInfo("en-US"),
						Value = "List of objects available for external users" } ,
					new ExternalLookupValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Список об'єктов, доступных external пользователям" }
				}
			},
			{
				"31549F32-5F93-4CDB-9AD5-ECCD540FF9BA",
				new List<ExternalLookupValue>() {
					new ExternalLookupValue() {
						Culture = new CultureInfo("en-US"),
						Value = "List of schema fields for external access" } ,
					new ExternalLookupValue {
						Culture = new CultureInfo("ru-RU"),
						Value = "Список доступных полей объектов для external пользователей" }
				}
			}
		};

		#endregion

		#region Methods: Private

		private void RenameLookup(KeyValuePair<string, List<ExternalLookupValue>> setting) {
			Entity lookupEntity = _entitySchemaManager.GetEntityByName("Lookup", _userConnection);
			var lookupCodeCondition = new Dictionary<string, object> {
				{ "Id", Guid.Parse(setting.Key)}
			};
			if (!lookupEntity.FetchFromDB(lookupCodeCondition)) {
				return;
			}
			var lookupString = new LocalizableString();
			foreach (ExternalLookupValue settingsLczValue in setting.Value) {
				lookupString.SetCultureValue(settingsLczValue.Culture, settingsLczValue.Value);
			}
			lookupEntity.SetColumnValue("Name", lookupString);
			lookupEntity.Save(false);
		}

		#endregion

		#region Methods: Public

		public void Execute(UserConnection userConnection) {
			_userConnection = userConnection;
			_entitySchemaManager = userConnection.EntitySchemaManager;
			foreach (var lookup in _lookups) {
				RenameLookup(lookup);
			}
		}

		#endregion

	}
}
