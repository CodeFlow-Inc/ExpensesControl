using ExpensesControl.Application.UseCases.Expenses.GetByUser.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Expenses.GetByUser.Validator;

/// <summary>
/// Validator class for validating the request to get expenses by user code.
/// Ensures that the UserCode is a positive integer.
/// </summary>
public class GetExpensesByUserCodeRequestValidator : AbstractValidator<GetExpensesByUserCodeRequest>
{
    public GetExpensesByUserCodeRequestValidator()
    {
        RuleFor(x => x.UserCode)
            .GreaterThan(0).WithMessage("O código do usuário deve ser um número inteiro positivo.");
    }
}
