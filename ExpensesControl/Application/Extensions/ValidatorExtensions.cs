using FluentValidation;
using ExpensesControl.Application.UseCases.Base;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Application.Extensions;

public static class ValidatorExtensions
{
    /// <summary>
    /// Validates an input using FluentValidation and adds error messages to the output if validation fails.
    /// </summary>
    /// <typeparam name="TInput">The type of the input object to validate.</typeparam>
    /// <param name="validator">The FluentValidation validator instance.</param>
    /// <param name="input">The input object to validate.</param>
    /// <param name="output">The output object to add error messages to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean indicating whether the validation succeeded or not.</returns>
    public static async Task<bool> ValidateAndAddErrorsAsync<TInput>(this IValidator<TInput> validator, TInput input, ILogger logger,  BaseOutput output, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed. Errors: {ValidationErrors}", string.Join(", ", output.ErrorMessages));
            output.AddErrorMessages<BaseOutput>(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            return false;
        }

        return true;
    }
}
