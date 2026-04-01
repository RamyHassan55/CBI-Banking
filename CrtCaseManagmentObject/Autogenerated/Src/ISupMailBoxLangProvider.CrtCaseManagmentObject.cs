namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Describes type, that provides language from mail boxes for incident registration.
	/// </summary>
	public interface ISupMailBoxLangProvider
	{
		/// <summary>
		/// Provides language from mail boxes for incident registration.
		/// </summary>
		/// <param name="recipients">List of emails.</param>
		/// <returns>Language identifier.</returns>
		Guid GetLanguage(List<string> recipients);
	}
}
