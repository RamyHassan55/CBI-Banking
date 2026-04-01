namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Configuration.Models;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	#region Class: HtmlWebPageRepository

	/// <summary>
	/// Class to represent repository for the web pages.
	/// </summary>
	public class HtmlWebPageRepository : WebPageRepository
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlWebPageRepository"/> class.
		/// </summary>
		/// <param name="userConnection">The user connection.</param>
		/// <param name="logger">The logger.</param>
		public HtmlWebPageRepository(UserConnection userConnection) : base(userConnection) { }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Gets all web pages from the database.
		/// </summary>
		/// <param name="landingIds">Identifiers of the Landing pages</param>
		public IEnumerable<WebPageModel> GetWebPages(IEnumerable<Guid> landingIds) {
			var select = GetWebPageSelect();
			select.InnerJoin("HTMLPage").As("HP").On("WP", "HTMLPageId").IsEqual("HP", "Id")
				.Where("HP", "LandingPageId").In(Column.Parameters(landingIds));
			return GetWebPageModels(select);
		}

		/// <summary>
		/// Creates record for new web page or updates existed record.
		/// </summary>
		/// <param name="pageTitle">Web page title</param>
		/// <param name="url">Url of landing page</param>
		/// <param name="htmlPageId">Identifier of the HTML page</param>
		public void CreateOrUpdatePage(string pageTitle, string url, Guid htmlPageId) {
			var webPage = GetOrCreatePage(pageTitle, url, new Dictionary<string, object> {
				{ "HTMLPageId", htmlPageId }
			});
			if (webPage.GetTypedColumnValue<Guid>("HTMLPageId") != htmlPageId) {
				webPage.SetColumnValue("HTMLPageId", htmlPageId);
				webPage.Save();
			}
		}

		#endregion

	}

	#endregion

}


