namespace MatomoConnector.API
{
	using System;
	using System.Collections.Generic;

	#region Class: UserIdModule

	/// <summary>
	/// Implementation of UserId module API.
	/// For more information, read https://developer.matomo.org/api-reference/reporting-api
	/// </summary>
	public class UserIdModule : BaseModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL and AuthToken.
		/// </summary>
		/// <param name="url">URL address of matomo service starting from http(s)://</param>
		/// <param name="apiKey">Auth token.</param>
		public UserIdModule(string url, string apiKey) : base(url, apiKey) {
		}

		#endregion

		#region Methods: Protected

		protected override string GetModuleName() => "UserId";

		#endregion

		#region Methods: Public

		/// <summary>
		/// Retrieves all UserDetails by the <paramref name="idSite"/> parameter.
		/// </summary>
		public List<UserDetails> GetUsers(int idSite, MatomoPeriod period, MatomoDate date, MatomoSegment segment) {
			var parameters = new ParametersCollection {
				new SimpleParameter("idSite", idSite),
				new SimpleParameter("period", period.ToString()),
				new DateParameter("date", date),
				new SegmentParameter(segment)
			};
			return SendRequest<UserDetails>("getUsers", parameters);
		}

		#endregion

	}

	#endregion

}
