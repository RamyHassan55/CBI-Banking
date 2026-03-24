namespace CrtDigitalAdsApp.Providers.Exceptions
{
	using System;

	/// <summary>
	/// Represents the exception that is thrown when an error occurs while working with ad platform.
	/// </summary>
	internal class AdPlatformException : Exception
	{

		#region Constructors: Public

		/// <summary>
		/// Creates instance of <see cref="AdPlatformException"/> class with specified error message.
		/// </summary>
		/// <param name="message"></param>
		public AdPlatformException(string message)
			: base(message) {
		}

		#endregion

	}
}