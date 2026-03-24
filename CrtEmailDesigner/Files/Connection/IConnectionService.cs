namespace CrtEmailDesigner.Connection
{
	using System;

	/// <summary>
	/// Provides logic to check configuration of services
	/// </summary>
	[Obsolete("Interface will be removed in upcoming releases")]
	public interface IConnectionService
	{

		#region Methods: Public

		/// <summary>
		/// Checks if the system settings are configured
		/// </summary>
		/// <returns>Bool value</returns>
		bool CheckSysSettingsConfiguration();

		#endregion

	}
}