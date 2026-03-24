namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;

	#region Class: BaseCaseEntityEventListener

	[EntityEventListener(SchemaName = "Case")]
	public class BaseCaseEntityEventListener : BaseEntityEventListener
	{
		private UserConnection _userConnection;
		private Entity _case;
		private Guid _currentContactId;
		private Guid _oldOwnerId;
		private Guid _newOwnerId;

		#region Methods: Private

		private string GetLocalizableString(string key) {
			return new LocalizableString(_userConnection.Workspace.ResourceStorage,
					"BaseCaseEntityEventListener", $"LocalizableStrings.{key}.Value");
		}


		private void ValidateSatisfactionLevel() {
			if (SatisfactionLevelChanged() && !_userConnection.DBSecurityEngine.GetCanExecuteOperation("CanChangeCaseSatisfactionLevel")) {
				string validationMessageText = new LocalizableString(_userConnection.Workspace.ResourceStorage,
					"CasePage", "LocalizableStrings.SatisfactionLevelTip.Value");
				throw new Exception(validationMessageText);
			}
		}

		private bool SatisfactionLevelChanged() {
			return _case.GetChangedColumnValues().Any(c =>
								((c.Name == "SatisfactionLevelId" && c.OldValue != c.Value)
								|| (c.Name == "SatisfactionLevelComment"
								&& !(String.IsNullOrEmpty(c.OldValue as string)
								&& String.IsNullOrEmpty(c.Value as string)))));
		}

		private void SetInitialData(object sender) {
			_case = (Entity)sender;
			_userConnection = _case.UserConnection;
		}

		private void SetProcessParameters() {
			_currentContactId = _userConnection.CurrentUser.ContactId;
			_oldOwnerId = _case.GetTypedOldColumnValue<Guid>("OwnerId");
			_newOwnerId = _case.GetTypedColumnValue<Guid>("OwnerId");
		}

		private void SynchronizeRemindings() {
			var isNeedToSendRemind = _oldOwnerId != _newOwnerId;
			var sendToOldOwner = _oldOwnerId != Guid.Empty && _oldOwnerId != _currentContactId;
			var sendToNewOwner = _newOwnerId != Guid.Empty && _newOwnerId != _currentContactId && _oldOwnerId != _newOwnerId;
			if (isNeedToSendRemind || _case.IsInDeleting) {
				if (_userConnection.CurrentUser.ConnectionType != UserType.SSP && (sendToOldOwner || (_case.IsInDeleting && _oldOwnerId != Guid.Empty))) {
					PrepareSynchronizeRemindings(_oldOwnerId, GetLocalizableString("OldOwnerMessage"));
				}
				if (sendToNewOwner) {
					PrepareSynchronizeRemindings(_newOwnerId, GetLocalizableString("NewOwnerMessage"));
				}
			}
		}

		private void PrepareSynchronizeRemindings(Guid ownerId, string messageFormat) {
			Entity remindingEntity = _userConnection.EntitySchemaManager.GetInstanceByName("Reminding")
				.CreateEntity(_userConnection);
			var caseId = _case.GetTypedColumnValue<Guid>("Id");
			var number = _case.GetTypedColumnValue<string>("Number");
			var condition = new Dictionary<string, object> {
				{
					"Author", _userConnection.CurrentUser.ContactId
				}, {
					"Contact", ownerId
				}, {
					"Source", RemindingConsts.RemindingSourceAuthorId
				}, {
					"Number", number
				}
			};
			var str = new StringBuilder();
			foreach (object value in condition.Values) {
				str.Append(value);
			}
			string hash = FileUtilities.GetMD5Hash(Encoding.Unicode.GetBytes(str.ToString()));
			remindingEntity.SetDefColumnValues();
			remindingEntity.SetColumnValue("AuthorId", _userConnection.CurrentUser.ContactId);
			remindingEntity.SetColumnValue("ContactId", ownerId);
			remindingEntity.SetColumnValue("SourceId", RemindingConsts.RemindingSourceAuthorId);
			remindingEntity.SetColumnValue("RemindTime", _userConnection.CurrentUser.GetCurrentDateTime());
			string caseSubject = _case.GetTypedColumnValue<string>("Subject");
			caseSubject = caseSubject.Length > 300 ? caseSubject.Substring(0, 300) : caseSubject;
			var subjectCaption = string.Format(messageFormat, _case.GetTypedColumnValue<string>("Number"), caseSubject);
			remindingEntity.SetColumnValue("SubjectCaption", subjectCaption);
			var caseSchema = _userConnection.EntitySchemaManager.GetInstanceByName("Case");
			remindingEntity.SetColumnValue("SysEntitySchemaId", caseSchema.UId);
			remindingEntity.SetColumnValue("SubjectId", caseId);
			remindingEntity.SetColumnValue("Hash", hash);
			remindingEntity.Save();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Sets subject for portal case.
		/// </summary>
		public virtual void SetPortalCaseSubject() {
			var subject = _case.GetTypedColumnValue<string>("Subject");
			if (!string.IsNullOrEmpty(subject) || _userConnection.CurrentUser.ConnectionType != UserType.SSP) {
				return;
			}
			var categoryId = _case.GetTypedColumnValue<Guid>("CategoryId");
			var categoryCaption = GetCaseGategoryCaption(categoryId);
			var byCaption = categoryId != CaseConsts.CaseCategoryServiceCallId ? GetLocalizableString("SubjectByString") : GetLocalizableString("SubjectForString");
			var serviceCaption = GetServiceCaption();
			var generatedSubject = string.Format("{0} {1} {2} ", categoryCaption, byCaption, serviceCaption);
			_case.SetColumnValue("Subject", generatedSubject);
		}

		/// <summary>
		/// Gets service caption.
		/// </summary>
		/// <returns>Returns service caption.</returns>
		public virtual string GetServiceCaption() {
			var serviceItemId = _case.GetTypedColumnValue<Guid>("ServiceItemId");
			var serviceQuery = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "ServiceItem");
			serviceQuery.AddColumn("Name");
			var service = serviceQuery.GetEntity(_userConnection, serviceItemId);
			if (service == null) {
				return string.Empty;
			}
			return service.GetTypedColumnValue<string>("Name");
		}

		/// <summary>
		/// Gets category caption.
		/// </summary>
		/// <param name="categoryId">Identifier of category.</param>
		/// <returns>Returns category caption.</returns>
		public virtual string GetCaseGategoryCaption(Guid categoryId) {
			var categoryQuery = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "CaseCategory");
			categoryQuery.AddColumn("Name");
			var category = categoryQuery.GetEntity(_userConnection, categoryId);
			if (category == null) {
				return string.Empty;
			}
			return category.GetTypedColumnValue<string>("Name");
		}

		/// <summary>
		/// Sets symptoms to the case.
		/// </summary>
		public virtual void SetSymptoms() {
			if (string.IsNullOrEmpty(_case.GetTypedColumnValue<string>("Symptoms"))) {
				var subject = _case.GetTypedColumnValue<string>("Subject");
				_case.SetColumnValue("Symptoms", subject);
			}
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnSaving"/>
		/// </summary>
		public override void OnSaving(object sender, EntityBeforeEventArgs e) {
			base.OnSaving(sender, e);
			SetInitialData(sender);
			if (!_userConnection.GetIsFeatureEnabled("UseCaseInCaseOldFunc")) {
				SetPortalCaseSubject();
			}
			SetProcessParameters();
			ValidateSatisfactionLevel();
		}

		public override void OnSaved(object sender, EntityAfterEventArgs e) {
			base.OnSaved(sender, e);
			SynchronizeRemindings();
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnInserting"/>
		/// </summary>
		public override void OnInserting(object sender, EntityBeforeEventArgs e) {
			base.OnInserting(sender, e);
			SetInitialData(sender);
			if (!_userConnection.GetIsFeatureEnabled("UseCaseInCaseOldFunc")) {
				SetSymptoms();
			}
		}

		/// <summary>
		/// <see cref="BaseEntityEventListener.OnDeleting"/>
		/// </summary>
		public override void OnDeleting(object sender, EntityBeforeEventArgs e) {
			base.OnDeleting(sender, e);
			SetInitialData(sender);
			SetProcessParameters();
			SynchronizeRemindings();
		}

		#endregion

	}

	#endregion

}
