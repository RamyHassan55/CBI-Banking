namespace CrtContentDesigner.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using CrtContentDesigner.Contracts;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	#region Class: TemplateRequestHandler

	/// <summary>
	/// Provides a handler for the handling requests from a designer.
	/// </summary>
	[DefaultBinding(typeof(ITemplateRequestHandler))]
	internal class TemplateRequestHandler : ITemplateRequestHandler
	{

		#region Fields: Private

		private readonly List<ITemplateLoad> _templateLoadServices = new List<ITemplateLoad>();
		private readonly List<ITemplateSave> _templateSaveServices = new List<ITemplateSave>();

		#endregion

		private static ILog _handlerLog;

		#region Constructors: Public

		public TemplateRequestHandler(UserConnection userConnection) {
			var templateSaveServices =
				ClassFactory.GetAll<ITemplateSave>(new ConstructorArgument("userConnection", userConnection));
			_templateSaveServices.AddRange(templateSaveServices);
			var templateLoadServices =
				ClassFactory.GetAll<ITemplateLoad>(new ConstructorArgument("userConnection", userConnection));
			_templateLoadServices.AddRange(templateLoadServices);
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the log.
		/// </summary>
		public virtual ILog HandlerLog {
			get => _handlerLog ?? (_handlerLog = LogManager.GetLogger("TemplateRequestHandler"));
			set => _handlerLog = value;
		}

		#endregion

		#region Methods: Private

		private static Func<T, bool> GetHandlerCodesFilter<T>(BaseTemplateRequest request)
			where T : ITemplateHandler {
			var codes = request.HandlerCodes = request.HandlerCodes ?? new List<string>();
#pragma warning disable CS0618 // Needed for backward compatibility
			if (request.HandlerCode.IsNotNullOrEmpty())
				request.HandlerCodes.AddIfNotExists(request.HandlerCode);
#pragma warning restore CS0618
			return handler => codes.Contains(handler.HandlerCode, StringComparer.InvariantCultureIgnoreCase);
		}

		private static void ThrowIfDuplicateHandlerCodes(IEnumerable<ITemplateHandler> handlers) {
			var duplicateHandlerCodes = handlers
				.GroupBy(handler => handler.HandlerCode, StringComparer.InvariantCultureIgnoreCase)
				.Where(group => group.Count() > 1).Select(group => group.Key).ToList();
			if (duplicateHandlerCodes.Any()) {
				var message = $"Duplicate handler codes found: {string.Join(", ", duplicateHandlerCodes)}.";
				throw new Exception(message);
			}
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc cref="TemplateRequestHandler.LoadTemplate"/>
		public LoadTemplateResponse LoadTemplate(LoadTemplateRequest loadRequest) {
			var response = new LoadTemplateResponse {
				Content = null
			};
			try {
				var templateLoadServices = _templateLoadServices
					.Where(GetHandlerCodesFilter<ITemplateLoad>(loadRequest))
					.OrderBy(loadService => loadService.Priority).ToList();
				ThrowIfDuplicateHandlerCodes(templateLoadServices);
				foreach (ITemplateLoad loadService in templateLoadServices)
					response.Content = loadService.LoadTemplate(loadRequest, response.Content);
			} catch (Exception exception) {
				var message =
					$"Error when loading the template. Exception: {exception}, TemplateId: {loadRequest.RecordId}.";
				HandlerLog.Error(message);
				response.Success = false;
				response.Error = message;
			}
			return response;
		}

		/// <inheritdoc cref="TemplateRequestHandler.SaveTemplate"/>
		public SaveTemplateResponse SaveTemplate(SaveTemplateRequest saveRequest) {
			var response = new SaveTemplateResponse();
			try {
				var templateSaveServices = _templateSaveServices
					.Where(GetHandlerCodesFilter<ITemplateSave>(saveRequest))
					.OrderBy(saveService => saveService.Priority).ToList();
				ThrowIfDuplicateHandlerCodes(templateSaveServices);
				foreach (ITemplateSave templateSaveService in templateSaveServices) {
					bool success = templateSaveService.SaveTemplate(saveRequest);
					if (!success) {
						response.Success = false;
						response.Error =
							$"Error when saving the template with handlerCode [{templateSaveService.HandlerCode}] in {templateSaveService.GetType().Name}";
						HandlerLog.Error(response.Error);
						return response;
					}
				}
				response.Success = true;
			} catch (Exception exception) {
				var message = $"Error when saving the template. Exception: {exception}, Request: {saveRequest}.";
				HandlerLog.Error(message);
				response.Success = false;
				response.Error = message;
			}
			return response;
		}

		#endregion

	}

	#endregion

}
