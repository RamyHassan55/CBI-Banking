namespace CrtDigitalAdsApp.Models.Requests
{
	using System.Runtime.Serialization;
	using Newtonsoft.Json;

	[DataContract]
    public class BaseDigitalAdsRequest {

        #region Properties: Public

        /// <summary>
        /// Application name
        /// </summary>
        [DataMember(Name = "application")]
        [JsonProperty("application")]
        public string Application { get; set; }

        /// <summary>
        /// Platform name
        /// </summary>
        [DataMember(Name = "platformName")]
        [JsonProperty("platformName")]
        public string PlatformName { get; set; }

        /// <summary>
        /// Platform user identifier
        /// </summary>
        [DataMember(Name = "platformUserId")]
        [JsonProperty("platformUserId")]
        public string PlatformUserId { get; set; }

        #endregion

    }
}