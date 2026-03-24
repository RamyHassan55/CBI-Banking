namespace CrtEmailDesigner.Contracts
{
    using System;
    using System.Runtime.Serialization;

    #region Class: EmailDesignerTemplateRequest

    /// <summary>
    /// Represents a base request to execute email-related template requests in <see cref="EmailBuilderTemplateService"/>
    /// </summary>
    [DataContract]
    public class EmailDesignerTemplateRequest : BaseEmailTemplateRequest
    {

        #region Properties: Public

        /// <summary>
        /// Email template identifier.
        /// </summary>
        [DataMember(Name = "emailId")]
        public Guid EmailId { get; set; }

        #endregion

    }

    #endregion

}
