using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;

namespace Terrasoft.Configuration.ServicePactService
{

	#region Class: ServicePactService

	/// <summary>
	/// ###### ######### #########.
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class ServicePactService: BaseService
	{
		#region Methods: Public

		/// <summary>
		/// ########## ###### ######### #########, ####### ######## ### ######### ######## # ###########.
		/// </summary>
		/// <param name="request">###### ## ########### ######### #########</param>
		/// <returns>###### ########## ######### #########</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		public IEnumerable<SuitableServicePact> GetSuitableServicePacts(SuitableServicePactsRequest request) {
			var userConnection = UserConnection;
			var utils = ClassFactory.Get<ServicePactDetermineUtilsV2>(new ConstructorArgument("userConnection", userConnection));
			return utils.GetSuitableServicePacts(request);
		}

		#endregion
	}

	#endregion

	#region Class: SuitableServicePactsRequest

	/// <summary>
	/// ###### ## ########### ######### ########.
	/// </summary>
	[DataContract]
	public class SuitableServicePactsRequest
	{
		#region Properties: Public

		/// <summary>
		/// #######.
		/// </summary>
		[DataMember]
		public Guid ContactId { 
			get;
			set;
		}

		/// <summary>
		/// ##########.
		/// </summary>
		[DataMember]
		public Guid AccountId { 
			get;
			set;
		}

		#endregion
	}

	#endregion

	#region Class: SuitableServicePact

	/// <summary>
	/// ###### ########### ########## ########.
	/// </summary>
	[DataContract]
	public class SuitableServicePact : IEquatable<SuitableServicePact>
	{
		#region Properties: Public

		/// <summary>
		/// ############# ########## ########.
		/// </summary>
		[DataMember]
		public Guid Id { 
			get;
			set;
		}

		/// <summary>
		/// ######## ########## ########.
		/// </summary>
		[DataMember]
		public string Name { 
			get;
			set;
		}

		/// <summary>
		/// ########## ######## ######## ###### ######### #######.
		/// </summary>
		[DataMember]
		public int SuitableLevel { 
			get;
			set;
		}

		#endregion

		#region Constructors: Public

		public SuitableServicePact() {}

		public SuitableServicePact(Guid id, string name, int suitableLevel) {
			Id = id;
			Name = name;
			SuitableLevel = suitableLevel;
		}

		#endregion

		#region Methods: Public
		
		public bool Equals(SuitableServicePact suitableServicePact) {
			return suitableServicePact != null && Id.Equals(suitableServicePact.Id);
		}
		
		#endregion
	}

	#endregion
}
