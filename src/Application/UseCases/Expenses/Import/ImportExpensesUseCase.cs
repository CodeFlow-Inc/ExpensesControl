using CsvHelper;
using CsvHelper.Configuration;
using ExpensesControl.Application.UseCases.Base.Records.Expense;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Map;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Request;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Response;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ExpensesControl.Application.UseCases.Expenses.Import;
public class ImportExpensesUseCase(IUnitOfWork unitOfWork, ILogger<ImportExpensesUseCase> logger) : IRequestHandler<ImportExpensesRequest, ImportExpensesResponse>
{
	public async Task<ImportExpensesResponse> Handle(ImportExpensesRequest request, CancellationToken cancellationToken)
	{
		var response = new ImportExpensesResponse();
		var result = new ImportResponse();

		using (var stream = new StreamReader(request.File.OpenReadStream()))
		using (var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<ExpenseRecordMap>();
			var records = csv.GetRecords<ExpenseRecord>().ToList();
			foreach (var record in records)
			{
				try
				{
					#region TRANSACTION
					await unitOfWork.BeginTransactionAsync(cancellationToken);
					var createdExpense = await unitOfWork.ExpenseRepository.CreateAsync(
						new(
							request.UserCode,
							record.Description,
							record.StartDate,
							record.EndDate,
							record.Category,
							record.Recurrence.IsRecurring,
							record.Recurrence.Periodicity,
							record.Recurrence.MaxOccurrences,
							record.Payment.TotalValue,
							record.Payment.Type,
							record.Payment.IsInstallment,
							record.Payment.InstallmentCount
						)
						, cancellationToken);
					if (!createdExpense.Validate(out var domainErrors))
					{
						logger.LogWarning("Failed to validate domain.");
						return response.AddErrorMessages<CreateExpenseResponse>(domainErrors);
					}
					await unitOfWork.CommitAsync(cancellationToken);
					logger.LogInformation("Expense successfully created. ID: {ExpenseId}", createdExpense.Id);
					#endregion					result.SuccessfulRecords++;
				}
				catch (Exception ex)
				{
					result.FailedRecords++;
					result.Errors.Add($"Error processing record {record}: {ex.Message}");
				}
			}

			result.TotalRecords = records.Count;
			response.SetResult(result);
		}

		return response;
	}
}