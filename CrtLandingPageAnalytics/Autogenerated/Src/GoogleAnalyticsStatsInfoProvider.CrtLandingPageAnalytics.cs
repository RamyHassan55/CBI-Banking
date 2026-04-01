namespace CrtLandingPageAnalytics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	#region Class: GoogleAnalyticsStatsInfoProvider

	/// <summary>
	/// Class to represent Google Analytics aggregated data provider.
	/// </summary>
	public class GoogleAnalyticsStatsInfoProvider
	{
		#region Fields: Private

		private readonly UserConnection _userConnection;
		private readonly Guid _publishedStateId = Guid.Parse("D121222C-0A28-4AE5-8EA9-ED4A64496AA0");
		private readonly Guid _accountConnectedStatusId = new Guid("51B41366-E59D-4891-AF51-35E1D762B382");

		#endregion

		#region Constructors: Public

		public GoogleAnalyticsStatsInfoProvider(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		#endregion

		#region Methods: Private

		private Select GetLastStatsForConnectedStreamsSelect(string statsName) {
			var select = new Select(_userConnection)
				.Column(Func.Max("STATS", "Date")).As("Date")
				.Column("WAA", "AccountId").As("AccountId")
				.Column("WAS", "GAPropertyId").As("GAPropertyId")
				.Column("WAS", "StreamId").As("StreamId")
				.Column("WAS", "Id").As("WebAnalyticsStreamId")
				.Column("LP", "Id").As("LandingPageId")
				.Column("LP", "Url").As("Url")
				.From("LandingPage").As("LP")
				.InnerJoin("WebAnalyticsStream").As("WAS")
					.On("WAS", "Id").IsEqual("LP", "AnalyticsStreamId")
				.InnerJoin("WebAnalyticsAccount").As("WAA")
					.On("WAA", "Id").IsEqual("WAS", "WebAnalyticsAccountId")
				.InnerJoin("HTMLPage").As("HP")
					.On("LP", "Id").IsEqual("HP", "LandingPageId")
				.InnerJoin("WebPage").As("WP")
					.On("WP", "HTMLPageId").IsEqual("HP", "Id")
				.LeftOuterJoin(statsName).As("STATS")
					.On("STATS", "WebPageId").IsEqual("WP", "Id")
						.And("STATS", "StreamId").IsEqual("LP", "AnalyticsStreamId")
				.Where("HP", "PublicationStateId").IsEqual(Column.Parameter(_publishedStateId))
					.And("WAA", "ConnectionStatusId").IsEqual(Column.Parameter(_accountConnectedStatusId))
				.GroupBy("WAA", "AccountId")
				.GroupBy("WAS", "GAPropertyId")
				.GroupBy("WAS", "StreamId")
				.GroupBy("WAS", "Id")
				.GroupBy("LP", "Id")
				.GroupBy("LP", "Url") as Select;
			select.SpecifyNoLockHints();
			return select;
		}

		private IEnumerable<GoogleAnalyticsLandingStreamDto> GetAnalyticsStats(Select select) {
			var result = new List<GoogleAnalyticsLandingStreamDto>();
			select.ExecuteReader(reader => {
				var dto = new GoogleAnalyticsLandingStreamDto {
					StreamId = reader.GetColumnValue<Guid>("WebAnalyticsStreamId"),
					GAStreamId = reader.GetColumnValue<string>("StreamId"),
					AccountId = reader.GetColumnValue<string>("AccountId"),
					PropertyId = reader.GetColumnValue<string>("GAPropertyId"),
					LandingPageUrl = reader.GetColumnValue<string>("Url"),
					Date = reader.GetColumnValue<DateTime>("Date"),
					LandingPageId = reader.GetColumnValue<Guid>("LandingPageId")
				};
				result.Add(dto);
			});
			return result;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns the last stream analytics date for all connected landing pages.
		/// </summary>
		/// <param name="statsName">Name of statistics data set</param>
		public IEnumerable<GoogleAnalyticsLandingStreamDto> GetLastAnalyticsStats(string statsName) {
			Select select = GetLastStatsForConnectedStreamsSelect(statsName);
			return GetAnalyticsStats(select);
		}

		/// <summary>
		/// Returns the last stream analytics date for a specific landing page.
		/// </summary>
		/// <param name="statsName">Name of statistics data set</param>
		/// <param name="landingPageId">Identifier of the landing page</param>
		/// <param name="streamId">Identifier of the web analytics stream</param>
		public GoogleAnalyticsLandingStreamDto GetLastAnalyticsStats(string statsName, Guid landingPageId,
				Guid streamId) {
			Select select = GetLastStatsForConnectedStreamsSelect(statsName);
			select.And("LP", "Id").IsEqual(Column.Parameter(landingPageId))
				.And("LP", "AnalyticsStreamId").IsEqual(Column.Parameter(streamId));
			var result = GetAnalyticsStats(select);
			return result.FirstOrDefault();
		}

		#endregion

	}

	#endregion

}

