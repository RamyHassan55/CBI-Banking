namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Terrasoft.Core.DB;
	using Terrasoft.Core;
	using Terrasoft.Common;
	using Terrasoft.Core.Entities;
	using Terrasoft.Web.Common;
	using System.Runtime.Serialization;
	using System.IO;

	#region Class: ActivityFilesResponse

	/// <summary>
	/// Represents response for portal file service.
	/// </summary>
	[DataContract]
	public class ActivityFilesResponse : ConfigurationServiceResponse
	{

		#region Properties: Public

		/// <summary>
		/// Collection of activity files.
		/// </summary>
		[DataMember(Name = "activityFiles")]
		public List<PortalFileService.ActivityFileContainer> ActivityFiles {
			get; set;
		}

		#endregion

	}

	#endregion

	#region Class: PortalFileService

	/// <summary>
	/// Portal file service.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class PortalFileService : BaseService
	{

		#region Class: ActivityFileContainer

		/// <summary>
		/// Represents container which stores files data.
		/// </summary>
		public class ActivityFileContainer
		{

			#region Properties: Public

			/// <summary>
			/// File Name.
			/// </summary>
			public string Name {
				get; set;
			}

			/// <summary>
			/// File Size.
			/// </summary>
			public int Size {
				get; set;
			}

			/// <summary>
			/// File Id.
			/// </summary>
			public Guid Id	{
				get; set;
			}

			#endregion

		}

		#endregion

		#region Class: PortalFileLoader

		/// <summary>
		/// Represents file loader wich can set UserConnection.
		/// </summary>
		protected class PortalFileLoader: FileService.FileService
		{

			#region Method: Public

			/// <summary>
			/// Set UserConnection property.
			/// </summary>
			/// <param name="userConnection">The user connection.<see cref="UserConnection"/></param>
			public void SetUserConnection(UserConnection userConnection) {
				UserConnection = userConnection;
			}

			#endregion

		}

		#endregion

		#region Contants: Private

		private readonly string ActivityFileSchemaUId = "080C9917-7EC9-42E5-86FF-75A683D4F124";

		#endregion

		#region Methods: Private

		private bool CheckActivityFileAccess(string caseMessageHistoryId, string fileId) {
			var activityFilesSelect = GetActivityFilesSelect(caseMessageHistoryId);
			var activityFiles = GetActivityFilesCollection(activityFilesSelect);
			var file = Guid.Parse(fileId);
			return activityFiles.Exists(x => x.Id.Equals(file));
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Generating a request with regard to the rights.
		/// </summary>
		/// <param name="caseMessageHistoryId">CaseMessageHistory uniqueidentifier.</param>
		/// <returns>Select query <see cref="Select"/>.</returns>
		protected Select GetActivityFilesSelect(string caseMessageHistoryId) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "CaseMessageHistory");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			var primaryKeyFilter = esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				esq.RootSchema.GetPrimaryColumnName(), caseMessageHistoryId);
			esq.Filters.Add(primaryKeyFilter);
			var caseMessageHistorySelect = esq.GetSelectQuery(UserConnection);
			caseMessageHistorySelect.InnerJoin("PortalEmailMessage").As("PEM").On("PEM", "CaseMessageHistoryId").IsEqual("CaseMessageHistory", "Id");
			caseMessageHistorySelect.InnerJoin("ActivityFile").As("AF").On("AF", "ActivityId").IsEqual("CaseMessageHistory", "RecordId");
			caseMessageHistorySelect.Column("AF", "Name").
				Column("AF", "Id").As("ActivityFileId").
				Column("AF", "Size").As("ActivityFileSize");
			return caseMessageHistorySelect;
		}

		/// <summary>
		/// Adds additional filters to file select when we get list of files.
		/// </summary>
		/// <param name="activityFilesSelect">Activity files query <see cref="Select"/>.</param>
		/// <returns>Select query <see cref="Select"/>.</returns>
		protected Select AddAdditionalFiltersToActivityFilesSelect(Select activityFilesSelect) {
			activityFilesSelect.And("AF", "Inline").IsEqual(Column.Const(false));
			return activityFilesSelect;
		}

		/// <summary>
		/// Get collection with data of activity files.
		/// </summary>
		/// <param name="activityFilesSelect">Query to File table.<see cref="Select"/></param>
		/// <returns>Collection with ActivityFileContainer.<see cref="ActivityFileContainer"/></returns>
		protected List<ActivityFileContainer> GetActivityFilesCollection(Select activityFilesSelect) {
			var resultCollection = new List<ActivityFileContainer>();
			using (var dbExecutor = UserConnection.EnsureDBConnection()) {
				using (var dataReader = activityFilesSelect.ExecuteReader(dbExecutor)) {
					while (dataReader.Read()) {
						resultCollection.Add(new ActivityFileContainer {
							Name = dataReader.GetColumnValue<string>("Name"),
							Id = dataReader.GetColumnValue<Guid>("ActivityFileId"),
							Size = dataReader.GetColumnValue<int>("ActivityFileSize")
						});
					}
				}
			}
			return resultCollection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Load ActivityFile if user have premissions.
		/// </summary>
		/// <param name="caseMessageHistoryId">CaseMessageHistory uniqueidentifier.</param>
		/// <param name="fileId">ActivityFile uniqueidentifier</param>
		[OperationContract]
		[WebGet(UriTemplate = "GetActivityFile/{caseMessageHistoryId}/{fileId}")]
		public Stream GetActivityFile(string caseMessageHistoryId, string fileId) {
			if (CheckActivityFileAccess(caseMessageHistoryId, fileId)) {
				var fileLoader = new PortalFileLoader();
				fileLoader.SetUserConnection(UserConnection.AppConnection.SystemUserConnection);
				return fileLoader.GetFile(ActivityFileSchemaUId, fileId);				
			}
			return Stream.Null;
		}

		/// <summary>
		/// Get activity files collection.
		/// </summary>
		/// <param name="caseMessageHistoryId">CaseMessageHistory uniqueidentifier.</param>
		/// <returns>Container with ActivityFiles collection <see cref="ActivityFilesResponse"/>.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
		ResponseFormat = WebMessageFormat.Json)]
		public ActivityFilesResponse GetActivityFiles(string caseMessageHistoryId) {
			var activityFilesSelect = GetActivityFilesSelect(caseMessageHistoryId);
			activityFilesSelect = AddAdditionalFiltersToActivityFilesSelect(activityFilesSelect);
			var activityFiles = GetActivityFilesCollection(activityFilesSelect);
			var result = new ActivityFilesResponse { ActivityFiles = activityFiles };
			return result;
		}

		#endregion

	}

	#endregion

}
