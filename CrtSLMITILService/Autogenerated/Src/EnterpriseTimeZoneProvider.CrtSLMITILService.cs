namespace Terrasoft.Configuration
{
	using System;
	using Core;
	using Core.Entities;
	using Core.Factories;

	#region Class: EnterpriseTimeZoneProvider

	/// <summary>
	/// Enterprise time zone provider implementation.
	/// Extends the chain of time zone retrieving.
	/// </summary>
	[Override]
	public class EnterpriseTimeZoneProvider : EntityTimeZoneProvider
	{

		#region Constants: Private

		private const string ServiceItemIdColumnName = "ServiceItemId";
		private const string ServicePactIdColumnName = "ServicePactId";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of <see cref="EnterpriseTimeZoneProvider"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="caseId">Sourcing case entity record identifier.</param>
		public EnterpriseTimeZoneProvider(UserConnection userConnection, Guid caseId)
			: base(userConnection, caseId) {
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Attempts to get time zone from service in service pact of sourcing case entity.
		/// </summary>
		/// <returns>Time zone.</returns>
		private TimeZoneInfo GetFromServiceInServicePact() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceInServicePact");
			EntitySchemaQueryColumn timeZoneCodeColumn = esq.AddColumn("Calendar.TimeZone.Code");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"ServiceItem", TimeZoneSource.GetTypedColumnValue<Guid>(ServiceItemIdColumnName)));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"ServicePact", TimeZoneSource.GetTypedColumnValue<Guid>(ServicePactIdColumnName)));
			var resultCollection = esq.GetEntityCollection(UserConnection);
			if (resultCollection.Count == 0) {
				return null;
			}
			Entity serviceInServicePact = resultCollection.First.Value;
			string code = serviceInServicePact.GetTypedColumnValue<string>(timeZoneCodeColumn.Name);
			return FindByCode(code);
		}

		/// <summary>
		/// Attempts to get time zone from service pact of sourcing case entity.
		/// </summary>
		/// <returns>Time zone.</returns>
		private TimeZoneInfo GetFromServicePact() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServicePact");
			EntitySchemaQueryColumn timeZoneCodeColumn = esq.AddColumn("Calendar.TimeZone.Code");
			Entity servicePact = esq.GetEntity(UserConnection, TimeZoneSource.GetTypedColumnValue<Guid>("ServicePactId"));
			string code = servicePact.GetTypedColumnValue<string>(timeZoneCodeColumn.Name);
			return FindByCode(code);
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Adds sourcing columns to given entity schema query.
		/// </summary>
		/// <param name="esq">Entity schema query.</param>
		protected override void AddSourcingColumns(EntitySchemaQuery esq) {
			base.AddSourcingColumns(esq);
			esq.AddColumn("ServiceItem.Id").Name = ServiceItemIdColumnName;
			esq.AddColumn("ServicePact.Id").Name = ServicePactIdColumnName;
		}

		/// <summary>
		/// Creates a chain of time zone retrieving.
		/// </summary>
		/// <returns>Chain.</returns>
		protected override ChainOfCircumstances<TimeZoneInfo> CreateChain() {
			var chain = new ChainOfCircumstances<TimeZoneInfo>();
			chain.AddLast(GetFromServiceInServicePact, chain.NotDefault());
			chain.AddLast(GetFromServicePact, chain.NotDefault());
			return chain + base.CreateChain();
		}

		#endregion

	}

	#endregion

}

