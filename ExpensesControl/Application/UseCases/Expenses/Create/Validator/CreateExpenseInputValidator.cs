using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Validator;

/// <summary>
/// Validator class for the CreateExpenseInput DTO.
/// Validates the input data when creating a new expense.
/// </summary>
public class CreateExpenseInputValidator : AbstractValidator<CreateExpenseInput>
{
    public CreateExpenseInputValidator()
    {
        RuleFor(x => x.UserCode)
            .NotEmpty().WithMessage("O código do usuário é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória e deve ter no máximo 255 caracteres.")
            .MaximumLength(255);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("A data de início é obrigatória.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.Recurrence)
            .SetValidator(new CreateExpenseRecurrenceInputValidator());

        RuleFor(x => x.PaymentMethod)
            .SetValidator(new CreateExpensePaymentInputValidator());

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("As notas devem ter no máximo 500 caracteres.");
    }
}
