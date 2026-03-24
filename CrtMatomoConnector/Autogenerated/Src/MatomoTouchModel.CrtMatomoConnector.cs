namespace Terrasoft.Configuration
{
	using Newtonsoft.Json;

	#region Class: MatomoTouchModel

	internal class MatomoTouchModel : TouchModel
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int MatomoSiteId { get; set; }
	}

	#endregion

}
