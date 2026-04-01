namespace Terrasoft.Core.Process
{

	using CrtCloudIntegration.Utilities;
	using CrtDigitalAdsApp;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: SynchronizeAdCampaignDataMethodsWrapper

	/// <exclude/>
	public class SynchronizeAdCampaignDataMethodsWrapper : ProcessModel
	{

		public SynchronizeAdCampaignDataMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ShowMessageNoConnectedAdAccountsScriptTaskExecute", ShowMessageNoConnectedAdAccountsScriptTaskExecute);
		}

		#region Methods: Private

		private bool ShowMessageNoConnectedAdAccountsScriptTaskExecute(ProcessExecutingContext context) {
			var notifier = ClassFactory.Get<IUINotifier>(CrtDigitalAdsAppConsts.UINotifierSenderName);
			notifier.Notify("no.connected.adaccounts");
			return true;
		}

		#endregion

	}

	#endregion

}

