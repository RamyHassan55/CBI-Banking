namespace CrtEmailDesigner.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtContentDesigner.Models;
	using CrtContentDesigner.Repositories;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: BfEmailTemplateRepository

	/// <summary>
	/// Provides repository logic for BfTemplate.
	/// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
	[DefaultBinding(typeof(ITemplateRepository), Name = "Email")]
#pragma warning restore CS0618 // Type or member is obsolete
	[DefaultBinding(typeof(IBfEmailTemplateRepository))]
	internal class BfEmailTemplateRepository : IBfEmailTemplateRepository
	{

		#region Constants: Private

		private const string TemplateSchemaName = "BfEmailTemplate";

		#endregion

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="BfEmailTemplateRepository"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public BfEmailTemplateRepository(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private static EmailDesignerTemplateModel EntityToModel(Entity entity) {
			return new EmailDesignerTemplateModel {
				Id = entity.GetTypedColumnValue<string>("Id"),
				EmailId = entity.GetTypedColumnValue<Guid>("EmailId"),
				IsDefault = entity.GetTypedColumnValue<bool>("IsDefault"),
				LanguageId = entity.GetTypedColumnValue<Guid>("TemplateLanguageId"),
				LanguageCode = entity.GetTypedColumnValue<string>("TemplateLanguageCode"),
				Language = entity.GetTypedColumnValue<string>("Language"),
				TemplateVersion = entity.GetTypedColumnValue<int>("TemplateVersion"),
				PageHtml = entity.GetTypedColumnValue<string>("PageHtml"),
				PageJson = entity.GetTypedColumnValue<string>("PageJson"),
				AmpHtml = entity.GetTypedColumnValue<string>("AmpHtml")
			};
		}

		private Entity CreateEntity() {
			Entity entity = _userConnection.EntitySchemaManager.GetInstanceByName(TemplateSchemaName)
				.CreateEntity(_userConnection);
			entity.SetDefColumnValues();
			return entity;
		}

		private Entity CreateInternal(EmailDesignerTemplateModel model) {
			Entity entity = CreateEntity();
			entity.SetColumnValue("EmailId", model.EmailId);
			entity.SetColumnValue("IsDefault", model.IsDefault);
			entity.SetColumnValue("TemplateLanguageId", model.LanguageId);
			entity.SetColumnValue("PageHtml", model.PageHtml);
			entity.SetColumnValue("PageJson", model.PageJson);
			entity.SetColumnValue("AmpHtml", model.AmpHtml);
			entity.Save();
			return entity;
		}

		private EntityCollection FetchByIdsInternal(IEnumerable<Guid> templateIds) {
			EntitySchemaQuery query = GetQuery();
			var orFilter = new EntitySchemaQueryFilterCollection(query, LogicalOperationStrict.Or);
			foreach (Guid id in templateIds)
				orFilter.Add(query.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", id));
			query.Filters.Add(orFilter);
			return query.GetEntityCollection(_userConnection);
		}

		private EntitySchemaQuery GetQuery() {
			var query = new EntitySchemaQuery(_userConnection.EntitySchemaManager, TemplateSchemaName) {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			query.AddAllSchemaColumns();
			query.AddColumn("TemplateLanguage.Code").Name = "TemplateLanguageCode";
			return query;
		}

		private Entity LoadTemplateEntity(Guid templateId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BfEmailTemplate") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			IEntitySchemaQueryFilterItem filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", templateId);
			esqResult.Filters.Add(filterById);
			EntityCollection entities = esqResult.GetEntityCollection(_userConnection);
			return entities.FirstOrDefault();
		}

		private TemplateModel MapEntityToObject(Entity entity) {
			return new TemplateModel {
				Id = entity.GetTypedColumnValue<string>("Id"),
				AmpHtml = entity.GetTypedColumnValue<string>("AmpHtml"),
				Language = entity.GetTypedColumnValue<string>("Language"),
				PageHtml = entity.GetTypedColumnValue<string>("PageHtml"),
				PageJson = entity.GetTypedColumnValue<string>("PageJson"),
				TemplateVersion = entity.GetTypedColumnValue<int>("TemplateVersion")
			};
		}

		private bool SaveOrCreateTemplate(TemplateModel emailTemplate, Guid templateId, Entity templateEntity) {
			templateEntity.SetDefColumnValues();
			templateEntity.SetColumnValue("Id", templateId);
			templateEntity.SetColumnValue("AmpHtml", emailTemplate.AmpHtml ?? string.Empty);
			templateEntity.SetColumnValue("Language", emailTemplate.Language ?? string.Empty);
			templateEntity.SetColumnValue("PageHtml", emailTemplate.PageHtml ?? string.Empty);
			templateEntity.SetColumnValue("PageJson", emailTemplate.PageJson ?? "{}");
			templateEntity.SetColumnValue("TemplateVersion", emailTemplate.TemplateVersion);
			return templateEntity.Save();
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IBfEmailTemplateRepository.Create"/>
		public EmailDesignerTemplateModel Create(EmailDesignerTemplateModel model) {
			Entity entity = CreateInternal(model);
			return FetchById(entity.PrimaryColumnValue);
		}

		/// <inheritdoc cref="IBfEmailTemplateRepository.CreateMany"/>
		public IList<EmailDesignerTemplateModel> CreateMany(IEnumerable<EmailDesignerTemplateModel> models) {
			var resultIds = models.Select(CreateInternal).Select(entity => entity.PrimaryColumnValue).ToList();
			return FetchByIds(resultIds);
		}

		/// <inheritdoc cref="IBfEmailTemplateRepository.DeleteMany"/>
		public IList<EmailDesignerTemplateModel> DeleteMany(Guid[] templateIds) {
			EntitySchemaQuery query = GetQuery();
			var orFilter = new EntitySchemaQueryFilterCollection(query, LogicalOperationStrict.Or);
			foreach (Guid id in templateIds)
				orFilter.Add(query.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", id));
			query.Filters.Add(orFilter);
			IList<Entity> entities = FetchByIdsInternal(templateIds).ToList();
			var results = entities.Select(EntityToModel).ToList();
			foreach (Entity entity in entities)
				entity.Delete();
			return results;
		}

		/// <inheritdoc/>
		public IList<EmailDesignerTemplateModel> FetchAllByEmailId(Guid emailId) {
			EntitySchemaQuery query = GetQuery();
			query.Filters.Add(query.CreateFilterWithParameters(FilterComparisonType.Equal, "EmailId", emailId));
			EntityCollection entities = query.GetEntityCollection(_userConnection);
			return entities.Select(EntityToModel).ToList();
		}

		/// <summary>
		/// Retrieves the existing templates for the specified id.
		/// </summary>
		/// <param name="templateId"></param>
		/// <returns></returns>
		public EmailDesignerTemplateModel FetchById(Guid templateId) {
			return FetchByIds(new[] { templateId }).FirstOrDefault();
		}

		/// <summary>
		/// Retrieves the existing templates for the specified ids.
		/// </summary>
		/// <param name="templateIds"></param>
		/// <returns></returns>
		public IList<EmailDesignerTemplateModel> FetchByIds(IEnumerable<Guid> templateIds) {
			EntityCollection entities = FetchByIdsInternal(templateIds);
			return entities.Select(EntityToModel).ToList();
		}

		/// <inheritdoc cref="ITemplateRepository.LoadTemplate"/>
		public TemplateModel LoadTemplate(Guid templateId) {
			if (templateId == Guid.Empty)
				throw new ArgumentException("Template id cannot be empty.", nameof(templateId));
			var emailTemplate = new TemplateModel();
			Entity loadedEntity = LoadTemplateEntity(templateId);
			if (loadedEntity == null) {
				emailTemplate.Id = templateId.ToString();
				Entity templateEntity = _userConnection.EntitySchemaManager.GetInstanceByName("BfEmailTemplate")
					.CreateEntity(_userConnection);
				SaveOrCreateTemplate(emailTemplate, templateId, templateEntity);
			} else {
				emailTemplate = MapEntityToObject(loadedEntity);
			}
			return emailTemplate;
		}

		/// <inheritdoc cref="ITemplateRepository.SaveTemplate"/>
		public bool SaveTemplate(TemplateModel emailTemplate) {
			if (emailTemplate == null)
				throw new ArgumentNullException(nameof(emailTemplate), "Email template model can not be null");
			Guid templateId = Guid.Parse(emailTemplate.Id);
			Entity templateEntity = LoadTemplateEntity(templateId) ?? _userConnection.EntitySchemaManager
				.GetInstanceByName("BfEmailTemplate").CreateEntity(_userConnection);
			return SaveOrCreateTemplate(emailTemplate, templateId, templateEntity);
		}

		#endregion

	}

	#endregion

}
