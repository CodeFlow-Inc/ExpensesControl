using ExpensesControl.Application.Extensions;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace ExpensesControl.Application.UseCases.Expenses.Create
{
    /// <summary>
    /// Handles the creation of a new expense.
    /// </summary>
    /// <param name="expenseRepository">The repository for managing expenses.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public class CreateExpenseUseCase(
        IExpenseRepository expenseRepository,
        IValidator<CreateExpenseInput> validator,
        ILogger<CreateExpenseUseCase> logger) : IRequestHandler<CreateExpenseInput, CreateExpenseOutput>
    {
        /// <summary>
        /// Handles the creation of a new expense.
        /// </summary>
        /// <param name="input">The input data for creating the expense.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An output object containing the result of the expense creation process.</returns>
        public async Task<CreateExpenseOutput> Handle(CreateExpenseInput input, CancellationToken cancellationToken)
        {
            using (LogContext.Push(
                        new PropertyEnricher("UserCode", input.UserCode)))
            {
                logger.LogDebug("Starting the process of creating a new expense.");
                var output = new CreateExpenseOutput();

                if (!await validator.ValidateAndAddErrorsAsync(input, logger, output, cancellationToken))
                    return output;

                var expense = input.Adapt<Expense>();
                try
                {
                    expense.SetCurrentUser(input.UserCode.ToString());
                    var createdExpense = await expenseRepository.CreateAsync(expense);
                    if (!createdExpense.Validate(out var errorsDoamin))
                    {
                        logger.LogWarning("Failed to validate domain.");
                        return output.AddErrorMessages<CreateExpenseOutput>(errorsDoamin);
                    }

                    _ = await expenseRepository.SaveChangesAsync();
                    logger.LogInformation("Expense successfully created. ID: {ExpenseId}", createdExpense.Id);

                    output.SetResult(new(createdExpense.Id));
                    return output;
                }
                catch (Exception expectedError) when
                (
                    expectedError is InvalidOperationException ||
                    expectedError is KeyNotFoundException
                )
                {
                    logger.LogWarning("Expected error occurred: {ErrorMessage}", expectedError.Message);
                    return output.AddErrorMessage<CreateExpenseOutput>(expectedError.Message);
                }
            }
        }
    }
}
