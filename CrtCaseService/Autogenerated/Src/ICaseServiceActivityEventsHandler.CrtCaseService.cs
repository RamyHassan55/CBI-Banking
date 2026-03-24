namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Interface: ICaseServiceActivityEventsHandler

	public interface ICaseServiceActivityEventsHandler
	{

		#region Methods: Public

		void HandleActivityInserted(Entity activityEntity, UserConnection userConnection,
			Action notificationAction = null);

		void HandleActivityUpdating(Entity activityEntity, UserConnection userConnection,
			Func<bool> getIsCategoryFilterMethod = null, Func<Guid, bool> isRequiredFieldsValidMethod = null,
			Action<Guid, Guid> setCategoryAction = null, Func<string, string, string> joinRecipientsMethod = null,
			Func<Guid, string> getRecipientsMethod = null);

		void HandleActivityUpdated(Entity activityEntity, UserConnection userConnection,
			Action notificationAction = null);

		#endregion

	}

	#endregion

}

