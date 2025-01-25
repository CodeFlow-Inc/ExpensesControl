using ExpensesControl.Application.UseCases.Revenue.Create.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Revenue.Create.Validator
{
    public class CreateRevenueValidator : AbstractValidator<CreateRevenueRequest>
    {
        public CreateRevenueValidator()
        {
            RuleFor(x => x.UserCode)
                .NotEmpty().WithMessage("O código do usuário é obrigatório.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória e deve ter no máximo 255 caracteres.")
                .MaximumLength(255);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("O saldo dessa receita é obrigatória.");

            RuleFor(x => x.ReceiptDate)
                .NotEmpty().WithMessage("A data da receita é obrigatória.");

            RuleFor(x => x.Recurrence)
                .SetValidator(new CreateRevenueRecurrenceRequestValidator());

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("O tipo da receita é obrigatório e deve ser um valor válido.");
        }
    }
}
