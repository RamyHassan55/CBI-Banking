namespace CrtEmailDesigner.Providers
{
	using System;
	using System.Threading.Tasks;
	using Creatio.ContentDesigner;
	using GetBfTokenResponse = CrtEmailDesigner.Models.GetBfTokenResponse;

	/// <summary>
	/// Provides logic to work with Beefree tokens
	/// </summary>
	[Obsolete("Interface will be removed in upcoming releases.")]
	public interface IBfTokenProvider
	{

		#region Methods: Public

		/// <summary>
		/// Gets the token for Beefree
		/// </summary>
		/// <param name="designerCode">Beefree designer code</param>
		/// <returns>Access token for Beefree</returns>
		Task<GetBfTokenResponse> GetToken(string designerCode);

		#endregion

	}
}