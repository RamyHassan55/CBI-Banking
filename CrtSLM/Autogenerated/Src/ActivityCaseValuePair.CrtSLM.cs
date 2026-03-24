namespace Terrasoft.Configuration
{
	using System;

	#region Class: ActivityCaseValuePair

	/// <summary>
	/// Represents the value pair of case and activity.
	/// </summary>
	public sealed class ActivityCaseValuePair
	{

		#region Properties: Public

		/// <summary>
		/// Activity identifier.
		/// </summary>
		public Guid ActivityId {
			get;
			set;
		}

		/// <summary>
		/// Case identifier.
		/// </summary>
		public Guid CaseId {
			get;
			set;
		}

		#endregion

	}

	#endregion

}
