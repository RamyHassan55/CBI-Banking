namespace Terrasoft.Configuration.CaseRatingManagementService
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Security;
	using System.Security.Principal;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using System.Text;
	using System.Threading;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Web.Common;
	using Terrasoft.Web.Http.Abstractions;

	#region Class: SatisfactionLevelResponse

	/// <summary>
	/// Represents response for satisfaction level lookup query.
	/// </summary>
	[DataContract]
	public class SatisfactionLevelResponse : ConfigurationServiceResponse
	{

		#region Properties: Public

		/// <summary>
		/// Collection of satisfaction levels.
		/// </summary>
		[DataMember(Name = "satisfactionLevels")]
		public List<CaseRatingManagementService.SatisfactionLevelsContainer> SatisfactionLevels {
			get; set;
		}

		#endregion
	}

	#endregion

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class CaseRatingManagementService : BaseService
	{
		#region Class: SatisfactionLevelResponse

		public class SatisfactionLevelsContainer
		{
			#region Properties: Public

			/// <summary>
			/// Name.
			/// </summary>
			public string Name {
				get; set;
			}

			/// <summary>
			/// Image.
			/// </summary>
			public Guid Image {
				get; set;
			}

			/// <summary>
			/// Id.
			/// </summary>
			public Guid Id	{
				get; set;
			}

			/// <summary>
			/// Point.
			/// </summary>
			public string Point {
				get; set;
			}

			#endregion
		}

		#endregion

		#region Constants: Private

		/// <summary>
		/// Point column name.
		/// </summary>
		private const string PointColumnName = "Point";
		private const string CommentColumnName = "Comment";
		private const string IntermediateLink = "{0}/ServiceModel/CaseRatingManagementService.svc/IntermediateSecureRateCase/{1}/{2}";
		private const string FeedbackPageSchemaName = "CaseRatingFeedbackPage";
		private const string FeedbackWithIconsPageSchemaName = "CaseRatingFeedbackPageWithIcons";
		private const string FeedbackPageUrl = @"/Nui/Feedback.aspx?vm=FeedbackModule&token={0}&schemaName={1}";
		private const string FeedbackWithIconsPageUrl = @"/Nui/Feedback.aspx?vm=FeedbackModule&token={0}&schemaName={1}&rate={2}";


		#endregion

		#region Fields: Private

		private string _sessionId = Guid.NewGuid().ToString("N");
		private string _sysUserName;
		/// <summary>
		/// Id of the item from catalog MessageNotifier.
		/// </summary>
		private Guid MessageNotifierId = new Guid("ED501EDB-8EBF-4C76-A35D-6F23BE243043");

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates inctance of <see cref="CaseRatingManagementService"> type.
		/// </summary>
		public CaseRatingManagementService() {
			UserConnection = AppConnection?.SystemUserConnection;
			_sysUserName = AppConnection?.SystemUserConnection.CurrentUser.Name;
			SecurityTokenUtilities = new SecurityTokenUtilities(UserConnection);
		}

		/// <summary>
		/// Creates inctance of <see cref="CaseRatingManagementService"> type.
		/// </summary>
		/// <param name="userConnection">UserConnection.</param>
		public CaseRatingManagementService(UserConnection userConnection) {
			UserConnection = userConnection;
			_sysUserName = userConnection.CurrentUser.Name;
		}

		#endregion

		#region Properties: Private

		public bool IsReusable {
			get {
				return true;
			}
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Instance of the <see cref="SecurityTokenUtilities"/> class
		/// </summary>
		public SecurityTokenUtilities SecurityTokenUtilities {
			get; 
			set;
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Validates that the token is in correct GUID format (32 characters).
		/// </summary>
		/// <param name="token">Security token to validate.</param>
		/// <exception cref="SecurityException">Thrown when token format is invalid.</exception>
		private void ValidateTokenFormat(string token) {
			if (string.IsNullOrWhiteSpace(token) || token.Length != 32 || !Guid.TryParse(token, out _)) {
				throw new SecurityException("Invalid security token format. Token must be a valid GUID in 32-character format.");
			}
		}

		/// <summary>
		/// Validates that the rating is a valid integer.
		/// </summary>
		/// <param name="rating">Rating value to validate.</param>
		/// <exception cref="SecurityException">Thrown when rating format is invalid.</exception>
		private void ValidateRatingFormat(string rating) {
			if (string.IsNullOrWhiteSpace(rating) || !int.TryParse(rating, out _)) {
				throw new SecurityException("Invalid rating format. Rating must be a valid integer.");
			}
		}

		/// <summary>
		/// Checks data corectness.
		/// </summary>
		/// <param name="selects"></param>
		/// <returns></returns>
		private bool IsDataCorrect(List<Select> selects) {
			var isDataCorrect = true;
			var selectResult = Guid.Empty;
			var prevResult = Guid.Empty;
			foreach (var select in selects) {
				selectResult = select.ExecuteScalar<Guid>();
				isDataCorrect = selectResult != prevResult && isDataCorrect;
				prevResult = selectResult;
			}
			return isDataCorrect;
		}

		/// <summary>
		/// Sets logo image to response by system setting.
		/// </summary>
		/// <param name="response">HTTP Response.</param>
		/// <param name="imageName">Logo system setting code.</param>
		private void SetResponseLogo(HttpResponse response, string imageName) {
			SetResponseText(response, string.Format("<center><label id=\"RaitingLogo\" style=\"background: url(terrasoft.axd?s=" +
				"nui-binary-syssetting&r={0}); width: 1280px;height: 900px; display: inline-block;\"></label></center>", imageName));
		}

		/// <summary>
		/// Set response text message into html
		/// </summary>
		/// <param name="response">HttpResponse</param>
		/// <param name="text">Response text message</param>
		private void SetResponseText(HttpResponse response, string text) {
			var label = string.Format("<HTML><BODY>{0}</BODY></HTML>",
				text);
			response.Write(label);
		}

		/// <summary>
		/// Set response page
		/// </summary>
		/// <param name="response">HttpResponse</param>
		/// <param name="caseId">Case id</param>
		private void SetResponsePage(HttpResponse response, Guid caseId) {
			string responseUrl = WebUtilities.GetBaseApplicationUrl(HttpContext.Current.Request);
			string schemaName = "CaseRatingFeedbackPage";
			var feedbackUrlFormat = @"/Nui/Feedback.aspx?vm=FeedbackModule&token={0}&schemaName={1}";
			responseUrl += string.Format(feedbackUrlFormat, caseId, schemaName);
			SetResponsePage(response, responseUrl);
		}

		private void SetResponsePage(HttpResponse response, string responseUrl) {
			response.Headers["Content-Type"] = "text/html; charset=utf-8";
			var htmlBuilder = new StringBuilder(512)
				.Append("<html>")
					.Append("<body>")
						.Append("<script>")
							.Append("(function () { window.location.href=\"" + responseUrl + "\" }());")
						.Append("</script>")
					.Append("</body>")
				.Append("</html>");
			response.Write(htmlBuilder.ToString());
		}

		private void SetIntermediateResponsePage(string token, string rating) {
			HttpContext context = HttpContextAccessor.GetInstance();
			SetResponsePage(context.Response, GetIntermediateReponseUrl(token, rating));
		}

		private string GetIntermediateReponseUrl(string token, string rating) {
			return string.Format(IntermediateLink, WebUtilities.GetBaseApplicationUrl(HttpContext.Current.Request),
				token, rating);
		}

		/// <summary>
		/// Create`s Case Message History record
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="caseId">Case Id</param>
		private void CreateCaseMessageHistoryRecord(string message, Guid caseId) {
			if (UserConnection.EntitySchemaManager.FindItemByName("CaseMessageHistory") != null) {
				EntitySchema schema = UserConnection.EntitySchemaManager.GetInstanceByName("CaseMessageHistory");
				Entity caseMessageHistoryEntity = schema.CreateEntity(UserConnection.AppConnection.SystemUserConnection);
				caseMessageHistoryEntity.SetDefColumnValues();
				caseMessageHistoryEntity.SetColumnValue("CaseId", caseId);
				caseMessageHistoryEntity.SetColumnValue("MessageNotifierId", MessageNotifierId);
				caseMessageHistoryEntity.SetColumnValue("HasAttachment", false);
				caseMessageHistoryEntity.SetColumnValue("Message", message);
				caseMessageHistoryEntity.Save();
			}
		}

		/// <summary>
		/// Create`s Case Message History record
		/// </summary>
		/// <param name="caseId">Case Id</param>
		/// <param name="columnName">Name of updated column</param>
		/// <param name="columnValue">Value of updated column</param>
		private void CreateSatisfactionUpdateRecord(Guid caseId, string columnName, object columnValue) {
			var insert = new Insert(UserConnection)
				.Into("SatisfactionUpdate")
					.Set("CreatedOn", Column.Parameter(DateTime.UtcNow))
					.Set("ModifiedOn", Column.Parameter(DateTime.UtcNow))
					.Set("CaseId", new QueryParameter(caseId))
					.Set(columnName, new QueryParameter(columnValue)) as Insert;
			insert.Execute();
		}

		private Dictionary<string, string> ParseTokenData(byte[] data) {
			return
				Encoding.UTF8.GetString(data)
					.Split('&')
					.Select(x => x.Split('=')).AsEnumerable()
					.ToDictionary(key => key[0], value => value[1]);
		}

		private void AddSatisfactionLevelAndComment(string caseId, string rating, string comment) {
			var caseIdent = Guid.Parse(caseId);
			if (!string.IsNullOrEmpty(rating)) {
				CreateSatisfactionUpdateRecord(caseIdent, PointColumnName, rating);
				CreateCaseMessageHistoryRecord(String.Format(new LocalizableString(UserConnection.Workspace.ResourceStorage,
					"CaseRatingManagementService", "LocalizableStrings.SatisfactionLevelMessage.Value"), rating), caseIdent);
			}
			if (!string.IsNullOrEmpty(comment)) {
				CreateSatisfactionUpdateRecord(caseIdent, CommentColumnName, comment);
				CreateCaseMessageHistoryRecord(new LocalizableString(UserConnection.Workspace.ResourceStorage,
					"CaseRatingManagementService", "LocalizableStrings.SatisfactionCommentMessage.Value"), caseIdent);
			}
		}

		protected EntitySchemaQuery GetSatisfatcionLevelEsq() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "SatisfactionLevel");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			esq.AddColumn("Image");
			esq.AddColumn("Name");
			esq.AddColumn("Point");
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"IsActive", 1));
			return esq;
		}

		private List<SatisfactionLevelsContainer> GetSatisfactionLevelCollection() {
			var resultCollection = new List<SatisfactionLevelsContainer>();
			var satisfactionLevelEsq = GetSatisfatcionLevelEsq();
			foreach (var satisfactionLevel in satisfactionLevelEsq.GetEntityCollection(UserConnection)) {
				resultCollection.Add(new SatisfactionLevelsContainer {
					Name = satisfactionLevel.GetTypedColumnValue<string>("Name"),
					Id = satisfactionLevel.GetTypedColumnValue<Guid>("Id"),
					Image = satisfactionLevel.GetTypedColumnValue<Guid>("ImageId"),
					Point = satisfactionLevel.GetTypedColumnValue<string>("Point"),
				});
			}
			return resultCollection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// The handler of the client signal evaluation with secure token.
		/// </summary>
		/// <param name="token">Security token.</param>
		/// <param name="rating">Case rating.</param>
		[OperationContract]
		[WebGet(UriTemplate = "SecureRateCase/{token}/{rating}")]
		public void TokenProcessRequest(string token, string rating) {
			ValidateTokenFormat(token);
			ValidateRatingFormat(rating);
			SetIntermediateResponsePage(token, rating);		
		}

		/// <summary>
		/// The handler of the client signal evaluation with secure token.
		/// </summary>
		/// <param name="token">Security token.</param>
		/// <param name="rating">Case rating.</param>
		[OperationContract]
		[WebGet(UriTemplate = "IntermediateSecureRateCase/{token}/{rating}")]
		public void IntermediateCallProcessRequest(string token, string rating) {
			ValidateTokenFormat(token);
			ValidateRatingFormat(rating);
			if (SecurityTokenUtilities.TryGetTokenData(token, out byte[] tokenData)) {
				var tokenParameters = ParseTokenData(tokenData);
				var caseId = tokenParameters["CaseId"];
				ProcessRequest(caseId, rating);
			} else {
				throw new SecurityException($"Security token with key {token} was not found.");
			}
		}

		/// <summary>
		/// The handler of the client signal evaluation.
		/// </summary>
		/// <param name="id">Case identifier.</param>
		/// <param name="rating">Case rating.</param>
		[OperationContract]
		[WebGet(UriTemplate = "RateCase/{id}/{rating}")]
		public void ProcessRequest(string id, string rating) {
			HttpContext context = HttpContextAccessor.GetInstance();
			if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(rating)) {
				throw new ArgumentNullOrEmptyException("rating");
			}
			bool isFeatureEnabled = GlobalAppSettings.UseCSATRatingService;
			string responceMessage = "ImageThanksForRaiting";
			Guid caseId = new Guid(id);
			int point = Convert.ToInt32(rating);
			bool isDataCorrect = true;
			string LocalizableString = new LocalizableString(UserConnection.Workspace.ResourceStorage,
					"CaseRatingManagementService", "LocalizableStrings.SatisfactionLevelMessage.Value");
			string message = String.Format(LocalizableString, rating);
			var conditionMessages = new Dictionary<string, List<Select>>();
			Thread.CurrentPrincipal = new TerrasoftPrincipal(new GenericIdentity(_sysUserName), new string[0], _sessionId);
			var selectExistsCase =
				new Select(UserConnection).Top(1)
					.Column("Id")
				.From("Case")
				.Where("Id").IsEqual(Column.Parameter(caseId)) as Select;
			var selectExistsLevel =
				new Select(UserConnection).Top(1)
					.Column("Id")
				.From("SatisfactionLevel")
				.Where(PointColumnName).IsEqual(Column.Parameter(point)) as Select;
			conditionMessages.Add("ImageRaitingNotFound", new List<Select>() { selectExistsLevel });
			conditionMessages.Add("ImageCaseNotFound", new List<Select>() { selectExistsCase });
			try {
				foreach (string conditionMessage in conditionMessages.Keys) {
					List<Select> selects = conditionMessages[conditionMessage];
					isDataCorrect = IsDataCorrect(selects);
					if (!isDataCorrect) {
						responceMessage = conditionMessage;
						break;
					}
				}
				if (isDataCorrect) {
					CreateSatisfactionUpdateRecord(caseId, PointColumnName, point);
					CreateCaseMessageHistoryRecord(message, caseId);
				}
				if (isFeatureEnabled) {
					SetResponsePage(context.Response, caseId);
				} else {
					SetResponseLogo(context.Response, responceMessage);
				}
			} catch (Exception ex) {
				SetResponseText(context.Response, ex.Message);
			}
		}

		/// <summary>
		/// Add feedback comment to Case.
		/// </summary>
		/// <param name="token">Json-formatted string of token.</param>
		/// <param name="comment">Json-formatted string of comment.</param>
		/// <returns>Service response.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public ConfigurationServiceResponse AddComment(string token, string comment) {
			var response = new ConfigurationServiceResponse();
			try {
				var caseId = Guid.Parse(token);
				string message = new LocalizableString(UserConnection.Workspace.ResourceStorage,
					"CaseRatingManagementService", "LocalizableStrings.SatisfactionCommentMessage.Value");
				CreateSatisfactionUpdateRecord(caseId, "Comment", comment);
				CreateCaseMessageHistoryRecord(message, caseId);
			} catch (Exception e) {
				response.Exception = e;
			}
			return response;
		}

		/// <summary>
		/// Get satisfaction level data from lookup.
		/// </summary>
		/// <returns>Service response with satisfaction level collection.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public SatisfactionLevelResponse GetSatisfactionLevel() {
			return new SatisfactionLevelResponse { SatisfactionLevels = GetSatisfactionLevelCollection() };
		}

		/// <summary>
		/// The handler of the client signal evaluation with secure token and without rating.
		/// </summary>
		/// <param name="token">Security token.</param>
		[OperationContract]
		[WebGet(UriTemplate = "EstimateCase/{token}/{rating=default}")]
		public void EstimateCaseRequest(string token, string rating) {
			HttpContext context = HttpContextAccessor.GetInstance();
			string responseUrl = WebUtilities.GetBaseApplicationUrl(HttpContext.Current.Request);
			responseUrl += UserConnection.GetIsFeatureEnabled("UseOldFeedbackPage") ?
				string.Format(FeedbackPageUrl, token, FeedbackPageSchemaName) : 
				string.Format(FeedbackWithIconsPageUrl, token, FeedbackWithIconsPageSchemaName, rating);
			SetResponsePage(context.Response, responseUrl);
		}

		// <summary>
		/// Add feedback rating and comment using to Case.
		/// </summary>
		/// <param name="token">Token for case record.</param>
		/// <param name="comment">Comment to case.</param>
		/// <param name="rating">Rating of the case.</param>
		/// <returns>Service response.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
			ResponseFormat = WebMessageFormat.Json)]
		public ConfigurationServiceResponse AddSatisfactionLevel(string token, string comment, string rating) {
			var response = new ConfigurationServiceResponse();
			try {
				if (SecurityTokenUtilities.TryGetTokenData(token, out byte[] tokenData)) {
					var tokenParameters = ParseTokenData(tokenData);
					var caseId = tokenParameters["CaseId"];
					AddSatisfactionLevelAndComment(caseId, rating, comment);
				} else {
					throw new SecurityException($"Security token with key {token} was not found.");
				}
			} catch (Exception e) {
				response.Exception = e;
			}
			return response;
		}

		#endregion

	}
}

