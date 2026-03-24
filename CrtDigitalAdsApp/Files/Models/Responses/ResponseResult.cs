using CrtDigitalAdsApp.Enums;

namespace CrtDigitalAdsApp.Models.Responses
{
	/// <summary>
	/// Generic model to provide result with error reason.
	/// </summary>
	public class ResponseResult<T>
	{
		public T Response { get; set; }
		public bool Success { get; set; }
		public ErrorReason ErrorReason { get; set; }

	}
}
