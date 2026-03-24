namespace Terrasoft.Configuration
{
	/// <summary>
	/// Interface of email header property handler.
	/// </summary>
	public interface IHeaderPropertyHandler
	{
		/// <summary>
		/// Method that indicates whether given value is acceptable by this handler or not.
		/// </summary>
		/// <param name="value">Header property value.</param>
		/// <returns>True if a value is acceptable and False otherwise.</returns>
		bool Check(object value);
	}
}

