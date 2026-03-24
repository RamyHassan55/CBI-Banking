namespace CrtGoogleAnalytics.API.Models
{
	using System;
	using System.Globalization;
	using CrtGoogleAnalytics.API.Parameters;
	using Terrasoft.Configuration;


	#region Class: GAWebStatsModelBase
	/// <summary>
	/// Class to represent base GA stats model.
	/// </summary>
	public class GAWebStatsModelBase : GAItemModelBase
	{

		#region Properties: Public

		/// <summary>
		/// Google analytics stream id.
		/// </summary>
		public string StreamId { get; set; }

		/// <summary>
		/// Host url.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Page path URL segment.
		/// </summary>
		private string _pagePath;
		public string PagePath {
			get => _pagePath == "/" ? string.Empty : _pagePath;
			set { _pagePath = value; }
		}

		/// <summary>
		/// Calculated page URL.
		/// </summary>
		public string PageUrl {
			get => $"{Host}{PagePath}";
		}

		/// <summary>
		/// Title of the web page.
		/// </summary>
		private string _pageTitle;
		public string PageTitle {
			get => string.IsNullOrWhiteSpace(_pageTitle) || _pageTitle == GADataConstants.NotSetValue
				? WebPageConstants.EmptyTitle
				: _pageTitle;
			set { _pageTitle = value; }
		}

		/// <summary>
		/// Date string in format YYYYMMDD
		/// </summary>
		private string _dateStr;
		public string DateStr {
			get => _dateStr;
			set {
				Date = ParseGADate(value);
				_dateStr = value;
			}
		}

		/// <summary>
		/// Statistics date.
		/// </summary>
		public DateTime Date { get; private set; }

		#endregion

		#region Methods: Private

		private DateTime ParseGADate(string gaDate) {
			var isStringParsed = DateTime.TryParseExact(gaDate, "yyyyMMdd", CultureInfo.InvariantCulture,
				DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var result);
			return isStringParsed ? result : DateTime.MinValue;
		}

		#endregion

	}

	#endregion

}
