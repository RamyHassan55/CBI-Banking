namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Class: SupMailBoxLangProvider

	/// <inheritdoc />
	public class SupMailBoxLangProvider : ISupMailBoxLangProvider
	{
		#region Fields: Private

		private ISupportMailBoxRepository _mailBoxRepository;
		private Entity[] _mailBoxes;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initialize new instance of <see cref="SupMailBoxLangProvider"/>.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="recipients">List of recipients in email.</param>
		public SupMailBoxLangProvider(UserConnection userConnection) {
			ConstructorArgument userConnectionArgument = new ConstructorArgument("userConnection", userConnection);
			ClassFactory.TryGet("MailBoxRepository", out _mailBoxRepository, userConnectionArgument);
			if (_mailBoxRepository != null) {
				_mailBoxes = _mailBoxRepository.GetMailboxes().ToArray();
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		public Guid GetLanguage(List<string> recipients) {
			if (_mailBoxRepository != null) {
				foreach (string recipient in recipients) {
					foreach (Entity mailBox in _mailBoxes) {
						if (recipient == mailBox.GetTypedColumnValue<string>("MailboxName")) {
							Guid langId = mailBox.GetTypedColumnValue<Guid>("MessageLanguageId");
							if (langId != Guid.Empty) {
								return langId;
							}
						}
					}
				}
			}
			return Guid.Empty;
		}

		#endregion

	}

	#endregion

}
