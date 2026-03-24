using Terrasoft.Configuration;

namespace MatomoConnector.API
{

	#region Class: ActionDetails

	public class ActionDetails
	{
		public string type { get; set; }
		public string url { get; set; }

		private string _pageTitle;
		public string pageTitle {
			get => string.IsNullOrWhiteSpace(_pageTitle) ? WebPageConstants.EmptyTitle : _pageTitle;
			set { _pageTitle = value; }
		}

		public string pageIdAction { get; set; }
		public string idpageview { get; set; }
		public string serverTimePretty { get; set; }
		public string pageId { get; set; }
		public string pageviewPosition { get; set; }
		public string title { get; set; }
		public string subtitle { get; set; }
		public string icon { get; set; }
		public string iconSVG { get; set; }
		public int timestamp { get; set; }
	}

	#endregion

}
