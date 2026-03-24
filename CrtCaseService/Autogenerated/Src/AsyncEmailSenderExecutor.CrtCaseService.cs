namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Creatio.FeatureToggling;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Mail.Sender;

	#region Class : AsyncEmailSenderExecutor

	/// <summary>
	/// Represents a asynchronous email sending job.
	/// </summary>
	public class AsyncEmailSenderExecutor : IJobExecutor 
	{

		#region Methods : Public

		/// <summary>
		/// Send email asynchronously.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="parameters">Parameters.</param>
		public virtual void Execute(UserConnection userConnection, IDictionary<string, object> parameters) {
			if (parameters == null) {
				throw new ArgumentNullOrEmptyException("parameters can't be null");
			}
			Guid activityId = parameters.GetValue("activityId", Guid.Empty);
			if (activityId != Guid.Empty) {
				var userConnectionArg = new ConstructorArgument("userConnection", userConnection);
				var emailClientFactory = ClassFactory.Get<EmailClientFactory>(userConnectionArg);
				ActivityEmailSenderBase sender = Features.GetIsEnabled("SecureEstimation") 
					? new SecureActivityEmailSenderBase(emailClientFactory, userConnection)
					: new ActivityEmailSenderBase(emailClientFactory, userConnection);
				sender.Send(activityId);
			} else {
				throw new ArgumentException("ActivityId must be specified");
			}
		}

		#endregion

	}

	#endregion

}
