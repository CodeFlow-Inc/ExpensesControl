using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
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
    public class CreateExpenseUseCase(IExpenseRepository expenseRepository, ILogger<CreateExpenseUseCase> logger) : IRequestHandler<CreateExpenseInput, CreateExpenseOutput>
    {
        /// <summary>
        /// Handles the creation of a new expense.
        /// </summary>
        /// <param name="input">The input data for creating the expense.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An output object containing the result of the expense creation process.</returns>
        public async Task<CreateExpenseOutput> Handle(CreateExpenseInput input, CancellationToken cancellationToken)
        {
            using (LogContext.Push (
                        new PropertyEnricher("UserCode", input.UserCode)))
            {
                logger.LogInformation("Starting the process of creating a new expense.");

                var output = new CreateExpenseOutput();
                var expense = input.Adapt<Expense>();
                try
                {
                    if (expense.Validate(out var errorsDoamin))
                    {
                        logger.LogWarning("Failed to validate domain.");
                        output.AddErrorMessages(errorsDoamin);
                    }

                    var createdExpense = await expenseRepository.CreateAsync(expense);

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
                    output.AddErrorMessage(expectedError.Message);
                    return output;
                }
            }
        }
    }
}
