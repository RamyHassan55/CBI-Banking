namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: FindContactForTAMethodsWrapper

	/// <exclude/>
	public class FindContactForTAMethodsWrapper : ProcessModel
	{

		public FindContactForTAMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("SearchContactsApolloExecute", SearchContactsApolloExecute);
			AddScriptTaskMethod("SearchContactsZoomInfoExecute", SearchContactsZoomInfoExecute);
			AddScriptTaskMethod("MergeResultsExecute", MergeResultsExecute);
		}

		#region Methods: Private

		private bool SearchContactsApolloExecute(ProcessExecutingContext context) {
			Guid accountId = Get<Guid>("AccountId");
			int rowCount = Get<int>("RowCount");
			string organizationId = GetOrganizationId("Account", "MrktApolloId", accountId);
			SearchContactForTA(
				new SearchContactParams {
					entityName = "MrktApolloPeopleSearch",
					rowCount = rowCount,
					organizationColumnName = "MrktOrganizationId",
					organizationId = organizationId,
					managementLevelColumnName = "MrktApolloManagementLevel",
					managementLevelId = new Guid("E2522D29-6A82-4F9A-992B-0D157BC3F689"), // C Suite
					jobTitleColumnName = "MrktTitle",
					jobTitle = Get<string>("JobTitle")
				});
			return true;
		}

		private bool SearchContactsZoomInfoExecute(ProcessExecutingContext context) {
			Guid accountId = Get<Guid>("AccountId");
			int rowCount = Get<int>("RowCount");
			string organizationId = GetOrganizationId("Account", "MrktZoomInfoId", accountId);
			SearchContactForTA(
				new SearchContactParams {
					entityName = "MrktZoominfoPersonSearch",
					rowCount = rowCount,
					organizationColumnName = "MrktAccount",
					organizationId = organizationId,
					managementLevelColumnName = "MrktManagementLevel",
					managementLevelId = new Guid("13ABBBF7-5E39-446D-9D6E-9FB877A9F6E0") // C Suite
				});
			return true;
		}

		private bool MergeResultsExecute(ProcessExecutingContext context) {
			MergeSearchResults("MrktApolloPeopleSearch", "MrktZoominfoPersonSearch");
			return true;
		}

			class SearchContactParams
			{
				public string entityName { get; set; }
				public int rowCount { get; set; }
				public string organizationColumnName { get; set; }
				public string organizationId { get; set; }
				public string managementLevelColumnName { get; set; }
				public Guid managementLevelId { get; set; }
				public string jobTitleColumnName { get; set; }
				public string jobTitle { get; set; }
			}
			
			private void SearchContactForTA(SearchContactParams parameters) {
				var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, parameters.entityName);
				esq.RowCount = parameters.rowCount;
				esq.AddAllSchemaColumns();
				var filters = new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And);
				filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
					parameters.organizationColumnName,
					parameters.organizationId));
				if (parameters.managementLevelColumnName.IsNotNullOrEmpty()) {
					filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
						parameters.managementLevelColumnName,
						parameters.managementLevelId));
				}
				if (parameters.jobTitleColumnName.IsNotNullOrEmpty() && parameters.jobTitle.IsNotNullOrEmpty()) {
					filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
						parameters.jobTitleColumnName,
						parameters.jobTitle));
				}
				esq.Filters.Add(filters);
				EntityCollection resultCollection = esq.GetEntityCollection(UserConnection);
				var mappedColumnNames = new Dictionary<string, string>
				{
					{ "MrktFirstName", "GivenName" },
					{ "MrktLastName", "Surname" },
					{ "MrktLinkedinUrl", "LinkedIn" },
					{ "MrktTitle", "JobTitle" },
					{ "MrktFacebookUrl", "Facebook" },
				};
				IEnumerable<CompositeObject> result = resultCollection
					.Select(entity => new CompositeObject(
						entity.GetColumnValues().ToDictionary(kvp =>
							mappedColumnNames.ContainsKey(kvp.Key) ? mappedColumnNames[kvp.Key] : kvp.Key,
							kvp => kvp.Value)))
					.ToList();
				Set(parameters.entityName, new CompositeObjectList<CompositeObject>(result));
			}
			
			private void MergeSearchResults(string firstCollectionName, string secondCollectionName) {
				int rowCount = Get<int>("RowCount");
				IEnumerable<CompositeObject> firstCollection = Get<List<CompositeObject>>(firstCollectionName);
				IEnumerable<CompositeObject> secondCollection = Get<List<CompositeObject>>(secondCollectionName);
				IEnumerable<CompositeObject> mergedCollection = firstCollection
					.Concat(secondCollection
						.Where(secondItem => !firstCollection
							.Any(firstItem => firstItem["GivenName"].Equals(secondItem["GivenName"]) ||
							                  firstItem["GivenName"].Equals(secondItem["Surname"]) ||
							                  firstItem["Surname"].Equals(secondItem["Surname"]) ||
							                  firstItem["Surname"].Equals(secondItem["GivenName"]))))
					.ToList().Take(rowCount);
				Set("EnrichedPersons", new CompositeObjectList<CompositeObject>(mergedCollection));
			}
			
			private string GetOrganizationId(string entityName, string columnName, Guid entityId) {
				var accountSchema = UserConnection.EntitySchemaManager.FindInstanceByName(entityName);
				var accountEntity = accountSchema.CreateEntity(UserConnection);
				string orgId = string.Empty;
				if (accountEntity.FetchFromDB(accountSchema.PrimaryColumn.Name, entityId, new [] { columnName })) {
					orgId = accountEntity.GetTypedColumnValue<string>(columnName);
				}
				return orgId;
			}

		#endregion

	}

	#endregion

}

