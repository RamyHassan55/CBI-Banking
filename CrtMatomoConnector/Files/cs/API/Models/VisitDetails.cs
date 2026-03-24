namespace MatomoConnector.API
{

	#region Class: VisitDetails

	public class VisitDetails
	{
		public string idSite { get; set; }
		public string userId { get; set; }
		public string idVisit { get; set; }
		public string visitorId { get; set; }
		public string visitIp { get; set; }
		public string fingerprint { get; set; }
		public ActionDetails[] actionDetails { get; set; }
		public int goalConversions { get; set; }
		public string siteCurrency { get; set; }
		public string siteCurrencySymbol { get; set; }
		public string serverDate { get; set; }
		public string visitServerHour { get; set; }
		public int lastActionTimestamp { get; set; }
		public string lastActionDateTime { get; set; }
		public string siteName { get; set; }
		public int serverTimestamp { get; set; }
		public int firstActionTimestamp { get; set; }
		public string serverTimePretty { get; set; }
		public string serverDatePretty { get; set; }
		public string serverDatePrettyFirstAction { get; set; }
		public string serverTimePrettyFirstAction { get; set; }
		public string visitorType { get; set; }
		public string visitorTypeIcon { get; set; }
		public string visitConverted { get; set; }
		public object visitConvertedIcon { get; set; }
		public string visitCount { get; set; }
		public string visitEcommerceStatus { get; set; }
		public object visitEcommerceStatusIcon { get; set; }
		public int daysSinceFirstVisit { get; set; }
		public string secondsSinceFirstVisit { get; set; }
		public int daysSinceLastEcommerceOrder { get; set; }
		public object secondsSinceLastEcommerceOrder { get; set; }
		public string visitDuration { get; set; }
		public string visitDurationPretty { get; set; }
		public string searches { get; set; }
		public string actions { get; set; }
		public string interactions { get; set; }
		public string referrerType { get; set; }
		public string referrerTypeName { get; set; }
		public string referrerName { get; set; }
		public string referrerKeyword { get; set; }
		public object referrerKeywordPosition { get; set; }
		public string referrerUrl { get; set; }
		public object referrerSearchEngineUrl { get; set; }
		public object referrerSearchEngineIcon { get; set; }
		public object referrerSocialNetworkUrl { get; set; }
		public object referrerSocialNetworkIcon { get; set; }
		public string languageCode { get; set; }
		public string language { get; set; }
		public string deviceType { get; set; }
		public string deviceTypeIcon { get; set; }
		public string deviceBrand { get; set; }
		public string deviceModel { get; set; }
		public string operatingSystem { get; set; }
		public string operatingSystemName { get; set; }
		public string operatingSystemIcon { get; set; }
		public string operatingSystemCode { get; set; }
		public string operatingSystemVersion { get; set; }
		public string browserFamily { get; set; }
		public string browserFamilyDescription { get; set; }
		public string browser { get; set; }
		public string browserName { get; set; }
		public string browserIcon { get; set; }
		public string browserCode { get; set; }
		public string browserVersion { get; set; }
		public string totalEcommerceRevenue { get; set; }
		public string totalEcommerceConversions { get; set; }
		public string totalEcommerceItems { get; set; }
		public string totalAbandonedCartsRevenue { get; set; }
		public string totalAbandonedCarts { get; set; }
		public string totalAbandonedCartsItems { get; set; }
		public string events { get; set; }
		public string continent { get; set; }
		public string continentCode { get; set; }
		public string country { get; set; }
		public string countryCode { get; set; }
		public string countryFlag { get; set; }
		public string region { get; set; }
		public string regionCode { get; set; }
		public string city { get; set; }
		public string location { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public string visitLocalTime { get; set; }
		public string visitLocalHour { get; set; }
		public int daysSinceLastVisit { get; set; }
		public string secondsSinceLastVisit { get; set; }
		public string resolution { get; set; }
		public string plugins { get; set; }
		public PluginsIcon[] pluginsIcons { get; set; }
		public object[] experiments { get; set; }
		public int formConversions { get; set; }
		public object sessionReplayUrl { get; set; }
		public string campaignId { get; set; }
		public string campaignContent { get; set; }
		public string campaignKeyword { get; set; }
		public string campaignMedium { get; set; }
		public string campaignName { get; set; }
		public string campaignSource { get; set; }
		public string campaignGroup { get; set; }
		public string campaignPlacement { get; set; }
	}

	#endregion

}
