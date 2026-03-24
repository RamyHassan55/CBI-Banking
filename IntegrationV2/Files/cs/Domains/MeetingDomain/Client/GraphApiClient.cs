namespace IntegrationV2.Files.cs.Domains.MeetingDomain.Client
{
	using System;
	using System.Text;
	using System.Threading.Tasks;
	using System.IO;
	using System.Net;
	using System.Web;
	using IntegrationV2.Files.cs.Domains.MeetingDomain.Logger;
	using IntegrationV2.Files.cs.Domains.MeetingDomain.Model;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Social.OAuth;

	#region Class: GraphApiClient
	/// <summary>
	/// PoC implementation of graph API usage.
	/// </summary>
	public class GraphApiClient
	{

		#region Fields: Private

		/// <summary>
		/// Service account token.
		/// </summary
		private readonly string _serviceAccountToken;

		/// <summary>
		/// Current office 365 user name.
		/// </summary>
		private readonly string _userName;

		/// <summary>
		/// <see cref="ICalendarLogger"/> implementation instance.
		/// </summary
		private readonly ICalendarLogger _log;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// .ctror.
		/// </summary>
		/// <param name="calendar"><see cref="CalendarSettings"/> instance.</param>
		/// <param name="log"><see cref="ICalendarLogger"/> implementation instance.</param>
		public GraphApiClient(CalendarSettings calendarSettings, ICalendarLogger log) {
			_serviceAccountToken = calendarSettings.ServiceAccountToken;
			_userName = calendarSettings.Login;
			_log = log;
			ValidateInstance();
		}

		#endregion

		#region Methods: Private

		private void ValidateInstance() {
			if (_serviceAccountToken.IsNotNullOrEmpty()
					&& _userName.IsNotNullOrEmpty()) {
				return;
			}
			throw new InvalidObjectStateException("OAuth parameters are not filled for Graph API. " +
				"Check OAuthApplication ClientId, SecretKey and TenantId columns.");
		}

		private async Task<string> SendRequest(string serviceUri, string method, string contentType, byte[] data,
				string token = null) {
			var requestFactory = ClassFactory.Get<IHttpWebRequestFactory>();
			WebRequest request = requestFactory.Create(serviceUri);
			request.Method = method;
			request.ContentType = contentType;
			if (token != null) {
				request.Headers.Add("Authorization", $"Bearer {token}");
			}
			request.Timeout = 5 * 60 * 1000;
			request.ContentLength = data.Length;
			WebResponse response = null;
			try {
				using (Stream stream = request.GetRequestStream()) {
					stream.Write(data, 0, data.Length);
				}
				response = request.GetResponse();
				using (Stream dataStream = response.GetResponseStream()) {
					StreamReader reader = new StreamReader(dataStream);
					return await reader.ReadToEndAsync();
				}
			} catch (Exception ex) {
				var logMessage = $"Error calling {serviceUri} (method {method}).";
				var responseContent = ex.GetExceptionContent();
				if (responseContent.IsNotNullOrEmpty()) {
					logMessage += $"\r\nResponse content '{responseContent}'";
				}
				_log?.LogError(logMessage, ex);
				throw;
			}
			finally {
				response?.Close();
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Converts meeting with <paramref name="meetingId"/> to Teems meeting in current user calendar.
		/// </summary>
		/// <param name="meetingId">Exchange appointment unique identifier.</param>
		/// <returns>Updated meeting in JSON format.</returns>
		public async Task<string> ConvertToTeamsMeeting(string meetingId) {
			var serviceUri = $"https://graph.microsoft.com/v1.0/users/{_userName}/calendar/events/" +
				$"{HttpUtility.UrlEncode(HttpUtility.UrlEncode(meetingId))}";
			var data = Encoding.UTF8.GetBytes("{\"isOnlineMeeting\": true}");
			return await SendRequest(serviceUri, "PATCH", "application/json; charset=utf-8", data, _serviceAccountToken);
		}

		#endregion

	}

	#endregion

}
