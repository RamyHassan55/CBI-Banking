namespace CrtGoogleAnalytics.API.Modules
{
	using CrtGoogleAnalytics.API.Dto;
	using System.Collections.Generic;
	using System.Linq;

	#region Class: AnalyticsModule

	internal class AdminModule : BaseModule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes module with URL.
		/// </summary>
		/// <param name="url">URL address of Google Analytics API service starting from http(s)://</param>
		public AdminModule(string url) : base(url) {
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns Google analytics accounts list.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// </summary>
		public IEnumerable<GAAccount> GetAccounts(string accessToken) {
			var servicePath = "accounts";
			var data = GetData<GAAccountsResponse>(accessToken, servicePath);
			var result = data?.Accounts;
			if (result == null) {
				return new List<GAAccount>();
			}
			foreach (var account in result) {
				account.Id = account.Id.Replace("accounts/", "");
			}
			return result;
		}

		/// <summary>
		/// Returns Google analytics properties by account identifier.
		/// <param name="accessToken">The access token to access Google analytics API</param>
		/// <param name="accountId">Google analytics account identifier</param>
		/// </summary>
		public IEnumerable<GAAccountProperty> GetProperties(string accessToken, string accountId) {
			var servicePath = $"properties?filter=parent:accounts/{accountId}";
			var data = GetData<GAPropertiesResponse>(accessToken, servicePath);
			var result = data?.Properties;
			if (result == null) {
				return new List<GAAccountProperty>();
			}
			foreach (var property in result) {
				property.Id = property.Id.Split('/').LastOrDefault();
			}
			return result;
		}

		public IEnumerable<GADataStream> GetDataStreams(string accessToken, string propertyId) {
			var servicePath = $"properties/{propertyId}/dataStreams";
			var data = GetData<GADataStreamsResponse>(accessToken, servicePath);
			var result = data?.DataStreams;
			if (result == null) {
				return new List<GADataStream>();
			}
			foreach (var stream in result) {
				stream.Id = stream.Id.Split('/').LastOrDefault();
			}
			return result;
		}
		#endregion

	}

	#endregion

}
