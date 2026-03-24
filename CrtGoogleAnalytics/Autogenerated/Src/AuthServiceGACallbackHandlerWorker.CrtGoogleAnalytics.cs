namespace CrtGoogleAnalytics.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtCloudIntegration.Services;
	using CrtGoogleAnalytics.API.Interfaces;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;


	#region Class: AuthServiceGACallbackHandlerWorker

	[DefaultBinding(typeof(IAuthServiceCallbackHandlerWorker), Name = "web_analytics")]
	public class AuthServiceGACallbackHandlerWorker : IAuthServiceCallbackHandlerWorker
	{

		#region Fields: Private

		private static readonly ILog Logger = LogManager.GetLogger("AuthServiceCallbackHandler");
		private readonly UserConnection _userConnection;
		private readonly GoogleAnalyticsAccountRepositoty _webAnalyticsAccountRepositoty;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthServiceCallbackHandlerWorker"/> class.
		/// </summary>
		public AuthServiceGACallbackHandlerWorker() {
			_userConnection = ClassFactory.Get<UserConnection>();
			_webAnalyticsAccountRepositoty = ClassFactory.Get<GoogleAnalyticsAccountRepositoty>(
				new ConstructorArgument("userConnection", _userConnection),
				new ConstructorArgument("logger", Logger)
			);
		}

		#endregion

		#region Methods: Private

		private void SaveAccountDetails(IGADataService dataService, string platformUserId) {
			var accounts = _webAnalyticsAccountRepositoty.GetAccountsForUser(platformUserId);
			foreach (var account in accounts) {
				var accountDetails = dataService.GetAccountDetails(account.Key);
				foreach (var propertyData in accountDetails.Properties) {
					_webAnalyticsAccountRepositoty.SaveStreams(
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
		/// Handles callback request for the specified platform.
		/// </summary>
		/// <param name="platform">The platform.</param>
		/// <param name="platformUserId">The platform user identifier.</param>
		/// <param name="application">The application.</param>
		public void Handle(string platform, string platformUserId, string application) {
			try {
				var analyticsDataService = ClassFactory.Get<IGADataService>(
					new ConstructorArgument("userConnection", _userConnection));
				var accountsList = analyticsDataService.GetAccountList(platformUserId);
				_webAnalyticsAccountRepositoty.SaveAccounts(accountsList, platformUserId);
				SaveAccountDetails(analyticsDataService, platformUserId);
			} catch (Exception e) {
				Logger.Error("AuthServiceCallbackHandlerWorker Handle failed. Exception:", e);
				return;
			}
		}


		#endregion

	}

	#endregion

}

