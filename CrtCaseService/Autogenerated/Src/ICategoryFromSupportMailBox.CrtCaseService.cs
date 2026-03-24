namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Provide support mail boxes with category.
	/// </summary>
	public interface ICategoryFromSupportMailBox
	{
		/// <summary>
		/// Get support mail boxes with category from system.
		/// </summary>
		/// <returns>Dictionary where key is support mail boxes address and value is category.</returns>
		Dictionary<string, Guid> GetMailBoxesWithCategories();
	}
}
