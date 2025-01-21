using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Domain.Enums;
using ExpensesControl.Domain.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

/// <summary>
/// Request to create a new expense.
/// </summary>
public class CreateExpenseInput : BaseInput<CreateExpenseOutput>
{
    /// <summary>
    /// User code associated with the expense.
    /// </summary>
    [Required]
    public int UserCode { get; set; }

    /// <summary>
    /// Description of the expense.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the expense.
    /// </summary>
    [Required]
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the expense (optional).
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Expense category.
    /// </summary>
    [Required]
    public ExpenseCategory Category { get; set; }

    /// <summary>
    /// Recurrence details of the expense.
    /// </summary>
    [Required]
    public CreateExpenseRecurrenceInput Recurrence { get; set; } = new CreateExpenseRecurrenceInput();

    /// <summary>
    /// Payment method of the expense.
    /// </summary>
    [Required]
    public CreateExpensePaymentInput PaymentMethod { get; set; } = new CreateExpensePaymentInput();

    /// <summary>
    /// Additional notes about the expense.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
