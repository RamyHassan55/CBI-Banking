namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Utils;

	#region Class: InvokableMacrosHelper

	/// <summary>
	/// Class which can use InvokeMethodMacrosWorker.
	/// </summary>
	public class InvokableMacrosHelper : GlobalMacrosHelper
	{

		#region Fields: Private

		private static readonly Guid _macrosWorkerId = Guid.Parse("16339F82-6FF0-4C75-B20D-13F07A79F854"); 

		#endregion

		#region Methods: Private

		/// <summary>
		/// Check if it is need to use invokation macros worker.
		/// </summary>
		private bool IsMacrosInvokableExists(List<MacrosInfo> macrosInfos) {
			return macrosInfos.Any(macrosInfo => macrosInfo.Id != Guid.Empty
				&& macrosInfo.ParentId == _macrosWorkerId);
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Specify invoke macros worker.
		/// </summary>
		/// <returns>Macros worker id.</returns>
		protected virtual Guid GetMacrosWorkerId() {
			return _macrosWorkerId;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Returns arguments for macros values.
		/// </summary>
		/// <param name="entityName">Entity name which used to get arguments.</param>
		/// <param name="recordId">Record which used to get arguments.</param>
		/// <param name="macrosInfos">Macros storage.</param>
		/// <returns>Arguments.</returns>
		public override Dictionary<Guid, Object> GetMacrosArguments(string entityName, Guid recordId,
				List<MacrosInfo> macrosInfos) {
			Dictionary<Guid, Object> arguments = base.GetMacrosArguments(entityName, recordId, macrosInfos);
			if (IsMacrosInvokableExists(macrosInfos)) {
                arguments[GetMacrosWorkerId()] = new KeyValuePair<string, Guid>(entityName, recordId);
			}
			return arguments;
		}

		#endregion

	}

	#endregion

}
