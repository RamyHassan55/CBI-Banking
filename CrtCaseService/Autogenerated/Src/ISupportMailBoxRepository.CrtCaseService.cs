namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Core.Entities;

	/// <summary>
	/// Provides obtaining of support mailboxes.
	/// </summary>
	public interface ISupportMailBoxRepository
	{
		/// <summary>
		/// Gets support mailboxes.
		/// </summary>
		/// <returns>Support mailboxes.</returns>
		IEnumerable<Entity> GetMailboxes();

		/// <summary>
		/// Gets support mailbox from system setting.
		/// </summary>
		/// <returns>Support mailbox.</returns>
		string GetMailboxFromSystemSetting();
	}
}
