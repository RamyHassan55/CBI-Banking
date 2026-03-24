namespace CrtEmailDesigner
{
	using CrtEmailDesigner.Interfaces;
	using System;
	using System.Linq;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using Terrasoft.Core.Factories;

	#region Class: BulkEmailEventListener

	[EntityEventListener(SchemaName = "BulkEmail")]
	public class BulkEmailEventListener : BaseEntityEventListener
	{
		#region Methods: Private

		private void DeleteTemplates(Entity bulkEmailEntity) {
			var userConnection = bulkEmailEntity.UserConnection;
			var emailId = bulkEmailEntity.GetTypedColumnValue<Guid>("Id");
			IEmailTemplateDeleteStrategy deleteStrategy = ClassFactory.Get<IEmailTemplateDeleteStrategy>(
				new ConstructorArgument("userConnection", userConnection));
			var bfEmailTemplateRepository = ClassFactory.Get<IBfEmailTemplateRepository>(
				new ConstructorArgument("userConnection", userConnection));
			var existingLanguageTemplateIds = bfEmailTemplateRepository.FetchAllByEmailId(emailId).Select(x => x.LanguageId).ToArray();
			deleteStrategy.DeleteMany(emailId, existingLanguageTemplateIds, new string[] { });
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public override void OnDeleted(object sender, EntityAfterEventArgs e)
		{
			DeleteTemplates((Entity)sender);
			base.OnDeleted(sender, e);
		}

		#endregion

	}

	#endregion

}

