using CsvHelper;
using CsvHelper.Configuration;
using ExpensesControl.Application.UseCases.Base.Records.Expense;
using ExpensesControl.Application.UseCases.Base.Response;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Map;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Request;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Response;
using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Validator;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ExpensesControl.Application.UseCases.Expenses.Import;

/// <summary>
/// Use case for importing expenses from a CSV file.s
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
public class ImportExpensesUseCase(
	IUnitOfWork unitOfWork,
	ILogger<ImportExpensesUseCase> logger) : IRequestHandler<ImportExpensesRequest, ImportExpensesResponse>
{
	/// <summary>
	/// 
	/// </summary>
	private readonly CsvFileValidator csvFileValidator = new(new ExpenseRecordMap());

	/// <summary>
	/// Handles the process of importing expenses from a CSV file.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<ImportExpensesResponse> Handle(
		ImportExpensesRequest request,
		CancellationToken cancellationToken)
	{
		var response = new ImportExpensesResponse();
		var result = new ImportResponse();

		if (!CsvFileValidator.IsCsvFile(request.File.FileName))
		{
			return response.AddErrorMessage<ImportExpensesResponse>("The file is not a CSV.");
		}

		using (var stream = new StreamReader(request.File.OpenReadStream()))
		{
			var configuration = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				Delimiter = ";",
				HasHeaderRecord = true,
				IgnoreBlankLines = true,
				TrimOptions = TrimOptions.Trim,
			};

			if (!csvFileValidator.HasValidHeaders(stream.BaseStream, configuration, out var headerErrors))
			{
				return response.AddErrorMessages<ImportExpensesResponse>(headerErrors);
			}

			using var csv = new CsvReader(stream, configuration);
			csv.Context.RegisterClassMap<ExpenseRecordMap>();
			var records = csv.GetRecords<ExpenseRecord>().ToList();

			await unitOfWork.BeginTransactionAsync(cancellationToken);

			foreach (var record in records)
			{
				try
				{
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
						), cancellationToken);
					if (!createdExpense.Validate(out var domainErrors))
					{
						logger.LogWarning("Failed to validate domain.");
						result.Errors.Add($"Error processing record {record}: {domainErrors}");
						result.FailedRecords++;
						continue;
					}
					result.SuccessfulRecords++;
				}
				catch (Exception ex)
				{
					result.FailedRecords++;
					result.Errors.Add($"Error processing record {record}: {ex.Message}");
				}
			}
			result.TotalRecords = records.Count;

			await unitOfWork.CommitAsync(cancellationToken);
			logger.LogInformation("Process import has completed.");

			response.SetResult(result);
		}

		return response;
	}
}