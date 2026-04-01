 namespace 	CrtContentDesigner
{
	using Terrasoft.Core;
	using Terrasoft.Common;
	using Terrasoft.Core.Factories;
	using CrtContentDesigner.Interfaces;

	#region Class: ContentDesignerMessageProvider

	[DefaultBinding(typeof(IInfoMessageProvider), Name = "ContentDesignerMessageProvider")]
	public class ContentDesignerMessageProvider : IInfoMessageProvider
	{

		#region Methods: Public

		/// <summary>
		/// Returns localizable string value for specified message.
		/// </summary>
		/// <param name="userConnection">UserConnection instance.</param>
		/// <param name="key">Message key.</param>
		public string GetMessage(UserConnection userConnection, string key) {
			string message = new LocalizableString(userConnection.ResourceStorage,
				nameof(ContentDesignerMessageProvider), $"LocalizableStrings.{key}.Value");
			return message;
		}

		#endregion

	}

	#endregion

}

