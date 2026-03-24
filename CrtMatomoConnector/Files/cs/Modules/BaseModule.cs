namespace MatomoConnector.API
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using Newtonsoft.Json;

	#region Class: BaseModule

	public abstract class BaseModule
	{

		#region Fields: Private

		/// <summary>
		/// URL address of the Matomo service. E.g.: https://account.matomo.cloud
		/// </summary>
		private readonly string _url;
		
		/// <summary>
		/// Auth token.
		/// </summary>
		private readonly string _apiKey = "anonymous";
		
		/// <summary>
		/// Number of records to return.
		/// </summary>
		private readonly int _filterLimit = 200;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL and AuthToken.
		/// </summary>
		/// <param name="url">URL address of matomo service starting from http(s)://</param>
		/// <param name="apiKey">Auth token.</param>
		public BaseModule(string url, string apiKey) {
			_url = url.TrimEnd('/');
			_apiKey = apiKey;
		}

		#endregion

		#region Methods: Private

		private string SendGetRequest(StringBuilder url) {
			var httpRequest = (HttpWebRequest)WebRequest.Create(url.ToString());
			var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
			var responseData = "";
			using (var streamResponse = httpResponse.GetResponseStream()) {
				using (var reader = new StreamReader(streamResponse, Encoding.GetEncoding("utf-8"))) {
					responseData = reader.ReadToEnd();
				}
			}
			httpResponse.Close();
			return responseData;
		}

		#endregion

		#region Methods: Protected

		protected abstract string GetModuleName();

		/// <summary>
		/// Sends GET request to specified URL and parses typed response.
		/// </summary>
		protected List<T> SendRequest<T>(string method, ParametersCollection parameters, int offset = 0) {
			if (string.IsNullOrEmpty(_url) || string.IsNullOrEmpty(_apiKey)) {
				throw new AuthException();
			}
			parameters.AddOnce(new SimpleParameter("token_auth", _apiKey));
			parameters.AddOnce(new SimpleParameter("method", GetModuleName() + "." + method));
			parameters.AddOrReplace(new SimpleParameter("filter_limit", _filterLimit));
			parameters.AddOrReplace(new SimpleParameter("filter_offset", offset));
			var url = new StringBuilder(_url);
			url.Append("/?module=API&format=json");
			url.Append(parameters.ToString());
			var responseData = SendGetRequest(url);
			if (string.IsNullOrEmpty(responseData)) {
				throw new EmptyResponseException(url.ToString());
			}
			List<T> result;
			try {
				result = JsonConvert.DeserializeObject<T[]>(responseData)?.ToList() ?? new List<T>();
			} catch (JsonSerializationException ex) {
				MatomoError errorDetails;
				try {
					errorDetails = JsonConvert.DeserializeObject<MatomoError>(responseData);
				} catch {
					throw ex;
				}
				throw new MatomoErrorException(errorDetails);
			}
			if (result.Count == _filterLimit) {
				var nextPage = SendRequest<T>(method, parameters, offset + _filterLimit);
				result.AddRange(nextPage);
			}
			return result;
		}

		/// <summary>
		/// Sends GET request to specified URL and parses typed response.
		/// </summary>
		protected List<T> SendRequest<T>(string method, Parameter parameter) =>
			SendRequest<T>(method, new ParametersCollection { parameter });

		#endregion

	}

	#endregion

	#region Class: ModuleExtensions

	public static class ModuleExtensions
	{

		#region Methods: Public

		public static T Get<T>(this List<BaseModule> modules) => modules.OfType<T>().FirstOrDefault();

		#endregion

	}

	#endregion

}
