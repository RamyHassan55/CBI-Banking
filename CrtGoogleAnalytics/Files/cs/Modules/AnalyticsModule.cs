namespace CrtGoogleAnalytics.API.Modules
{
	using CrtGoogleAnalytics.API.Dto;
	using CrtGoogleAnalytics.API.Parameters;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Common;

	#region Class: AnalyticsModule

	internal class AnalyticsModule : BaseModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL.
		/// </summary>
		/// <param name="url">URL address of Google Analytics API service starting from http(s)://</param>
		public AnalyticsModule(string url) : base(url) {
		}

		#endregion

		#region Methods: Private

		private List<DataFilter> CreateEventFilter(EventFilterCondition condition) {
			var filters = new List<DataFilter>();
			switch (condition) {
				case EventFilterCondition.ONLY_ACTIONS:
					filters.Add(new DataFilter {
						NotExpression = new DataFilter {
							Filter = new Filter {
								FieldName = "eventName",
								StringFilter = new StringFilter {
									MatchType = StringFilterOperator.Exact,
									Value = "session_start"
								}
							}
						}
					});
					filters.Add(new DataFilter {
						NotExpression = new DataFilter {
							Filter = new Filter {
								FieldName = "eventName",
								StringFilter = new StringFilter {
									MatchType = StringFilterOperator.Exact,
									Value = "user_engagement"
								}
							}
						}
					});
					break;
				case EventFilterCondition.ALL:
				default:
					break;
			}
			return filters;
		}

		private DataFilter CreateUserFilter(string userId) {
			if (userId == null) {
				return null;
			}
			return new DataFilter {
				Filter = new Filter {
					FieldName = "customEvent:crt_session_id",
					StringFilter = new StringFilter {
						MatchType = StringFilterOperator.BeginsWith,
						Value = $"{userId}:"
					}
				}
			};
		}

		private DataFilter CreateContactFilter(string contactId) {
			if (contactId == null) {
				return null;
			}
			return new DataFilter {
				Filter = new Filter {
					FieldName = "customEvent:crt_user_id",
					StringFilter = new StringFilter {
						MatchType = StringFilterOperator.Exact,
						Value = contactId
					}
				}
			};
		}

		private DataFilter CreateSessionsFilter(string[] sessions) {
			if (sessions == null) {
				return null;
			}
			return new DataFilter {
				Filter = new Filter {
					FieldName = "customEvent:crt_session_id",
					InListFilter = new InListFilter {
						Values = sessions
					}
				}
			};
		}

		private DataFilter CreateUsersFilter(string[] users) {
			if (users == null) {
				return null;
			}
			var filters = new List<DataFilter>() { };
			users.Where(x => x.IsNotNullOrEmpty()).ForEach(u => {
				filters.Add(CreateUserFilter(u));
			});
			return new GADimensionFilter {
				OrGroup = new FilterGroup {
					Expressions = filters.ToArray()
				}
			};
		}

		private DataFilter CreateStreamFilter(string streamId) {
			if (string.IsNullOrWhiteSpace(streamId)) {
				return null;
			}
			return new DataFilter {
				Filter = new Filter {
					FieldName = "streamId",
					StringFilter = new StringFilter {
						MatchType = StringFilterOperator.Exact,
						Value = streamId
					}
				}
			};
		}

		private OrderBy[] CreateOrderBy() {
			return new OrderBy[] {
				new OrderBy {
					Dimension = new OrderDimension {
						DimensionName = "dateHourMinute",
						OrderType = OrderType.Numeric
					}
				}
			};
		}

		private GADataRequest CreateRequestBody(GADataRequestParameters parameters) {
			var dimensionsList = new List<Dimension>();
			var metricsList = new List<Dimension>();
			foreach (var dimension in parameters.Dimensions) {
				dimensionsList.Add(new Dimension { Name = dimension });
			}
			if (parameters.Metrics != null) {
				foreach (var metric in parameters.Metrics) {
					metricsList.Add(new Dimension { Name = metric });
				}
			}
			var startDate = GetStartDate(parameters);
			var endDate = GetEndDate(parameters);
			var request = new GADataRequest {
				Dimensions = dimensionsList.ToArray(),
				Metrics = metricsList.Any() ? metricsList.ToArray() : null,
				DateRanges = new[] {
					new DateRange {
						StartDate = startDate,
						EndDate = endDate
					}
				},
				DimensionFilter = CreateDimensionFilter(parameters),
				Limit = parameters.Limit,
				Offset = parameters.Offset
			};
			return request;
		}

		#endregion

		#region Methods: Protected

		protected virtual string GetStartDate(GADataRequestParameters parameters) {
			return parameters.FromDate == default
				? $"{parameters.PeriodDays}daysAgo"
				: parameters.FromDate.ToString("yyyy-MM-dd");
		}

		protected virtual string GetEndDate(GADataRequestParameters parameters) {
			return "today";
		}

		/// <summary>
		/// Builds dimension filter.
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		protected virtual GADimensionFilter CreateDimensionFilter(GADataRequestParameters parameters) {
			var filters = new List<DataFilter>() {
				CreateUserFilter(parameters.UserId),
				CreateContactFilter(parameters.ContactId),
				CreateSessionsFilter(parameters.Sessions),
				CreateUsersFilter(parameters.Users),
				CreateStreamFilter(parameters.StreamId)
			}.Where(f => f != null).ToList();
			filters.AddRange(CreateEventFilter(parameters.EventFilter));
			if (filters.Count() > 1) {
				return new GADimensionFilter {
					AndGroup = new FilterGroup {
						Expressions = filters.ToArray()
					}
				};
			}
			if (filters.Any()) {
				var group = filters.FirstOrDefault() as GADimensionFilter;
				if (group != null) {
					return group;
				}
				return new GADimensionFilter(filters.FirstOrDefault());
			}
			return null;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns analytics data.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="parameters">Request parameters</param>
		/// </summary>
		public GADataResponse GetData(string accessToken, GADataRequestParameters parameters) {
			var servicePath = $"properties/{parameters.PropertyId}:runReport";
			var requestBody = CreateRequestBody(parameters);
			var usedDimensions = requestBody.GetUsedDimensions();
			if (usedDimensions.Count() > 9) {
				throw new Exception($"Requests are limited to 9 dimensions." +
					$" This request is for {usedDimensions.Count()} dimensions.");
			}
			return GetData<GADataResponse>(accessToken, servicePath, requestBody);
		}

		#endregion

	}

	#endregion

}
