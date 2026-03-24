namespace CrtEmailDesigner.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CrtContentDesigner.Repositories;
	using CrtEmailDesigner.Contracts;

	/// <summary>
	/// Template repository for email templates.
	/// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
	internal interface IBfEmailTemplateRepository : ITemplateRepository
#pragma warning restore CS0618 // Type or member is obsolete
	{

		#region Methods: Public

		/// <summary>
		/// Creates a new template.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		EmailDesignerTemplateModel Create(EmailDesignerTemplateModel model);

		/// <summary>
		/// Creates many templates
		/// </summary>
		/// <param name="models"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> CreateMany(IEnumerable<EmailDesignerTemplateModel> models);

		/// <summary>
		/// Deletes the specified templates by IDs.
		/// </summary>
		/// <param name="templateIds"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> DeleteMany(Guid[] templateIds);

		/// <summary>
		/// Retrieves the existing templates for the specified email.
		/// </summary>
		/// <param name="emailId"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> FetchAllByEmailId(Guid emailId);

		/// <summary>
		/// Retrieves the existing templates for the specified id.
		/// </summary>
		/// <param name="templateId"></param>
		/// <returns></returns>
		EmailDesignerTemplateModel FetchById(Guid templateId);

		/// <summary>
		/// Retrieves the existing templates for the specified ids.
		/// </summary>
		/// <param name="templateIds"></param>
		/// <returns></returns>
		IList<EmailDesignerTemplateModel> FetchByIds(IEnumerable<Guid> templateIds);

		#endregion

	}
}
