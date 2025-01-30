using ExpensesControl.Application.UseCases.Revenues.Create.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Revenues.Create.Validator;

public class CreateRevenueRecurrenceRequestValidator : AbstractValidator<CreateRevenueRecurrenceRequest>
{
	public CreateRevenueRecurrenceRequestValidator()
	{
		RuleFor(x => x.IsRecurring)
			.NotNull().WithMessage("A informação sobre recorrência é obrigatória.");

		RuleFor(x => x.Periodicity)
			.IsInEnum().WithMessage("A periodicidade é obrigatória.");
	}
}
