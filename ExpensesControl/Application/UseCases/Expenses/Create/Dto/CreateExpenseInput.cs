using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

public class CreateExpenseInput : BaseInput<CreateExpenseOutput>
{
    public int UserCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ExpenseCategory Category { get; set; }
    public CreateExpenseRecurrenceInput Recurrence { get; set; } = new CreateExpenseRecurrenceInput();
    public CreateExpensePaymentInput PaymentMethod { get; set; } = new CreateExpensePaymentInput();
    public string? Notes { get; set; }
}
