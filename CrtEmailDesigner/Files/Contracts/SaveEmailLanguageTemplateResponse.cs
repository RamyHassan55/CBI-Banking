namespace CrtEmailDesigner.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Terrasoft.Core.ServiceModelContract;

    /// <summary>
    /// Represents a response to save email language templates.
    /// </summary>
    [DataContract]
    public class SaveEmailLanguageTemplateResponse : BaseResponse
    {
        /// <summary>
        /// Templates to delete
        /// </summary>
        [DataMember(Name = "templatesToDelete")]
        public IList<EmailDesignerTemplateModel> TemplatesToDelete { get; set; }
        
        /// <summary>
        /// Templates to Create
        /// </summary>
        [DataMember(Name = "templatesToCreate")]
        public IList<EmailDesignerTemplateModel> TemplatesToCreate { get; set; }
    }
}
