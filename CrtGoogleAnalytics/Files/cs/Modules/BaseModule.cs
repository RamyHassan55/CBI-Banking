namespace CrtGoogleAnalytics.API.Modules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using Terrasoft.Common.Json;

	#region Class: BaseModule

	internal abstract class BaseModule
	{

		#region Fields: Private

		/// <summary>
		/// URL address of the Google Analytics API service. E.g.: https://analytics.googleapis.com/v1alpha/runReport
		/// </summary>
		private readonly string _serviceUrl;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL.
		/// </summary>
		/// <param name="url">URL address of Google Analytics service starting from http(s)://</param>
		public BaseModule(string url) {
			_serviceUrl = url.TrimEnd('/');
		}

		#endregion

		#region Methods: Protected

		protected T GetData<T>(string authToken, string servicePath, object requestBody = null) {
			var requestUrl = $"{_serviceUrl}/{servicePath}";
			HttpClient _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			HttpResponseMessage response;
			if (requestBody != null) {
				var content = new StringContent(Json.Serialize(requestBody, true), Encoding.UTF8, "application/json");
				response = _httpClient.PostAsync(requestUrl, content).GetAwaiter().GetResult();
			} else {
				response = _httpClient.GetAsync(requestUrl).GetAwaiter().GetResult();
			}
			var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			if (!response.IsSuccessStatusCode) {
				throw new Exception($"Google Analytics API request failed: {responseContent}");
			}
			var responseData = Json.Deserialize<T>(responseContent);
			return responseData;
		}

		#endregion

	}

	#endregion

	#region Class: ModuleExtensions

	internal static class ModuleExtensions
	{

		#region Methods: Public

		public static T Get<T>(this List<BaseModule> modules) => modules.OfType<T>().FirstOrDefault();

		#endregion

	}

	#endregion

}
