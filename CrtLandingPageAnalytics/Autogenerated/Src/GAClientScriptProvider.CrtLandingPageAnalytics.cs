namespace CrtLandingPageAnalytics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CrtLandingPage;
	using CrtLandingPage.Dto;
	using CrtLandingPage.Parameters;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	[DefaultBinding(typeof(IClientScriptProvider), Name = "GoogleAnalytics")]
	public class GAClientScriptProvider : IClientScriptProvider
	{

		private readonly UserConnection _userConnection;
		private readonly string _clientScriptCdnUrl = "https://webtracking-v01.creatio.com/JS/";
		private readonly string _gaIntegrationScriptMarker = "GA Integration";
		private readonly Guid _googleAnalyticsPlatformId = new Guid("310174AE-1DF5-4BB1-8955-474548D52666");

		/// <summary>
		/// Initializes a new instance of the <see cref="GAClientScriptProvider"/> class.
		/// </summary>
		/// <param name="userConnection"></param>
		public GAClientScriptProvider(UserConnection userConnection) {
			_userConnection = userConnection;
		}

		private Entity GetAnalyticsStream(Guid landingId) {
			var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "WebAnalyticsStream");
			esq.AddAllSchemaColumns();
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"WebAnalyticsAccount.WebAnalyticsPlatform.Id", _googleAnalyticsPlatformId));
			esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal,
				"[LandingPage:AnalyticsStream].Id", landingId));
			return esq.GetEntityCollection(_userConnection).FirstOrDefault();
		}

		private string BuildIntegrationScript(string measurementId) {
			if (string.IsNullOrWhiteSpace(measurementId)) {
				return string.Empty;
			}
			//TODO: Separate the logic for ContactId from create-object.js
			return $"\t<script>window.TAG_ID='{measurementId}';</script>\r\n" +
				$"\t<script async src=\"https://www.googletagmanager.com/gtag/js?id={measurementId}\"></script>\r\n" +
				$"\t<script src=\"{_clientScriptCdnUrl}create-object.js\"></script>\r\n" +
				$"\t<script src=\"{_clientScriptCdnUrl}crt-gtag.js\"></script>\r\n" +
				$"\t<script src=\"{_clientScriptCdnUrl}crt-gtag-with-tracking-form-data.js\"></script>";
		}

		/// <summary>
		/// Builds Google Analytics integration script.
		/// </summary>
		/// <param name="landingId">The identifier of landing page.</param>
		/// <returns>The list of client script dtos.</returns>
		public IEnumerable<LandingClientScript> GetScripts(Guid landingId) {
			var stream = GetAnalyticsStream(landingId);
			var measurementId = stream?.GetTypedColumnValue<string>("MeasurementId");
			yield return new LandingClientScript(_gaIntegrationScriptMarker) {
				Position = ClientScriptPosition.HEAD_BEGIN,
				Content = BuildIntegrationScript(measurementId)
			};
		}
	}
}

