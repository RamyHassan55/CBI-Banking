namespace CrtEmailDesigner.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #region Class: BaseEmailTemplateRequest

    /// <summary>
    ///     Base class for email template requests.
    /// </summary>
    [DataContract]
    public abstract class BaseEmailTemplateRequest
    {

        #region Properties: Public

        /// <summary>
        /// Gets or sets the handler codes for processing the save request.
        /// </summary>
        [DataMember(Name = "handlerCodes")]
        public IList<string> HandlerCodes { get; set; } = new List<string>();

        #endregion

    }

    #endregion

}
