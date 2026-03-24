namespace MatomoConnector.API
{

	#region Class: SiteDetails

	public class SiteDetails : BaseSiteDetails
	{
		public string name { get; set; }
		public string main_url { get; set; }
		public string ts_created { get; set; }
		public string ecommerce { get; set; }
		public string sitesearch { get; set; }
		public string sitesearch_keyword_parameters { get; set; }
		public string sitesearch_category_parameters { get; set; }
		public string timezone { get; set; }
		public string currency { get; set; }
		public string exclude_unknown_urls { get; set; }
		public string excluded_ips { get; set; }
		public string excluded_parameters { get; set; }
		public string excluded_user_agents { get; set; }
		public string group { get; set; }
		public string type { get; set; }
		public string keep_url_fragment { get; set; }
		public string timezone_name { get; set; }
		public string currency_name { get; set; }
	}

	#endregion

}
