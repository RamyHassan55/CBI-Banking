namespace CrtGoogleAnalytics
{
	using System;
	using Newtonsoft.Json;
	using Terrasoft.Common.Json;

	#region Class: GAVisitorModel

	/// <summary>
	/// Class to represent Google Analytics visitor model.
	/// </summary>
	public class GAVisitorModel
	{

		#region Properties: Public

		[JsonIgnore]
		public DateTime LastActionDateTime { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string VisitorId { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Guid WebAnalyticsVisitorId { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Guid StreamId { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string AccountId { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string GAPropertyId { get; set; }

		#endregion

		#region Methods: Publlic

		/// <summary>
		/// Compares current instance with another one specified.
		/// </summary>
		/// <param name="other">Instance of the other <see cref="GAVisitorModel"/> to compare.</param>
		/// <returns>Comparison result.</returns>
		public virtual bool Equals(GAVisitorModel other) => WebAnalyticsVisitorId.Equals(other.WebAnalyticsVisitorId)
			&& LastActionDateTime.Equals(other.LastActionDateTime)
			&& StreamId.Equals(other.StreamId)
			&& (VisitorId == null
				? other.VisitorId == null
				: VisitorId.Equals(other.VisitorId))
			&& (GAPropertyId == null
				? other.GAPropertyId == null
				: GAPropertyId.Equals(other.GAPropertyId));

		/// <inheritdoc/>
		public override string ToString() {
			return Json.Serialize(this, true);
		}

		#endregion

	}

	#endregion

}
