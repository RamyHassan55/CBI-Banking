namespace Terrasoft.Configuration
{
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Freedom email page URL provider.
	/// </summary>
	[DefaultBinding(typeof(IEmailPageUrlProvider), Name = "FreedomEmailPageUrlProvider")]
	internal class FreedomEmailPageUrlProvider : EmailPageUrlProviderBase
	{

		#region Constructors: Public

		/// <summary>
		/// Creates instance of type <see cref="FreedomEmailPageUrlProvider"/>.
		/// </summary>
		/// <param name="userConnection">User connection</param>
		public FreedomEmailPageUrlProvider(UserConnection userConnection)
			: base(userConnection) {
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Bulk email page name.
		/// </summary>
		public override string BulkEmailPageName => "BulkEmails_FormPage";

		/// <summary>
		/// Designer type.
		/// </summary>
		public override string DesignerTypeId => "fdb47538-d702-42df-9cfa-c66fa7f6dbf6";

		/// <summary>
		/// Email page URL template.
		/// </summary>
		public override string EmailPageUrlTemplate => "{0}#Card/{1}/edit/{2}";

		/// <summary>
		/// Trigger email page name.
		/// </summary>
		public override string TriggerEmailPageName => BulkEmailPageName;

		#endregion

	}
}
