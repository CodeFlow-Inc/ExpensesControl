using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Revenue.Create.Dto.Request
{
    public record CreateRevenueRecurrenceRequest(
        bool IsRecurring,
        RecurrencePeriodicity Periodicity,
        int? MaxOccurrences);
}
