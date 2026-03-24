namespace CrtEmailDesigner.Utilities
{
	using Common.Logging;
	using CrtContentDesigner.Utilities;
	using System;
	using Terrasoft.Core;

	#region Class: BfTemplateRequestHandler

	/// <summary>
	/// Provides a handler for the handling requests from a designer.
	/// </summary>
	[Obsolete("Use TemplateRequestHandler instead.")]
	internal class BfTemplateRequestHandler : TemplateRequestHandler
	{

		#region Fields: Private

		private static ILog _handlerLog;

		#endregion

		#region Constructors: Public

		public BfTemplateRequestHandler(UserConnection userConnection) : base(userConnection) { }

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the log.
		/// </summary>
		public override ILog HandlerLog {
			get => _handlerLog ?? (_handlerLog = LogManager.GetLogger("BfTemplateRequestHandler"));
			set => _handlerLog = value;
		}

		#endregion
	}

	#endregion
}