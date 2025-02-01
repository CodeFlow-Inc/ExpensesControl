using System.ComponentModel;

namespace ExpensesControl.Domain.Enums;

/// <summary>
/// Enum representing recurrence periodicity.
/// </summary>
public enum RecurrencePeriodicity
{
	/// <summary>
	/// Recurs daily.
	/// </summary>
	[Description("Diariamente")]
	Daily = 10,

	/// <summary>
	/// Recurs weekly.
	/// </summary>
	[Description("Semanalmente")]
	Weekly = 20,

	/// <summary>
	/// Recurs monthly.
	/// </summary>
	[Description("Mensalmente")]
	Monthly = 30,

	/// <summary>
	/// Recurs yearly.
	/// </summary>
	[Description("Anualmente")]
	Yearly = 40
}
