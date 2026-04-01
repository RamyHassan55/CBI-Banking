using System;

namespace Terrasoft.Configuration.ServiceTerm
{
	/// <summary>
	/// Represents service term calculator.
	/// </summary>
	public interface ITermCalculator
	{
		/// <summary>
		/// Calculate response date.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <returns>Response date.</returns>
		DateTime CalculateResponseDate(DateTime startDate);
		/// <summary>
		/// Calculate solution date.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <returns>Solution date.</returns>
		DateTime CalculateSolutionDate(DateTime startDate);
	}
}
