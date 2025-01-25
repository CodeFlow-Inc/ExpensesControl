using ExpensesControl.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesControl.Domain.Entities.ValueObjects;

/// <summary>
/// Represents a payment method for an expense.
/// </summary>
public class Payment
{
	public Payment()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Payment"/> class.
	/// </summary>
	/// <param name="type">The type of payment.</param>
	/// <param name="isInstallment">Indicates whether the payment is in installments.</param>
	/// <param name="installmentCount">The number of installments.</param>
	/// <param name="notes">Additional notes about the payment.</param>
	public Payment(
		PaymentType type,
		bool isInstallment = false,
		int? installmentCount = null,
		string? notes = null)
	{
		Type = type;
		IsInstallment = isInstallment;
		InstallmentCount = installmentCount;
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
	/// Additional details or notes about the payment method.
	/// </summary>
	public string? Notes { get; set; }

	/// <summary>
	/// Gets the value of each installment based on the total value and installment count.
	/// </summary>
	[NotMapped]
	public decimal? InstallmentValue => InstallmentCount.HasValue && InstallmentCount > 0 ? TotalValue / InstallmentCount.Value : null;

	/// <summary>
	/// Total value of the payment (e.g., total cost of the expense).
	/// </summary>
	public decimal TotalValue { get; set; }

	/// <summary>
	/// Validates the installment payment rules and adds error messages to the response parameter.
	/// </summary>
	/// <param name="errors">An response list containing error messages if validation fails.</param>
	/// <returns>
	/// Returns <c>true</c> if validation passes without errors; otherwise, <c>false</c>.
	/// </returns>
	public bool Validate(out List<string> errors)
	{
		errors = [];

		if (IsInstallment)
		{
			if (InstallmentCount == null || InstallmentCount <= 0)
			{
				errors.Add("O número de parcelas deve ser maior que zero para pagamentos parcelados.");
			}

			if (InstallmentValue <= 0)
			{
				errors.Add("O valor da parcela deve ser maior que zero para pagamentos parcelados.");
			}

			if (InstallmentCount.HasValue && InstallmentValue > 0 && InstallmentCount.Value * InstallmentValue != TotalValue)
			{
				errors.Add("O valor total das parcelas deve ser igual ao valor total do pagamento.");
			}
		}
		else
		{
			if (InstallmentCount.HasValue)
			{
				errors.Add("Pagamentos não parcelados não devem ter número de parcelas.");
			}
		}

		return errors.Count == 0;
	}
}
