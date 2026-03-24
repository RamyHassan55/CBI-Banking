namespace Creatio.LandingPageDesigner.Utils
{
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Text;

	internal static class SlugGenerator
	{
		private static string CharToLatin(char c) {
			switch (c) {
				case '\u0410': return "A";
				case '\u0411': return "B";
				case '\u0412': return "V";
				case '\u0413': return "G";
				case '\u0414': return "D";
				case '\u0415': return "E";
				case '\u0416': return "Zh";
				case '\u0417': return "Z";
				case '\u0418': return "I";
				case '\u0419': return "I";
				case '\u041A': return "K";
				case '\u041B': return "L";
				case '\u041C': return "M";
				case '\u041D': return "N";
				case '\u041E': return "O";
				case '\u041F': return "P";
				case '\u0420': return "R";
				case '\u0421': return "S";
				case '\u0422': return "T";
				case '\u0423': return "U";
				case '\u0424': return "F";
				case '\u0425': return "Kh";
				case '\u0426': return "Ts";
				case '\u0427': return "Ch";
				case '\u0428': return "Sh";
				case '\u0429': return "Shch";
				case '\u042B': return "Ye";
				case '\u042D': return "E";
				case '\u042E': return "Iu";
				case '\u042F': return "I";
				case '\u0430': return "a";
				case '\u0431': return "l";
				case '\u0432': return "v";
				case '\u0433': return "g";
				case '\u0434': return "d";
				case '\u0435': return "e";
				case '\u0436': return "zh";
				case '\u0437': return "z";
				case '\u0438': return "i";
				case '\u0439': return "i";
				case '\u043A': return "k";
				case '\u043B': return "l";
				case '\u043C': return "m";
				case '\u043D': return "n";
				case '\u043E': return "o";
				case '\u043F': return "p";
				case '\u0440': return "r";
				case '\u0441': return "s";
				case '\u0442': return "t";
				case '\u0443': return "u";
				case '\u0444': return "f";
				case '\u0445': return "kh";
				case '\u0446': return "ts";
				case '\u0447': return "ch";
				case '\u0448': return "sh";
				case '\u0449': return "shch";
				case '\u044B': return "yi";
				case '\u044D': return "e";
				case '\u044E': return "iu";
				case '\u044F': return "ia";
				case '\u0456': return "i";
				case '\u0159': return "r";
				case '\u0142': return "l";
				case '\u00e1': return "a";
				case '\u00e9': return "e";
				case '\u00ed': return "i";
				case '\u0119': return "e";
				default: return c.ToString();
			}
		}

		private static string RemoveAccent(string txt) {
			byte[] bytes = Encoding.Unicode.GetBytes(txt);
			return Encoding.ASCII.GetString(bytes);
		}

		/// <summary>
		/// Generates slug by phrase.
		/// </summary>
		/// <param name="phrase">The original string that must be converted to slug.</param>
		/// <returns>Slug string</returns>
		public static string GenerateSlug(string phrase) {
			string str = string.Join("", phrase.ToCharArray().Select(CharToLatin));
			str = RemoveAccent(str).ToLower();
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			str = Regex.Replace(str, @"\s+", " ").Trim();
			str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();
			str = Regex.Replace(str, @"\s", "-");
			return str;
		}

	}
}
