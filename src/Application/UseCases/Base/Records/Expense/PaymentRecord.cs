using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Records.Expense;

public record PaymentRecord(
	PaymentType Type,
	bool IsInstallment,
	int? InstallmentCount,
	string? Notes,
	decimal TotalValue)
{
	public decimal? InstallmentValue => InstallmentCount.HasValue && InstallmentCount > 0 ? TotalValue / InstallmentCount.Value : null;
}
