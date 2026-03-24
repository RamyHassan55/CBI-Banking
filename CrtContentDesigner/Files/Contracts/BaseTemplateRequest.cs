namespace CrtContentDesigner.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	#region Class: BaseTemplateRequest

    /// <summary>
    /// Represents a base request for a template.
    /// </summary>
    [DataContract]
	public abstract class BaseTemplateRequest
	{

		#region Properties: Public

        /// <summary>
        /// Gets or sets the handler code for processing the save request.
        /// </summary>
        [DataMember(Name = "handlerCode")]
		[Obsolete("Use HandlerCodes instead.")]
		public string HandlerCode { get; set; }

        /// <summary>
        /// Gets or sets the handler codes for processing the save request.
        /// </summary>
        [DataMember(Name = "handlerCodes")]
		public List<string> HandlerCodes { get; set; } = new List<string>();

		#endregion

	}

	#endregion

}
