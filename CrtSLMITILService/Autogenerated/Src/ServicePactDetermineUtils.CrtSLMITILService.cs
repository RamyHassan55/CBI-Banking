using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace Terrasoft.Configuration.ServicePactService
{

	#region Class: ServicePactDetermineUtils

	/// <summary>
	/// ######### #### ### ########## ########## ######### #########.
	/// </summary>
	public class ServicePactDetermineUtilsV2
	{
		#region Constructors: Public
		
		public ServicePactDetermineUtilsV2() {
		}
		
		public ServicePactDetermineUtilsV2(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Fields: Private

		private Entity _contact;
		
		private SuitableServicePactsRequest _request;

		#endregion

		#region Properties: Protected

		protected UserConnection UserConnection { 
			get; 
			private set; 
		}

		#endregion
		
		#region Methods: Private

		private string GetServicePactName(Guid servicePactId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServicePact");
			esq.AddColumn("Name");
			Entity entity = esq.GetEntity(UserConnection, servicePactId);
			return entity != null ? entity.GetTypedColumnValue<string>("Name") : string.Empty;
		}

		#endregion

		#region Methods: Protected

		protected virtual Entity GetContactEntityInfo(Guid contactId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Contact");
			esq.AddColumn("Department");
			esq.AddColumn("Account");
			return esq.GetEntity(UserConnection, contactId);
		}

		protected virtual IEnumerable<Entity> GetServiceObjects() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ServiceObject");
			esq.AddAllSchemaColumns();
			IEntitySchemaQueryFilterItem isActiveFilter = esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"ServicePact.Status.IsActive", true);
			IEntitySchemaQueryFilterItem notExpiredFilter = esq.CreateFilterWithParameters(
				FilterComparisonType.GreaterOrEqual,
				"ServicePact.EndDate",
				UserConnection.CurrentUser.GetCurrentDateTime());

			var relationFilterGroup = new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.Or);
			if (_request.ContactId != Guid.Empty) {
				relationFilterGroup.Add(new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And) {
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Contact", _request.ContactId),
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Type", 
						ServiceObjectConstants.ContactTypeId)
				});
			}

			if (_request.AccountId != Guid.Empty) {
				relationFilterGroup.Add(new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And) {
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Account", _request.AccountId),
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Type", 
						ServiceObjectConstants.AccountTypeId)
				});
			}

			var accountId = _contact?.GetTypedColumnValue<Guid>("AccountId") ?? Guid.Empty;
			if (_contact != null && accountId != Guid.Empty) {
				relationFilterGroup.Add(new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And) {
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Account", accountId),
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Type", 
						ServiceObjectConstants.AccountTypeId)
				});
			}

			var departmentId = _contact?.GetTypedColumnValue<Guid>("DepartmentId") ?? Guid.Empty;
			if (_contact != null && accountId != Guid.Empty && departmentId != Guid.Empty) {
				relationFilterGroup.Add(new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And) {
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Department", accountId),
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Account", departmentId),
					esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Type", 
						ServiceObjectConstants.DepartmentTypeId)
				});
			}

			esq.Filters.Add(isActiveFilter);
			esq.Filters.Add(notExpiredFilter);
			esq.Filters.Add(relationFilterGroup);
			return esq.GetEntityCollection(UserConnection);
		}

		protected virtual IEnumerable<SuitableServicePact> ConvertToSuitableServicePacts(
			IEnumerable<Entity> serviceObjects) {
			var resultList = new List<SuitableServicePact>();
			List<Entity> serviceObjectsList = serviceObjects.ToList();

			resultList.AddRange(
				serviceObjectsList.Where(o => o.GetTypedColumnValue<Guid>("TypeId") == ServiceObjectConstants.ContactTypeId)
					.Select(s => s.ToSuitableServicePact(1)));
			resultList.AddRange(
				serviceObjectsList.Where(o => o.GetTypedColumnValue<Guid>("TypeId") == ServiceObjectConstants.DepartmentTypeId)
					.Select(s => s.ToSuitableServicePact(2)));

			var accountId = _contact?.GetTypedColumnValue<Guid>("AccountId") ?? Guid.Empty;
			if (_contact != null && accountId != Guid.Empty) {
				resultList.AddRange(
					serviceObjectsList
						.Where(o => o.GetTypedColumnValue<Guid>("AccountId") == accountId &&
							o.GetTypedColumnValue<Guid>("TypeId") == ServiceObjectConstants.AccountTypeId)
						.Select(s => s.ToSuitableServicePact(3)));
			}
			if (_request.AccountId != Guid.Empty) {
				resultList.AddRange(
					serviceObjectsList
						.Where(o => o.GetTypedColumnValue<Guid>("AccountId") == _request.AccountId &&
							o.GetTypedColumnValue<Guid>("TypeId") == ServiceObjectConstants.AccountTypeId)
						.Select(s => s.ToSuitableServicePact(4)));
			}
			var defaultServicePactId = Guid.Empty;
			var defaultServicePact = Core.Configuration.SysSettings.GetValue(UserConnection, 
				"DefaultServicePact");
			if (defaultServicePact != null) {
				defaultServicePactId = (Guid)defaultServicePact;
			}
			if (defaultServicePactId != Guid.Empty) {
				resultList.Add(new SuitableServicePact(defaultServicePactId, GetServicePactName(defaultServicePactId), 
					5));
			}

			return resultList.Distinct();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// ########## ###### ########## ######### #########. ######## ############ ## #########
		/// ######## ############ ######### ######## # ###########.
		/// </summary>
		/// <param name="request">###### ## ########### #########</param>
		/// <returns>###### ########## ######### #########</returns>
		public virtual IEnumerable<SuitableServicePact> GetSuitableServicePacts(SuitableServicePactsRequest request) {
			_request = request;
			_contact = GetContactEntityInfo(_request.ContactId);
			return ConvertToSuitableServicePacts(GetServiceObjects());
		}

		#endregion
	}

	#endregion

	#region Class: SuitableServicePactExtenssions

	/// <summary>
	/// ############ ########## ### #######, ####### ######### # ########### ########## ######### #########.
	/// </summary>
	public static class SuitableServicePactExtensions
	{
		#region Methods: Public

		/// <summary>
		/// ########## ###### ########### ########## ######## ## ###### ########, ######### ## ##.
		/// </summary>
		/// <param name="serviceObject">######## ######### #######</param>
		/// <param name="suitableLevel">########## ######## ######## ###### ######### #######</param>
		/// <returns>###### ########### ########## ########</returns>
		public static SuitableServicePact ToSuitableServicePact(this Entity serviceObject, int suitableLevel) {
			return new SuitableServicePact(serviceObject.GetTypedColumnValue<Guid>("ServicePactId"),
				serviceObject.GetTypedColumnValue<string>("ServicePactName"), suitableLevel);
		}

		#endregion
	}

	#endregion
}
