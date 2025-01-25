using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Expense;

public record RecurrenceResponse(
	bool IsRecurring,
	RecurrencePeriodicity Periodicity,
	int? MaxOccurrences);
