namespace Terrasoft.Configuration
{
	using System;

	/// <summary>
	/// Provide case category from mail box.
	/// </summary>
	public interface ICategoryProviderWrapper
	{
		/// <summary>
		/// Get category related from support mail box placed on recipients.
		/// </summary>
		/// <param name="recipients">Recipients, joined in string.</param>
		/// <returns>Category from support mail box.</returns>
		Guid GetCategoryFromSupportMailBox(string recipients);
	}
}
