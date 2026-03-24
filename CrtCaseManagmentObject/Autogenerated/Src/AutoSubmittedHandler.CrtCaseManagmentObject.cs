namespace Terrasoft.Configuration
{
	using System;

	/// <summary>
	/// A class that represents a handler of "Auto-Submitted" property in email header.
	/// </summary>
	public class AutoSubmittedHandler : IHeaderPropertyHandler
	{

		#region IHeaderPropertyHandler Members

		/// <summary>
		/// Method checks the value of header property "Auto-Submitted".
		/// </summary>
		/// <param name="value">Value of "Auto-Submitted" header property.</param>
		/// <returns>Returns True if value is "No" and False otherwise.</returns>
		public bool Check(object value) {
			return string.Equals(value.ToString(), CaseConsts.AllowedAutoSubmittedPropertyValue,
				StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
	
}
