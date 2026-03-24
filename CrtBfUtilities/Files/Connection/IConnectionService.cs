namespace Creatio.ContentDesigner
{
	/// <summary>
	/// Provides logic to check configuration of services
	/// </summary>
	internal interface IConnectionService
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