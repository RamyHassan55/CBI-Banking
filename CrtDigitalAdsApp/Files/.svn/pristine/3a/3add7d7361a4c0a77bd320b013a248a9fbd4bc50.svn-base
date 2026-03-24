namespace CrtDigitalAdsApp.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Provides the data about AdPlatforms.
	/// </summary>
	public interface IAdPlatformRepository
	{

        #region Methods: Public

		/// <summary>
		/// Gets the existing ad platforms.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <returns>Dictionary, where key is id of ad platform and value is its name</returns>
        Dictionary<Guid, string> GetExistingPlatforms(UserConnection userConnection);

		/// <summary>
		/// Gets the platform identifier.
		/// </summary>
		/// <param name="platform">The platform.</param>
		/// <param name="userConnection">The user connection.</param>
		/// <returns>Id of platform by name.</returns>
        Guid GetPlatformId(string platform, UserConnection userConnection);

		/// <summary>
		/// Gets the name of the platform by Id.
		/// </summary>
		/// <param name="platformId">The platform identifier.</param>
		/// <param name="userConnection">The user connection.</param>
		/// <returns>Name of platform by id.</returns>
        string GetPlatformName(Guid platformId, UserConnection userConnection);

		#endregion

	}

	/// <summary>
	/// Provides the data about AdPlatforms.
	/// </summary>
	/// <seealso cref="IAdPlatformRepository" />
	[DefaultBinding(typeof(IAdPlatformRepository))]
	internal class AdPlatformRepository: IAdPlatformRepository
    {

		#region Fields: Private

		private static Dictionary<Guid, string> _platformNames = new Dictionary<Guid, string>();

        #endregion

        #region Methods: Public

        /// <summary>
        /// Gets the existing ad platforms.
        /// </summary>
        /// <param name="userConnection">The user connection.</param>
        /// <returns>Dictionary, where key is id of ad platform and value is its name.</returns>
        public Dictionary<Guid, string> GetExistingPlatforms(UserConnection userConnection) {
			var esqResult = new EntitySchemaQuery(userConnection.EntitySchemaManager, "AdPlatform") {
				PrimaryQueryColumn = { IsVisible = true }
			};
			esqResult.AddColumn("Id");
			esqResult.AddColumn("Name");
			var entities = esqResult.GetEntityCollection(userConnection);
			var resultDictionary = new Dictionary<Guid, string>();
			foreach (var entity in entities) {
				var id = entity.GetTypedColumnValue<Guid>("Id");
				var name = entity.GetTypedColumnValue<string>("Name");
				resultDictionary[id] = name;
			}
			return resultDictionary;
		}

		/// <summary>
		/// Gets the platform identifier.
		/// </summary>
		/// <param name="platform">The platform.</param>
		/// <param name="userConnection">The user connection.</param>
		/// <returns>Id of platform by name.</returns>
		public Guid GetPlatformId(string platform, UserConnection userConnection) {
			if (string.IsNullOrEmpty(platform)) {
				throw new ArgumentException("Platform is null or empty");
			}
			if (!_platformNames.Any()) {
				_platformNames = GetExistingPlatforms(userConnection);
			}
			return _platformNames.Any(x => string.Equals(x.Value, platform, StringComparison.InvariantCultureIgnoreCase))
				? _platformNames.FirstOrDefault(x => 
					string.Equals(x.Value, platform, StringComparison.InvariantCultureIgnoreCase)).Key
				: Guid.Empty;
		}

		/// <summary>
		/// Gets the name of the platform by Id.
		/// </summary>
		/// <param name="platformId">The platform identifier.</param>
		/// <param name="userConnection">The user connection.</param>
		/// <returns>Name of platform by id.</returns>
		public string GetPlatformName(Guid platformId, UserConnection userConnection) {
			if (!_platformNames.Any()) {
				_platformNames = GetExistingPlatforms(userConnection);
			}
			return !_platformNames.ContainsKey(platformId) ? string.Empty : _platformNames[platformId];
		}

		#endregion

	}
}