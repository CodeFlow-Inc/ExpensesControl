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
    /// Validates the consistency of the payment method data.
    /// </summary>
    /// <param name="totalValue">The total value of the payment.</param>
    /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
    public void Validate(decimal totalValue)
    {
        if (IsInstallment)
        {
            if (InstallmentCount == null || InstallmentCount <= 0)
            {
                throw new InvalidOperationException("O número de parcelas deve ser maior que zero para pagamentos parcelados.");
            }

            if (InstallmentValue == null || InstallmentValue <= 0)
            {
                throw new InvalidOperationException("O valor da parcela deve ser maior que zero para pagamentos parcelados.");
            }

            if (InstallmentCount * InstallmentValue != totalValue)
            {
                throw new InvalidOperationException("O valor total das parcelas deve ser igual ao valor total do pagamento.");
            }
        }
        else
        {
            if (InstallmentCount.HasValue || InstallmentValue.HasValue)
            {
                throw new InvalidOperationException("Pagamentos não parcelados não devem ter número de parcelas ou valor de parcela.");
            }
        }
    }
}
