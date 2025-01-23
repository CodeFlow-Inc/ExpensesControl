using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Validator;

/// <summary>
/// Validator class for the CreateExpensePaymentRequest DTO.
/// Validates the payment details when creating a new expense.
/// </summary>
public class CreateExpensePaymentRequestValidator : AbstractValidator<CreateExpensePaymentRequest>
{
    public CreateExpensePaymentRequestValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("O tipo de pagamento é obrigatório.");

        RuleFor(x => x.IsInstallment)
            .NotNull().WithMessage("A informação sobre parcelamento é obrigatória.");

        RuleFor(x => x.TotalValue)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("As notas devem ter no máximo 500 caracteres.");
    }
}
