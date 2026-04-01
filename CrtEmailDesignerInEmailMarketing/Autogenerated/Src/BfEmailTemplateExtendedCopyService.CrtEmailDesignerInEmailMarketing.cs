namespace CrtEmailDesigner
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Factories;
	using Terrasoft.Core.Factories;

	#region Class: EmailBuilderTemplateService

	/// <summary>
	/// Implements the IExtendedEmailCopyService interface to copy email templates.
	/// </summary>
	[DefaultBinding(typeof(IExtendedEmailCopyService))]
	internal class BfEmailTemplateExtendedCopyService : IExtendedEmailCopyService
	{

		#region Fields: Private

		private readonly IBfEmailTemplateRepository _bfTemplateRepository;

		private readonly UserConnection _userConnection;

		private readonly IEntityFactory _entityFactory;

		#endregion

		#region Constructors: Public

		public BfEmailTemplateExtendedCopyService(UserConnection userConnection, IEntityFactory entityFactory) {
			_bfTemplateRepository =
				ClassFactory.Get<IBfEmailTemplateRepository>(new ConstructorArgument("userConnection", userConnection));
			_userConnection = userConnection;
			_entityFactory = entityFactory;
		}

		#endregion

		#region Methods: Private

		private EntityCollection LoadDisplayConditions(Guid oldBulkEmailId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BfDisplayCondition") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			var filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "BulkEmail", oldBulkEmailId);
			esqResult.Filters.Add(filterById);
			return esqResult.GetEntityCollection(_userConnection);
		}

		private Dictionary<Guid, Guid> CopyDisplayConditions(EntityCollection sourceDisplayConditions, Guid newBulkEmailId) {
			var oldNewIdConditionMap = new Dictionary<Guid, Guid>();
			foreach (var sourceCondition in sourceDisplayConditions) {
				var newCondition = _entityFactory.CreateEntity("BfDisplayCondition");
				var newConditionId = Guid.NewGuid();
				newCondition.SetColumnValue("Id", newConditionId);
				newCondition.SetColumnValue("BulkEmailId", newBulkEmailId);
				newCondition.SetColumnValue("ConditionName", sourceCondition.GetTypedColumnValue<string>("ConditionName"));
				newCondition.SetColumnValue("ConditionRule", sourceCondition.GetBytesValue("ConditionRule"));
				newCondition.SetColumnValue("ConditionDescription", sourceCondition.GetTypedColumnValue<string>("ConditionDescription"));
				newCondition.Save();
				oldNewIdConditionMap.Add(sourceCondition.PrimaryColumnValue, newConditionId);
			}
			return oldNewIdConditionMap;
		}

		private Dictionary<Guid, Guid> CopyDisplayConditions(ExtendedEmailCopyRequest request) {
			var sourceDisplayConditions = LoadDisplayConditions(request.OldEmailId);
			if (!sourceDisplayConditions.Any()) {
				return new Dictionary<Guid, Guid>();
			}
			return CopyDisplayConditions(sourceDisplayConditions, request.NewEmailId);
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Copies BFEmailTemplate for provided bulk email ids
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public ExtendedEmailCopyResult ExtendCopy(ExtendedEmailCopyRequest request) {
			var result = new ExtendedEmailCopyResult {
				NewEmailId = request.NewEmailId,
				OldEmailId = request.OldEmailId
			};
			try {
				if (_bfTemplateRepository == null) {
					result.IsSuccess = false;
					return result;
				}
				var oldNewIdConditionMap = CopyDisplayConditions(request);
				var templates = _bfTemplateRepository.FetchAllByEmailId(request.OldEmailId);
				if (templates.Any()) {
					var newTemplates = templates.Select(template => {
						var pageHtml = template.PageHtml;
						var pageJson = template.PageJson;
						if (oldNewIdConditionMap.Any()) {
							pageHtml = BFTemplateTransformerService.ReplaceConditionRulesInHtmlTemplate(
								template.PageHtml, oldNewIdConditionMap);
							pageJson = BFTemplateTransformerService.ReplaceConditionRulesInJSONTemplate(
								template.PageJson, oldNewIdConditionMap);
						}
						return new EmailDesignerTemplateModel {
							AmpHtml = template.AmpHtml,
							EmailId = request.NewEmailId,
							Language = template.Language,
							LanguageId = template.LanguageId,
							PageHtml = pageHtml,
							PageJson = pageJson,
							IsDefault = template.IsDefault,
							TemplateVersion = template.TemplateVersion
						};
					}).ToList();
					_bfTemplateRepository.CreateMany(newTemplates);
				}
				result.IsSuccess = true;
			} catch (Exception ex) {
				result.Error = ex;
				result.IsSuccess = false;
			}
			return result;
		}

		#endregion

	}

	#endregion

}

