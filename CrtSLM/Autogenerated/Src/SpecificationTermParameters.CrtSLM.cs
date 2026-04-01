using System;
using System.Collections.Generic;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace Terrasoft.Configuration.ServiceTerm
{
	/// <summary>
	/// Represents a specification term parameters.
	/// </summary>
	public class SpecificationTermParameters : ISpecificationTermParameters
	{
		#region Constants

		private static readonly string BaseCalendarSysSettingName = "BaseCalendar";
		private static readonly string ServiceItemTableName = "ServiceItem";
		private static readonly string ServicePactTableName = "ServicePact";
		private static readonly string ServiceInServicePactTableName = "ServiceInServicePact";
		private static readonly string CalendarColumnName = "Calendar";
		private static readonly string ReactionTimeUnitCodeColumnName = "ReactionTimeUnit.Code";
		private static readonly string ReactionTimeValueColumnName = "ReactionTimeValue";
		private static readonly string SolutionTimeUnitCodeColumnName = "SolutionTimeUnit.Code";
		private static readonly string SolutionTimeValueColumnName = "SolutionTimeValue";

		#endregion

		#region Preperties

		private readonly UserConnection _userConnection;
		private Guid _servicePactId;
		private Guid _serviceId;
		private string _calendarColumnName;
		private string _reactionTimeUnitCodeColumnName;
		private string _reactionTimeValueColumnName;
		private string _solutionTimeUnitCodeColumnName;
		private string _solutionTimeValueColumnName;

		#endregion

		#region Constructors

		/// <summary>
		/// Create new specification term parameters.
		/// </summary>
		/// <param name="conditions">Term parameter select condition.</param>
		/// <param name="userConnection">User connection.</param>
		public SpecificationTermParameters(IDictionary<string, object> conditions, UserConnection userConnection) {
			_userConnection = userConnection;
			InitializeConditions(conditions);
		}

		#endregion

		#region Methods

		#region Private

		/// <summary>
		/// Initializes select term parameter conditions.
		/// </summary>
		/// <param name="conditions">Term parameter select condition.</param>
		private void InitializeConditions(IDictionary<string, object> conditions) {
			if (conditions == null) {
				throw new ArgumentNullException("conditions");
			}
			if (conditions.ContainsKey(ServiceItemTableName)) {
				_serviceId = ParseCondition(conditions, ServiceItemTableName);
			}
			if (_serviceId == Guid.Empty) {
				throw new ArgumentException();
			}
			if (conditions.ContainsKey(ServicePactTableName)) {
				_servicePactId = ParseCondition(conditions, ServicePactTableName);
			}
		}

		/// <summary>
		/// Adds columns to term select query.
		/// </summary>
		/// <param name="query">Query.</param>
		private void AddTermSelectQueryColumns(EntitySchemaQuery query) {
			_calendarColumnName = query.AddColumn(CalendarColumnName).Name;
			_reactionTimeUnitCodeColumnName = query.AddColumn(ReactionTimeUnitCodeColumnName).Name;
			_reactionTimeValueColumnName = query.AddColumn(ReactionTimeValueColumnName).Name;
			_solutionTimeUnitCodeColumnName = query.AddColumn(SolutionTimeUnitCodeColumnName).Name;
			_solutionTimeValueColumnName = query.AddColumn(SolutionTimeValueColumnName).Name;
		}

		/// <summary>
		/// Adds filters to term select query.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="serviceContractId">Service contract identifier.</param>
		/// <param name="serviceId">Service identifier.</param>
		private void AddTermSelectQueryFilters(EntitySchemaQuery query, Guid serviceContractId, Guid serviceId) {
			if (serviceContractId != Guid.Empty) {
				query.Filters.Add(
					query.CreateFilterWithParameters(FilterComparisonType.Equal, ServicePactTableName, serviceContractId));
				query.Filters.Add(
					query.CreateFilterWithParameters(FilterComparisonType.Equal, "ServicePact.Status.IsActive", true));
				if (serviceId != Guid.Empty) {
					query.Filters.Add(
						query.CreateFilterWithParameters(FilterComparisonType.Equal, ServiceItemTableName, serviceId));
					query.Filters.Add(
						query.CreateFilterWithParameters(FilterComparisonType.Equal, "ServiceItem.Status.Active", true));
				}
				return;
			}
			if (serviceId != Guid.Empty) {
				query.Filters.Add(
					query.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", serviceId));
				query.Filters.Add(
					query.CreateFilterWithParameters(FilterComparisonType.Equal, "Status.Active", true));
			}
		}

		/// <summary>
		/// Parses time unit.
		/// </summary>
		/// <param name="code">Time unit code.</param>
		/// <returns>Time unit.</returns>
		private Calendars.TimeUnit ParseTimeUnit(string code) {
			Calendars.TimeUnit timeUnit;
			if (!Enum.TryParse(code, out timeUnit)) {
				throw new InvalidCastException();
			}
			return timeUnit;
		}

		/// <summary>
		/// Parses Term parameter select conditions.
		/// </summary>
		/// <param name="conditions">Term parameter select condition.</param>
		/// <param name="key">Condition key</param>
		/// <returns>Condition value.</returns>
		private Guid ParseCondition(IDictionary<string, object> conditions, string key) {
			var value = conditions[key];
			if (value == null) {
				throw new ArgumentNullException("conditions");
			}
			return Guid.Parse(value.ToString());
		}

		#endregion

		#region Protected

		/// <summary>
		/// Gets term select query.
		/// </summary>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="servicePactId">Service pact identifier.</param>
		/// <param name="serviceId">Service identifier,</param>
		/// <returns>Query.</returns>
		protected virtual EntitySchemaQuery GetTermSelectQuery(string schemaName, Guid servicePactId, Guid serviceId) {
			var query = new EntitySchemaQuery(_userConnection.EntitySchemaManager, schemaName);
			AddTermSelectQueryColumns(query);
			AddTermSelectQueryFilters(query, servicePactId, serviceId);
			return query;
		}

		/// <summary>
		/// Creates term parameters.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <returns>Term parameters.</returns>
		protected virtual TermParameters CreateTermParameters(Entity entity) {
			var parameters = new TermParameters();
			if (entity == null) {
				return parameters;
			}
			var calendarId = entity.GetTypedColumnValue<Guid>(_calendarColumnName + "Id");
			var responseTimeUnit = ParseTimeUnit(entity.GetTypedColumnValue<string>(_reactionTimeUnitCodeColumnName));
			var responseTimeUnitValue = entity.GetTypedColumnValue<int>(_reactionTimeValueColumnName);
			var solutionTimeUnit = ParseTimeUnit(entity.GetTypedColumnValue<string>(_solutionTimeUnitCodeColumnName));
			var solutionTimeValue = entity.GetTypedColumnValue<int>(_solutionTimeValueColumnName);
			parameters.CalendarId = calendarId;
			parameters.ResponseParams = new KeyValuePair<Calendars.TimeUnit, int>(responseTimeUnit, responseTimeUnitValue);
			parameters.SolutionParams = new KeyValuePair<Calendars.TimeUnit, int>(solutionTimeUnit, solutionTimeValue);
			return parameters;
		}

		/// <summary>
		/// Gets term parameters by entity schema name.
		/// </summary>
		/// <param name="entityName">Entity schema name.</param>
		/// <param name="servicePactId"></param>
		/// <returns>Term parameters.</returns>
		protected virtual TermParameters GetTermParamsByEntity(string entityName, Guid servicePactId) {
			var query = GetTermSelectQuery(entityName, servicePactId, _serviceId);
			var collection = query.GetEntityCollection(_userConnection);
			return collection.Count == 0 ? new TermParameters() : CreateTermParameters(collection[0]);
		}

		/// <summary>
		/// Gets calendar identifier by service pact.
		/// </summary>
		/// <returns>Calendar identifier.</returns>
		protected virtual Guid GetCalendarByServicePact() {
			var query = new EntitySchemaQuery(_userConnection.EntitySchemaManager, ServicePactTableName);
			query.AddColumn(CalendarColumnName);
			var entity = query.GetEntity(_userConnection, _servicePactId);
			return entity == null ? Guid.Empty : entity.GetTypedColumnValue<Guid>(CalendarColumnName + "Id");
		}

		/// <summary>
		/// Gets calendar identifier by base calendar system setting.
		/// </summary>
		/// <returns>Calendar identifier.</returns>
		protected virtual Guid GetCalendarBySystemSetting() {
			return Core.Configuration.SysSettings.GetValue(_userConnection, BaseCalendarSysSettingName, Guid.Empty);
		}

		/// <summary>
		/// Get term parameters by service item.
		/// </summary>
		/// <returns>Term parameters.</returns>
		protected virtual TermParameters GetTermParametersByServiceItem() {
			var parameters = GetTermParamsByEntity(ServiceItemTableName, Guid.Empty);
			if (parameters.CalendarId == Guid.Empty) {
				parameters.CalendarId = GetCalendarBySystemSetting();
			}
			return parameters;
		}

		/// <summary>
		/// Get term parameters by service pact.
		/// </summary>
		/// <returns>Term parameters.</returns>
		protected virtual TermParameters GetTermParametersByServicePact() {
			var parameters = GetTermParamsByEntity(ServiceInServicePactTableName, _servicePactId);
			if (parameters.CalendarId != Guid.Empty) {
				return parameters;
			}
			parameters.CalendarId = GetCalendarByServicePact();
			if (parameters.CalendarId == Guid.Empty) {
				parameters.CalendarId = GetCalendarBySystemSetting();
			}
			return parameters;
		}

		#endregion

		#region Public

		/// <summary>
		/// Get term parameters.
		/// </summary>
		/// <returns>Term parameters.</returns>
		public virtual TermParameters GetTermParameters() {
			TermParameters parameters;
			if (_servicePactId == Guid.Empty) {
				parameters = GetTermParametersByServiceItem();
				return parameters.IsCorectParams() ? parameters : null;
			}
			parameters = GetTermParametersByServicePact();
			if (parameters.IsCorectParams()) {
				return parameters;
			}
			if (parameters.CalendarId == Guid.Empty) {
				return null;
			}
			var termParametersByService = GetTermParametersByServiceItem();
			if (!parameters.IsCorrectResponseParams()) {
				parameters.ResponseParams = termParametersByService.ResponseParams;
			}
			if (!parameters.IsCorrectSolutionParams()) {
				parameters.SolutionParams = termParametersByService.SolutionParams;
			}
			return parameters.IsCorectParams() ? parameters : null;
		}

		#endregion

		#endregion
	}

}

