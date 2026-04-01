using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;

namespace Terrasoft.Configuration
{
	public static class SysEntityRightsHelper
	{
		public static readonly Guid ManualRightSourceId = new Guid("8A248A03-E9A5-DF11-9989-485B39C18470");

		private const string SysRightTablePattern = "Sys{0}Right";

		public class CopyEntityRightsParams
		{
			public string SourceSchemaName { get; private set; }

			public Guid SourceEntityId { get; private set; }

			public string TargetSchemaName { get; private set; }

			public Guid TargetEntityId { get; private set; }

			public CopyEntityRightsParams(string sourceSchemaName, Guid sourceEntityId, string targetSchemaName, Guid targetEntityId)
			{
				SourceSchemaName = sourceSchemaName;
				SourceEntityId = sourceEntityId;
				TargetSchemaName = targetSchemaName;
				TargetEntityId = targetEntityId;
			}
		}

		public static int CopyEntityAdministrateByRecordsRights(UserConnection userConnection, CopyEntityRightsParams copyParams)
		{
			var sourceSchema = userConnection.EntitySchemaManager.GetInstanceByName(copyParams.SourceSchemaName);
			var targetSchema = userConnection.EntitySchemaManager.GetInstanceByName(copyParams.TargetSchemaName);
			if (!sourceSchema.AdministratedByRecords || !targetSchema.AdministratedByRecords) {
				return 0;
			}
			var sourceRightTableName = string.Format(SysRightTablePattern, copyParams.SourceSchemaName);
			var targetRightTableName = string.Format(SysRightTablePattern, copyParams.TargetSchemaName);

			var targetRightsSelect = new Select(userConnection)
				.Column("Id")
				.Column("SysAdminUnitId")
				.Column("Operation")
				.From(targetRightTableName)
				.Where("RecordId").IsEqual(Column.Const(copyParams.TargetEntityId));

			var sourceRightsSelect = new Select(userConnection)
				.Column(Column.Const(DateTime.UtcNow)).As("CreatedOn")
				.Column(Column.Const(userConnection.CurrentUser.ContactId)).As("CreatedById")
				.Column(Column.Const(DateTime.UtcNow)).As("ModifiedOn")
				.Column(Column.Const(userConnection.CurrentUser.ContactId)).As("ModifiedById")
				.Column(Column.Const(copyParams.TargetEntityId)).As("RecordId")
				.Column("srt", "SysAdminUnitId")
				.Column("srt", "Operation")
				.Column("srt", "RightLevel")
				.Column("srt", "Position")
				.Column(Column.Const(ManualRightSourceId)).As("SourceId")
				.From(sourceRightTableName).As("srt")
				.LeftOuterJoin(targetRightsSelect).As("trt")
				.On("srt", "SysAdminUnitId").IsEqual("trt", "SysAdminUnitId")
				.And("srt", "Operation").IsEqual("trt", "Operation")
				.Where("srt", "RecordId").IsEqual(Column.Const(copyParams.SourceEntityId))
				.And("trt", "Id").IsNull();

			var insertTargetRights = new InsertSelect(userConnection)
				.Into(targetRightTableName)
				.Set("CreatedOn", "CreatedById", "ModifiedOn", "ModifiedById", "RecordId", "SysAdminUnitId", "Operation", "RightLevel", "Position", "SourceId")
				.FromSelect(sourceRightsSelect);

			return insertTargetRights.Execute();
		}
	}
}
