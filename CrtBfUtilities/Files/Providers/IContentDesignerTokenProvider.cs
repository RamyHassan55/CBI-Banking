namespace Creatio.ContentDesigner
{
	using System.Threading.Tasks;

	/// <summary>
	/// Provides logic to work with designer tokens.
	/// </summary>
	internal interface IContentDesignerTokenProvider
	{

		#region Methods: Public

		/// <summary>
		/// Gets the token for designer.
		/// </summary>
		/// <param name="designerCode">Designer type code</param>
		/// <returns>Access token for designer</returns>
		Task<GetDesignerTokenResponse> GetToken(string designerCode, string appVersion);

		#endregion

	}
}