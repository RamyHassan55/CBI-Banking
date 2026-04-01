namespace Terrasoft.Configuration
{
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: CaseServiceActivityEntityEventListener

	[EntityEventListener(SchemaName = "Activity", Name = nameof(CaseServiceActivityEntityEventListener))]
	internal class CaseServiceActivityEntityEventListener : BaseEntityEventListener
	{

		#region Methods: Public

		/// <summary>Handles entity Inserting event.</summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">The <see cref="T:Terrasoft.Core.Entities.EntityBeforeEventArgs" /> 
		/// instance containing the event data.</param>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			var handler = ClassFactory.Get<ICaseServiceActivityEventsHandler>();
			var activity = sender as Entity;
			handler.HandleActivityInserted(activity, activity.UserConnection);
			base.OnInserted(sender, e);
		}

		/// <summary>Handles entity Updating event.</summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">The <see cref="T:Terrasoft.Core.Entities.EntityBeforeEventArgs" /> 
		/// instance containing the event data.</param>
		public override void OnUpdating(object sender, EntityBeforeEventArgs e) {
			var handler = ClassFactory.Get<ICaseServiceActivityEventsHandler>();
			var activity = sender as Entity;
			handler.HandleActivityUpdating(activity, activity.UserConnection);
			base.OnUpdating(sender, e);
		}

		/// <summary>Handles entity Updated event.</summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">The <see cref="T:Terrasoft.Core.Entities.EntityAfterEventArgs" /> 
		/// instance containing the event data.</param>
		public override void OnUpdated(object sender, EntityAfterEventArgs e) {
			var handler = ClassFactory.Get<ICaseServiceActivityEventsHandler>();
			var activity = sender as Entity;
			handler.HandleActivityUpdated(activity, activity.UserConnection);
			base.OnUpdated(sender, e);
		}

		#endregion

	}

	#endregion

}

