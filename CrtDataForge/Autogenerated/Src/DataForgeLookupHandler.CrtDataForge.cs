namespace Terrasoft.Configuration.DataForge
{
	using Common;
	using Core.Configuration;
	using Core.DB;
	using global::Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Interface: ILookupHandler

	/// <summary>
	/// Provides functionality to identify and extract lookup definitions and values.
	/// </summary>
	public interface ILookupHandler
	{
		/// <summary>
		/// Determines whether the given entity represents a lookup schema.
		/// </summary>
		/// <param name="entity">The entity to evaluate.</param>
		/// <returns><c>true</c> if the entity is a lookup schema; otherwise, <c>false</c>.</returns>
		bool IsLookup(Entity entity);

		/// <summary>
		/// Determines whether the given entity is a lookup value.
		/// </summary>
		/// <param name="entity">The entity to evaluate.</param>
		/// <returns><c>true</c> if the entity is a lookup value; otherwise, <c>false</c>.</returns>
		bool IsLookupValue(Entity entity);

		/// <summary>
		/// Constructs a complete lookup definition from the specified lookup schema entity,
		/// </summary>
		/// <param name="entity">The lookup schema entity.</param>
		/// <returns>A populated <see cref="LookupDefinition"/> object.</returns>
		LookupDefinition GetLookupDefinition(Entity entity);

		/// <summary>
		/// Constructs a definition of a single lookup value (record) from the given entity.
		/// </summary>
		/// <param name="entity">The entity representing a lookup record.</param>
		/// <returns>A populated <see cref="LookupValueDefinition"/> object.</returns>
		LookupValueDefinition GetLookupValueDefinition(Entity entity);

		/// <summary>
		/// Retrieves a collection of full lookup definitions for the specified lookup schema identifiers.
		/// </summary>
		/// <param name="ids">The unique identifiers of the lookup schemas.</param>
		/// <returns>A list of <see cref="LookupDefinition"/> objects.</returns>
		List<LookupDefinition> GetLookupDefinitions(IReadOnlyList<Guid> ids = null);

		/// <summary>
		/// Maps a <see cref="LookupDefinition"/> to a <see cref="LookupSummary"/>.
		/// </summary>
		/// <param name="def">The lookup definition to map.</param>
		/// <returns>A <see cref="LookupSummary"/> object.</returns>
		LookupSummary MapToLookupSummary(LookupDefinition def);

		/// <summary>
		/// Maps a <see cref="LookupValueDefinition"/> to a <see cref="LookupValueSummary"/>.
		/// </summary>
		/// <param name="def">The lookup value definition to map.</param>
		/// <returns>A <see cref="LookupValueSummary"/> object.</returns>
		LookupValueSummary MapToLookupValueSummary(LookupValueDefinition def);

		/// <summary>
		/// Retrieves all lookup definitions that include the specified lookup value entity.
		/// </summary>
		/// <param name="lookupValue">The entity representing the lookup value.</param>
		/// <returns>A list of <see cref="LookupDefinition"/> objects that reference the given value.</returns>
		List<LookupDefinition> GetLookupDefinitionsForValue(Entity lookupValue);

		/// <summary>
		/// Retrieves lookup value definitions for the specified collection of lookup definitions.
		/// </summary>
		/// <param name="lookupDefinitions">The collection of lookup definitions.</param>
		/// <returns>A list of <see cref="LookupValueDefinition"/> objects associated with the provided lookups.</returns>
		List<LookupValueDefinition> GetLookupValueDefinitionsForLookups(IReadOnlyList<LookupDefinition> lookupDefinitions);

	}

	#endregion

	#region Class: LookupHandler

	/// <summary>
	/// Resolves lookup metadata and value definitions.
	/// </summary>
	[DefaultBinding(typeof(ILookupHandler))]
	internal class LookupHandler : ILookupHandler
	{

		#region Constants: Private

		private const string LookupSchemaName = "Lookup";
		private const string BaseLookupSchemaName = "BaseLookup";
		private const string BaseHierarchicalLookupSchemaName = "BaseHierarchicalLookup";
		private const string BaseCodeLookupSchemaName = "BaseCodeLookup";
		private const string BaseImageLookupSchemaName = "BaseImageLookup";
		private const string BaseCodeImageLookupSchemaName = "BaseCodeImageLookup";
		private const string SysSchemaName = "SysSchema";

		private const int DataForgeLookupRecordSoftLimit = 3000;
		private const int DataForgeLookupRecordHardLimit = 19999;

		private static readonly HashSet<string> LookupSchemas = new HashSet<string>() {
			LookupSchemaName,
			BaseLookupSchemaName,
			BaseHierarchicalLookupSchemaName,
			BaseCodeLookupSchemaName,
			BaseImageLookupSchemaName,
			BaseCodeImageLookupSchemaName,
		};

		/// <summary>
		/// SysSchema.ParentId values that indicate a lookup-family schema.
		/// </summary>
		private static readonly Guid[] LookupBaseParentUIds = new[] {
			new Guid("D8D9C657-DF07-48DD-9B83-FC1FF01D1939"), // Lookup
			new Guid("50C4D8F3-45C4-4989-A68E-2EDCEE2B0B53"), // BaseHierarchicalLookup
			new Guid("A8A295CB-B02A-4D82-896B-66EBDB378E98"), // BaseCodeLookup
			new Guid("A23AE877-EF9E-412B-BB96-A1D328122B37"), // BaseImageLookup
			new Guid("D7307337-6F57-43DB-B710-E72114A8CEE7"), // BaseCodeImageLookup
			new Guid("19125E51-A235-4540-A4C7-5D9CAFF66621")  // BaseLookup
		};

		#endregion

		#region Fields: Private

		private static readonly ILog _log = LogManager.GetLogger("DataForge");
		private readonly UserConnection _userConnection;
		private readonly IChecksumProvider _checksumProvider;
		private int? _effectiveLimit = null;

		#endregion

		#region Properties: Private

		private int EffectiveLimit {
			get {
				if (!_effectiveLimit.HasValue) {
					int softLimit = SysSettings.GetValue(_userConnection, "DataForgeLookupRecordLimit", DataForgeLookupRecordSoftLimit);
					_effectiveLimit = Math.Min(softLimit, DataForgeLookupRecordHardLimit);
				}
				return _effectiveLimit.Value;
			}
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates a new instance of <see cref="LookupHandler"/>.
		/// </summary>
		public LookupHandler(UserConnection userConnection, IChecksumProvider checksumProvider) {
			_userConnection = userConnection;
			_checksumProvider = checksumProvider;
		}

		#endregion

		#region Methods: Private

		private EntityCollection QueryEntities(string schemaName, IReadOnlyList<Guid> ids = null, Action<EntitySchemaQuery> configure = null, int? rowCount = null) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, schemaName);
			esq.AddAllSchemaColumns();
			if (ids != null && ids.Any()) {
				esq.Filters.Add(esq.CreateFilterWithParameters(
					FilterComparisonType.Equal, "Id", ids.Cast<object>().ToArray()
				));
			}
			if (rowCount.HasValue) {
				esq.RowCount = rowCount.Value;
			}
			configure?.Invoke(esq);
			return esq.GetEntityCollection(_userConnection);
		}

		private EntitySchema GetAssociatedSchema(Entity lookupEntity) {
			Guid schemaUId = Guid.Empty;
			try {
				if (lookupEntity.Schema?.Name == SysSchemaName) {
					schemaUId = lookupEntity.GetTypedColumnValue<Guid>("UId");
				} else {
					schemaUId = lookupEntity.GetTypedColumnValue<Guid>("SysEntitySchemaUId");
				}
			} catch {
				try {
					schemaUId = lookupEntity.GetTypedColumnValue<Guid>("UId");
				} catch { }
				if (schemaUId.IsEmpty()) {
					try {
						schemaUId = lookupEntity.GetTypedColumnValue<Guid>("SysEntitySchemaUId");
					} catch { }
				}
			}
			EntitySchema schema = schemaUId.IsEmpty()
				? null
				: _userConnection.EntitySchemaManager.FindInstanceByUId(schemaUId);
			if (schema == null) {
				_log.Warn($"Associated schema not found for UId: {schemaUId}");
			}
			return schema;
		}

		private string TryGetName(Entity entity) {
			try {
				var caption = entity.GetTypedColumnValue<string>("Caption");
				if (!string.IsNullOrWhiteSpace(caption)) {
					return caption;
				}
			} catch { }
			try {
				return entity.GetTypedColumnValue<string>("Name");
			} catch {
				var col = entity.Schema.PrimaryDisplayColumn;
				return col != null && entity.GetIsColumnValueLoaded(col)
					? entity.PrimaryDisplayColumnValue
					: null;
			}
		}

		private string TryGetDescription(Entity entity, string name) {
			try {
				var desc = entity.GetTypedColumnValue<string>("Description");
				return string.IsNullOrWhiteSpace(desc) || string.Equals(name, desc.Trim(), StringComparison.InvariantCultureIgnoreCase)
					? null
					: desc.Trim();
			} catch {
				return null;
			}
		}

		private static bool IsSaneDate(DateTime dt) =>
			dt != default && dt != DateTime.MinValue && dt.Year >= 2000;

		private string TryGetModifiedOn(Entity entity) {
			try {
				var modified = entity.GetTypedColumnValue<DateTime>("ModifiedOn");
				if (IsSaneDate(modified)) {
					return modified.ToString("o");
				}
			} catch { }
			try {
				var created = entity.GetTypedColumnValue<DateTime>("CreatedOn");
				if (IsSaneDate(created)) {
					return created.ToString("o");
				}
			} catch { }
			return null;
		}

		private (Guid Id, string Name, string Description, string ModifiedOn, string Checksum) ExtractData(Entity entity) {
			var id = entity.GetTypedColumnValue<Guid>("Id");
			var name = TryGetName(entity);
			var description = TryGetDescription(entity, name);
			var modifiedOn = TryGetModifiedOn(entity);
			var checksum = _checksumProvider.GetChecksum(name ?? string.Empty, description ?? string.Empty);
			return (id, name, description, modifiedOn, checksum);
		}

		private List<T> SelectValid<T>(IEnumerable<Entity> entities, Func<Entity, T> extractor)
			where T : class {
			return entities.Select(extractor).Where(x => x != null).ToList();
		}

		private bool TryExtractValid(Entity entity, out (Guid Id, string Name, string Description, string ModifiedOn, string Checksum) data) {
			data = ExtractData(entity);
			if (data.Id == Guid.Empty) {
				return false;
			}
			if (string.IsNullOrWhiteSpace(data.Name)) {
				return false;
			}
			if (string.IsNullOrWhiteSpace(data.ModifiedOn)) {
				return false;
			}
			return true;
		}

		private HashSet<Guid> GetVirtualSchemaUIds() {
			var result = new HashSet<Guid>();

			IEnumerable<ISchemaManagerItem<EntitySchema>> items;
			try {
				items = _userConnection.EntitySchemaManager.GetItems()
					?? Enumerable.Empty<ISchemaManagerItem<EntitySchema>>();
			} catch (Exception ex) {
				_log.Warn("GetVirtualSchemaUIds: failed to enumerate schema manager items.", ex);
				return result;
			}

			int skipped = 0;
			foreach (var mgrItem in items) {
				if (mgrItem == null) {
					continue;
				}
				try {
					var schema = mgrItem.Instance;
					if (schema != null && schema.IsVirtual && !schema.UId.IsEmpty()) {
						result.Add(schema.UId);
					}
				} catch (Exception ex) {
					skipped++;
					_log.Debug("GetVirtualSchemaUIds: skipped one schema item due to error.", ex);
				}
			}

			if (skipped > 0) {
				_log.Warn($"GetVirtualSchemaUIds: completed with {skipped} skipped items; collected {result.Count} virtual schema UIds.");
			}
			return result;
		}


		private List<Guid> GetAllowedLookupSysSchemaIds(IReadOnlyList<Guid> onlyIds = null) {
			var keys = new List<(Guid Id, Guid Uid)>();

			Select sel = new Select(_userConnection)
					.Column("s", "Id")
					.Column("s", "UId")
					.From("SysSchema").As("s")
					.LeftOuterJoin("DFSyncExceptions").As("ex")
						.On("s", "Name").IsEqual("ex", "Name")
					.Where("s", "ParentId").In(Column.Parameters(LookupBaseParentUIds.Cast<object>().ToArray()))
					.And("ex", "Name").IsNull()
				as Select;

			if (onlyIds != null && onlyIds.Count > 0) {
				sel.And("s", "Id").In(Column.Parameters(onlyIds.Cast<object>().ToArray()));
			}

			sel.SpecifyNoLockHints();

			using (DBExecutor executor = _userConnection.EnsureDBConnection())
			using (IDataReader reader = sel.ExecuteReader(executor)) {
				while (reader.Read()) {
					Guid id = reader.GetColumnValue<Guid>("Id");
					Guid uid = reader.GetColumnValue<Guid>("UId");
					if (id != Guid.Empty && uid != Guid.Empty) {
						keys.Add((id, uid));
					}
				}
			}

			HashSet<Guid> virtualUids = GetVirtualSchemaUIds();
			return keys.Where(k => !virtualUids.Contains(k.Uid)).Select(k => k.Id).ToList();
		}

		private List<Guid> GetAllowedLookupIds() {
			List<Guid> allowedLookupIds = new List<Guid>();
			Select select = new Select(_userConnection)
					.Column("Lookup", "Id")
					.From("Lookup").As("Lookup")
					.InnerJoin("SysSchema").As("Schema")
					.On("Lookup", "SysEntitySchemaUId").IsEqual("Schema", "UId")
					.LeftOuterJoin("DFSyncExceptions").As("Exception")
					.On("Schema", "Name").IsEqual("Exception", "Name")
					.Where("Exception", "Name").IsNull()
				as Select;
			select.SpecifyNoLockHints();
			using (DBExecutor executor = _userConnection.EnsureDBConnection())
			using (IDataReader reader = select.ExecuteReader(executor)) {
				while (reader.Read()) {
					Guid id = reader.GetColumnValue<Guid>("Id");
					allowedLookupIds.Add(id);
				}
			}
			return allowedLookupIds;
		}

		private IEnumerable<Entity> GetLookupsFromSysSchema(IReadOnlyList<Guid> onlyIds = null) {
			List<Guid> allowedIds = GetAllowedLookupSysSchemaIds(onlyIds);
			if (allowedIds.Count == 0) {
				return Enumerable.Empty<Entity>();
			}

			if (onlyIds != null && onlyIds.Count > 0) {
				allowedIds = allowedIds.Intersect(onlyIds).ToList();
				if (allowedIds.Count == 0) {
					return Enumerable.Empty<Entity>();
				}
			}
			return QueryEntities(SysSchemaName, allowedIds);
		}

		private IEnumerable<Entity> GetLookupsFromLookupSchema(IReadOnlyList<Guid> onlyIds = null) {
			List<Guid> allowedIds = GetAllowedLookupIds();
			if (allowedIds.Count == 0) {
				return Enumerable.Empty<Entity>();
			}
			if (onlyIds != null && onlyIds.Count > 0) {
				allowedIds = allowedIds.Intersect(onlyIds).ToList();
				if (allowedIds.Count == 0) {
					return Enumerable.Empty<Entity>();
				}
			}
			return QueryEntities(LookupSchemaName, allowedIds);
		}

		private static bool IsDerivedFromLookupFamily(EntitySchema schema) {
			var s = schema;
			while (s != null) {
				if (LookupSchemas.Contains(s.Name, StringComparer.InvariantCultureIgnoreCase)) {
					return true;
				}
				s = s.ParentSchema;
			}
			return false;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc/>
		public bool IsLookup(Entity entity) {
			if (entity?.Schema?.Name == LookupSchemaName) {
				return true;
			}
			if (entity?.Schema?.Name == SysSchemaName) {
				try {
					var uId = entity.GetTypedColumnValue<Guid>("UId");
					var schema = _userConnection.EntitySchemaManager.FindInstanceByUId(uId);
					return IsDerivedFromLookupFamily(schema);
				} catch {
					return false;
				}
			}
			return false;
		}

		/// <inheritdoc/>
		public bool IsLookupValue(Entity entity) => entity != null
			&& entity.Schema?.ParentSchema != null
			&& LookupSchemas.Contains(entity.Schema.ParentSchema.Name);

		/// <inheritdoc/>
		public LookupDefinition GetLookupDefinition(Entity entity) {
			if (!TryExtractValid(entity, out var d)) {
				return null;
			}

			var definition = new LookupDefinition {
				Id = d.Id,
				Name = d.Name,
				Description = d.Description,
				ModifiedOn = d.ModifiedOn,
				Checksum = d.Checksum
			};

			EntitySchema associated = GetAssociatedSchema(entity);
			if (associated != null && !associated.IsVirtual) {
				definition.ValuesSchemaName = associated.Name;
				definition.ValuesSchemaUId = associated.UId;
			}

			return definition;
		}

		/// <inheritdoc/>
		public LookupValueDefinition GetLookupValueDefinition(Entity entity) {
			if (!TryExtractValid(entity, out var d)) {
				return null;
			}
			return new LookupValueDefinition {
				Id = d.Id,
				Name = d.Name,
				Description = d.Description,
				ModifiedOn = d.ModifiedOn,
				Checksum = d.Checksum,
				SchemaUId = entity.Schema.UId
			};
		}

		/// <inheritdoc/>
		public LookupSummary MapToLookupSummary(LookupDefinition def) {
			if (def == null) {
				return null;
			}
			return new LookupSummary {
				Id = def.Id,
				ModifiedOn = def.ModifiedOn,
				Checksum = def.Checksum,
				ValuesSchemaUId = def.ValuesSchemaUId
			};
		}

		/// <inheritdoc/>
		public LookupValueSummary MapToLookupValueSummary(LookupValueDefinition def) {
			if (def == null) {
				return null;
			}
			return new LookupValueSummary {
				Id = def.Id,
				SchemaUId = def.SchemaUId,
				ModifiedOn = def.ModifiedOn,
				Checksum = def.Checksum
			};
		}

		/// <inheritdoc/>
		public List<LookupDefinition> GetLookupDefinitions(IReadOnlyList<Guid> lookupIds = null) {
			IEnumerable<Entity> sysSchemaLookups = GetLookupsFromSysSchema(lookupIds);
			IEnumerable<Entity> lookupRows = GetLookupsFromLookupSchema(lookupIds);

			List<LookupDefinition> defs = SelectValid(sysSchemaLookups.Concat(lookupRows), GetLookupDefinition);

			return defs;
		}

		/// <inheritdoc/>
		public List<LookupDefinition> GetLookupDefinitionsForValue(Entity lookupValue) {
			EntitySchema valueSchema = lookupValue.Schema;

			var allowedSSIds = GetAllowedLookupSysSchemaIds();
			EntityCollection sysSchemas = QueryEntities(SysSchemaName, allowedSSIds, esq => {
				esq.Filters.Add(esq.CreateFilterWithParameters(
					FilterComparisonType.Equal, "UId", valueSchema.UId));
			});

			var allowedIds = GetAllowedLookupIds();
			EntityCollection lookupRows = QueryEntities(LookupSchemaName, allowedIds, esq => {
				esq.Filters.Add(esq.CreateFilterWithParameters(
					FilterComparisonType.Equal, "SysEntitySchemaUId", valueSchema.UId));
			});

			var all = sysSchemas.Cast<Entity>().Concat(lookupRows.Cast<Entity>());
			return SelectValid(all, GetLookupDefinition);
		}

		/// <inheritdoc/>
		public List<LookupValueDefinition> GetLookupValueDefinitionsForLookups(IReadOnlyList<LookupDefinition> lookupDefinitions) {
			var lookupValueDefinitions = new List<LookupValueDefinition>();
			var processedSchemas = new HashSet<Guid>();

			foreach (LookupDefinition lookupDefinition in lookupDefinitions) {
				if (string.IsNullOrWhiteSpace(lookupDefinition.ValuesSchemaName) || lookupDefinition.ValuesSchemaUId.IsEmpty() || processedSchemas.Contains(lookupDefinition.ValuesSchemaUId)) {
					continue;
				}
				var values = SelectValid(QueryEntities(lookupDefinition.ValuesSchemaName, rowCount: EffectiveLimit), GetLookupValueDefinition);
				lookupValueDefinitions.AddRange(values);
				processedSchemas.Add(lookupDefinition.ValuesSchemaUId);
			}

			return lookupValueDefinitions;
		}

		#endregion

	}

	#endregion

}

