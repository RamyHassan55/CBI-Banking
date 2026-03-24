namespace CrtEmailDesignerInEmailMarketing
{
	using System;
	using Creatio.FeatureToggling;
	using System.Linq;
	using Common.Logging;
	using CrtContentDesigner;
	using CrtEmailDesignerInEmailMarketing.Services;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.DynamicContent;
	using Terrasoft.Configuration.DynamicContent.DataContract;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using CrtContentDesigner.Contracts;
	using CrtContentDesigner.Models;
	using CrtEmailDesigner.Interfaces;
	using CrtEmailDesigner.Repositories;
	using CrtEmailDesigner.Utilities;
	using Terrasoft.Common;
	using System.Collections.Generic;

	#region Class: DcTemplateSaveService
	/// <summary>
	/// Provides a service for saving dynamic content templates for email marketing.
	/// </summary>
	[DefaultBinding(typeof(ITemplateSave), Name = "DCTemplate")]
	internal class DcTemplateSaveService : ITemplateSave
	{

		#region Constants: Private

		private const string BfEmailDesignerTypeId = "FDB47538-D702-42DF-9CFA-C66FA7F6DBF6";
		private const string ChangeDraftToPlannedProcessName = "ChangeBulkEmailStatusDraftToPlannedExtended";

		#endregion

		#region Fields: Private

		private DCTemplateService _dCTemplateService;
		private BulkEmailTemplateService _bulkEmailTemplateService;
		private ILog _saveServiceLog;
		private IBulkEmailHyperlinkCorrector _bulkEmailHyperlinkCorrector;
		private EmailDesignerReplicaBuilder _emailDesignerReplicaBuilder;
		private readonly UserConnection _userConnection;
		private IBfTemplateLanguageService _bfTemplateLanguageService;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="DcTemplateSaveService"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		public DcTemplateSaveService(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the bf template language service.
		/// </summary>
		public IBfTemplateLanguageService BfTemplateLanguageService {
			get => _bfTemplateLanguageService ?? (_bfTemplateLanguageService = new BfTemplateLanguageService(_userConnection));
			set => _bfTemplateLanguageService = value;
		}

		/// <summary>
		/// Gets or sets the bulk email hyperlink corrector.
		/// </summary>
		public IBulkEmailHyperlinkCorrector BulkEmailHyperlinkCorrector {
			get => _bulkEmailHyperlinkCorrector ?? (_bulkEmailHyperlinkCorrector = new BulkEmailHyperlinkCorrector());
			set => _bulkEmailHyperlinkCorrector = value;
		}

		/// <summary>
		/// Gets or sets the bulk email hyperlink corrector.
		/// </summary>
		public EmailDesignerReplicaBuilder EmailDesignerReplicaBuilder {
			get => _emailDesignerReplicaBuilder ?? (_emailDesignerReplicaBuilder = new EmailDesignerReplicaBuilder(_userConnection));
			set => _emailDesignerReplicaBuilder = value;
		}

		/// <summary>
		/// Gets or sets the bulk email template service.
		/// </summary>
		public BulkEmailTemplateService BulkEmailTemplateService {
			get => _bulkEmailTemplateService ?? (_bulkEmailTemplateService = new BulkEmailTemplateService());
			set => _bulkEmailTemplateService = value;
		}

		/// <summary>
		/// Gets or sets the dc template service.
		/// </summary>
		public DCTemplateService DcTemplateService {
			get => _dCTemplateService ?? (_dCTemplateService = new DCTemplateService());
			set => _dCTemplateService = value;
		}

		/// <inheritdoc cref="ITemplateHandler.HandlerCode"/>
		public string HandlerCode => "BulkEmail";

		/// <inheritdoc cref="ITemplateHandler.Priority"/>
		public int Priority => 20;

		/// <summary>
		/// Gets or sets the save service log.
		/// </summary>
		public ILog SaveServiceLog {
			get => _saveServiceLog ?? (_saveServiceLog = LogManager.GetLogger("DcTemplateSaveService"));
			set => _saveServiceLog = value;
		}

		#endregion

		#region Methods: Private

		private static ReplicaHeaders GetDefaultReplicaHeader(Guid replicaId, int maskId) {
			return new ReplicaHeaders {
				ReplicaId = replicaId,
				Preheader = string.Empty,
				SenderName = string.Empty,
				SenderEmail = string.Empty,
				ReplicaMask = maskId,
				Subject = string.Empty,
			};
		}

		private int ObtainMaxBulkEmailTrackId(Guid bulkEmailId, Guid languageId) {
			var dcTemplateQuery = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "DCTemplate") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			dcTemplateQuery.Filters.Add(dcTemplateQuery.CreateFilterWithParameters(FilterComparisonType.Equal, "RecordId", bulkEmailId));
			dcTemplateQuery.Filters.Add(dcTemplateQuery.CreateFilterWithParameters(FilterComparisonType.Equal, "Language.Id", languageId));
			var dcTemplate = dcTemplateQuery.GetEntityCollection(_userConnection).FirstOrDefault();
			var query = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BulkEmailHyperlink");
			var maxTrackIdColumn = query.AddColumn("BpmTrackId");
			maxTrackIdColumn.SummaryType = AggregationType.Max;
			maxTrackIdColumn.Name = "MaxTrackId";
			query.Filters.Add(query.CreateFilterWithParameters(FilterComparisonType.Equal, "BulkEmail.Id", bulkEmailId));
			if (dcTemplate != null) {
				query.Filters.Add(query.CreateFilterWithParameters(FilterComparisonType.NotEqual, "DCTemplateId", dcTemplate.PrimaryColumnValue));
			}
			Entity result = query.GetSummaryEntity(_userConnection);
			return result.GetTypedColumnValue<int>("MaxTrackId");
		}

		private string GetHtmlWithCorrectedHyperLinks(string pageHtml, Guid bulkEmailId, Guid languageId) {
			if (_userConnection.GetIsFeatureEnabled("BulkEmailMultilanguage")) {
				int maxTrackId = ObtainMaxBulkEmailTrackId(bulkEmailId, languageId);
				pageHtml = BulkEmailHyperlinkCorrector.ApplyBpmParametersToHyperlinks(pageHtml, 0, ++maxTrackId);
			} else {
				pageHtml = BulkEmailHyperlinkCorrector.ApplyBpmParametersToHyperlinks(pageHtml, 0, 1);
			}
			return pageHtml;
		}

		private Entity LoadBulkEmailEntity(Guid bulkEmailId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BulkEmail") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			var filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", bulkEmailId);
			esqResult.Filters.Add(filterById);
			var entities = esqResult.GetEntityCollection(_userConnection);
			return entities.FirstOrDefault();
		}

		private EntityCollection LoadBulkEmailReplicaHeaders(Guid bulkEmailId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "BulkEmailReplicaHeaders") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			esqResult.AddColumn("DCReplica.DCTemplate.Language.Id");
			var filterById =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "BulkEmail", bulkEmailId);
			esqResult.Filters.Add(filterById);
			return esqResult.GetEntityCollection(_userConnection);
		}

		private EntityCollection LoadReplicas(Guid bulkEmailId) {
			var esqResult = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "DCReplica") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esqResult.AddAllSchemaColumns();
			esqResult.AddColumn("DCTemplate.Language.Id");
			var recordIdFilter =
				esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "DCTemplate.RecordId", bulkEmailId);
			esqResult.Filters.Add(recordIdFilter);
			return esqResult.GetEntityCollection(_userConnection);
		}

		private ConfigurationServiceResponse SaveHyperLinks(string pageHtml, Guid bulkEmailId, Guid dcTemplateId) {
			var linksFromHtml = BulkEmailHyperlinkCorrector.ExtractHyperlinks(pageHtml);
			var request = new SaveBulkEmailHyperlinkRequest {
				BulkEmailId = bulkEmailId,
				Hyperlinks = linksFromHtml
			};
			if (_userConnection.GetIsFeatureEnabled("BulkEmailMultilanguage")) {
				request.DCTemplateId = dcTemplateId;
			}
			return BulkEmailTemplateService.SaveBulkEmailHyperlinks(request);
		}

		private static void PopulateDefaultReplicaFromBulkEmail(Entity bulkEmail, ReplicaHeaders replicaHeader) {
			PopulateReplicaFromBulkEmail(bulkEmail, replicaHeader);
			replicaHeader.Subject = bulkEmail.GetTypedColumnValue<string>("TemplateSubject");
		}

		private static void PopulateReplicaFromBulkEmail(Entity bulkEmail, ReplicaHeaders replicaHeader) {
			replicaHeader.SenderName = bulkEmail.GetTypedColumnValue<string>("SenderName");
			replicaHeader.SenderEmail = bulkEmail.GetTypedColumnValue<string>("SenderEmail");
			replicaHeader.Preheader = bulkEmail.GetTypedColumnValue<string>("Preheader");
		}

		private static void PopulateReplicaFromExistingReplica(ReplicaHeaders replicaHeader, Entity existingReplica) {
			replicaHeader.Preheader = existingReplica.GetTypedColumnValue<string>("Preheader");
			replicaHeader.SenderName = existingReplica.GetTypedColumnValue<string>("SenderName");
			replicaHeader.SenderEmail = existingReplica.GetTypedColumnValue<string>("SenderEmail");
			replicaHeader.Subject = existingReplica.GetTypedColumnValue<string>("Subject");
		}

		private ConfigurationServiceResponse SaveReplicaHeaders(Guid bulkEmailId, EntityCollection previousHeaders) {
			var replicas = LoadReplicas(bulkEmailId);
			var defaultTemplateLanguageId = BfTemplateLanguageService.GetDefaultLanguageId();
			var replicaHeadersModels = replicas.Select(c => {
				var currentReplicaLanguageId = c.GetTypedColumnValue<Guid>("DCTemplate_Language_Id");
				var existedReplicaHeader = previousHeaders.FirstOrDefault(e => e.GetTypedColumnValue<Guid>("DCReplica_DCTemplate_Language_Id") == currentReplicaLanguageId);
				var replicaId = c.PrimaryColumnValue;
				var mask = c.GetTypedColumnValue<int>("Mask");
				var replicaHeader = GetDefaultReplicaHeader(replicaId, mask);
				if (existedReplicaHeader == null && defaultTemplateLanguageId == currentReplicaLanguageId) {
					var bulkEmail = LoadBulkEmailEntity(bulkEmailId);
					PopulateDefaultReplicaFromBulkEmail(bulkEmail, replicaHeader);
				}
				if (existedReplicaHeader != null) {
					PopulateReplicaFromExistingReplica(replicaHeader, existedReplicaHeader);
				}
				if (existedReplicaHeader == null && defaultTemplateLanguageId != currentReplicaLanguageId) {
					var bulkEmail = LoadBulkEmailEntity(bulkEmailId);
					PopulateReplicaFromBulkEmail(bulkEmail, replicaHeader);
				}
				return replicaHeader;
			}).ToList();
			var request = new SaveBulkEmailHeadersRequest {
				BulkEmailId = bulkEmailId,
				Headers = replicaHeadersModels.ToArray()
			};
			return BulkEmailTemplateService.SaveBulkEmailReplicaHeaders(request);
		}

		private void StartChangeStatusProcess(Guid bulkEmailId) {
			var parameters = new Dictionary<string, object> {
				{ "ManualRunEmailId", bulkEmailId }
			};
			_userConnection.RunProcess(ChangeDraftToPlannedProcessName, parameters);
		}

		private bool SetDesignerTypeForBulkEmail(Guid bulkEmailId) {
			var bulkEmail = LoadBulkEmailEntity(bulkEmailId);
			bulkEmail.SetColumnValue("EmailDesignerTypeId", Guid.Parse(BfEmailDesignerTypeId));
			var savingResult = bulkEmail.Save();
			return savingResult;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="ITemplateSave.SaveTemplate"/>
		public bool SaveTemplate(SaveTemplateRequest saveRequest) {
			try {
				var templateId = Guid.Parse(saveRequest.Content.Id);
				var bulkEmailId = Guid.Parse(saveRequest.RecordId);
				var languageId = Guid.Parse(saveRequest.LanguageId);
				var isDesignerTypeForBulkEmailSet = SetDesignerTypeForBulkEmail(bulkEmailId);
				if (!isDesignerTypeForBulkEmailSet) {
					SaveServiceLog.Error(
						$"Failed to set the email designer type ({BfEmailDesignerTypeId}). TemplateId: {templateId}, BulkEmailId: {bulkEmailId}.");
					return false;
				}
				var htmlContent = GetHtmlWithCorrectedHyperLinks(saveRequest.Content.PageHtml, bulkEmailId, languageId);
				var dcTemplateContract = EmailDesignerReplicaBuilder.BuildDcTemplateContract(saveRequest, htmlContent);
				var previousReplicaHeaders = LoadBulkEmailReplicaHeaders(bulkEmailId);
				var saveTemplateResponse = DcTemplateService.Save(dcTemplateContract);
				if (!saveTemplateResponse.Success) {
					SaveServiceLog.Error(
						$"Error when saving the template. Error message: {saveTemplateResponse.ErrorInfo.Message}, TemplateId: {templateId}, BulkEmailId: {bulkEmailId}.");
					return false;
				}
				var saveHyperLinksResponse = SaveHyperLinks(htmlContent, bulkEmailId, dcTemplateContract.Id);
				if (!saveHyperLinksResponse.Success) {
					SaveServiceLog.Error(
					$"Error when saving hyper links. Error message: {saveHyperLinksResponse.ErrorInfo.Message}, TemplateId: {templateId}, BulkEmailId: {bulkEmailId}.");
					return false;
				}
				if (!_userConnection.GetIsFeatureEnabled("BulkEmailMultilanguage")) {
					return true;
				}
				var saveReplicaHeadersResponse = SaveReplicaHeaders(bulkEmailId, previousReplicaHeaders);
				if (saveReplicaHeadersResponse.Success) {
					StartChangeStatusProcess(bulkEmailId);
					return true;
				}
				SaveServiceLog.Error(
					$"Error when saving bulk email headers. Error message: {saveReplicaHeadersResponse.ErrorInfo.Message}, TemplateId: {templateId}, BulkEmailId: {bulkEmailId}.");
				return false;
			} catch (Exception exception) {
				SaveServiceLog.Error(
					$"Error when saving the template. Exception: {exception}, TemplateId: {saveRequest?.Content?.Id}, BulkEmailId: {saveRequest?.RecordId}.");
			}
			return false;
		}

		#endregion

	}

	#endregion
}
