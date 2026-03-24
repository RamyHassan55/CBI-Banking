namespace CrtEmailDesigner.Contracts
{
	using CrtContentDesigner.Contracts;
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents a base response.
	/// </summary>
	[DataContract]
	[Obsolete("Use BaseTemplateResponse instead.")]
	public class BaseEmailTemplateResponse : BaseTemplateResponse { }
}