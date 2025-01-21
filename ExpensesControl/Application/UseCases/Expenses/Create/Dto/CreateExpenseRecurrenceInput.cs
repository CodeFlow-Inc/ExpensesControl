using ExpensesControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

/// <summary>
/// Request to create a new recurrence for an expense.
/// </summary>
public class CreateExpenseRecurrenceInput
{
    /// <summary>
    /// Indicates whether the expense is recurring.
    /// </summary>
    [Required]
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Periodicity of the recurrence (e.g., daily, weekly, monthly, yearly).
    /// </summary>
    [Required]
    public RecurrencePeriodicity Periodicity { get; set; }

    /// <summary>
    /// Maximum number of occurrences for the recurrence.
    /// If null, the recurrence is indefinite.
    /// </summary>
    public int? MaxOccurrences { get; set; }
}
