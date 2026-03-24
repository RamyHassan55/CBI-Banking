namespace CrtContentDesigner.Interfaces
{
	using Terrasoft.Core;

	#region Interface IInfoMessageProvider

	public interface IInfoMessageProvider
	{
		/// <summary>
		/// Returns localizable string value for specified message.
		/// </summary>
		/// <param name="userConnection">UserConnection instance.</param>
		/// <param name="key">Message key.</param>
		string GetMessage(UserConnection userConnection, string key);
	}

	#endregion

}
