using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Expense;

public record ExpenseResponse(
	string? Description,
	DateOnly StartDate,
	DateOnly? EndDate,
	ExpenseCategory Category,
	RecurrenceResponse Recurrence,
	PaymentResponse Payment,
	string? Notes);
