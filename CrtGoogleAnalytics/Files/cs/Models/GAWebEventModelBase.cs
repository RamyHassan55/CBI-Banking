namespace CrtGoogleAnalytics.API.Models
{
	using System;
	using System.Linq;

	public abstract class GAWebEventModelBase : GAItemModelBase
	{

		#region Properties: Public

		/// <summary>
		/// Google client_id with session_id.
		/// </summary>
		private string _sessionStr;
		public string SessionStr {
			get => _sessionStr;
			set {
				(SessionId, ClientId, MeasurementId) = GetGaDetails(value);
				SessionStart = ConvertTimestampToDateTime(SessionId);
				_sessionStr = value;
			}
		}

		/// <summary>
		/// Google session_id.
		/// </summary>
		public string SessionId { get; private set; }

		/// <summary>
		/// Google client_id.
		/// </summary>
		public string ClientId { get; private set; }

		/// <summary>
		/// Google Measurement Id.
		/// </summary>
		public string MeasurementId { get; private set; }

		/// <summary>
		/// The session start date and time from the SessionId.
		/// </summary>
		public DateTime SessionStart { get; private set; }

		/// <summary>
		/// Returns visitor code to filter sessions.
		/// </summary>
		public string VisitorCode => $"{ClientId}:";

		#endregion

		#region Methods: Private

		private (string sessionId, string userId, string streamId) GetGaDetails(string sessionId) {
			var sessionStringParts = sessionId.Split(':');
			var gaSessionId = sessionStringParts.LastOrDefault();
			if (sessionStringParts.Count() < 2) {
				return (gaSessionId, null, null);
			}
			var gaClientId = sessionStringParts.FirstOrDefault();
			var gaStreamId = sessionStringParts[1];
			return (gaSessionId, gaClientId, gaStreamId);
		}

		private DateTime ConvertTimestampToDateTime(string timestamp) {
			try {
				var intTimestamp = Convert.ToInt32(timestamp);
				return DateTimeOffset.FromUnixTimeSeconds(intTimestamp).DateTime;
			} catch {
				return default;
			}
		}

		#endregion

	}
}
