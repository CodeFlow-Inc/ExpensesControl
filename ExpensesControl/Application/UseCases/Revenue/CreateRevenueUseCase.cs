using ExpensesControl.Application.Extensions;
using ExpensesControl.Application.UseCases.Revenue.Dto.Request;
using ExpensesControl.Application.UseCases.Revenue.Dto.Response;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace ExpensesControl.Application.UseCases.Revenue
{   /// <summary>
    /// Handles the creation of a new revenue.
    /// </summary>
    /// <param name="revenueRepository">The repository for managing revenue.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public class CreateRevenueUseCase(
        IUnitOfWork unitOfWork,
        IValidator<CreateRevenueRequest> validator,
        ILogger<CreateRevenueUseCase> logger) : IRequestHandler<CreateRevenueRequest, CreateRevenueResponse>
    {
        /// <summary>
        /// Handles the creation of a new revenue.
        /// </summary>
        /// <param name="request">The request data for creating the revenue.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An response object containing the result of the revenue creation process.</returns>
        public async Task<CreateRevenueResponse> Handle(CreateRevenueRequest request, CancellationToken cancellationToken)
        {
            using (LogContext.Push(
                        new PropertyEnricher("UserCode", request.UserCode)))
            {
                logger.LogDebug("Starting the process of creating a new renevue.");
                var response = new CreateRevenueResponse();

                if (!await validator.ValidateAndAddErrorsAsync(request, logger, response, cancellationToken))
                    return response;

                var renevue = request.Adapt<Domain.Entities.AggregateRoot.Revenue>();
                try
                {
                    renevue.SetCurrentUser(request.UserCode.ToString());

                    #region TRANSACTION
                    await unitOfWork.BeginTransactionAsync(cancellationToken);
                    var createdRevenue = await unitOfWork.RevenueRepository.CreateAsync(renevue, cancellationToken);
                    if (!createdRevenue.Validate(out var domainErrors))
                    {
                        logger.LogWarning("Failed to validate domain.");
                        return response.AddErrorMessages<CreateRevenueResponse>(domainErrors);
                    }
                    await unitOfWork.CommitAsync(cancellationToken);
                    logger.LogInformation("Revenue successfully created. ID: {ExpenseId}", createdRevenue.Id);
                    #endregion

                    response.SetResult(new(createdRevenue.Id));
                    return response;
                }
                catch (Exception expectedError) when
                (
                    expectedError is InvalidOperationException ||
                    expectedError is KeyNotFoundException
                )
                {
                    await unitOfWork.RollbackAsync();
                    logger.LogWarning("Expected error occurred: {ErrorMessage}", expectedError.Message);
                    return response.AddErrorMessage<CreateRevenueResponse>(expectedError.Message);
                }
            }
        }
    }
}