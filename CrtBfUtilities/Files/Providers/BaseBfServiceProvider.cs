namespace Creatio.ContentDesigner
{
	using Common.Logging;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Terrasoft.Core;
	using System.Net;
	using System.Net.Http.Headers;
	using Newtonsoft.Json;
	using System.Text;

	internal abstract class BaseBfServiceProvider
	{

		#region Fields: Private

		private HttpClient _httpClient;
		private ILog _logger;

		#endregion

		#region Properties: Protected

		protected abstract string LoggerName { get; }

		protected string BfRequestExceptionTemplate => $"{LoggerName}. Send request failed. " +
			"RequestUri: {0}, ResponseStatusCode: {1}, ResponseContent: {2}, Exception: {3}";

		#endregion

		#region Properties: Public

		public UserConnection UserConnection { get; set; }

		public ILog Logger {
			get => _logger ?? (_logger = LogManager.GetLogger(LoggerName));
			set => _logger = value;
		}

		public HttpClient HttpClient {
			get => _httpClient ?? (_httpClient = new HttpClient());
			set => _httpClient = value;
		}

		#endregion

		#region Methods: Protected

		protected void LogError(HttpRequestMessage httpRequestMessage, string errorString,
				HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, Exception exception = default) {
			Logger.Error(string.Format(BfRequestExceptionTemplate, httpRequestMessage.RequestUri, httpStatusCode, errorString,
				exception));
		}

		protected HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string requestUrl, string accessToken) {
			var httpRequestMessage = new HttpRequestMessage(method, requestUrl);
			if (!string.IsNullOrEmpty(accessToken)) {
				httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			}
			return httpRequestMessage;
		}

		protected HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string requestUrl, string accessToken,
				object requestBody) {
			var httpRequestMessage = GetHttpRequestMessage(method, requestUrl, accessToken);
			var stringBody = requestBody is string ? (string)requestBody : JsonConvert.SerializeObject(requestBody);
			httpRequestMessage.Content = new StringContent(stringBody, Encoding.UTF8, "application/json");
			return httpRequestMessage;
		}

		protected async Task<ResponseResult<T>> SendRequest<T>(HttpRequestMessage httpRequestMessage) {
			HttpResponseMessage response = null;
			try {
				response = await HttpClient.SendAsync(httpRequestMessage);
				if (!response.IsSuccessStatusCode) {
					var errorContentString = response?.Content.ReadAsStringAsync().Result;
					LogError(httpRequestMessage, errorContentString, response.StatusCode);
					return new ResponseResult<T> { Success = false };
				}
				var content = await response.Content.ReadAsStringAsync();
				var result = typeof(T).Name == "String"
					? (T)Convert.ChangeType(content, typeof(T))
					: JsonConvert.DeserializeObject<T>(content);
				return new ResponseResult<T> { Success = true, Content = result };
			} catch (Exception e) {
				LogError(httpRequestMessage, e.Message, exception: e);
				return new ResponseResult<T> { Success = false, Error = e.Message };
			}
		}

		#endregion

	}
}
