namespace CrtContentDesigner
{
	/// <summary>
	/// Provides a code by which the service determines its participation in processing a template.
	/// </summary>
	public interface ITemplateHandler
	{

		#region Properties: Public

		/// <summary>
		/// Gets the code by which the service determines its participation in processing a template.
		/// </summary>
		string HandlerCode { get; }

		/// <summary>
		/// Gets the priority of the handler.
		/// </summary>
		int Priority { get; }

	#endregion

}
}

