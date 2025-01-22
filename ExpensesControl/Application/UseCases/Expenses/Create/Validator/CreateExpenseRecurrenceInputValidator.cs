using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Validator
{
    /// <summary>
    /// Validator class for the CreateExpenseRecurrenceInput DTO.
    /// Validates the recurrence details when creating a new expense.
    /// </summary>
    public class CreateExpenseRecurrenceInputValidator : AbstractValidator<CreateExpenseRecurrenceInput>
    {
        public CreateExpenseRecurrenceInputValidator()
        {
            RuleFor(x => x.IsRecurring)
                .NotNull().WithMessage("A informação sobre recorrência é obrigatória.");

            RuleFor(x => x.Periodicity)
                .IsInEnum().WithMessage("A periodicidade é obrigatória.");
        }
    }
}
