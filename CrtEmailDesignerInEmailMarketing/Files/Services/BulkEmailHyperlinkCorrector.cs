namespace CrtEmailDesignerInEmailMarketing.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Web;
	using AngleSharp;
	using AngleSharp.Dom;
	using Terrasoft.Configuration;

	#region Class: BulkEmailHyperlinkCorrector

	/// <inheritdoc cref="IBulkEmailHyperlinkCorrector"/>
	internal class BulkEmailHyperlinkCorrector : IBulkEmailHyperlinkCorrector
	{

		#region Constants: Private

		private const string ReplicaParameterName = "bpmreplica";
		private const string TrackIdParameterName = "bpmtrackid";

		#endregion

		#region Fields: Private

		private readonly Regex _fullUrlRegexp =
			new Regex(
				@"<a[^>]*?href\s*=\s*([""'])((http|ftp|https):\/\/.*?)([""'])[^>]*>([^<]*(?:(?!<\/a)<[^<]*)*)<\/a>",
				RegexOptions.Compiled);

		private readonly Regex _hrefContentRegexp = new Regex(@"([""'])(.*?)([""'])", RegexOptions.Compiled);

		private readonly Regex _hrefUrlRegexp =
			new Regex(
				@"<(?![^>]*\bxmlns:v=[""']urn:schemas-microsoft-com:vml[""'])[^>]*\bhref\s*=\s*[""']((http|ftp|https):\/\/.*?)([""'])([\w[,@?^=%&:\/~+#-])?",
				RegexOptions.Compiled);

		private readonly Regex _textContentRegex = new Regex(">([^<>]+)<", RegexOptions.Compiled);

		private readonly Regex _urlRegexp =
			new Regex(@"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@^=%~+#-])?",
				RegexOptions.Compiled);

		#endregion

		#region Methods: Private

		private List<HyperlinkData> GetDistinctHyperlinks(List<HyperlinkData> allLinks) {
			var distinctLinks = new List<HyperlinkData>();
			var uniqueUrls = new HashSet<string>();
			var trimmedList = allLinks.Where(link => link != null);
			foreach (HyperlinkData link in trimmedList) {
				if (uniqueUrls.Add(link.Url)) {
					distinctLinks.Add(link);
				}
			}
			return distinctLinks;
		}
		
		private HyperlinkData ExtractHyperlinkData(IElement element) {
			string href = element.GetAttribute("href")?.Trim();
			if (string.IsNullOrWhiteSpace(href)) {
				return null;
			}
			if (!href.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
			    !href.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
				return null;
			}
			string caption = element.GetAttribute("title")?.Trim();
			if (string.IsNullOrWhiteSpace(caption)) {
				caption = element.TextContent?.Trim();
			}
			if (string.IsNullOrWhiteSpace(caption)) {
				IElement altNode = element.QuerySelector("[alt]");
				caption = altNode?.GetAttribute("alt")?.Trim();
			}
			if (string.IsNullOrWhiteSpace(caption)) {
				caption = href;
			}
			return new HyperlinkData {
				Url = href,
				Caption = caption
			};
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="IBulkEmailHyperlinkCorrector.ApplyBpmParametersToHyperlinks"/>
		public string ApplyBpmParametersToHyperlinks(string htmlContent, int replicaIndex, int startTrackId) {
			int bpmTrackId = startTrackId;
			return _hrefUrlRegexp.Replace(htmlContent, href => {
				return _hrefContentRegexp.Replace(href.Value, match => {
					string url = match.Groups[2].Value;
					if (!_urlRegexp.IsMatch(url)) {
						return match.Value;
					}
					string correctedUrl;
					var hyperlinkParser = new HyperlinkParser(url);
					if (!hyperlinkParser.IsValid) {
						correctedUrl = url;
					} else {
						hyperlinkParser.SetQueryParameter(TrackIdParameterName, bpmTrackId++.ToString());
						hyperlinkParser.SetQueryParameter(ReplicaParameterName, replicaIndex.ToString());
						correctedUrl = hyperlinkParser.ToString();
					}
					return $"{match.Groups[1].Value}{correctedUrl}{match.Groups[3].Value}";
				});
			});
		}

		/// <inheritdoc cref="IBulkEmailHyperlinkCorrector.ExtractHyperlinks"/>
		public HyperlinkData[] ExtractHyperlinks(string htmlContent) {
			var config = Configuration.Default;
			var context = BrowsingContext.New(config);
			var document = context.OpenAsync(req => req.Content(htmlContent)).Result;
			var links = new List<HyperlinkData>();
			foreach (var a in document.QuerySelectorAll("a[href]")) {
				links.Add(ExtractHyperlinkData(a));
			}
			return GetDistinctHyperlinks(links).ToArray();
		}

		#endregion

	}

	#endregion
	
	#region Class: HyperlinkParser

	internal class HyperlinkParser
	{

		#region Fields: Private

		private readonly string _anchorPart;
		private readonly string _baseUrl;

		private readonly Match _match;
		private string _queryString;

		#endregion

		private static readonly Regex _urlSegmentsRegexp =
			new Regex(@"^((?:http(?:s)?|ftp):\/\/[^\?#]*)(?:\?)?((?:[^#]|(?<=\[)#|#(?=\]))*)?(?:#)?(.*)?$",
				RegexOptions.Compiled | RegexOptions.Multiline);

		#region Constructors: Public

		/// <summary>Initializes a new instance of the <see cref="HyperlinkParser"/> class.</summary>
		public HyperlinkParser(string originalUrl) {
			OriginalUrl = originalUrl;
			string decodedUrl = HttpUtility.HtmlDecode(originalUrl);
			_match = _urlSegmentsRegexp.Match(decodedUrl);
			if (!IsValid) {
				return;
			}
			_baseUrl = _match.Groups[1].Value;
			_queryString = _match.Groups.Count > 2 ? _match.Groups[2].Value : "";
			_anchorPart = _match.Groups.Count > 3 ? _match.Groups[3].Value : "";
		}

		#endregion

		#region Properties: Public

		public string OriginalUrl { get; }

		public bool IsValid => _match.Success && _match.Groups.Count >= 2;

		#endregion

		#region Methods: Private

		private Regex GetParameterRegex(string parameterName) {
			return new Regex(parameterName + "=([^&#]*)", RegexOptions.Singleline);
		}

		#endregion

		#region Methods: Public

		public void SetQueryParameter(string parameterName, string value) {
			Regex regex = GetParameterRegex(parameterName);
			if (!regex.IsMatch(_queryString)) {
				string delimiter = string.IsNullOrEmpty(_queryString) ? "" : "&";
				_queryString += $"{delimiter}{parameterName}={value}";
			} else {
				_queryString = regex.Replace(_queryString, $"{parameterName}={value}");
			}
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() {
			string anchorPart = string.IsNullOrWhiteSpace(_anchorPart) ? "" : $"#{_anchorPart}";
			return $"{_baseUrl}?{_queryString}{anchorPart}";
		}

		#endregion

	}
	
	#endregion

}
