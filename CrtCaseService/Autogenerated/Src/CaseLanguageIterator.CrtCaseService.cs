namespace Terrasoft.Configuration
{
	using Terrasoft.Core;

	#region Class: CaseLanguageIterator

	/// <summary>
	/// Iterator for listing by case languages.
	/// </summary>
	public class CaseLanguageIterator : BaseLanguageIterator
	{
		#region Constructors: Public

		/// <summary>
		/// Initializes new instance of <see cref="CaseLanguageIterator"/>. 
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		public CaseLanguageIterator(UserConnection userConnection)
			: base(userConnection) {
			LanguageRules = new ILanguageRule[] {
				new MailBoxCaseRegistrationLanguageRule(UserConnection),
				new DefaultContactLanguageRule(UserConnection, "Case"),
				new MailBoxLanguageRule(UserConnection),
				new DefSupportBoxLanguageRule(UserConnection),
				new DefaultLanguageRule(UserConnection),
			};
		}


		#endregion

	}

	#endregion

}
