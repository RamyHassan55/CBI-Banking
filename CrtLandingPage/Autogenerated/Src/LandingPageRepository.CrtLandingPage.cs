namespace CrtLandingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Interface: ILandingPageRepository

	internal interface ILandingPageRepository
	{
		#region Methods: Internal

		Entity GetHtmlPage(string slug);

		Entity GetLandingPageById(Guid id);

		IEnumerable<Entity> GetLandingHtmlPages(Guid langindId);

		#endregion

	}

	#endregion

	#region Class: LandingPageRepository

	[DefaultBinding(typeof(ILandingPageRepository))]
	internal class LandingPageRepository : ILandingPageRepository
	{

		#region Fields: Private

		private static readonly Guid _publishedStateUId = new Guid("d121222c-0a28-4ae5-8ea9-ed4a64496aa0");
		private readonly AppConnection _appConnection;

		#endregion

		#region Constructors: Public

		public LandingPageRepository(AppConnection appConnection) {
			_appConnection = appConnection;
		}

		#endregion

		#region Methods: Private

		private EntitySchemaQuery GetLandingEntityESQ() {
			UserConnection userConnection = _appConnection.SystemUserConnection;
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "LandingPage") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esq.AddAllSchemaColumns();
			return esq;
		}


		private EntitySchemaQuery GetHtmlPageEntityESQ() {
			UserConnection userConnection = _appConnection.SystemUserConnection;
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "HTMLPage") {
				PrimaryQueryColumn = {
					IsAlwaysSelect = true
				}
			};
			esq.AddAllSchemaColumns();
			return esq;
		}
		private Entity GetHtmlPageEntity(string slug) {
			UserConnection userConnection = _appConnection.SystemUserConnection;
			var esq = GetHtmlPageEntityESQ();
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "ContentSlug", slug));
			return esq.GetEntityCollection(userConnection).FirstOrDefault();
		}

		private Entity GetLandingEntity(Guid recordId) {
			UserConnection userConnection = _appConnection.SystemUserConnection;
			var esq = GetLandingEntityESQ();
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", recordId));
			return esq.GetEntityCollection(userConnection).FirstOrDefault();
		}

		private IEnumerable<Entity> GetHtmlPageCollection(Guid landingId) {
			UserConnection userConnection = _appConnection.SystemUserConnection;
			var esq = GetHtmlPageEntityESQ();
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "LandingPage", landingId));
			return esq.GetEntityCollection(userConnection);
		}

		private EntityCollection GetLandingHtmlPagesEntityCollection(Guid landingRecordId) {
			// TODO: #RND-67310
			throw new NotImplementedException();
		}

		#endregion

		#region Methods: Public

		public Entity GetHtmlPage(string slug) {
			return GetHtmlPageEntity(slug);
		}

		public Entity GetLandingPageById(Guid recordId) {
			return GetLandingEntity(recordId);
		}

		public IEnumerable<Entity> GetLandingHtmlPages(Guid langindId) {
			return GetHtmlPageCollection(langindId);
		}

		#endregion

	}

	#endregion

}
