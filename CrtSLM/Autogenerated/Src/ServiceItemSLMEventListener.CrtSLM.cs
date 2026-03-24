 using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;

namespace Terrasoft.Configuration
{

	#region Class: ServiceItemSLMEventListener

	[EntityEventListener(SchemaName = "ServiceItem")]
	public class ServiceItemSLMEventListener : BaseEntityEventListener
	{
	
		#region Methods: Public

		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			base.OnSaving(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "TimeUnit");
			esq.AddColumn("Name");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", entity.GetTypedColumnValue<Guid>("ReactionTimeUnitId")));
			var entityCollection = esq.GetEntityCollection(userConnection);
			entity.SetColumnValue("ReactionTime", entityCollection[0].GetTypedColumnValue<string>("Name") +": "+ entity.GetTypedColumnValue<int>("ReactionTimeValue").ToString());
			esq.Filters.Clear();
			esq.ResetSelectQuery();
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", entity.GetTypedColumnValue<Guid>("SolutionTimeUnitId")));
			entityCollection = esq.GetEntityCollection(userConnection);
			entityCollection[0].GetTypedColumnValue<string>("Name");
			entity.SetColumnValue("SolutionTime", entityCollection[0].GetTypedColumnValue<string>("Name") +": "+ entity.GetTypedColumnValue<int>("SolutionTimeValue").ToString());
		}

		#endregion

	}

	#endregion

}
