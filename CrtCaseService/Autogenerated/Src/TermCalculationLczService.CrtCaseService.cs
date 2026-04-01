namespace Terrasoft.Configuration
{
	using Terrasoft.Core;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using Terrasoft.Web.Common;
	using Terrasoft.Common;
	using System.Linq;

	#region Class: TermCalculationLczService

	/// <summary>
	/// Service for loading localizable data for term calculation description.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class TermCalculationLczService : BaseService
	{

		#region Methods: Private

		private Dictionary<string, string> GetCaseSchemaResources() {
			var columnCaptions = new Dictionary<string, string>();
			var caseSchema = UserConnection.EntitySchemaManager.GetInstanceByName("Case");
			caseSchema.Columns.ForEach((col) => columnCaptions[col.Name] = col.Caption);
			return columnCaptions;

		}

		private Dictionary<string, string> GetCaseTermCalculationDescribeUtilitiesSchemaResources() {
			var resourcePrefix = "ui_";
			var resources = new Dictionary<string, string>();
			var clientUnitSchemaManager = (ClientUnitSchemaManager)UserConnection
				.GetSchemaManager("ClientUnitSchemaManager");
			ClientUnitSchema clientUnitSchema = clientUnitSchemaManager.GetInstanceByName("CaseTermCalculationDescribeUtilities");
			var resourceSet = clientUnitSchema.LocalizableStrings;
			resourceSet.Where(res => res.Name.StartsWith(resourcePrefix))
				.ForEach((lcz) => resources[lcz.Name.Substring(resourcePrefix.Length)] = lcz.Value.ToString());
			return resources;
		}

		#endregion
	
		#region Methods: Public

		/// <summary>
		/// Returns localizable data.
		/// </summary>
		/// <returns>Dictionary of localizable strings.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		public Dictionary<string, string> GetCaseTermsDescribeData() {
			return GetCaseTermCalculationDescribeUtilitiesSchemaResources()
				.Union(GetCaseSchemaResources()).ToDictionary(d => d.Key, d => d.Value);
		}

		#endregion
	
	}

	#endregion

}
