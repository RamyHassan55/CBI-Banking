namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Configuration;
	using Terrasoft.Core;

	#region Class: CategoryProviderWrapper

	/// <summary>
	/// Wrapper for <see cref="CategoryProvider"/>.
	/// </summary>
	public class CategoryProviderWrapper : ICategoryProviderWrapper
	{
		#region Properties: Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
			private set;
		}

		/// <summary>
		/// Provider for category from support mail box
		/// </summary>
		public CategoryProvider CategoryProvider {
			get;
			set;
		}

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="CategoryProviderWrapper"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public CategoryProviderWrapper(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get category from support mail box.
		/// </summary>
		/// <param name="recipients">Recipients emails, joined in string.</param>
		/// <returns>Case category.</returns>
		public Guid GetCategoryFromSupportMailBox(string recipients) {
			string[] recipientsCollection = recipients.ParseEmailAddress().ToArray();
			if (CategoryProvider != null) {
				return CategoryProvider.GetCaseGategory(recipientsCollection);
			}
			return Guid.Empty;
		}

		#endregion

	}

	#endregion
	
}
