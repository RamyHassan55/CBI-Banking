namespace CrtEmailDesigner.Contracts
{
	using CrtEmailDesigner.Models;
	using CrtContentDesigner.Contracts;
	using System.Runtime.Serialization;
	using System;

	#region Class: SaveBfEmailTemplateRequest

	/// <summary>
	/// Represents a request to save an email template.
	/// </summary>
	[DataContract]
	[Obsolete("This class is obsolete. Use SaveTemplateRequest instead.")]
	public class SaveBfEmailTemplateRequest : SaveTemplateRequest
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the email template model.
		/// </summary>
		[DataMember(Name = "bfEmailTemplate")]
		[Obsolete("This property is obsolete. Use Content instead.")]
		public BfEmailTemplateModel BfEmailTemplate {
			get => (BfEmailTemplateModel)Content;
			set => Content = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether only the BfTemplate should be saved.
		/// </summary>
		[Obsolete("This property is obsolete. It doent't use anymore.")]
		[DataMember(Name = "saveBfTemplateOnly")]
		public bool SaveBfTemplateOnly { get; set; }

		#endregion

	}

	#endregion
}