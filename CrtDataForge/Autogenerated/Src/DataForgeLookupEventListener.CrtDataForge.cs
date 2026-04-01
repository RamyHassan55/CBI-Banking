using Creatio.FeatureToggling;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;
using static Terrasoft.Configuration.DataForge.DataForgeFeatures;

namespace Terrasoft.Configuration.DataForge
{

	/// <summary>
	/// Listens to entity lifecycle events (insert, update, delete) 
	/// and synchronizes lookup and lookup value data with the DataForge service.
	/// </summary>
	[EntityEventListener(IsGlobal = true)]
	public class DataForgeLookupEventListener : BaseEntityEventListener
	{

		#region Methods: Private

		private static void ProcessEntity(object sender, EntityAfterEventArgs e) {
			if (!Features.GetIsEnabled<RealtimeLookupSync>()) {
				return;
			}

			var entity = (Entity)sender;
			var userConnection = entity.UserConnection;

			var checksumProvider = ClassFactory.Get<IChecksumProvider>(
				new ConstructorArgument("userConnection", userConnection)
			);
			var lookupHandler = ClassFactory.Get<ILookupHandler>(
				new ConstructorArgument("userConnection", userConnection),
				new ConstructorArgument("checksumProvider", checksumProvider)
			);
			var dataForgeServiceFactory = ClassFactory.Get<IDataForgeServiceFactory>(
				new ConstructorArgument("userConnection", userConnection)
			);
			var dataForgeService = dataForgeServiceFactory.Create();

			if (lookupHandler.IsLookup(entity)) {
				dataForgeService.UploadLookup(entity);
			} else if (lookupHandler.IsLookupValue(entity)) {
				dataForgeService.UpdateLookupsForValue(entity);
			}
		}

		private static void ProcessDeletedEntity(object sender, EntityAfterEventArgs e) {
			if (!Features.GetIsEnabled<RealtimeLookupSync>()) {
				return;
			}

			var entity = (Entity)sender;
			var userConnection = entity.UserConnection;

			var checksumProvider = ClassFactory.Get<IChecksumProvider>(
				new ConstructorArgument("userConnection", userConnection)
			);
			var lookupHandler = ClassFactory.Get<ILookupHandler>(
				new ConstructorArgument("userConnection", userConnection),
				new ConstructorArgument("checksumProvider", checksumProvider)
			);
			var dataForgeServiceFactory = ClassFactory.Get<IDataForgeServiceFactory>(
				new ConstructorArgument("userConnection", userConnection)
			);
			var dataForgeService = dataForgeServiceFactory.Create();

			if (lookupHandler.IsLookup(entity)) {
				dataForgeService.DeleteLookup(entity);
			} else if (lookupHandler.IsLookupValue(entity)) {
				dataForgeService.UpdateLookupsForValue(entity);
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Handles the logic to execute after an entity is inserted.
		/// Creates a lookup or lookup value in the DataForge service based on the entity type.
		/// </summary>
		public override void OnInserted(object sender, EntityAfterEventArgs e) {
			base.OnInserted(sender, e);
			ProcessEntity(sender, e);
		}

		/// <summary>
		/// Handles the logic to execute after an entity is updated.
		/// Updates the corresponding lookup or lookup value in the DataForge service.
		/// </summary>
		public override void OnUpdated(object sender, EntityAfterEventArgs e) {
			base.OnUpdated(sender, e);
			ProcessEntity(sender, e);
		}

		/// <summary>
		/// Handles the logic to execute after an entity is deleted.
		/// Deletes the corresponding lookup or lookup value from the DataForge service.
		/// </summary>
		public override void OnDeleted(object sender, EntityAfterEventArgs e) {
			base.OnDeleted(sender, e);
			ProcessDeletedEntity(sender, e);
		}

		#endregion
	}
}
