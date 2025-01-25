using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Validator;

/// <summary>
/// Validator class for the CreateExpenseRecurrenceRequest DTO.
/// Validates the recurrence details when creating a new expense.
/// </summary>
public class CreateExpenseRecurrenceRequestValidator : AbstractValidator<CreateExpenseRecurrenceRequest>
{
    public CreateExpenseRecurrenceRequestValidator()
    {
        RuleFor(x => x.IsRecurring)
            .NotNull().WithMessage("A informação sobre recorrência é obrigatória.");

        RuleFor(x => x.Periodicity)
            .IsInEnum().WithMessage("A periodicidade é obrigatória.");
    }
}
