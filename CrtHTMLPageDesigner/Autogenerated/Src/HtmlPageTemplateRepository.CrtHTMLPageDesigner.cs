namespace CrtHTMLPageDesigner
{
	using System;
	using System.Linq;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using CrtContentDesigner.Models;
	using CrtContentDesigner.Repositories;

	#region Class: HtmlPageTemplateRepository
	/// <summary>
	/// Provides repository logic for content for HtmlPages.
	/// </summary>
#pragma warning disable
	[DefaultBinding(typeof(ITemplateRepository), Name = "DefaultHTML")]
	internal class HtmlPageTemplateRepository : ITemplateRepository
#pragma warning restore
	{

		#region Fields: Private

		private readonly UserConnection _userConnection;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlPageTemplateRepository"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public HtmlPageTemplateRepository(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private Entity LoadTemplateEntity(Guid templateId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "HTMLPage") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			var filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", templateId);
			esqResult.Filters.Add(filterById);
			var entities = esqResult.GetEntityCollection(_userConnection);
			return entities.FirstOrDefault();
		}

		private TemplateModel MapEntityToObject(Entity entity) {
			var response = new TemplateModel { };
			if (entity != null) {
				response.Id = entity.GetTypedColumnValue<string>("Id");
				response.PageHtml = entity.GetTypedColumnValue<string>("PageHtml");
				response.PageJson = entity.GetTypedColumnValue<string>("PageJson");
			}
			return response;
		}

		private bool SaveTemplate(TemplateModel contentTemplate, Guid templateId, Entity templateEntity) {
			if (templateEntity == null) {
				templateEntity = _userConnection.EntitySchemaManager
					.GetInstanceByName("HTMLPage").CreateEntity(_userConnection);
				templateEntity.SetDefColumnValues();
				templateEntity.SetColumnValue("Id", contentTemplate.Id);
			}
			templateEntity.SetColumnValue("PageHtml", contentTemplate.PageHtml ?? string.Empty);
			templateEntity.SetColumnValue("PageJson", contentTemplate.PageJson ?? "{}");
			return templateEntity.Save();
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="ITemplateRepository.LoadTemplate"/>
		public TemplateModel LoadTemplate(Guid templateId) {
			var loadedEntity = LoadTemplateEntity(templateId);
			return MapEntityToObject(loadedEntity);
		}

		/// <inheritdoc cref="ITemplateRepository.SaveTemplate"/>
		public bool SaveTemplate(TemplateModel contentTemplate) {
			var templateId = Guid.Parse(contentTemplate.Id);
			var templateEntity = LoadTemplateEntity(templateId);
			return SaveTemplate(contentTemplate, templateId, templateEntity);
		}

		#endregion

	}

	#endregion
}

