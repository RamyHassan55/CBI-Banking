namespace CrtEmailDesigner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CrtContentDesigner;
    using CrtContentDesigner.Contracts;
    using CrtContentDesigner.Models;
    using CrtEmailDesigner.Contracts;
	using CrtEmailDesigner.Interfaces;
    using Terrasoft.Core;
    using Terrasoft.Core.Factories;
    using LoadTemplateRequest = CrtContentDesigner.Contracts.LoadTemplateRequest;

    #region Class: EmailBuilderTemplateService

	/// <summary>
	///     Implements the ITemplateLoad interface to load email templates.
	/// </summary>
	[DefaultBinding(typeof(ITemplateLoad), Name = "Email")]
    [DefaultBinding(typeof(ITemplateSave), Name = "Email")]
    internal class EmailBuilderTemplateService : ITemplateLoad, ITemplateSave
    {

        #region Fields: Private

        private readonly IBfEmailTemplateRepository _bfTemplateRepository;

        #endregion

        #region Constructors: Public

        public EmailBuilderTemplateService(UserConnection userConnection) {
            _bfTemplateRepository =
                ClassFactory.Get<IBfEmailTemplateRepository>(new ConstructorArgument("userConnection", userConnection));
        }

        #endregion

        #region Properties: Public

        /// <inheritdoc cref="ITemplateHandler.HandlerCode"/>
        public string HandlerCode => "Email";

        /// <inheritdoc cref="ITemplateHandler.Priority"/>
        public int Priority => 10;

        #endregion

        #region Methods: Public

		/// <summary>
		///     Loads the email template with the specified load request details.
		/// </summary>
		/// <param name="loadRequest">The details of the load request.</param>
		/// <param name="emailTemplate">The email template to process.</param>
		/// <returns>The processed email template model.</returns>
		public TemplateModel LoadTemplate(LoadTemplateRequest loadRequest, TemplateModel emailTemplate) {
            // TODO It should be initial loading. Do we need support modifications of the template? (case when we insert handlers with less priopiry before)
            return _bfTemplateRepository.LoadTemplate(loadRequest.RecordId);
        }

		/// <summary>
		///     Saves the email template with the specified save request details.
		/// </summary>
		/// <param name="saveRequest">The details of the save request.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		public bool SaveTemplate(SaveTemplateRequest saveRequest) {
            return _bfTemplateRepository.SaveTemplate(saveRequest.Content);
        }

        #endregion

    }

    #endregion

}

