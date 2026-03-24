namespace MatomoConnector.API
{

	#region Class: UserDetails

	public class UserDetails
	{
		/// <summary>
		/// Last visitor's id but not the least.
		/// </summary>
		public string idvisitor { get; set; }

		/// <summary>
		/// UserId.
		/// </summary>
		public string label { get; set; }

		/// <summary>
		/// Filter to get user.
		/// </summary>
		public string segment { get; set; }

		public int nb_uniq_visitors { get; set; }
		public int nb_visits { get; set; }
		public int nb_actions { get; set; }
		public int nb_users { get; set; }
		public int max_actions { get; set; }
		public int sum_visit_length { get; set; }
		public int bounce_count { get; set; }
		public int nb_visits_converted { get; set; }
	}

	#endregion

}
