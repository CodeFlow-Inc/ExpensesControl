using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Validator;

/// <summary>
/// Validator class for the CreateExpenseRequest DTO.
/// Validates the request data when creating a new expense.
/// </summary>
public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
{
	public CreateExpenseRequestValidator()
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
			.SetValidator(new CreateExpenseRecurrenceRequestValidator());

		RuleFor(x => x.PaymentMethod)
			.SetValidator(new CreateExpensePaymentRequestValidator());

		RuleFor(x => x.Notes)
			.MaximumLength(500).WithMessage("As notas devem ter no máximo 500 caracteres.");
	}
}
