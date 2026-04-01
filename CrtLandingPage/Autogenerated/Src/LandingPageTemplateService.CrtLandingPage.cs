namespace CrtLandingPage
{
	using CrtContentDesigner;
	using CrtContentDesigner.Contracts;
	using CrtContentDesigner.Models;
	using CrtContentDesigner.Repositories;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	/// <summary>
	/// Implements the ITemplateSave interface to save templates.
	/// </summary>
	[DefaultBinding(typeof(ITemplateLoad), Name = "LandingPage")]
	[DefaultBinding(typeof(ITemplateSave), Name = "LandingPage")]
	internal class LandingPageTemplateService: ITemplateSave, ITemplateLoad
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LandingPageTemplateService"/> class.
		/// </summary>
		/// <param name="userConnection"></param>
		public LandingPageTemplateService(UserConnection userConnection) {
			_userConnection = userConnection;
			_bfTemplateRepository =
#pragma warning disable
				ClassFactory.Get<ITemplateRepository>("DefaultHTML", new ConstructorArgument("userConnection", userConnection));
#pragma warning restore
		}

#pragma warning disable
		private readonly ITemplateRepository _bfTemplateRepository;
#pragma warning restore
		private readonly UserConnection _userConnection;

		/// <inheritdoc cref="ITemplateSave.HandlerCode"/>
		public string HandlerCode => "LandingPage";

		/// <inheritdoc cref="ITemplateSave.Priority"/>
		public int Priority => 10;

		/// <summary>
		/// Saves the template with the specified save request details.
		/// </summary>
		/// <param name="saveRequest">The details of the save request.</param>
		/// <returns>A boolean value indicating whether the save operation was successful.</returns>
		public bool SaveTemplate(SaveTemplateRequest saveRequest) {
			// TODO: RND-73921: check record related to landing page in HTMLPage and create it if it doesn't exist.
			return _bfTemplateRepository.SaveTemplate(saveRequest.Content);
		}

		public TemplateModel LoadTemplate(LoadTemplateRequest loadRequest, TemplateModel template) {
			return _bfTemplateRepository.LoadTemplate(loadRequest.RecordId);
		}
	}
}

