namespace CrtEmailDesigner.Contracts
{
	using System.Runtime.Serialization;
	using CrtEmailDesigner.Models;
	using CrtContentDesigner.Contracts;
	using System;

	/// <summary>
	/// Represents a response of an email template.
	/// </summary>
	[DataContract]
	[Obsolete]
	public class BfEmailTemplateResponse: LoadTemplateResponse {

		#region Properties: Public

		/// <summary>
		/// Gets or sets the email template model.
		/// </summary>
		[DataMember(Name = "bfEmailTemplate")]
		[Obsolete("This property is obsolete. Use Template instead.")]
		public BfEmailTemplateModel BfEmailTemplate { get; set; }

		#endregion

	}
}