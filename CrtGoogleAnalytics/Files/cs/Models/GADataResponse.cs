namespace CrtGoogleAnalytics.API.Models
{
	using System.Collections.Generic;

	public class GADataResponse<T>
	{
		public IEnumerable<T> Events { get; set; }
		public GAPropertyQuota Quota { get; set; }
	}
}
