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
	private static readonly CultureInfo BrazilianCulture = new("pt-BR");

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
		var headerStyle = CreateHeaderStyle(workbook);
		var dataStyle = CreateDataStyle(workbook);

		// Cabeçalho
		worksheet.Cell(1, 1).Value = "Descrição";
		worksheet.Cell(1, 2).Value = "Data";
		worksheet.Cell(1, 3).Value = "Categoria";
		worksheet.Cell(1, 4).Value = "Valor Total";
		worksheet.Cell(1, 5).Value = "Tipo de Pagamento";
		worksheet.Cell(1, 6).Value = "Parcelado";
		worksheet.Range("A1:F1").Style = headerStyle;

		// Dados
		var row = 2;
		foreach (var expense in expenses)
		{
			worksheet.Cell(row, 1).Value = expense.Description;
			worksheet.Cell(row, 2).Value = expense.StartDate.ToString("dd/MM/yyyy", BrazilianCulture);
			worksheet.Cell(row, 3).Value = expense.Category.ToString();
			worksheet.Cell(row, 4).Value = expense.Payment.TotalValue;
			worksheet.Cell(row, 4).Style.NumberFormat.Format = CurrencyFormat;
			worksheet.Cell(row, 5).Value = expense.Payment.Type.ToString();
			worksheet.Cell(row, 6).Value = expense.Payment.IsInstallment ? "Sim" : "Não";

			if (expense.Payment.TotalValue > 1000)
				worksheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.LightYellow);

			row++;
		}

		// Formatação
		if (row > 2)
		{
			worksheet.Range(2, 1, row - 1, 6).Style = dataStyle;
			worksheet.Columns().AdjustToContents();
		}
	}

	private static void AddRevenuesWorksheet(XLWorkbook workbook, IEnumerable<Revenue> revenues)
	{
		var worksheet = workbook.Worksheets.Add("Receitas");
		var headerStyle = CreateHeaderStyle(workbook);
		var dataStyle = CreateDataStyle(workbook);

		// Cabeçalho
		worksheet.Cell(1, 1).Value = "Descrição";
		worksheet.Cell(1, 2).Value = "Data Recebimento";
		worksheet.Cell(1, 3).Value = "Tipo";
		worksheet.Cell(1, 4).Value = "Valor";
		worksheet.Cell(1, 5).Value = "Recorrente";
		worksheet.Range("A1:E1").Style = headerStyle;

		// Dados
		var row = 2;
		foreach (var revenue in revenues)
		{
			worksheet.Cell(row, 1).Value = revenue.Description;
			worksheet.Cell(row, 2).Value = revenue.ReceiptDate.ToString("dd/MM/yyyy", BrazilianCulture);
			worksheet.Cell(row, 3).Value = revenue.Type.ToString();
			worksheet.Cell(row, 4).Value = revenue.Amount;
			worksheet.Cell(row, 4).Style.NumberFormat.Format = CurrencyFormat;
			worksheet.Cell(row, 5).Value = revenue.Recurrence.IsRecurring ? "Sim" : "Não";

			if (revenue.Amount > 5000)
				worksheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.LightGreen);

			row++;
		}

		// Formatação
		if (row > 2)
		{
			worksheet.Range(2, 1, row - 1, 5).Style = dataStyle;
			worksheet.Columns().AdjustToContents();
		}
	}

	private static void AddBalanceWorksheet(XLWorkbook workbook, IEnumerable<Expense> expenses, IEnumerable<Revenue> revenues)
	{
		var worksheet = workbook.Worksheets.Add("Saldo");
		var headerStyle = CreateHeaderStyle(workbook);
		var totalReceitas = revenues.Sum(r => r.Amount);
		var totalDespesas = expenses.Sum(e => e.Payment.TotalValue);
		var saldoFinal = totalReceitas - totalDespesas;

		// Título
		var titleCell = worksheet.Cell("A1");
		titleCell.Value = "Resumo Financeiro Mensal";
		titleCell.Style = headerStyle;
		worksheet.Range("A1:B1").Merge();
		worksheet.Row(1).Height = 30;

		// Dados
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
				worksheet.Cell(row, 2).Style.Font.SetFontColor(color);
				worksheet.Cell(row, 2).Style.Font.SetBold(true);
			}

			row++;
		}

		worksheet.Columns().AdjustToContents();
	}

	private static IXLStyle CreateHeaderStyle(XLWorkbook workbook)
	{
		var style = workbook.Style;
		style.Font.SetBold(true);
		style.Fill.SetBackgroundColor(XLColor.DarkBlue);
		style.Font.SetFontColor(XLColor.White);
		style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
		style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
		style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
		return style;
	}

	private static IXLStyle CreateDataStyle(XLWorkbook workbook)
	{
		var style = workbook.Style;
		style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
		style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
		style.Fill.SetBackgroundColor(XLColor.LightGray);
		return style;
	}
}