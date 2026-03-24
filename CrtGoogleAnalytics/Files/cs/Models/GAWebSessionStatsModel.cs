namespace CrtGoogleAnalytics.API.Models
{
	using CrtGoogleAnalytics.API.Parameters;

	#region Class: GAWebSessionStatsModel

	/// <summary>
	/// Class to represent session GA statistics model.
	/// </summary>
	public class GAWebSessionStatsModel : GAWebStatsModelBase
	{

		#region Properties: Public

		/// <summary>
		/// Metric for the user sessions count.
		/// </summary>
		public int SessionCount { get; set; }

		/// <summary>
		/// Session channel.
		/// </summary>
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
		/// Session source.
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

		#endregion

	}

	#endregion

}
