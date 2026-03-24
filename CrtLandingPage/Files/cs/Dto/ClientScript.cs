namespace CrtLandingPage.Dto
{
	using CrtLandingPage.Parameters;

	public class LandingClientScript
	{
		public LandingClientScript(string marker) {
			Marker = marker;
		}

		public ClientScriptPosition Position { get; set; }
		public string Content { get; set; }
		public string Marker { get; }
	}
}
