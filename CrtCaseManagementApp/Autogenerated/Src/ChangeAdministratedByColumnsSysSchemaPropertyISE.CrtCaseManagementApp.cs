 namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	public class ChangeAdministratedByColumnsSysSchemaPropertyISE : IInstallScriptExecutor
	{
		private string[] _schemasName = new string[] { "Case", "KnowledgeBase" };
		private string _maintainer = "Terrasoft";

		public void Execute(UserConnection userConnection) {
			_schemasName.ForEach(schemaName => {
				var select = new Select(userConnection).From("SysSchemaProperty").As("SSP")
					.Column("SSP", "Id")
					.Column("SP", "Maintainer")
					.InnerJoin("SysSchema").As("SS").On("SSP", "SysSchemaId").IsEqual("SS", "Id")
					.InnerJoin("SysPackage").As("SP").On("SS", "SysPackageId").IsEqual("SP", "Id")
					.Where("SSP", "Name").IsEqual(Column.Parameter("AdministratedByColumns"))
						.And("SS", "Name").IsEqual(Column.Parameter(schemaName)) as Select;
				var records = new List<(Guid id, string maintainer)>();
				using (DBExecutor dbExecutor = userConnection.EnsureDBConnection()) {
					using (IDataReader r = select.ExecuteReader(dbExecutor)) {
						while (r.Read()) {
							records.Add((id: r.GetColumnValue<Guid>("Id"), maintainer: r.GetColumnValue<string>("Maintainer")));
						}
					}
				}
				if (records.All(r => r.maintainer == _maintainer)) {
					var update = new Update(userConnection, "SysSchemaProperty")
						.Set("Value", Column.Parameter("True"))
						.Where("Id").In(Column.Parameters(records.Select(r => r.id)));
					update.Execute();
				}
			});

		}
	}
}
