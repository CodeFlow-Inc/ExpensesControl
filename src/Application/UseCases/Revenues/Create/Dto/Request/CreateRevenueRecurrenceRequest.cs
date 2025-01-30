using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Revenues.Create.Dto.Request;

public record CreateRevenueRecurrenceRequest(
	bool IsRecurring,
	RecurrencePeriodicity Periodicity,
	int? MaxOccurrences);
