namespace Terrasoft.Configuration
{
	public static class JunkFilterConsts
	{
		public const string EmailTypeCode = "Email";
		public const string DomainTypeCode = "Domain";
		public const string EntryTypeCode = "Entry";
		public const string HeaderPropertyTo = "to";
		public const string HeaderPropertyValuesPattern = @"({0})[=:](.+)";
		public static readonly string DomainPattern = @"@(.)+";
		public static readonly string SenderColumnName = "Sender";
	}
}
