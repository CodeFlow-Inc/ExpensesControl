using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Records.Expense;

public record ExpenseRecord(
	string? Description,
	DateOnly StartDate,
	DateOnly? EndDate,
	ExpenseCategory Category,
	RecurrenceRecord Recurrence,
	PaymentRecord Payment,
	string? Notes);
