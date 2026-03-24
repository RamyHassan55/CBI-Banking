namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: EmailFooterSupplier

	/// <summary>
	/// Form reply part using Activty.
	/// </summary>
	public class EmailFooterSupplier
	{
		#region Constants: Protected

		protected const string HtmlTemplate = @"<br><div><hr><span style='font-weight: bold;'>{6}:
			</span><span>{0}</span><br><span style='font-weight: bold;'>{7}:
			</span><span>{1}</span><br><span style='font-weight: bold;'>{8}: 
			</span><span>{2}</span><br><span style='font-weight: bold;'>{9}: 
			</span><span>{3}</span><br><span style='font-weight: bold;'>{10}: 
			</span><span>{4}</span><br><div>{5}";
		protected const string PlainTextTemplate = "{6}: {0}{7}: {1}{8}: {2}{9}: {3}{10}: {4}{5}";
		protected const string FromCaption = "From";
		protected const string SendCaption = "Sent on";
		protected const string RecipientCaption = "To";
		protected const string CopyRecipientCaption = "Cc";
		protected const string TitleCaption = "Subject";

		#endregion

		#region Properties: Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
			private set;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailFooterSupplier"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public EmailFooterSupplier(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private string GetReplyTemplate(bool isHtmlBody) {
			return isHtmlBody ? HtmlTemplate : PlainTextTemplate;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Add Activity columns wich will be selected.
		/// </summary>
		/// <param name="esq">Activity esq.</param>
		protected virtual void AddActivityColumns(EntitySchemaQuery esq) {
			esq.AddColumn("Body");
			esq.AddColumn("Sender");
			esq.AddColumn("StartDate");
			esq.AddColumn("Recepient");
			esq.AddColumn("CopyRecepient");
			esq.AddColumn("Title");
			esq.AddColumn("IsHtmlBody");
		}

		/// <summary>
		/// Get activity data.
		/// </summary>
		/// <param name="activityId">Activity identifier.</param>
		protected virtual Entity GetActivityData(Guid activityId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Activity");
			AddActivityColumns(esq);
			return esq.GetEntity(UserConnection, activityId);
		}

		/// <summary>
		/// Format template using activity data.
		/// </summary>
		/// <param name="template">format template.</param>
		/// <param name="Entity">Activity entity.</param>
		protected virtual string GenerateReplyBody(string template, Entity activity) {
			return String.Format(template, activity.GetTypedColumnValue<string>("Sender"),
				activity.GetTypedColumnValue<string>("StartDate"), activity.GetTypedColumnValue<string>("Recepient"),
				activity.GetTypedColumnValue<string>("CopyRecepient"), activity.GetTypedColumnValue<string>("Title"),
				activity.GetTypedColumnValue<string>("Body"), FromCaption,SendCaption, RecipientCaption, CopyRecipientCaption,
				TitleCaption);
		}

		/// <summary>
		/// Get template and generate reply.
		/// </summary>
		/// <param name="activity">Activity entity.</param>
		protected virtual string GenerateReply(Entity activity) {
			var isHtmlBody = activity.GetTypedColumnValue<bool>("IsHtmlBody");
			var template = GetReplyTemplate(isHtmlBody);
			return GenerateReplyBody(template, activity);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Form footer using activity.
		/// </summary>
		/// <param name="activityId">Activity identifier.</param>
		public virtual string GetFooter(Guid activityId) {
			var activity = GetActivityData(activityId);
			string footer = activity != null ? GenerateReply(activity) : String.Empty;
			return footer;
		}

		#endregion

	}

	#endregion

}
