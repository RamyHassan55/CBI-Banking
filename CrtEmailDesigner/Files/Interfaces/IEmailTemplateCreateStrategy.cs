namespace CrtEmailDesigner.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CrtEmailDesigner.Contracts;

	/// <summary>
	/// For internal use.
	/// Represents a strategy for creating email templates in CrtEmailDesigner.
	/// </summary>
    internal interface IEmailTemplateCreateStrategy
	{
		/// <summary>
		/// Executes the strategy to create email templates.
		/// </summary>
		/// <param name="emailId"></param>
		/// <param name="languageIds"></param>
		/// <param name="handlerCodes"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> CreateMany(Guid emailId, Guid[] languageIds, IList<string> handlerCodes);
		
		/// <summary>
		/// Executes the strategy to create default email template.
		/// </summary>
		/// <param name="emailId"></param>
		/// <param name="handlerCodes"></param>
		/// <returns></returns>
		EmailDesignerTemplateModel CreateDefault(Guid emailId, IList<string> handlerCodes);
	}
}
