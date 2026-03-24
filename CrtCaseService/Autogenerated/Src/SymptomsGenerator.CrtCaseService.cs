namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using Common;
	using Core;
	using Core.Entities;

	#region Class: SymptomsGenerator

	public class SymptomsGenerator : IMacrosInvokable
	{
		#region Methods : Protected

		/// <summary>
		/// Gets case id from argument object.
		/// </summary>
		/// <param name="argument">Argument object.</param>
		/// <returns>Record id.</returns>
		protected virtual Guid GetCaseId(object argument) {
			var recordArgument = argument as KeyValuePair<string, Guid>?;
			return recordArgument.HasValue ? recordArgument.Value.Value : Guid.Empty;
		}

		#endregion

		#region Properties : Public

		/// <summary>
		/// User connection.
		/// </summary>
		public UserConnection UserConnection {
			get;
			set;
		}

		#endregion

		#region Methods : Public

		/// <summary>
		/// Returns string value for macros.
		/// </summary>
		/// <param name="arguments">Arguments object.</param>
		/// <returns>Result string.</returns>
		public string GetMacrosValue(object arguments) {
			Guid caseId = GetCaseId(arguments);
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			esq.AddColumn("Symptoms");
			var caseEntity = esq.GetEntity(UserConnection, caseId);
			var symptoms = caseEntity.GetTypedColumnValue<string>("Symptoms");
			if (symptoms.IsNotNullOrEmpty()) {
				symptoms = Regex.Replace(symptoms, @"[\r\n]{2,}", "\r\n");
				symptoms = symptoms.Replace("\r\n", "<br />").TrimEnd("<br />".ToCharArray());
			}
			return symptoms;
		}

		#endregion

	}

	#endregion

}
