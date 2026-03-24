namespace CrtGoogleAnalytics.API.Models
{
	using CrtGoogleAnalytics.API.Parameters;
	using System.Linq;

	/// <summary>
	/// Web session model in CrtGoogleAnalytics.
	/// </summary>
	public class GAWebSessionModel : GAWebEventModelBase
	{

		/// <summary>
		/// ISO 3166-1 alpha-2 country code.
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// City ID according to https://developers.google.com/google-ads/api/data/geotargets.
		/// </summary>
		public string CityId { get; set; }

		/// <summary>
		/// Standard country-language codes are comprised of two arguments separated by a dash.
		/// ISO 639-1 and ISO 3166-1.
		/// </summary>
		private string _languageCode;
		public string LanguageCode {
			get => _languageCode;
			set {
				if (string.IsNullOrWhiteSpace(value)) {
					_languageCode = string.Empty;
					return;
				}
				if (value.Contains('-')) {
					var parts = value.Split('-');
					_languageCode = $"{parts.First().ToLower()}-{parts.Last().ToUpper()}";
				} else {
					_languageCode = $"{value.ToLower()}-{value.ToUpper()}";
				}
			}
		}

		/// <summary>
		/// Referrer of the page or the empty value in case of direct navigation.
		/// </summary>
		public string PageReferrer { get; set; }

		/// <summary>
		/// A combination of the source and medium. Populated by utm parameters.
		/// </summary>
		private string _sourceMedium;
		public string SourceMedium {
			get => _sourceMedium;
			set {
				if (!string.IsNullOrWhiteSpace(value) && value.Contains('/')) {
					var parts = value.Split('/');
					Source = $"{parts.First().Trim()}";
					Medium = $"{parts.Last().Trim()}";
				}
				_sourceMedium = value;
			}
		}

		/// <summary>
		/// The name of the source. Populated by utm parameters.
		/// </summary>
		private string _source;
		public string Source {
			get => _source;
			set {
				if (value != GADataConstants.DirectValue) {
					_source = value;
				}
			}
		}

		/// <summary>
		/// The name of the medium. Populated by utm parameters.
		/// </summary
		private string _medium;
		public string Medium {
			get => _medium;
			set {
				if (value != GADataConstants.NoneValue) {
					_medium = value;
				}
			}
		}

		/// <summary>
		/// The name of the marketing campaign. Populated by utm_campaign URL parameter.
		/// </summary>
		private string _campaign;
		public string Campaign {
			get => _campaign;
			set {
				if (value != GADataConstants.DirectValue && value != GADataConstants.ReferralValue) {
					_campaign = value;
				}
			}
		}
	}
}