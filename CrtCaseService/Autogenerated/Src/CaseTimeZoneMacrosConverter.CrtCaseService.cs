namespace Terrasoft.Configuration
{
	using System;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;

	#region Class: CaseTimeZoneMacrosConverter

	/// <summary>
	/// Represents converter for DateTime macroces in Case entity.
	/// </summary>
	public class CaseTimeZoneMacrosConverter: BaseTimeZoneMacrosConverter
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes new instance of <see cref="CaseTimeZoneMacrosConverter"/> class.
		/// </summary>
		/// <param name="userConnection">User connection.</param>
		/// <param name="schemaName">Entity schema name.</param>
		public CaseTimeZoneMacrosConverter(UserConnection userConnection, string schemaName)
			: base(userConnection, schemaName) {
		}

		#endregion

		#region Methods: Protected

		protected override TimeZoneInfo GetTimeZone(Guid entityRecordId) {
			ITimeZoneProvider<Entity> provider = ClassFactory.Get<EntityTimeZoneProvider>(
				new ConstructorArgument("userConnection", UserConnection),
				new ConstructorArgument("caseId", entityRecordId)
			);
			return provider.GetTimeZone();
		}

		#endregion

	}

	#endregion

}
