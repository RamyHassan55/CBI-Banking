namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;

	#region Class: CaseClosureEventListener

	[EntityEventListener(SchemaName = "Case")]
	public class CaseClosureEventListener : BaseEntityEventListener
	{

		#region Methods: private
		
		private (Guid, bool) GetClosureCode(Guid caseStatusId, UserConnection userConnection) {
			var select = new Select(userConnection)
				.Column("ClosureCodeId")
				.Column("IsFinal")
			.From("CaseStatus")
			.Where("Id").IsEqual(Column.Parameter(caseStatusId)) as Select;

			select.ExecuteSingleRecord(reader => (
				reader.GetColumnValue<Guid>("ClosureCodeId"),
				reader.GetColumnValue<bool>("IsFinal")
			), out var result);
			return result;
		}
		
		private Guid GetDefaultClosureCode(UserConnection userConnection) {
			return Core.Configuration.SysSettings.GetValue(userConnection, "CaseClosureCodeDef", Guid.Empty);
		}

		#endregion

		#region Methods: public
		/// <summary>
		/// Before case saved event handler
		/// </summary>
		/// <param name="sender">Case entity</param>
		/// <param name="e"></param>
		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			base.OnSaving(sender, e);
			Entity currentCase = (Entity)sender;
			if (currentCase.GetTypedColumnValue<Guid>("ClosureCodeId") != Guid.Empty) {
				return;
			}
			var userConnection = currentCase.UserConnection;
			if (!userConnection.GetIsFeatureEnabled("CommonCaseClosureCode")) {
				return;
			}
			(Guid closureCode, bool isFinalStatus) = GetClosureCode(currentCase.GetTypedColumnValue<Guid>("StatusId"), userConnection);
			if (!isFinalStatus) {
				return;
			}
			if (closureCode == Guid.Empty) {
				closureCode = GetDefaultClosureCode(userConnection);
			}
			if (closureCode != Guid.Empty) {
				currentCase.SetColumnValue("ClosureCodeId", closureCode);
			}
		}
		#endregion

	}

	#endregion
}
