using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities;

/// <summary>
/// Represents a payment method for an expense.
/// </summary>
public class PaymentMethod
{
    public PaymentMethod()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="type">The type of payment.</param>
    /// <param name="isInstallment">Whether the payment is in installments.</param>
    /// <param name="installmentCount">The number of installments.</param>
    /// <param name="installmentValue">The value of each installment.</param>
    /// <param name="notes">Additional notes about the payment.</param>
    public PaymentMethod(
        PaymentType type,
        bool isInstallment = false,
        int? installmentCount = null,
        decimal? installmentValue = null,
        string? notes = null)
    {
        Type = type;
        IsInstallment = isInstallment;
        InstallmentCount = installmentCount;
        InstallmentValue = installmentValue;
        Notes = notes;
    }

    /// <summary>
    /// Type of the payment (e.g., cash, credit card, debit card, etc.).
    /// </summary>
    public PaymentType Type { get; set; }

    /// <summary>
    /// Indicates whether the payment is divided into installments.
    /// </summary>
    public bool IsInstallment { get; set; }

    /// <summary>
    /// Number of installments, applicable if the payment is in installments.
    /// </summary>
    public int? InstallmentCount { get; set; }

    /// <summary>
    /// Value of each installment, applicable if the payment is in installments.
    /// </summary>
    public decimal? InstallmentValue { get; set; }

    /// <summary>
    /// Additional details or notes about the payment method.
    /// </summary>
    public string? Notes { get; set; }


    /// <summary>
    /// Validates the consistency of the payment method data.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
    public void Validate(decimal totalValue)
    {
        if (IsInstallment)
        {
            if (InstallmentCount == null || InstallmentCount <= 0)
            {
                throw new InvalidOperationException("The number of installments must be greater than zero for installment payments.");
            }

            if (InstallmentValue == null || InstallmentValue <= 0)
            {
                throw new InvalidOperationException("The installment value must be greater than zero for installment payments.");
            }

            if (InstallmentCount * InstallmentValue != totalValue)
            {
                throw new InvalidOperationException("The total value of installments must equal the total payment amount.");
            }
        }
        else
        {
            if (InstallmentCount.HasValue || InstallmentValue.HasValue)
            {
                throw new InvalidOperationException("Non-installment payments should not have installment count or installment value.");
            }
        }
    }
}
