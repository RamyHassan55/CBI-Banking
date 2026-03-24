 namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: ExtendedEmailWithMacrosManager

	/// <summary>
	/// E-mail sender which allows to control the flow of activity creating, filling and macroses processing.
	/// As a macros processor it uses <see cref="InvokableMacrosHelper"/>.
	/// It also automates e-mail sender obtaining if it is not specified.
	/// Add reply part from parent Activity.
	/// </summary>
	public class ExtendedEmailWithMacrosManagerBase : EmailWithMacrosManagerBase
	{
		#region Properties: Public

		/// <summary>
		/// Form footer part of reply email.
		/// instance of the <see cref="EmailFooterSupplier"/> class
		/// </summary>
		public EmailFooterSupplier EmailFooterSupplier	{ get; set; }

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedEmailWithMacrosManagerBase"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public ExtendedEmailWithMacrosManagerBase(UserConnection userConnection)
			: base(userConnection) {
			EmailFooterSupplier = new EmailFooterSupplier(userConnection);
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Get footer part using parent Activity.
		/// </summary>
		/// <param name="activityId">Activity identifier.</param>
		protected virtual string GetReplyPartFromParentActivity(Guid activityId) {
			return EmailFooterSupplier.GetFooter(activityId);
		}

		protected override void FillEntityWithCaseData(Entity activity, CaseData data) {
			base.FillEntityWithCaseData(activity, data);
			var replyPart = GetReplyPartFromParentActivity(data.ParentActivityId);
			var body = activity.GetTypedColumnValue<string>("Body");
			activity.SetColumnValue("Body", body + replyPart);
		}

		#endregion

	}

	#endregion
}
