using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Expense;

public record PaymentResponse(
    PaymentType Type,
    bool IsInstallment,
    int? InstallmentCount,
    string? Notes,
    decimal TotalValue)
{
    public decimal? InstallmentValue => InstallmentCount.HasValue && InstallmentCount > 0 ? TotalValue / InstallmentCount.Value : null;
}
