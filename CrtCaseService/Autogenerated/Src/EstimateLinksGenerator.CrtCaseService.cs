namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Web.Common;
	using Terrasoft.Web.Http.Abstractions;
	using SystemSettings = Core.Configuration.SysSettings;

	#region Class: EstimateLinksGenerator

	/// <summary>
	/// Satisfaction level image links generator.
	/// </summary>
	public class EstimateLinksGenerator : IMacrosInvokable
	{
		#region Class : EstimateLinksGeneratorProperties

		private class EstimateLinksGeneratorProperties {

			#region Fields: Public

			public readonly Func<StringBuilder> ExternalOpenTag;
			public readonly string ExternalIntermediateTagBegin;
			public readonly string ExternalIntermediateTagClose;
			public readonly string ExternalCloseTag;
			public readonly Func<string, int, int, string, StringBuilder> AppendInternalOpenTag;
			public readonly Func<string, int, int, string, StringBuilder> AppendImage;
			public readonly Func<StringBuilder> AppendInternalCloseTag;

			#endregion

			#region Constructors : Public

			/// <summary>
			/// Initializes EstimateLinksGeneratorProperties with required values
			/// </summary>
			/// <param name="isGenerateUsingSpan">Is feature GenerateUsingSpan enabled</param>
			/// <param name="isGenerateUsingImageUrl">Is feature GenerateUsingImageUrl enabled</param>
			public EstimateLinksGeneratorProperties(bool isGenerateUsingSpan, bool isGenerateUsingImageUrl) {
				if (isGenerateUsingSpan) {
					ExternalOpenTag = () => new StringBuilder().AppendFormat("<span style=\"height: {0}px;\">", IconSize);
					ExternalIntermediateTagBegin = string.Empty;
					ExternalIntermediateTagClose = string.Empty;
					ExternalCloseTag = "</span>";
					AppendInternalOpenTag = (link, padding, point, divStyle) => new StringBuilder().AppendFormat(HrefFormat, link, padding);
					AppendInternalCloseTag = () => new StringBuilder().Append("</a>");
				} else {
					ExternalOpenTag = () => new StringBuilder().AppendFormat("<table border=\"0\" cellpadding=\"{0}\" cellspacing=\"{0}\">", CellPadding);
					ExternalIntermediateTagBegin = "<tr>";
					ExternalIntermediateTagClose = "</tr>";
					ExternalCloseTag = "</table>";
					AppendInternalOpenTag = (link, size, point, divStyle) => new StringBuilder()
						.Append("<td>")
						.AppendFormat(DivFormat, point, divStyle ?? string.Format(DivStyleFormat, IconSize))
						.AppendFormat("<a href=\"{0}\">", link);
					AppendInternalCloseTag = () => new StringBuilder().Append("</a>").Append("</div>").Append("</td>");
				}
				if (isGenerateUsingImageUrl) {
					AppendImage = (imageData, size, point, mimeType) => new StringBuilder().AppendFormat(AttachedImgFormat, imageData, size, point);
				} else {
					AppendImage = (imageData, size, point, mimeType) => new StringBuilder().AppendFormat(ImgFormat, mimeType, imageData, size, point);
				}
			}

			#endregion
		}

		#endregion

		#region Constants: Private
		
		private const int RowLengthLimit = 10;
		private const int IconSize = 40;
		private const int Padding = 5;
		private const int CellPadding = 10;
		private const string HrefFormat = "<a href=\"{0}\" style=\"margin: {1}px; padding: {1}px\">";
		private const string DivFormat = "<div data-item-marker=\"satisfaction-level-{0}\" style=\"{1}\">";
		private const string DivStyleFormat = "width: {0}px; height: {0}px; vertical-align: middle; float: left;";
		private const string LinkFormat = "{0}/ServiceModel/CaseRatingManagementService.svc/RateCase/{1}/{2}";
		private const string TokenLinkFormat =
			"{0}/ServiceModel/CaseRatingManagementService.svc/SecureRateCase/[?CaseId={1}?]/{2}";
		private const string NoSubmitTokenLinkFormat =
			"{0}/ServiceModel/CaseRatingManagementService.svc/EstimateCase/[?CaseId={1}?]/{2}";
		private const string ImgFormat = "<img src=\"data:{0};base64,{1}\" width={2} height={2} alt=\"{3}\" />";
		private const string AttachedImgFormat = "<img src=\"{0}\" width={1} height={1} alt=\"{2}\" />";

		#endregion

		#region Constructors: Public

		public EstimateLinksGenerator() {
		}

		public EstimateLinksGenerator(IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
		}

		#endregion

		#region Properties : Private

		private IHttpContextAccessor _httpContextAccessor;
		private IHttpContextAccessor HttpContextAccessor {
			get {
				if (_httpContextAccessor == null) {
					_httpContextAccessor = HttpContext.HttpContextAccessor;
				}
				return _httpContextAccessor;
			}
		}
		private EstimateLinksGeneratorProperties _estimateLinksGeneratorProperties;
		private EstimateLinksGeneratorProperties EstimateLinksGeneratorValues =>
			_estimateLinksGeneratorProperties ?? new EstimateLinksGeneratorProperties(IsFeatureGenerateImagesListUsingDivEnabled, IsFeatureAttachedImagesForCaseEstimationEmoticonsEnabled);
		private bool? _isFeatureGenerateImagesListUsingDivEnabled = null;
		private bool IsFeatureGenerateImagesListUsingDivEnabled => _isFeatureGenerateImagesListUsingDivEnabled ?? Features.GetIsEnabled("GenerateImagesListUsingDiv");
		private bool? _isFeatureAttachedImagesForCaseEstimationEmoticonsEnabled = null;
		private bool IsFeatureAttachedImagesForCaseEstimationEmoticonsEnabled => _isFeatureAttachedImagesForCaseEstimationEmoticonsEnabled ?? Features.GetIsEnabled("AttachedImagesForCaseEstimationEmoticons");
		#endregion

		#region Properties : Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection { get; set; }

		#endregion

		#region Methods : Private

		private string GenerateImageContainer(string link, string imageData, int point, string mimeType = "", string divStyle = null) {
			var sb = new StringBuilder(1024);
			sb.Append(EstimateLinksGeneratorValues.AppendInternalOpenTag(link, Padding, point, divStyle))
				.Append(EstimateLinksGeneratorValues.AppendImage(imageData, IconSize, point, mimeType))
				.Append(EstimateLinksGeneratorValues.AppendInternalCloseTag());
			return sb.ToString();
		}

		private string GetTokenLink() {
			return Features.GetIsEnabled("EstimationWithNoSubmitting") ? NoSubmitTokenLinkFormat : TokenLinkFormat;
		}

		#endregion

		#region Methods : Protected

		/// <summary>
		/// Returns application url.
		/// </summary>
		/// <returns>Url string.</returns>
		protected virtual string GetApplicationUrl() {
			var portalSiteUrl = SystemSettings.GetValue(UserConnection, "PortalSiteUrl", string.Empty);
			if (string.IsNullOrEmpty(portalSiteUrl)) {
				HttpContext httpContext = HttpContextAccessor.GetInstance();
				return httpContext != null
					? WebUtilities.GetBaseApplicationUrl(httpContext.Request)
					: SystemSettings.GetValue(UserConnection, "SiteUrl", string.Empty);
			}		
			return portalSiteUrl;
		}

		/// <summary>
		/// Gets record id from argument object.
		/// </summary>
		/// <param name="argument">Argument object.</param>
		/// <returns>Record id.</returns>
		protected virtual Guid GetRecordId(object argument) {
			var recordArgument = argument as KeyValuePair<string, Guid>?;
			return recordArgument.HasValue ? recordArgument.Value.Value : Guid.Empty;
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Returns string value for macros.
		/// </summary>
		/// <param name="arguments">Arguments object.</param>
		/// <returns>Result string.</returns>
		public virtual string GetMacrosValue(object arguments) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "SatisfactionLevel");
			EntitySchemaQueryColumn pointColumn = esq.AddColumn("Point");
			pointColumn.OrderDirection = OrderDirection.Descending;
			string pointColumnName = pointColumn.Name;
			string imageIdColumnName = esq.AddColumn("Image.Id").Name;
			string dataColumnName = IsFeatureAttachedImagesForCaseEstimationEmoticonsEnabled
				? ""
				: esq.AddColumn("Image.Data").Name;
			string mimeTypeColumnName = IsFeatureAttachedImagesForCaseEstimationEmoticonsEnabled
				? ""
				: esq.AddColumn("Image.MimeType").Name;
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "IsActive", true));
			EntityCollection satisfactionLevels = esq.GetEntityCollection(UserConnection);
			if (!satisfactionLevels.Any())
				return string.Empty;
			Guid id = GetRecordId(arguments);
			var count = satisfactionLevels.Count;
			var rows = (count + RowLengthLimit - 1) / RowLengthLimit;
			var rowLenght = count / rows;
			var remainder = count % rows;
			var result = new StringBuilder(1024);
			result.Append(EstimateLinksGeneratorValues.ExternalOpenTag());
			int index = 0;
			var sysImageSchemaUId = UserConnection.EntitySchemaManager.GetInstanceByName("SysImage").UId;
			for (var i = 0; i < rows && index < count; i++, remainder--) {
				result.Append(EstimateLinksGeneratorValues.ExternalIntermediateTagBegin);
				for (var j = 0; j < rowLenght + (remainder > 0 ? 1 : 0) && index < count; j++, index++) {
					var satisfactionLevel = satisfactionLevels[index];
					var point = satisfactionLevel.GetTypedColumnValue<int>(pointColumnName);
					string appRootUrl = GetApplicationUrl();
					string linkFormat = Features.GetIsEnabled("SecureEstimation") ? GetTokenLink() : LinkFormat;
					string link = string.Format(linkFormat, appRootUrl, id, point);
					if (IsFeatureAttachedImagesForCaseEstimationEmoticonsEnabled) {
						var imageId = satisfactionLevel.GetTypedColumnValue<Guid>(imageIdColumnName);
						if (imageId != Guid.Empty) {
							var imageUrl = string.Format("../rest/FileService/GetFile/{0}/{1}", sysImageSchemaUId, imageId);
							string container = GenerateImageContainer(link, imageUrl, point);
							result.Append(container);
						}
					} else {
						var image = satisfactionLevel.GetStreamValue(dataColumnName);
						if (image != null) {
							string imageData = Convert.ToBase64String(image.ReadAllBytes());
							var mimeType = satisfactionLevel.GetTypedColumnValue<string>(mimeTypeColumnName);
							string container = GenerateImageContainer(link, imageData, point, mimeType);
							result.Append(container);
						}
					}
				}
				result.Append(EstimateLinksGeneratorValues.ExternalIntermediateTagClose);
			}
			result.Append(EstimateLinksGeneratorValues.ExternalCloseTag);
			return result.ToString();
		}

		#endregion

	}

	#endregion

}

