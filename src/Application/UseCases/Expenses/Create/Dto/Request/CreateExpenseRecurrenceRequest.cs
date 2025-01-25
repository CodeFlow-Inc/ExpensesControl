using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public record CreateExpenseRecurrenceRequest(
	bool IsRecurring,
	RecurrencePeriodicity Periodicity,
	int? MaxOccurrences);
