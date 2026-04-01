 namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;

	public class GrantRightsToExternalColumnsExecutor : IInstallScriptExecutor
	{

		#region Fields: Private

		private UserConnection _userConnection;
		private EntitySchemaManager _entitySchemaManager;
		private readonly Guid _portalRoleGuid = new Guid("720B771C-E7A7-4F31-9CFB-52CD21C3739F");
		private readonly string _caseSchemaUId = "117D32F9-8275-4534-8411-1C66115CE9CD";
		private readonly Guid _allEmployesRoleGuid = new Guid("A29A3BA5-4B0D-DE11-9A51-005056C00008");
		private List<string> _caseSchemaColumns = new List<string> { "AE0E45CA-C495-4FE7-A39D-3AB7278E1617", "E80190A5-03B2-4095-90F7-A193A960ADEE", 
			"EBF6BB93-8AA6-4A01-900D-C6EA67AFFE21", "9928EDEC-4272-425A-93BB-48743FEE4B04", "3015559E-CBC6-406A-88AF-07F7930BE832", 
			"3FABD836-6A53-4D8D-9069-6DF88D9DAE1E", "7B8CE6B4-29CA-40C7-BEA1-B5BF7B7C404A", "C91A9A6F-008D-4B2E-83D5-03868AD68C99", 
			"FFE8526D-044F-4222-A1EF-FCA83A0772D8", "6F9B3E63-5998-4C16-B1B0-CD712516AD18", "70620D00-E4EA-48D1-9FDC-4572FCD8D41B", 
			"25280121-C075-441F-B4F8-FEEB6CC7CA38", "624839D1-3BD0-45E0-95D1-28326703F19C", "A71ADAEA-3464-4DEE-BB4F-C7A535450AD7", 
			"4BC0DB67-0ABE-4FBD-A70A-54D0BEE0C42D", "6A2E8EE8-F1CB-45AD-A731-6B082109D507", "A93CB111-CE30-4DA4-89EC-D2A2F3DD71C4", 
			"B15302C9-B5C4-4D94-AFD5-3D409F9ADFE1", "6396F46E-C49F-4FB1-850D-824869FF04B3", "9147230C-AB53-4EB4-B0B4-AC78BE6F8326",
			"02612DC8-7243-4ACB-B0BD-ABBD19E24136", "81552F0A-FD92-4865-9533-F4C3AB2861A7", "E12191FF-2811-430D-AECA-7A4435E4B1B9", 
			"D2FB7A30-8A2E-462B-963A-B88FF1F52F0E", "AAFDA194-DBD6-4337-923A-B19A456EEEA9", "CCFC5FBF-4DC9-47E4-91F3-54EA9251AB18", 
			"AF280321-E749-41DD-98E5-383906747E29", "1ED4E080-0BF8-4F4F-97E8-B3E22F3240A0", "0FA66EFC-D2D0-47B9-ABAB-9E3AD2EA82D3", 
			"E66437F6-089C-4F5C-B4BA-D1F8C71506D0", "FCE13454-F217-4C5E-AF89-35E4D8EBDDA3", "6AB42472-7828-4D51-A3E6-C38B485F3252",
			"61CCDC6D-F9DC-4B22-A86D-212127E717A6", "35F11903-AB15-47EE-B0A4-2F003922667D", "869FD0EA-052E-4107-B426-EA8176E370DC", 
			"6BC89A93-6A89-4D71-AD7C-DF9D81C15978", "A587B494-47F3-4B27-BE42-A259E579F061", "86DD6835-A362-4BC6-AFEE-03C603CF8335", 
			"86567001-01F1-403B-8B2E-34EE5D17C811", "C0B67CD6-C592-4F3E-9EFE-12E783C4D396", "2042C68F-9121-4974-87FD-D7D8C8B596F0", 
			"58CEC386-8FDF-42E3-8014-CA4001C615EC", "28DFD64F-463A-449C-9FCD-A9BFED4A5686", "7C5530E9-CA8E-4FA7-B4B1-C88631206E5A", 
			"B59A15EA-751E-4FD8-8281-F1A3DC7190FF", };
		private List<string> _systemColumns = new List<string> { "EBF6BB93-8AA6-4A01-900D-C6EA67AFFE21","3015559E-CBC6-406A-88AF-07F7930BE832",
			"9928EDEC-4272-425A-93BB-48743FEE4B04","E80190A5-03B2-4095-90F7-A193A960ADEE"};

		#endregion

		#region Methods: Private

		private List<(string SchemaUId, string ColumnUId)> GetColumns() {
			var columns = new List<(string SchemaUId, string columnUId)>();
			var select =
				new Select(_userConnection)
					.Column("PortalSchemaAccessList", "SchemaUId").As("SchemaUId")
					.Column("PortalColumnAccessList", "ColumnUId").As("ColumnUId")
				.From("PortalSchemaAccessList")
					.InnerJoin("PortalColumnAccessList").On("PortalColumnAccessList", "PortalSchemaListId")
						.IsEqual("PortalSchemaAccessList", "Id")
					 as Select;
			using (DBExecutor dbExecutor = _userConnection.EnsureDBConnection()) {
				using (IDataReader dataReader = select.ExecuteReader(dbExecutor)) {
					while (dataReader.Read()) {
						columns.Add((
							 dataReader.GetColumnValue<string>("SchemaUId"),
							 dataReader.GetColumnValue<string>("ColumnUId")
						));
					}
				}
				var schemaUids = columns.Select(column => column.SchemaUId).Distinct().ToList();
				foreach (var schemaUId in schemaUids) {
					foreach (var columnUId in _systemColumns) {
						columns.Add((schemaUId, columnUId));
					}
				}
				return columns;
			}
		}
		
		private void SetEntitySchemaColumnRightLevel(Guid adminUnitId, Guid schemaUId, Guid columnUId,
				EntitySchemaColumnRightLevel rightLevel, int? position = null) {
			Select recordIdSelect =
				new Select(_userConnection)
					.Column("Id")
				.From("SysEntitySchemaColumnRight")
				.Where("SubjectSchemaUId")
					.IsEqual(Column.Parameter(schemaUId))
				.And("SubjectColumnUId")
					.IsEqual(Column.Parameter(columnUId))
				.And("SysAdminUnitId")
					.IsEqual(Column.Parameter(adminUnitId)) as Select;
			Guid recordId = Guid.Empty;
			using (DBExecutor dbExecutor = _userConnection.EnsureDBConnection()) {
				using (IDataReader dataReader = recordIdSelect.ExecuteReader(dbExecutor)) {
					if (dataReader.Read()) {
						recordId = _userConnection.DBTypeConverter.DBValueToGuid(dataReader[0]);
					}
				}
			}
			if (!recordId.Equals(Guid.Empty)) {
				return;
			}
			recordId = Guid.NewGuid();
			Insert columnRightsInsert =
				new Insert(_userConnection)
					.Into("SysEntitySchemaColumnRight")
					.Set("Id", Column.Parameter(recordId))
					.Set("CreatedOn", Column.Parameter(DateTime.Now))
					.Set("CreatedById", Column.Parameter(_userConnection.CurrentUser.Id))
					.Set("ModifiedOn", Column.Parameter(DateTime.Now))
					.Set("ModifiedById", Column.Parameter(_userConnection.CurrentUser.Id))
					.Set("SubjectSchemaUId", Column.Parameter(schemaUId))
					.Set("SubjectColumnUId", Column.Parameter(columnUId))
					.Set("SysAdminUnitId", Column.Parameter(adminUnitId))
					.Set("RightLevel", Column.Parameter(rightLevel));
			StoredProcedure setRecordPositionProcedure =
				new StoredProcedure(_userConnection, "tsp_SetRecordPosition")
					.WithParameter("TableName", "SysEntitySchemaColumnRight")
					.WithParameter("PrimaryColumnName", "Id")
					.WithParameter("PrimaryColumnValue", recordId)
					.WithParameter("GrouppingColumnNames", "SubjectSchemaUId, SubjectColumnUId")
					.WithParameter("Position", position ?? 0) as StoredProcedure;
			setRecordPositionProcedure.PackageName = _userConnection.DBEngine.SystemPackageName;
			using (DBExecutor dbExecutor = _userConnection.EnsureDBConnection()) {
				dbExecutor.StartTransaction();
				columnRightsInsert.Execute(dbExecutor);
				setRecordPositionProcedure.Execute(dbExecutor);
				dbExecutor.CommitTransaction();
			}
		}

		private void GrantRights() {
			Entity portalColumnAccessListEntity = _entitySchemaManager.GetEntityByName("PortalColumnAccessList", _userConnection);
			if (portalColumnAccessListEntity == null) {
				return;
			}
			List<(string SchemaUId, string ColumnUId)> columns = GetColumns();
			foreach (var column in columns) {
				if (!string.IsNullOrEmpty(column.SchemaUId) && !string.IsNullOrEmpty(column.ColumnUId)) {
					SetEntitySchemaColumnRightLevel(
						_portalRoleGuid, 
						Guid.Parse(column.SchemaUId), 
						Guid.Parse(column.ColumnUId), 
						EntitySchemaColumnRightLevel.CanEdit);
					SetEntitySchemaColumnRightLevel(
						_allEmployesRoleGuid,
						Guid.Parse(column.SchemaUId),
						Guid.Parse(column.ColumnUId),
						EntitySchemaColumnRightLevel.CanEdit);
				}
			}
			AddRightsToAllColumnsForAllEmployees(columns);
		}

		private void AddRightsToAllColumnsForAllEmployees(List<(string SchemaUId, string ColumnUId)> columns) {
			GrantRightsForAllEmployees(GetAllCaseColumnsWithoutAvailableForExternal(columns));
		}

		private void GrantRightsForAllEmployees(List<(string SchemaUId, string ColumnUId)> columns) {
			foreach (var column in columns)	{
				if (!string.IsNullOrEmpty(column.SchemaUId) && !string.IsNullOrEmpty(column.ColumnUId))	{
					SetEntitySchemaColumnRightLevel(
						_allEmployesRoleGuid,
						Guid.Parse(column.SchemaUId),
						Guid.Parse(column.ColumnUId),
						EntitySchemaColumnRightLevel.CanEdit);
				}
			}
		}

		private List<(string SchemaUId, string ColumnUId)> GetAllCaseColumnsWithoutAvailableForExternal(List<(string SchemaUId, string ColumnUId)> columns) {
			var resultColumns = new List<(string SchemaUId, string ColumnUId)>();
			foreach (var columnItem in _caseSchemaColumns) {
				if (!columns.Any(column => column.SchemaUId == _caseSchemaUId && column.ColumnUId == columnItem))	{
					resultColumns.Add((_caseSchemaUId, columnItem));
				}
			}
			return resultColumns;
		}

		#endregion

		#region Methods: Public

		public void Execute(UserConnection userConnection) {
			_userConnection = userConnection;
			_entitySchemaManager = userConnection.EntitySchemaManager;
			GrantRights();
		}

		#endregion

	}
}

