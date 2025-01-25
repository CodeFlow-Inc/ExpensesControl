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
	Daily,

	/// <summary>
	/// Recurs weekly.
	/// </summary>
	[Description("Semanalmente")]
	Weekly,

	/// <summary>
	/// Recurs monthly.
	/// </summary>
	[Description("Mensalmente")]
	Monthly,

	/// <summary>
	/// Recurs yearly.
	/// </summary>
	[Description("Anualmente")]
	Yearly
}
