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
	private static readonly ExpenseRecordMap expenseRecordMap = new();
	private readonly CsvFileValidator csvFileValidator = new(expenseRecordMap);

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
			return response.AddErrorMessage<ImportExpensesResponse>("O arquivo não é do tipo CSV.");
		}

		var configuration = new CsvConfiguration(CultureInfo.CurrentCulture)
		{
			Delimiter = ";",
			HasHeaderRecord = true,
			IgnoreBlankLines = true,
			TrimOptions = TrimOptions.Trim,
			BadDataFound = null,
			Quote = '"'
		};
		using (var stream = new StreamReader(request.File.OpenReadStream()))
		{

			if (!csvFileValidator.HasValidHeaders(stream.BaseStream, configuration, out var headerErrors))
			{
				return response.AddErrorMessages<ImportExpensesResponse>(headerErrors);
			}
		}

		using (var stream = new StreamReader(request.File.OpenReadStream()))
		using (var csv = new CsvReader(stream, configuration))
		{
			csv.Context.RegisterClassMap(expenseRecordMap);
			var records = csv.GetRecords<ExpenseRecord>().ToList();

			foreach (var record in records)
			{
				try
				{
					await unitOfWork.BeginTransactionAsync(cancellationToken);
					var createdExpense = await unitOfWork.ExpenseRepository.CreateAsync(
						new(
							request.UserCode,
							record.Description,
							record.StartDate,
							record.EndDate,
							record.Category,
							record.Recurrence!.IsRecurring,
							record.Recurrence.Periodicity,
							record.Recurrence.MaxOccurrences,
							record.Payment!.TotalValue,
							record.Payment.Type,
							record.Payment.IsInstallment,
							record.Payment.InstallmentCount
						), cancellationToken);
					createdExpense.SetCurrentUser(request.UserCode.ToString());

					if (!createdExpense.Validate(out var domainErrors))
					{
						logger.LogWarning("Failed to validate domain.");
						result.Errors.Add($"Erro processando o registro com erro: {domainErrors}");
						result.FailedRecords++;
						await unitOfWork.RollbackAsync();
						continue;
					}
					await unitOfWork.CommitAsync(cancellationToken);
					result.SuccessfulRecords++;
				}
				catch (Exception ex)
				{
					result.FailedRecords++;
					result.Errors.Add($"Erro processando o registro com erro: {ex.Message}");
					await unitOfWork.RollbackAsync();
				}
			}

			result.TotalRecords = records.Count;
			logger.LogInformation("Process import has completed.");
			response.SetResult(result);
		}

		return response;
	}
}