using ExpensesControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

/// <summary>
/// Request to create a new payment for an expense.
/// </summary>
public class CreateExpensePaymentInput
{
    /// <summary>
    /// Type of the payment (e.g., cash, credit card, debit card, etc.).
    /// </summary>
    [Required]
    public PaymentType Type { get; set; }

    /// <summary>
    /// Indicates whether the payment is divided into installments.
    /// </summary>
    [Required]
    public bool IsInstallment { get; set; }

    /// <summary>
    /// Number of installments, applicable if the payment is in installments.
    /// </summary>
    public int? InstallmentCount { get; set; }

    /// <summary>
    /// Total value of the payment (e.g., total cost of the expense).
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Additional details or notes about the payment method.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
