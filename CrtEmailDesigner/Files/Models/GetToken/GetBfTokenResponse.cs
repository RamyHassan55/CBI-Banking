namespace CrtEmailDesigner.Models
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	[Obsolete("Use Creatio.ContentDesigner.GetBfTokenResponse instead.")]
	public class GetBfTokenResponse : Creatio.ContentDesigner.GetDesignerTokenResponse
	{
	}
}