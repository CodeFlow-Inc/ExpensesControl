using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public class CreateExpenseRecurrenceRequest
{
    public bool IsRecurring { get; set; }
    public RecurrencePeriodicity Periodicity { get; set; }
    public int? MaxOccurrences { get; set; }
}
