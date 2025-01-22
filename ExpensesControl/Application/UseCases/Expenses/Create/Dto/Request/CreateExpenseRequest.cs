using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public class CreateExpenseRequest : BaseRequest<CreateExpenseResponse>
{
    public int UserCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ExpenseCategory Category { get; set; }
    public CreateExpenseRecurrenceRequest Recurrence { get; set; } = new CreateExpenseRecurrenceRequest();
    public CreateExpensePaymentRequest PaymentMethod { get; set; } = new CreateExpensePaymentRequest();
    public string? Notes { get; set; }
}
