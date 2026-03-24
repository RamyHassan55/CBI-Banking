namespace CrtGoogleAnalytics.API.Modules
{
	using System.Linq;
	using CrtGoogleAnalytics.API.Dto;
	using CrtGoogleAnalytics.API.Parameters;

	#region Class: AnalyticsModule

	internal class AnalyticsStatsModule : AnalyticsModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL.
		/// </summary>
		/// <param name="url">URL address of Google Analytics API service starting from http(s)://</param>
		public AnalyticsStatsModule(string url) : base(url) {
		}

		#endregion

		#region Methods: Private

		private GADimensionFilter CreateFilterGroupForStatsItem(StreamWebSite item) {
			return new GADimensionFilter {
				AndGroup = new FilterGroup {
					Expressions = new DataFilter[] { 
						new DataFilter {
							Filter = new Filter {
								FieldName = "streamId",
								StringFilter = new StringFilter {
									MatchType = StringFilterOperator.Exact,
									Value = item.StreamId
								}
							}
						},
						new DataFilter {
							Filter = new Filter {
								FieldName = "hostName",
								StringFilter = new StringFilter {
									MatchType = StringFilterOperator.Contains,
									Value = item.HostUrl
								}
							}
						}
					}
				}
			};
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Builds dimension filter.
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		protected override GADimensionFilter CreateDimensionFilter(GADataRequestParameters parameters) {
			var filters = parameters.StreamWebSites.Select(x => CreateFilterGroupForStatsItem(x));
			if (!filters.Any()) {
				return null;
			}
			if (filters.Count() > 1) {
				return new GADimensionFilter {
					OrGroup = new FilterGroup {
						Expressions = filters.ToArray()
					}
				};
			}
			return filters.FirstOrDefault();
		}

		#endregion

	}

	#endregion

}
