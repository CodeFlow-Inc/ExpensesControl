using ExpensesControl.Application.Extensions;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
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
        IUnitOfWork unitOfWork,
        IValidator<CreateExpenseRequest> validator,
        ILogger<CreateExpenseUseCase> logger) : IRequestHandler<CreateExpenseRequest, CreateExpenseResponse>
    {
        /// <summary>
        /// Handles the creation of a new expense.
        /// </summary>
        /// <param name="request">The request data for creating the expense.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An response object containing the result of the expense creation process.</returns>
        public async Task<CreateExpenseResponse> Handle(CreateExpenseRequest request, CancellationToken cancellationToken)
        {
            using (LogContext.Push(
                        new PropertyEnricher("UserCode", request.UserCode)))
            {
                logger.LogDebug("Starting the process of creating a new expense.");
                var response = new CreateExpenseResponse();

                if (!await validator.ValidateAndAddErrorsAsync(request, logger, response, cancellationToken))
                    return response;

                var expense = request.Adapt<Expense>();
                try
                {
                    expense.SetCurrentUser(request.UserCode.ToString());

                    #region TRANSACTION
                    await unitOfWork.BeginTransactionAsync(cancellationToken);
                    var createdExpense = await unitOfWork.ExpenseRepository.CreateAsync(expense);
                    if (!createdExpense.Validate(out var errorsDoamin))
                    {
                        logger.LogWarning("Failed to validate domain.");
                        return response.AddErrorMessages<CreateExpenseResponse>(errorsDoamin);
                    }
                    await unitOfWork.CommitAsync(cancellationToken);
                    logger.LogInformation("Expense successfully created. ID: {ExpenseId}", createdExpense.Id);
                    #endregion

                    response.SetResult(new(createdExpense.Id));
                    return response;
                }
                catch (Exception expectedError) when
                (
                    expectedError is InvalidOperationException ||
                    expectedError is KeyNotFoundException
                )
                {
                    logger.LogWarning("Expected error occurred: {ErrorMessage}", expectedError.Message);
                    return response.AddErrorMessage<CreateExpenseResponse>(expectedError.Message);
                }
            }
        }
    }
}
