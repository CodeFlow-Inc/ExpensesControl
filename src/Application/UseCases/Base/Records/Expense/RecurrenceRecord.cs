using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Base.Records.Expense;

public record RecurrenceRecord(
                bool IsRecurring,
                RecurrencePeriodicity Periodicity,
                int? MaxOccurrences)
{
    public RecurrenceRecord() : this(default, default, default)
    {
    }
}
	