namespace MatomoConnector.API
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text.RegularExpressions;

	#region Class: MatomoServiceMock

	public class MatomoServiceMock : IMatomoService
	{

		#region Fields: Private

		/// <summary>
		/// Defines account settings like number of sites, users, their parameters etc.
		/// </summary>
		private string MockParameters = "";

		#endregion

		#region Constructors: Public

		public MatomoServiceMock(string apiUrl, string apiKey) {
			MockParameters = apiKey;
		}

		#endregion

		#region Methods: Private

		protected bool TrySetPropertyValue(VisitDetails visit, string propertyName, string value) {
			if (string.IsNullOrEmpty(propertyName)) {
				return false;
			}
			var propertyDescriptors = TypeDescriptor.GetProperties(visit);
			var propertyDescriptor = propertyDescriptors[propertyName];
			if (propertyDescriptor == null) {
				return false;
			}
			propertyDescriptor.SetValue(visit, value);
			return true;
		}

		private int FindIntParameter(string name) {
			var regex = new Regex(name + @"=(\d+);{0,1}");
			var match = regex.Match(MockParameters);
			var result = 0;
			if (match.Success && match.Groups.Count > 1) {
				int.TryParse(match.Groups[1].Value, out result);
			}
			return result;
		}

		private string GetValue(string[] parameters, int index) =>
			parameters.Length == 0 ? string.Empty : parameters[index % parameters.Length];

		private string[] FindStringParameters(string name) {
			var regex = new Regex(name + @"=([\w\-_. :\/?{#}=,]+);{0,1}");
			var match = regex.Match(MockParameters);
			if (match.Success && match.Groups.Count > 1) {
				return match.Groups[1].Value.Split(',');
			}
			return new string[] { };
		}

		private IEnumerable<UserDetails> GetUserDetails() {
			var result = new List<UserDetails>();
			var usersCount = FindIntParameter("users_count");
			for (var i = 0; i < usersCount; i++) {
				result.Add(new UserDetails {
					idvisitor = $"TestVisitor_{i}",
					label = $"TestUser_{i}"
				});
			}
			return result;
		}

		private int GetTimestamp() =>
			Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

		private IEnumerable<ActionDetails> GetActionDetails(int sessionNumber) {
			var result = new List<ActionDetails>();
			var action_url = FindStringParameters("action_url");
			var action_pagetitle = FindStringParameters("action_pagetitle");
			var actionsCount = FindIntParameter("actions_count");
			var actionType = FindStringParameters("action_type");
			var pageviewId = FindStringParameters("action_id");
			for (var i = 0; i < actionsCount; i++) {
				var viewId = GetValue(pageviewId, sessionNumber);
				viewId = string.IsNullOrWhiteSpace(viewId) ? i.ToString() + sessionNumber.ToString() : viewId;
				var action = new ActionDetails {
					url = GetValue(action_url, i),
					pageTitle = GetValue(action_pagetitle, i),
					idpageview = viewId,
					type = GetValue(actionType, i),
					timestamp = GetTimestamp() + (sessionNumber * actionsCount) + i
				};
				result.Add(action);
			}
			return result;
		}

		private IEnumerable<VisitDetails> GetVisitDetails(string segmentName, IEnumerable<string> ids) {
			var result = new List<VisitDetails>();
			if (!ids.Any()) {
				return default;
			}
			var props = typeof(VisitDetails)
				.GetProperties()
				.Where(x => x.PropertyType == typeof(string))
				.ToList();
			var usersCount = FindIntParameter("users_count");
			var sessionsCount = FindIntParameter("sessions_count");
			for (var j = 0; j < usersCount; j++) {
				for (var i = 0; i < sessionsCount; i++) {
					var visit = new VisitDetails {
						idVisit = $"TestSession_{j}_{i}",
						actionDetails = GetActionDetails(i).ToArray(),
						userId = $"TestUser_{j}",
						idSite = "0"
					};
					props.ForEach(x => {
						var stringParams = FindStringParameters(x.Name);
						if (stringParams.Any()) {
							var valueToSet = GetValue(stringParams, i);
							TrySetPropertyValue(visit, x.Name, valueToSet);
						}
					});
					switch (segmentName) {
						case "userId":
							visit.userId = GetValue(ids.ToArray(), j);
							break;
						case "visitorId":
							visit.visitorId = GetValue(ids.ToArray(), i);
							break;
					}
					result.Add(visit);
				}
			}
			return result;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns users list of size specified at the Matomo API key.
		/// E.g.: Call for an ApiKey="users_count=15languageCode=ru-ru" returns 15 users for any <paramref name="siteUrl"/>.
		/// </summary>
		public IEnumerable<UserDetails> GetUsersBySiteUrl(string siteUrl, MatomoPeriod period, MatomoDate date,
				MatomoSegment segment) => GetUserDetails();

		/// <summary>
		/// <see cref="IMatomoService.GetLastVisitDetails(string, string, DateTime, IEnumerable[string])"/>.
		/// </summary>
		public LastVisitDetails GetLastVisitDetails(string siteUrl, MatomoPeriod period, MatomoDate date,
				string segmentName, IEnumerable<string> visitorIds) {
			return GetLastVisitDetails(new List<int> { 0 }, period, date, segmentName, visitorIds);
		}

		/// <summary>
		/// <see cref="IMatomoService.GetLastVisitDetails(List[int], string, DateTime, IEnumerable[string])"/>.
		/// </summary>
		public LastVisitDetails GetLastVisitDetails(List<int> siteIds, MatomoPeriod period, MatomoDate date,
				string segmentName, IEnumerable<string> ids) {
			var result = new LastVisitDetails {
				VisitDetails = GetVisitDetails(segmentName, ids).ToArray()
			};
			return result;
		}

		/// <summary>
		/// Returns one record of a test site.
		/// </summary>
		public IEnumerable<SiteDetails> GetAllSites() =>
			new List<SiteDetails> {
				new SiteDetails {
					idsite = "0",
					name = "TestSite_0"
				}
			};

		#endregion

	}

	#endregion
}
