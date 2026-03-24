using System.Collections.Generic;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace Terrasoft.Configuration.ServiceTerm
{
	/// <summary>
	/// Term provider factory.
	/// </summary>
	public class TermProviderFactory
	{
		/// <summary>
		/// Gets specification term parameters.
		/// </summary>
		/// <param name="conditions">Term parameter select condition.</param>
		/// <param name="userConnection">User connection.</param>
		/// <returns>Specification term parameters.</returns>
		public virtual ISpecificationTermParameters GetSpecificationTermParameters(Dictionary<string, object> conditions, UserConnection userConnection) {
			return ClassFactory.Get<SpecificationTermParameters>(
				new ConstructorArgument("conditions", conditions),
				new ConstructorArgument("userConnection", userConnection));
		}
	}
}
