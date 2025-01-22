using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

public class CreateExpenseRecurrenceInput
{
    public bool IsRecurring { get; set; }
    public RecurrencePeriodicity Periodicity { get; set; }
    public int? MaxOccurrences { get; set; }
}
