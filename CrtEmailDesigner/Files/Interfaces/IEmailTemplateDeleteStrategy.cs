namespace CrtEmailDesigner.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CrtEmailDesigner.Contracts;

	/// <summary>
	/// For internal use.
	/// Represents a strategy for deleting email templates in CrtEmailDesigner.
	/// </summary>
    internal interface IEmailTemplateDeleteStrategy
	{
		/// <summary>
		/// Executes the strategy to delete email templates.
		/// </summary>
		/// <param name="emailId"></param>
		/// <param name="languageIds"></param>
		/// <param name="handlerCodes"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> DeleteMany(Guid emailId, Guid[] languageIds, IList<string> handlerCodes);
	}
}
