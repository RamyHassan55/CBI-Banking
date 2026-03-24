namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using System.Web;
	using Terrasoft.Configuration.ServicePactService;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	
	#region Class: ITILIncidentRegistrationFromEmailHelper
	/// <summary>
	/// A class that allows incident registration from email.
	/// </summary>
	[Override]
	public class ITILIncidentRegistrationFromEmailHelper : IncidentRegistrationFromEmailHelper
	{
		#region Constructors: Public
		
		public ITILIncidentRegistrationFromEmailHelper() {
		}
		
		public ITILIncidentRegistrationFromEmailHelper(UserConnection userConnection)
			:base(userConnection) {
		}
		
		#endregion

		/// <summary>
		/// Returns a case with account and contact by service pact.
		/// </summary>
		/// <param name="caseEntity">Case entity.</param>
		/// <param name="contactId">Contact id.</param>
		/// <param name="accountId">Account id.</param>
		/// <returns>Case entity.</returns>
		protected override Entity GetCaseWithServicePact(Entity caseEntity, Guid contactId, Guid accountId) {
			if(contactId != Guid.Empty && accountId != Guid.Empty) {
				var servicePactDetermineUtils = ClassFactory.Get<ServicePactDetermineUtilsV2>(
					new ConstructorArgument("userConnection", UserConnection));
				IEnumerable<SuitableServicePact> suitableServicePacts = servicePactDetermineUtils.GetSuitableServicePacts(
					new SuitableServicePactsRequest {
						ContactId = contactId,
						AccountId = accountId
					});
				SuitableServicePact mostSuitableservicePact = null;
				if (suitableServicePacts.Any()) {
					mostSuitableservicePact = suitableServicePacts.Aggregate((curmin, x) => 
						(curmin == null || x.SuitableLevel < curmin.SuitableLevel ? x : curmin));
				}
				if (mostSuitableservicePact != null) {
					caseEntity.SetColumnValue("ServicePactId", mostSuitableservicePact.Id);
				}
			}
			return caseEntity;
		}
	}
	
	#endregion
}
