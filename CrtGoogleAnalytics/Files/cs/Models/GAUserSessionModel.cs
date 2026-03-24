namespace CrtGoogleAnalytics.API.Models
{
	using System;

	/// <summary>
	/// Class to represent user specified session model from GA.
	/// </summary>
	public class GAUserSessionModel : GAWebEventModelBase
	{
		private string _userId;

		/// <summary>
		/// User id which was tracked by the google analytics.
		/// </summary>
		public string UserId {
			get => _userId;
			set {
				_userId = value;
				if (Guid.TryParse(_userId, out var id)) {
					ContactId = id;
				}
			}
		}

		/// <summary>
		/// GA stream identifier.
		/// </summary>
		public string StreamId { get; set; }

		/// <summary>
		/// Creatio contact identifier.
		/// </summary>
		public Guid ContactId { get; set; }
	}
}
