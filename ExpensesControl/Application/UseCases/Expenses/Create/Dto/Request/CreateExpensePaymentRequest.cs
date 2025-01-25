using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public record CreateExpensePaymentRequest(
    PaymentType Type,
    bool IsInstallment,
    int? InstallmentCount,
    decimal TotalValue,
    string? Notes);
