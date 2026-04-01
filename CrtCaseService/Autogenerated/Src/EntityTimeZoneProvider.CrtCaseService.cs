namespace Terrasoft.Configuration
{
	using System;
	using Calendars;
	using Core;
	using Core.Entities;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: CaseTimeZoneProvider

	/// <summary>
	/// Case time zone provider implementation.
	/// </summary>
	public class EntityTimeZoneProvider : ITimeZoneProvider<Entity>
	{

		#region Constants: Protected

		/// <summary>
		/// The alias for service item calendar column.
		/// </summary>
		protected const string ServiceItemCalendarColumnName = "ServiceItemCalendar";

		#endregion

		#region Properties: Protected

		private Entity _timeZoneSource;
		/// <summary>
		/// Sourcing case entity.
		/// </summary>
		protected Entity TimeZoneSource {
			get {
				return _timeZoneSource ?? (_timeZoneSource = GetTimeZoneSource());
			}
		}

		/// <summary>
		/// User connection.
		/// </summary>
		protected UserConnection UserConnection {
			get;
			private set;
		}

		/// <summary>
		/// Sourcing case entity record identifier.
		/// </summary>
		protected Guid CaseId {
			get;
			private set;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of <see cref="EntityTimeZoneProvider"/>. 
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="caseId">Sourcing case entity record identifier.</param>
		public EntityTimeZoneProvider(UserConnection userConnection, Guid caseId) {
			UserConnection = userConnection;
			CaseId = caseId;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Gets time zone source.
		/// </summary>
		/// <returns>Sourcing case entity.</returns>
		protected virtual Entity GetTimeZoneSource() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			AddSourcingColumns(esq);
			return esq.GetEntity(UserConnection, CaseId);
		}

		/// <summary>
		/// Adds sourcing columns to given entity schema query.
		/// </summary>
		/// <param name="esq">Entity schema query.</param>
		protected virtual void AddSourcingColumns(EntitySchemaQuery esq) {
			esq.AddColumn("ServiceItem.Calendar.Id").Name = ServiceItemCalendarColumnName;
		}

		/// <summary>
		/// Gets default time zone record identifier from system setting.
		/// </summary>
		/// <returns>Time zone record identifier.</returns>
		protected Guid GetDefaultTimeZoneId() {
			return SystemSettings.GetValue(UserConnection, CalendarConsts.DefaultTimeZoneCode, default(Guid));
		}

		/// <summary>
		/// Finds system time zone by its code.
		/// </summary>
		/// <param name="timeZoneCode">Time zone code.</param>
		/// <returns>Time zone.</returns>
		protected TimeZoneInfo FindByCode(string timeZoneCode) {
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneCode);
			return timeZoneInfo;
		}

		/// <summary>
		/// Creates a chain of time zone retrieving.
		/// </summary>
		/// <returns>Chain.</returns>
		protected virtual ChainOfCircumstances<TimeZoneInfo> CreateChain() {
			var chain = new ChainOfCircumstances<TimeZoneInfo>();
			chain.AddLast(GetFromServiceCalendar, chain.NotDefault());
			chain.AddLast(GetFromSysSetting);
			return chain;
		}

		/// <summary>
		/// Attempts to get time zone from calendar of service of sourcing case entity.
		/// </summary>
		/// <returns>Time zone.</returns>
		protected TimeZoneInfo GetFromServiceCalendar() {
			var calendarId = TimeZoneSource.GetTypedColumnValue<Guid>(ServiceItemCalendarColumnName);
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Calendar");
			var timeZoneCodeColumnName = esq.AddColumn("TimeZone.Code").Name;
			Entity calendar = esq.GetEntity(UserConnection, calendarId);
			var timeZoneCode = calendar.GetTypedColumnValue<string>(timeZoneCodeColumnName);
			return FindByCode(timeZoneCode);
		}

		/// <summary>
		/// Attempts to get default time zone from system setting.
		/// </summary>
		/// <returns>Time zone.</returns>
		protected TimeZoneInfo GetFromSysSetting() {
			var defaultTimeZoneId = GetDefaultTimeZoneId();
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "TimeZone");
			string timeZoneCodeColumnName = esq.AddColumn("Code").Name;
			Entity entity = esq.GetEntity(UserConnection, defaultTimeZoneId);
			var timeZoneCode = entity.GetTypedColumnValue<string>(timeZoneCodeColumnName);
			return FindByCode(timeZoneCode);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets time zone.
		/// </summary>
		/// <returns>Time zone.</returns>
		public TimeZoneInfo GetTimeZone() {
			var chain = CreateChain();
			return chain.Execute();
		}

		#endregion

	}

	#endregion

} 
