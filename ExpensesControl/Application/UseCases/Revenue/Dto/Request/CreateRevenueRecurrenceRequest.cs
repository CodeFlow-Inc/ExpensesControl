using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Revenue.Dto.Request
{
    public class CreateRevenueRecurrenceRequest
    {
        public bool IsRecurring { get; set; }
        public RecurrencePeriodicity Periodicity { get; set; }
        public int? MaxOccurrences { get; set; }
    }
}
