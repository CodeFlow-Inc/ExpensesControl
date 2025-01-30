using ClosedXML.Excel;
using CodeFlow.Start.Package.Extensions;
using ExpensesControl.Application.Specs;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Response;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Domain.Enums;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ExpensesControl.Application.UseCases.Exports.MonthlyReport;

public class ExportMonthlyReportUseCase(
	IUnitOfWork unitOfWork,
	IValidator<ExportMonthlyReportRequest> validator,
	ILogger<ExportMonthlyReportUseCase> logger) : IRequestHandler<ExportMonthlyReportRequest, ExportMonthlyReportResponse>
{
	private const string CurrencyFormat = "R$ #,##0.00;[Red]-R$ #,##0.00";
	private static readonly CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

	public async Task<ExportMonthlyReportResponse> Handle(ExportMonthlyReportRequest request, CancellationToken cancellationToken)
	{
		var response = new ExportMonthlyReportResponse();

		if (!await validator.ValidateAndAddErrorsAsync(request, logger, response, cancellationToken))
			return response;

		var expenseSpec = new ExpenseSpec(request.UserCode, request.Month, request.Year);
		var expenses = await unitOfWork.ExpenseRepository.ListBySpecificationAsync(expenseSpec, cancellationToken);

		var revenueSpec = new RevenueSpec(request.UserCode, request.Month, request.Year);
		var revenues = await unitOfWork.RevenueRepository.ListBySpecificationAsync(revenueSpec, cancellationToken);

		using var workbook = new XLWorkbook();

		AddExpensesWorksheet(workbook, expenses);
		AddRevenuesWorksheet(workbook, revenues);
		AddBalanceWorksheet(workbook, expenses, revenues);

		using var stream = new MemoryStream();
		workbook.SaveAs(stream);

		response.SetResult(
			stream.ToArray(),
			$"relatorio_{request.Month:00}_{request.Year}.xlsx",
			"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

		logger.LogInformation("Relatório gerado para o usuário {UserCode}", request.UserCode);
		return response;
	}

	private static void AddExpensesWorksheet(XLWorkbook workbook, IEnumerable<Expense> expenses)
	{
		var worksheet = workbook.Worksheets.Add("Despesas");

		// Cabeçalho
		var headerRange = worksheet.Range("A1:F1");
		headerRange.Style.Font.Bold = true;
		headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
		headerRange.Style.Font.FontColor = XLColor.White;
		headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

		worksheet.Cell(1, 1).Value = "Descrição";
		worksheet.Cell(1, 2).Value = "Data";
		worksheet.Cell(1, 3).Value = "Categoria";
		worksheet.Cell(1, 4).Value = "Valor Total";
		worksheet.Cell(1, 5).Value = "Tipo de Pagamento";
		worksheet.Cell(1, 6).Value = "Parcelado";

		// Dados
		var row = 2;
		foreach (var expense in expenses)
		{
			var currentRow = worksheet.Row(row);

			// Zebrado apenas na área de dados
			currentRow.Style.Fill.BackgroundColor = row % 2 == 0
				? XLColor.White
				: XLColor.LightGray;

			worksheet.Cell(row, 1).Value = expense.Description;
			worksheet.Cell(row, 2).Value = expense.StartDate.ToString("d", CurrentCulture);
			worksheet.Cell(row, 3).Value = expense.Category.ToString();
			worksheet.Cell(row, 4).Value = expense.Payment.TotalValue;
			worksheet.Cell(row, 4).Style.NumberFormat.Format = CurrencyFormat;
			worksheet.Cell(row, 5).Value = expense.Payment.Type.ToString();
			worksheet.Cell(row, 6).Value = expense.Payment.IsInstallment ? "Sim" : "Não";

			if (expense.Payment.TotalValue > 1000)
			{
				worksheet.Cell(row, 4).Style.Fill.BackgroundColor = XLColor.LightSalmon;
				worksheet.Cell(row, 4).Style.Font.Bold = true;
			}

			row++;
		}

		// Formatação apenas na área de dados
		if (row > 2)
		{
			var dataRange = worksheet.Range(2, 1, row - 1, 6);
			dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			worksheet.Columns().AdjustToContents();
		}
	}

	private static void AddRevenuesWorksheet(XLWorkbook workbook, IEnumerable<Revenue> revenues)
	{
		var worksheet = workbook.Worksheets.Add("Receitas");

		// Cabeçalho
		var headerRange = worksheet.Range("A1:E1");
		headerRange.Style.Font.Bold = true;
		headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
		headerRange.Style.Font.FontColor = XLColor.White;
		headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

		worksheet.Cell(1, 1).Value = "Descrição";
		worksheet.Cell(1, 2).Value = "Data Recebimento";
		worksheet.Cell(1, 3).Value = "Tipo";
		worksheet.Cell(1, 4).Value = "Valor";
		worksheet.Cell(1, 5).Value = "Recorrente";

		// Dados
		var row = 2;
		foreach (var revenue in revenues)
		{
			var currentRow = worksheet.Row(row);

			// Zebrado apenas na área de dados
			currentRow.Style.Fill.BackgroundColor = row % 2 == 0
				? XLColor.White
				: XLColor.LightGray;

			worksheet.Cell(row, 1).Value = revenue.Description;
			worksheet.Cell(row, 2).Value = revenue.ReceiptDate.ToString("d", CurrentCulture);
			worksheet.Cell(row, 3).Value = revenue.Type.ToString();
			worksheet.Cell(row, 4).Value = revenue.Amount;
			worksheet.Cell(row, 4).Style.NumberFormat.Format = CurrencyFormat;
			worksheet.Cell(row, 5).Value = revenue.Recurrence.IsRecurring ? "Sim" : "Não";

			if (revenue.Amount > 5000)
			{
				worksheet.Cell(row, 4).Style.Fill.BackgroundColor = XLColor.LightGreen;
				worksheet.Cell(row, 4).Style.Font.Bold = true;
			}

			row++;
		}

		// Formatação apenas na área de dados
		if (row > 2)
		{
			var dataRange = worksheet.Range(2, 1, row - 1, 5);
			dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			worksheet.Columns().AdjustToContents();
		}
	}

	private static void AddBalanceWorksheet(XLWorkbook workbook, IEnumerable<Expense> expenses, IEnumerable<Revenue> revenues)
	{
		var worksheet = workbook.Worksheets.Add("Saldo");

		// Cabeçalho
		var headerRange = worksheet.Range("A1:B1");
		headerRange.Style.Font.Bold = true;
		headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
		headerRange.Style.Font.FontColor = XLColor.White;
		headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		headerRange.Merge();
		worksheet.Row(1).Height = 30;

		worksheet.Cell(1, 1).Value = "Resumo Financeiro Mensal";

		// Dados
		var totalReceitas = revenues.Sum(r => r.Amount);
		var totalDespesas = expenses.Sum(e => e.Payment.TotalValue);
		var saldoFinal = totalReceitas - totalDespesas;

		var data = new List<(string Descricao, decimal Valor)>
		{
			("Total de Receitas", totalReceitas),
			("Total de Despesas", totalDespesas),
			("Saldo Final", saldoFinal)
		};

		var row = 3;
		foreach (var item in data)
		{
			worksheet.Cell(row, 1).Value = item.Descricao;
			worksheet.Cell(row, 2).Value = item.Valor;
			worksheet.Cell(row, 2).Style.NumberFormat.Format = CurrencyFormat;

			if (item.Descricao == "Saldo Final")
			{
				var color = item.Valor >= 0 ? XLColor.Green : XLColor.Red;
				worksheet.Cell(row, 2).Style.Font.FontColor = color;
				worksheet.Cell(row, 2).Style.Font.Bold = true;
				worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightYellow;
			}

			row++;
		}

		worksheet.Columns().AdjustToContents();
	}
}