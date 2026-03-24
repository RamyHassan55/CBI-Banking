namespace Terrasoft.Configuration.Section
{
    using global::Common.Logging;
    using System;
	using System.Collections.Generic;
	using System.Data;
    using System.Diagnostics;
    using System.Linq;
	using Common;
	using Core;
	using Core.DB;
	using Core.Entities;
	using Core.Factories;
	using Core.Store;

	#region Class SectionRepository

	[DefaultBinding(typeof(ISectionRepository), Name = "General")]
	public class SectionRepository : BaseSectionRepository
	{

		#region Fileds: Private

		private readonly string[] _sectionRelatedEntitySufixes = { "File", "Folder", "InFolder", "Tag", "InTag" };

		/// <summary>
		/// <see cref="ILog"/> implementation instance.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger("Workplace");

		#endregion

		#region Fields: Protected

		/// <summary>
		/// <see cref="UserConnection"/> instance.
		/// </summary>
		protected readonly UserConnection UserConnection;

		/// <summary>
		/// <see cref="EntitySchemaManager"/> instance.
		/// </summary>
		protected readonly EntitySchemaManager EntitySchemaManager;

		/// <summary>
		/// <see cref="ICacheStore"/> implementation instance.
		/// Represents session level cache.
		/// </summary>
		protected readonly ICacheStore SessionCache;

		/// <summary>
		/// <see cref="IResourceStorage"/> implementation instance.
		/// </summary>
		protected readonly IResourceStorage ResourceStorage;

		#endregion

		#region Constructors: Public

		public SectionRepository(UserConnection uc) {
			UserConnection = uc;
			EntitySchemaManager = uc.EntitySchemaManager;
			SessionCache = uc.SessionCache;
			ResourceStorage = uc.ResourceStorage;
		}

		#endregion

		#region Methods: Private

		/// <summary>
		/// Creates new <see cref="Section"/> instance, using information from <paramref name="dataReader"/>.
		/// </summary>
		/// <param name="dataReader"><see cref="IDataReader"/> implementation instance.</param>
		/// <param name="sectionWorkplaceIds">Sections workplaces identifiers collection.</param>
		/// <returns><see cref="Section"/> instance.</returns>
		private Section CreateSectionInstance(IDataReader dataReader, Dictionary<Guid, List<Guid>> sectionWorkplaceIds) {
			Guid sectionId = dataReader.GetColumnValue<Guid>("Id");
			int type = dataReader.GetColumnValue<int>("Type");
			string caption = dataReader.GetColumnValue<string>("Caption");
			string schemaName = dataReader.GetColumnValue<string>("SectionSchema");
			string moduleName = dataReader.GetColumnValue<string>("SectionModuleSchema");
			bool isModule = string.IsNullOrEmpty(schemaName) && !string.IsNullOrEmpty(moduleName);
			string sectionSchemaName = isModule ? moduleName : schemaName;
			string typeColumnName = GetTypeColumnName(dataReader);
			string iconBackground = dataReader.GetColumnValue<string>("IconBackground");
			Guid sysModuleEntityId = dataReader.GetColumnValue<Guid>("SysModuleEntityId");
			Guid entityUId = dataReader.GetColumnValue<Guid>("EntityUId");
			Guid sysModuleVisaEntityUId = dataReader.GetColumnValue<Guid>("VisaSchemaUId");
			Guid image32Id = dataReader.GetColumnValue<Guid>("Image32Id");
			Guid moduleSchemaUId = dataReader.GetColumnValue<Guid>("ModuleSchemaUId");
			var section = new Section(sectionId, sysModuleEntityId, (SectionType)type) {
				Caption = caption,
				Code = dataReader.GetColumnValue<string>("Code"),
				SchemaName = sectionSchemaName,
				EntityUId = entityUId,
				TypeColumnName = typeColumnName,
				SysModuleVisaEntityUId = sysModuleVisaEntityUId,
				IconBackground = iconBackground,
				Image32Id = image32Id,
				IsModule = isModule,
				ModuleSchemaUId = moduleSchemaUId,
			};
			if (sectionWorkplaceIds.ContainsKey(sectionId)){
				section.AddSectionInWorkplaceRange(sectionWorkplaceIds[sectionId]);
			}
			return section;
		}

		/// <summary>
		/// Gets type column name.
		/// </summary>
		/// <param name="dataReader"><see cref="IDataReader"/> implementation instance.</param>
		/// <returns>Type column name.</returns>
		private string GetTypeColumnName(IDataReader dataReader) {
			return dataReader.GetColumnValue<string>("Attribute");
		}

		/// <summary>
		/// Loads workplaces info.
		/// </summary>
		/// <returns>Sections workplaces identifiers collection.</returns>

		private Dictionary<Guid, List<Guid>> LoadSectionWorkplaceIds() {
			var select = new Select(UserConnection)
				.Column("smiw", "SysModuleId")
				.Column("smiw", "SysWorkplaceId")
				.From("SysModuleInWorkplace").As("smiw")
				.OrderByAsc("smiw", "Position") as Select;
			Dictionary<Guid, List<Guid>> result = new Dictionary<Guid, List<Guid>>();
			using (DBExecutor dbExecutor = UserConnection.EnsureDBConnection()) {
				using (IDataReader dataReader = select.ExecuteReader(dbExecutor)) {
					while (dataReader.Read()){
						Guid sectionId = dataReader.GetColumnValue<Guid>("SysModuleId");
						if (!result.ContainsKey(sectionId)){
							result[sectionId] = new List<Guid>();
						}
						result[sectionId].AddIfNotExists(dataReader.GetColumnValue<Guid>("SysWorkplaceId"));
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Returns <see cref="Section"/> collection from cache.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <returns><see cref="Section"/> collection.</returns>
		private List<Section> GetFromCache(string key) {
			var sections = SessionCache[key] as List<Section>;
			if (sections == null || !sections.Any()) {
				return null;
			}
			return sections;
		}

		/// <summary>
		/// Sets <paramref name="value"/> to cache in <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Cache item value.</param>
		private void SetInCache(string key, List<Section> value) {
			SessionCache[key] = value;
		}

		/// <summary>
		/// Clears all related to <paramref name="workplaceId"/> cache items.
		/// </summary>
		private void ClearSectionCache() {
			SessionCache.Remove(GetAllCacheKey());
			SessionCache.Remove(GetSectionsByTypeKey(SectionType.General));
			SessionCache.Remove(GetSectionsByTypeKey(SectionType.SSP));
		}

		/// <summary>
		/// Returns all sections cache key.
		/// </summary>
		/// <returns>All sections cache key.</returns>
		private string GetAllCacheKey() {
			return "All_Sections";
		}

		/// <summary>
		/// When <paramref name="item"/> is not null, adds <paramref name="item"/> unique identifier to <paramref name="list"/>.
		/// </summary>
		/// <param name="list"><see cref="List{Guid}"/> instance.</param>
		/// <param name="item"><see cref="ISchemaManagerItem"/> instance.</param>
		private void AddUIdIfNotNull(List<Guid> list, ISchemaManagerItem item) {
			if (item != null) {
				list.AddIfNotExists(item.UId);
			}
		}

		/// <summary>
		/// Returns section required entities unique identifiers list.
		/// </summary>
		/// <param name="sectionMainEntity">Section main entity <see cref="ISchemaManagerItem"/> instance.</param>
		/// <returns>Section required entities unique identifiers list.</returns>
		private IEnumerable<Guid> GetSectionRequiredEntityIds(ISchemaManagerItem sectionMainEntity) {
			var result = new List<Guid>();
			result.AddIfNotExists(sectionMainEntity.UId);
			foreach (var entityNameSuffix in _sectionRelatedEntitySufixes) {
				AddUIdIfNotNull(result, EntitySchemaManager.FindItemByName(sectionMainEntity.Name + entityNameSuffix));
			}
			return result;
		}

		/// <summary>
		/// Creates section not found exception message.
		/// </summary>
		/// <param name="sectionId"><see cref="Section"/> unique identifier.</param>
		/// <returns>Section not found exception message.</returns>
		private string GetItemNotFoundMessage(Guid sectionId) {
			var messageTpl = new LocalizableString(ResourceStorage, "SectionExceptionResources",
					"LocalizableStrings.SectionNotFoundByIdTpl.Value").ToString();
			return string.Format(messageTpl, sectionId.ToString());
		}

		private void RemoveByWorkplaceIdInternal(Guid workplaceId) {
			new Delete(UserConnection)
				.From("SysModuleInWorkplace")
				.Where("SysWorkplaceId").IsEqual(Column.Parameter(workplaceId))
				.Execute();
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Creates <see cref="Section"/> data select.
		/// </summary>
		/// <returns><see cref="Select"/> instance.</returns>
		protected Select GetSectionsSelect() {
			Guid cultureId = UserConnection.CurrentUser.SysCultureId;
			var select =
				new Select(UserConnection)
					.Column("SysModule", "Id")
					.Column("SysModule", "Type")
					.Column("SysModule", "Code")
					.Column("SysModule", "Attribute")
					.Column("SysModule", "SysModuleEntityId")
					.Column("SysModule", "IconBackground")
					.Column("SysModule", "Image32Id")
					.Column("SysModule", "SectionModuleSchemaUId").As("ModuleSchemaUId")
					.Column("smv", "VisaSchemaUId")
					.Column("vscus", "Name").As("SectionSchema")
					.Column("vscus1", "Name").As("SectionModuleSchema")
					.Column("sme", "SysEntitySchemaUId").As("EntityUId")
					.Column("sme", "TypeColumnUId").As("TypeColumnUId")
				.From("SysModule").As("SysModule")
				.InnerJoin("SysModuleEntity").As("sme")
					.On("sme", "Id").IsEqual("SysModule", "SysModuleEntityId")
				.LeftOuterJoin("SysModuleVisa").As("smv")
					.On("SysModule", "SysModuleVisaId").IsEqual("smv", "Id")
				.LeftOuterJoin("VwSysClientUnitSchema").As("vscus")
					.On("SysModule", "SectionSchemaUId").IsEqual("vscus", "UId")
					.And("vscus", "SysWorkspaceId").IsEqual(Column.Parameter(UserConnection.Workspace.Id))
				.LeftOuterJoin("VwSysClientUnitSchema").As("vscus1")
					.On("SysModule", "SectionModuleSchemaUId").IsEqual("vscus1", "UId")
					.And("vscus1", "SysWorkspaceId").IsEqual(Column.Parameter(UserConnection.Workspace.Id)) as Select;
			select.AddColumnLocalization("SysModule", "Caption", "Caption", cultureId);
			return select;
		}

		/// <summary>
		/// Selects sections data using <paramref name="select"/> and creates <see cref="Section"/> collection.
		/// If cached result avaliable, select will be skipped.
		/// </summary>
		/// <param name="select"><see cref="Select"/> instance.</param>
		/// <param name="cacheKey">Session cache item key.</param>
		/// <returns><see cref="Section"/> collection.</returns>
		protected List<Section> GetSections(Select select, string cacheKey) {
			var watch = Stopwatch.StartNew();
			_log.Debug($"[GetSections] [{watch.ElapsedMilliseconds}ms] Start.");
			List<Section> sections = GetFromCache(cacheKey);
			if (sections != null) {
				_log.Debug($"[GetSections] [{watch.ElapsedMilliseconds}ms] End from cache.");
				return sections;
			}
			sections = GetSectionsFromDb(select);
			_log.Debug($"[GetSections] [{watch.ElapsedMilliseconds}ms] End from DB.");
			SetInCache(cacheKey, sections);
			_log.Debug($"[GetSections] [{watch.ElapsedMilliseconds}ms] Set to cache.");
			return sections;
		}

		/// <summary>
		/// Selects sections data using <paramref name="select"/> and creates <see cref="Section"/> collection.
		/// </summary>
		/// <param name="select"><see cref="Select"/> instance.</param>
		/// <returns><see cref="Section"/> collection.</returns>
		protected List<Section> GetSectionsFromDb(Select select) {
			var sectionWorkplaceIds = LoadSectionWorkplaceIds();
			List<Section> sections = new List<Section>();
			using (DBExecutor dbExecutor = UserConnection.EnsureDBConnection()) {
				using (IDataReader dataReader = select.ExecuteReader(dbExecutor)) {
					while (dataReader.Read()) {
						sections.AddIfNotExists(CreateSectionInstance(dataReader, sectionWorkplaceIds));
					}
				}
			}
			return sections;
		}

		/// <summary>
		/// Returns <see cref="ISchemaManagerItem"/> used by <paramref name="section"/>.
		/// </summary>
		/// <param name="section"><see cref="Section"/> instance.</param>
		/// <returns><see cref="ISchemaManagerItem"/> used by <paramref name="section"/>.</returns>
		protected ISchemaManagerItem GetSectionEntitySchemaItem(Section section) {
			return EntitySchemaManager.FindItemByUId(section.EntityUId);
		}

		/// <summary>
		/// Returns sections with type general cache key.
		/// </summary>
		/// <returns>Sections with type general cache key.</returns>
		protected string GetSectionsByTypeKey(SectionType type) {
			return $"Sections_{type}";
		}

		/// <summary>
		/// Sets sections by type filters to <paramref name="select"/>.
		/// </summary>
		/// <param name="select"><see cref="Select"/> instance.</param>
		/// <param name="type">Type filter value/</param>
		protected virtual void SetSectionsByTypeFilters(Select select, SectionType type) {
			select.Where("SysModule", "Type").IsEqual(Column.Parameter(type));
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		public override Section Get(Guid sectionId) {
			var select = GetSectionsSelect();
			var cacheKey = GetAllCacheKey();
			var sections = GetSections(select, cacheKey);
			var section = sections.FirstOrDefault(s => s.Id.Equals(sectionId));
			if (section == null) {
				var message = GetItemNotFoundMessage(sectionId);
				throw new ItemNotFoundException(message);
			}
			return section;
		}

		/// <inheritdoc />
		public override IEnumerable<Section> GetAll() {
			var select = GetSectionsSelect();
			var cacheKey = GetAllCacheKey();
			return GetSections(select, cacheKey);
		}

		/// <inheritdoc />
		public override IEnumerable<Section> GetByType(SectionType type) {
			var select = GetSectionsSelect();
			SetSectionsByTypeFilters(select, type);
			var cacheKey = GetSectionsByTypeKey(type);
			return GetSections(select, cacheKey);
		}

		/// <inheritdoc />
		public override IEnumerable<Guid> GetRelatedEntityIds(Section section) {
			var result = new List<Guid>();
			var sectionMainEntity = GetSectionEntitySchemaItem(section);
			if (sectionMainEntity == null) {
				return result;
			}
			result.AddRangeIfNotExists(GetSectionRequiredEntityIds(sectionMainEntity));
			if (section.SysModuleVisaEntityUId.IsNotEmpty()) {
				AddUIdIfNotNull(result, EntitySchemaManager.FindItemByUId(section.SysModuleVisaEntityUId));
			}
			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetSectionNonAdministratedByRecordsEntityCaptions(Section section) {
			return new List<string>();
		}

		/// <inheritdoc />
		public override void Save(Section section) {
			ClearCache();
		}

		/// <inheritdoc />
		public override void ClearCache() {
			ClearSectionCache();
		}

		public override void RemoveByWorkplaceId(Guid workplaceId) {
			workplaceId.CheckArgumentEmpty(nameof(workplaceId));
			base.RemoveByWorkplaceId(workplaceId);
			RemoveByWorkplaceIdInternal(workplaceId);
		}

		#endregion

	}

	#endregion

}