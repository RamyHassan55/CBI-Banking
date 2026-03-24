namespace CrtDigitalAdsApp.Models.Responses
{
	using System;
	using Newtonsoft.Json;

	/// <summary>
	/// Model of response, which contains properties of
	/// ad account which was retrieved from ad platform
	/// </summary>
	[Serializable]
	public class AdAccountResponse
	{

		#region Properties: Public

		/// <summary>
		/// Ad account status
		/// </summary>
		[JsonProperty("accountStatus")]
		public string AccountStatus { get; set; }

		/// <summary>
		/// Ad account url
		/// </summary>
		[JsonProperty("adAccountUrl")]
		public string AdAccountUrl { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of ad account
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Ad account identifier
		/// </summary>
		[JsonProperty("platformAdAccountId")]
		public string PlatformAdAccountId { get; set; }

		/// <summary>
		/// Platform name
		/// </summary>
		[JsonProperty("platformName")]
		public string PlatformName { get; set; }

        /// <summary>
        /// Platform user identifier
        /// </summary>
        public string  PlatformUserId { get; set; }

		#endregion

	}
}