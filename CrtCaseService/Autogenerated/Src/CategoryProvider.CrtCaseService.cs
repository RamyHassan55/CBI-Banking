namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;

	#region Class: CategoryProvider

	/// <summary>
	/// Get case category from support mail boxes.
	/// </summary>
	public class CategoryProvider
	{
		#region Fields: Private

		private readonly ICategoryFromSupportMailBox _supportMailBoxRepository;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="CategoryProvider"/>.
		/// </summary>
		/// <param name="supportMailBoxRepository">Support mail box repository.</param>
		public CategoryProvider(ICategoryFromSupportMailBox supportMailBoxRepository) {
			_supportMailBoxRepository = supportMailBoxRepository;
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Get categoryId from recipients mail boxes.
		/// </summary>
		/// <param name="recipients">Array of recipients email.</param>
		/// <returns>Case categoryId.</returns>
		public Guid GetCaseGategory(string[] recipients) {
			Dictionary<string, Guid> supportMailBoxes = _supportMailBoxRepository.GetMailBoxesWithCategories();
			foreach (string recipient in recipients) {
				foreach (string supportEmail in supportMailBoxes.Keys) {
					if (recipient.Equals(supportEmail, StringComparison.OrdinalIgnoreCase)) {
						return supportMailBoxes[supportEmail];
					}
				}
			}
			return Guid.Empty;
		}

		#endregion

	}

	#endregion
}
