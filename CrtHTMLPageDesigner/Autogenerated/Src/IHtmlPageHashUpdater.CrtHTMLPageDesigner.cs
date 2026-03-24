namespace CrtHTMLPageDesigner
{
	using Terrasoft.Core.Entities;

	#region Interface: IHtmlPageHashUpdater

	public interface IHtmlPageHashUpdater
	{

		#region Methods: Public

		/// <summary>
		/// Sets new hash value into the Hash entity column.
		/// </summary>
		/// <param name="page">HTML page <see cref="Entity"/>.</param>
		/// <returns>New hash text.</returns>
		string UpdateHash(Entity page);

		#endregion

	}

	#endregion

}
