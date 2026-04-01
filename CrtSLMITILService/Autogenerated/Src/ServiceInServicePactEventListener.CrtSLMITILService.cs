namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;

	#region Class : ServiceInServicePactEventListener

	/// <summary>
	/// Class, that contains event listeners for ServiceInServicePact.
	/// </summary>
	[EntityEventListener(SchemaName = "ServiceInServicePact")]
	public class ServiceInServicePactEventListener : BaseEntityEventListener
	{
		#region Methods : Private

		private static string FetchPrimaryDisplayValue(string schemaName, Guid id, UserConnection userConnection) {
			var entitySchema = userConnection.EntitySchemaManager.GetInstanceByName(schemaName);
			var entity = entitySchema.CreateEntity(userConnection);
			entity.FetchFromDB(id);
			return entity.PrimaryDisplayColumnValue;
		}

		private static Entity GetServiceItem(Guid serviceItemId, UserConnection userConnection) {
			var serviceItemSchema = userConnection.EntitySchemaManager.GetInstanceByName("ServiceItem");
			var serviceItem = serviceItemSchema.CreateEntity(userConnection);
			serviceItem.FetchFromDB(serviceItemId);
			return serviceItem;
		}

		private static void CopyServiceItemData(Entity serviceInServicePact) {
			var userConnection = serviceInServicePact.UserConnection;
			var service = GetServiceItem(serviceInServicePact.GetTypedColumnValue<Guid>("ServiceItemId"), userConnection);

			IEnumerable<string> columnNamesToCopy = new string[] {
				"ReactionTimeValue",
				"ReactionTimeUnit",
				"SolutionTimeValue",
				"SolutionTimeUnit",
				"Status"
			};
			columnNamesToCopy.ForEach(columnName => {
				CoalesceValues(columnName, service, serviceInServicePact);
			});
			
			serviceInServicePact.SetColumnValue("ReactionTime", GetTimeString(
				serviceInServicePact.GetTypedColumnValue<Guid>("ReactionTimeUnitId"),
				serviceInServicePact.GetTypedColumnValue<int>("ReactionTimeValue"),
				userConnection
			));

			serviceInServicePact.SetColumnValue("SolutionTime", GetTimeString(
				serviceInServicePact.GetTypedColumnValue<Guid>("SolutionTimeUnitId"),
				serviceInServicePact.GetTypedColumnValue<int>("SolutionTimeValue"),
				userConnection
			));

			string servicePactName = FetchPrimaryDisplayValue("ServicePact",
				serviceInServicePact.GetTypedColumnValue<Guid>("ServicePactId"), userConnection);
			string serviceItemName = service.GetTypedColumnValue<string>("Name");
			serviceInServicePact.SetColumnValue("ConcatName", GetServiceInServicePactName(serviceItemName, servicePactName));
		}

		private static void CoalesceValues(string columnName, Entity source, Entity destination) {
			var columnPath = destination.Schema.GetSchemaColumnByPath(columnName).IsLookupType ? $"{columnName}Id" : columnName;
			if (destination.GetColumnValue(columnPath) == null) {
				destination.SetColumnValue(columnPath, source.GetColumnValue(columnPath));
			}
		}

		private static string GetTimeString(Guid timeUditId, int value, UserConnection userConnection) {
			var timeUnitTitle = FetchPrimaryDisplayValue("TimeUnit", timeUditId, userConnection);
			return $"{timeUnitTitle}: {value}";
		}

		private static string GetServiceInServicePactName(string serviceItemName, string servicePactName) {
			if (serviceItemName != null && servicePactName != null) {
				string concatName = serviceItemName + " " + servicePactName;
				concatName = concatName.Length > 500 ? concatName.Substring(0, 500) : concatName;
				return concatName;
			} else {
				return "Default name";
			}
		}

		private static void FillReactionSolutionTime(Entity serviceInServicePact) {
			var userConnection = serviceInServicePact.UserConnection;
			var reactionTimeUnitId = serviceInServicePact.GetTypedColumnValue<Guid>("ReactionTimeUnitId");
			var reactionTimeUnitName = (reactionTimeUnitId == Guid.Empty) ? "" : FetchPrimaryDisplayValue("TimeUnit", reactionTimeUnitId, userConnection);
			var solutionTimeUnitId = serviceInServicePact.GetTypedColumnValue<Guid>("SolutionTimeUnitId");
			string solutionTimeUnitName;
			if (solutionTimeUnitId == reactionTimeUnitId) {
				solutionTimeUnitName = reactionTimeUnitName;
			} else {
				solutionTimeUnitName = (solutionTimeUnitId == Guid.Empty) ? "" : FetchPrimaryDisplayValue("TimeUnit", solutionTimeUnitId, userConnection);
			}
			serviceInServicePact.SetColumnValue("ReactionTime",
				$"{serviceInServicePact.GetTypedColumnValue<int>("ReactionTimeValue")} {reactionTimeUnitName}");
			serviceInServicePact.SetColumnValue("SolutionTime",
				$"{serviceInServicePact.GetTypedColumnValue<int>("SolutionTimeValue")} {solutionTimeUnitName}");
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnInserting"/>
		/// </summary>
		public override void OnInserting(object sender, EntityBeforeEventArgs e) {
			base.OnInserting(sender, e);
			Entity serviceInServicePact = (Entity)sender;
			UserConnection userConnection = serviceInServicePact.UserConnection;
			if (!Features.GetIsEnabled("UseServiceInServicePactInSLMITILServiceOldFunc")) {
				CopyServiceItemData(serviceInServicePact);
				FillReactionSolutionTime(serviceInServicePact);
			}
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnUpdating"/>
		/// </summary>
		public override void OnUpdating(object sender, EntityBeforeEventArgs e) {
			base.OnUpdating(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
			if (!Features.GetIsEnabled("UseServiceInServicePactInSLMITILServiceOldFunc")) {
				FillReactionSolutionTime(entity);
			}
		}

		#endregion

	}

	#endregion

}
