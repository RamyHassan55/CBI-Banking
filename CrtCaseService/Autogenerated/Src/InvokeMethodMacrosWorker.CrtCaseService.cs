namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Configuration.Utils;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;
	using Terrasoft.Web.Http.Abstractions;

	#region Class: InvokeMethodMacrosWorker

	[MacrosWorker("{16339F82-6FF0-4C75-B20D-13F07A79F854}")]
	public class InvokeMethodMacrosWorker : IMacrosWorker
	{
		#region Fields: Private

		/// <summary>
		/// User connection.
		/// </summary>
		private UserConnection _userConnection;

		#endregion

		#region Properties: Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get {
				if (_userConnection == null) {
					_userConnection = HttpContext.Current.Session["UserConnection"] as UserConnection;
				}
				return _userConnection;
			}
			set {
				_userConnection = value;
			}
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Method that returns instance of macros handler class.
		/// </summary>
		/// <param name="className">Name of instantiated class.</param>
		/// <returns>Strategy class instance.</returns>
		protected virtual IMacrosInvokable GetClassInstance(string className) {
			var workspaceTypeProvider = ClassFactory.Get<IWorkspaceTypeProvider>();
			Type classType = workspaceTypeProvider.GetType(className);
			if (classType == null) {
				return null;
			}
			string assemblyName = classType.AssemblyQualifiedName;
			var macrosInvokable = ClassFactory.ForceGet<IMacrosInvokable>(assemblyName);
			return macrosInvokable;
		}

		#endregion

		#region Methods: Public

		public Dictionary<string, string> Proceed(IEnumerable<MacrosInfo> macrosInfoCollection) {
			return Proceed(macrosInfoCollection, null);
		}

		public Dictionary<string, string> Proceed(IEnumerable<MacrosInfo> macrosInfoCollection, object arguments) {
			var macrosValues = new Dictionary<string, string>();
			foreach (MacrosInfo macrosInfo in macrosInfoCollection) {
				string className = macrosInfo.ColumnPath;
				string parameterName = macrosInfo.Alias;
				IMacrosInvokable macrosInvokable = className != null ? GetClassInstance(className) : null;
				if (macrosInvokable == null) {
					return macrosValues;
				}
				try {
					macrosInvokable.UserConnection = UserConnection;
					macrosValues.Add(parameterName, macrosInvokable.GetMacrosValue(arguments));
				} catch (Exception e) {
					throw new NullReferenceException(e.Message);
				}
			}
			return macrosValues;
		}

		public Dictionary<object, Dictionary<string, string>> ProcceedCollection(
				IEnumerable<MacrosInfo> macrosInfoCollection) {
			return null;
		}

		public Dictionary<object, Dictionary<string, string>> ProcceedCollection(
				IEnumerable<MacrosInfo> macrosInfoCollection,
			object arguments) {
			return null;
		}

		#endregion
	}

	#endregion

}
