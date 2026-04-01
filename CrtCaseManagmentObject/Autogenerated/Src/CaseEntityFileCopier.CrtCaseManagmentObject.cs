namespace Terrasoft.Configuration.Packages.Case
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Configuration.FileUpload;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Common;

	#region Class: CaseEntityFileCopier

	/// <summary>
	/// CopyingFileDetailUtility for CaseFile.
	/// </summary>
	[DefaultBinding(typeof(IEntityFileCopier), Name = "Case")]
	public class CaseEntityFileCopier : IEntityFileCopier
	{

		#region Class: CaseFileEntityUploadInfo

		/// <summary>
		/// Represents file info for CaseFile.
		/// </summary>
		protected class CaseFileEntityUploadInfo : FileEntityUploadInfo
		{

			#region Constants: Private

			private const string TargetFileSchemaName = "CaseFile";
			private const string TargetSchema = "Case";

			#endregion

			#region Public: Constructors

			/// <summary>
			/// Initializes new instance of <see cref="CaseFileEntityUploadInfo"/>. 
			/// </summary>
			public CaseFileEntityUploadInfo(string entitySchemaName, Guid fileId, string fileName)
					: base(entitySchemaName, fileId, fileName) {
				IsNeedToNotify = false;
			}

			/// <summary>
			/// Initializes new instance of <see cref="CaseFileEntityUploadInfo"/>. 
			/// </summary>
			public CaseFileEntityUploadInfo(string entitySchemaName, Guid fileId, string fileName, bool isNeedToNotify)
					: base(entitySchemaName, fileId, fileName) {
				IsNeedToNotify = isNeedToNotify;
			}

			#endregion

			#region Properties: Public

			/// <summary>
			/// Indicates if it is need to notify listeners of CaseFile.
			/// </summary>
			public bool IsNeedToNotify {
				get; set;
			}

			#endregion

			#region Methods: Public

			/// <summary>
			/// Make copy from source <see cref="IFileUploadInfo"/>.
			/// </summary>
			/// <param name="source">Source file upload information.</param>
			/// <param name="parentColumnValue">Parent column value uniqueidentifier.</param>
			/// <param name="content">File content <see cref="FileUploadInfo.Content"/>.</param>
			/// <returns>File info for CaseFile <see cref="CaseFileEntityUploadInfo"/>.</returns>
			public static CaseFileEntityUploadInfo CopyFromFileUploadInfo(IFileUploadInfo source, 
					Guid parentColumnValue, Stream content) {
				content.Position = 0;
				var fileEntityInfo =
					new CaseFileEntityUploadInfo(TargetFileSchemaName, Guid.NewGuid(), source.FileName) {
						ParentColumnName = TargetSchema,
						ParentColumnValue = parentColumnValue,
						TotalFileLength = source.TotalFileLength,
						Content = content,
					};
				return fileEntityInfo;
			}

			#endregion
		}

		#endregion

		#region Class: CaseFileRepository

		protected class CaseFileRepository : FileRepository
		{

			#region Class: CaseFileUploader

			protected class CaseFileUploader : FileUploader
			{

				#region Constructors: Public

				/// <summary>
				/// Creates instance of type <see cref="CaseFileUploader"/>.
				/// </summary>
				/// <param name="userConnection">User connection.</param>
				public CaseFileUploader(UserConnection userConnection) : base(userConnection) {
				}

				#endregion

				#region Methods: Protected

				protected override void FillAttributes(Dictionary<string, object> attributes, IFileUploadInfo fileUploadInfo,
						bool isSetBaseAttributes) {
					base.FillAttributes(attributes, fileUploadInfo, isSetBaseAttributes);
					if (isSetBaseAttributes && (fileUploadInfo is CaseFileEntityUploadInfo caseFileInfo)) {
						attributes["IsNeedToNotify"] = caseFileInfo.IsNeedToNotify;
					}
				}

				#endregion

				#region Methods: Public

				/// <summary>
				/// Saves file content into database.
				/// </summary>
				/// <param name="fileUploadInfo">File upload information.</param>
				/// <param name="isSetCustomColumns">Is set custom columns.</param>
				public void SaveIntoDb(IFileUploadInfo fileUploadInfo, bool isSetCustomColumns = true) {
					Save(fileUploadInfo, isSetCustomColumns);
				}

				#endregion

			}

			#endregion

			#region Constructors: Public

			/// <summary>
			/// Creates instance of type <see cref="CaseFileRepository"/>.
			/// </summary>
			/// <param name="userConnection">User connection.</param>
			public CaseFileRepository(UserConnection userConnection) : base(userConnection) {
			}

			#endregion

			#region Methods: Public

			/// <summary>
			/// Uploads file.
			/// </summary>
			/// <param name="fileUploadInfo">File upload information.</param>
			/// <param name="isSetCustomColumns">Is set custom columns.</param>
			/// <returns>Is operation successful.</returns>
			public override bool UploadFile(IFileUploadInfo fileUploadInfo, bool isSetCustomColumns) {
				var uploader = new CaseFileUploader(UserConnection);
				uploader.UploadFile(fileUploadInfo, isSetCustomColumns);
				return true;
			}

			#endregion
		}

		#endregion

		#region Properties: Protected

		/// <summary>
		/// User connection.
		/// </summary>
		protected UserConnection UserConnection { get; }

		/// <summary>
		/// File repository <see cref="Terrasoft.Configuration.FileRepository"/>
		/// </summary>
		protected FileRepository FileRepository { get; }

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes new instance of <see cref="CaseEntityFileCopier"/>. 
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="fileRepository">File repository <see cref="Terrasoft.Configuration.FileRepository"/></param>
		public CaseEntityFileCopier(UserConnection userConnection, FileRepository fileRepository) {
			UserConnection = userConnection;
			FileRepository = fileRepository;
		}

		/// <summary>
		/// Initializes new instance of <see cref="CaseEntityFileCopier"/>. 
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public CaseEntityFileCopier(UserConnection userConnection) {
			UserConnection = userConnection;
			FileRepository = new CaseFileRepository(userConnection);
		}

		#endregion

		#region Methods: Private

		private Dictionary<string, object> GetAdditionalParams(Guid sourceFileSchemaUId, Guid fileId) {
			Dictionary<string, object> additionalParams = new Dictionary<string, object>();
			List<string> fieldsToCopy = new List<string>() {
				"CreatedById", "ModifiedById"
			};
			var schema = UserConnection.EntitySchemaManager.GetItemByUId(sourceFileSchemaUId);
			var entity = UserConnection.EntitySchemaManager.GetEntityByName(schema.Name, UserConnection);

			if (entity.FetchFromDB(fileId)) {
				fieldsToCopy.ForEach(field => {
					additionalParams.Add(field, entity.GetColumnValue(field));
				});
			}
			return additionalParams;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Returns file <see cref="EntitySchema"/> by master schema name.
		/// </summary>
		/// <param name="masterSchemaName">Master schema name.</param>
		/// <returns>File <see cref="EntitySchema"/>.</returns>
		protected EntitySchema GetFileSchema(string masterSchemaName) {
			var fileSchema = UserConnection.EntitySchemaManager.GetInstanceByName($"{masterSchemaName}File");
			return fileSchema;
		}

		/// <summary>
		/// Returns files <see cref="EntityCollection"/> for copying.
		/// </summary>
		/// <param name="sourceMasterSchemaName">Source master schema name.</param>
		/// <param name="sourceMasterRecordId">Source master record uniqueidentifier.</param>
		/// <param name="primaryColumnName">Generated <see cref="EntitySchemaQuery"/> primary column name.</param>
		/// <returns>Files <see cref="EntityCollection"/>.</returns>
		protected virtual EntityCollection GetFilesToCopy(string sourceMasterSchemaName, Guid sourceMasterRecordId,
				out string primaryColumnName) {
			EntitySchema sourceFileSchema = GetFileSchema(sourceMasterSchemaName);
			var filesEsq = new EntitySchemaQuery(sourceFileSchema);
			filesEsq.UseAdminRights = false;
			primaryColumnName = filesEsq.AddColumn(filesEsq.RootSchema.GetPrimaryColumnName()).Name;
			filesEsq.Filters.Add(filesEsq
				.CreateFilterWithParameters(FilterComparisonType.Equal, sourceMasterSchemaName, sourceMasterRecordId));
			return filesEsq.GetEntityCollection(UserConnection);
		}

		#endregion

		#region Methods: Public

		//// <inheritdoc/>
		public void CopyAll(string sourceMasterSchemaName, Guid sourceMasterRecordId, Guid targetMasterRecordId) {
			EntitySchema sourceFileSchema = GetFileSchema(sourceMasterSchemaName);
			var sourceList = GetFilesToCopy(sourceMasterSchemaName, sourceMasterRecordId, out var primaryColumnName);
			Copy(sourceFileSchema.UId, targetMasterRecordId, sourceList
				.Select(entity => entity.GetTypedColumnValue<Guid>(primaryColumnName)).ToList());
		}

		/// <summary>
		/// Copy files from one entity to another.
		/// </summary>
		/// <param name="sourceFileSchemaUId">Schema uniqueidentifier of source file entity.</param>
		/// <param name="targetMasterRecordId">Target master record uniqueidentifier.</param>
		/// <param name="sourceFileRecods">List of files record uniqueidentifier of source master entity.</param>
		public void Copy(Guid sourceFileSchemaUId, Guid targetMasterRecordId, IList<Guid> sourceFileRecods) {
			foreach (var fileId in sourceFileRecods) {
				using (var memoryStream = new MemoryStream()) {
					var bwriter = new BinaryWriter(memoryStream);
					var fileInfo = FileRepository.LoadFile(sourceFileSchemaUId, fileId, bwriter);
					var fileCopy = CaseFileEntityUploadInfo
						.CopyFromFileUploadInfo(fileInfo, targetMasterRecordId, memoryStream);
					fileCopy.AdditionalParams = GetAdditionalParams(sourceFileSchemaUId, fileId);
					FileRepository.UploadFile(fileCopy);
				}
			}
		}

		#endregion

	}

	#endregion

}
