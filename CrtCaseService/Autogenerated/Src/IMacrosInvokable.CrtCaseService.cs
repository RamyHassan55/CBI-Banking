namespace Terrasoft.Configuration
{
	using Terrasoft.Core;

	#region Interface: IMacrosinvokable

	/// <summary>
	/// Interface of macros for invoke.
	/// </summary>
	public interface IMacrosInvokable
	{
		/// <summary>
		/// User connection.
		/// </summary>
		UserConnection UserConnection {
			get;
			set;
		}

		/// <summary>
		/// Method that returns string value for macros.
		/// </summary>
		/// <returns>result string</returns>
		string GetMacrosValue(object arguments);

	}

	#endregion

}
