namespace CrtEmailDesigner.Contracts
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a request to save email language templates.
    /// </summary>
    [DataContract]
    public class SaveEmailLanguageTemplateRequest : BaseEmailTemplateRequest
    {
        /// <summary>
        /// Email ID
        /// </summary>
        [DataMember(Name = "emailId")]
        public Guid EmailId { get; set; }
        
        /// <summary>
        /// Language IDs
        /// </summary>
        [DataMember(Name = "languageIds")]
        public Guid[] LanguageIds { get; set; }

        /// <summary>
        /// Dry run flag, if true, no changes will be saved.
        /// </summary>
        [DataMember(Name = "dryRun")]
        public bool DryRun { get; set; }
        
    }
}
