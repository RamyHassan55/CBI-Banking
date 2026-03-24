namespace CrtGoogleAnalytics.API.Models
{
	using System;
	using System.Globalization;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using CrtGoogleAnalytics.API.Parameters;
	using Terrasoft.Configuration;

	/// <summary>
	/// Web action model in CrtGoogleAnalytics.
	/// </summary>
	public class GAWebActionModel : GAWebEventModelBase
	{

		#region Properties: Public

		/// <summary>
		/// GoogleAnalytics datetime string in the yyyyMMddHHmm format.
		/// </summary>
		private string _dateTimeStr;
		public string DateTimeStr {
			get => _dateTimeStr;
			set {
				DateTime = ParseGADateTime(value);
				_dateTimeStr = value;
			}
		}

		/// <summary>
		/// Date and time of the action.
		/// </summary>
		public DateTime DateTime { get; private set; }

		/// <summary>
		/// The event's name.
		/// </summary>
		public string EventName { get; set; }

		/// <summary>
		/// URL of the page with protocol and parameters.
		/// </summary>
		public string FullPageUrl { get; set; }

		/// <summary>
		/// The title of the page.
		/// </summary>
		private string _pageTitle;
		public string PageTitle {
			get => string.IsNullOrWhiteSpace(_pageTitle) || _pageTitle == GADataConstants.NotSetValue
				? WebPageConstants.EmptyTitle
				: _pageTitle;
			set { _pageTitle = value; }
		}

		/// <summary>
		/// The number of the same events.
		/// </summary>
		public int EventsCount { get; set; }

		/// <summary>
		/// Hash string that identifies an action.
		/// </summary>
		public string ActionId => SHA1Hash($"{ClientId}{SessionId}{DateTimeStr}{EventName}{FullPageUrl}{PageTitle}");

		#endregion

		#region Methods: Private

		private DateTime ParseGADateTime(string gaDateTime) {
			var isStringParsed = DateTime.TryParseExact(gaDateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture,
				DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var result);
			return isStringParsed ? result : SessionStart;
		}

		private string SHA1Hash(string input) {
			var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Concat(hash.Select(b => b.ToString("x2")));
		}

		#endregion

	}
}