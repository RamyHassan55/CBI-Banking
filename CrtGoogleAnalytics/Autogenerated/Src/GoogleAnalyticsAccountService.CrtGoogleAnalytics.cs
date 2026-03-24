namespace CrtGoogleAnalytics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Web;
	using Common.Logging;
	using CrtGoogleAnalytics.API.Interfaces;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.ServiceModelContract;
	using Terrasoft.Web.Common;

	[ServiceContract]
	public class GoogleAnalyticsAccountService : BaseService
	{

		#region Fields: Private

		private readonly ILog _logger = LogManager.GetLogger("GoogleAnalytics");

		#endregion

		#region Properties: Private

		private GoogleAnalyticsAccountRepositoty _accountRepository;
		public GoogleAnalyticsAccountRepositoty AccountRepository {
			get => _accountRepository ?? (_accountRepository = ClassFactory.Get<GoogleAnalyticsAccountRepositoty>(
				new ConstructorArgument("userConnection", UserConnection),
				new ConstructorArgument("logger", _logger)));
			set => _accountRepository = value;
		}

		private IGADataService _dataService;
		public IGADataService DataService {
			get => _dataService ?? (_dataService = ClassFactory.Get<IGADataService>(
					new ConstructorArgument("userConnection", UserConnection)));
			set => _dataService = value;
		}

		#endregion

		#region Methods: Private

		private IDictionary<string, Guid> GetConnectedAccounts() {
			var collection = AccountRepository.GetConnectedAccounts();
			var accounts = new Dictionary<string, Guid>();
			collection.ForEach(x => {
				var accountId = x.GetTypedColumnValue<string>("AccountId");
				if (!accounts.ContainsKey(accountId)) {
					accounts.Add(accountId, x.PrimaryColumnValue);
				}
			});
			return accounts;
		}

		private void UpdateStreams() {
			var accounts = GetConnectedAccounts();
			foreach (var account in accounts) {
				var accountDetails = DataService.GetAccountDetails(account.Key);
				foreach (var propertyData in accountDetails.Properties) {
					AccountRepository.SaveStreams(
						propertyData.DataStreams.Select(x => new WebAnalyticsStreamModel {
							Id = x.MeasurementId,
							Name = x.Name,
							DefaultUri = x.DefaultUri,
							StreamId = x.StreamId
						}),
						account.Value,
						new Dictionary<string, object> {
							{ "GAPropertyId", propertyData.Id }
						}
					);
				}
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Updates Google Analytics streams.
		/// </summary>
		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		public BaseResponse RefreshStreams() {
			try {
				UpdateStreams();
				return new BaseResponse {
					Success = true
				};
			} catch (Exception) {
				return new BaseResponse {
					Success = false
				};
			}
		}

		#endregion

	}
} 
