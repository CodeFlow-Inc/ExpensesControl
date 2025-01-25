using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;
using ExpensesControl.Domain.Enums;
using MediatR;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public record CreateExpenseRequest(
	int UserCode,
	DateOnly StartDate,
	DateOnly? EndDate,
	ExpenseCategory Category,
	CreateExpenseRecurrenceRequest Recurrence,
	CreateExpensePaymentRequest PaymentMethod,
	string? Notes) : IRequest<CreateExpenseResponse>
{
	public string Description { get; set; } = string.Empty;
}
