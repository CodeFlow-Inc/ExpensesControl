using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.ValueObjects;

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
    /// <param name="isInstallment">Indicates whether the payment is in installments.</param>
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
    /// Validates the installment payment rules and adds error messages to the output parameter.
    /// </summary>
    /// <param name="totalValue">The total value of the payment.</param>
    /// <param name="errors">An output list containing error messages if validation fails.</param>
    /// <returns>
    /// Returns <c>true</c> if validation passes without errors; otherwise, <c>false</c>.
    /// </returns>
    public bool Validate(decimal totalValue, out List<string> errors)
    {
        errors = [];

        if (IsInstallment)
        {
            if (InstallmentCount == null || InstallmentCount <= 0)
            {
                errors.Add("The number of installments must be greater than zero for installment payments.");
            }

            if (InstallmentValue == null || InstallmentValue <= 0)
            {
                errors.Add("The installment value must be greater than zero for installment payments.");
            }

            if (InstallmentCount.HasValue && InstallmentValue.HasValue &&
                InstallmentCount * InstallmentValue != totalValue)
            {
                errors.Add("The total value of installments must equal the total payment value.");
            }
        }
        else
        {
            if (InstallmentCount.HasValue || InstallmentValue.HasValue)
            {
                errors.Add("Non-installment payments must not have installment count or installment value.");
            }
        }

        return errors.Count == 0;
    }
}
