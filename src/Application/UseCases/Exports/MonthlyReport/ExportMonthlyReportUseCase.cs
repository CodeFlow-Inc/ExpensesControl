using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using CodeFlow.Start.Package.Extensions;
using ExpensesControl.Application.ExcelReports;
using ExpensesControl.Application.Specs;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Response;
using ExpensesControl.Domain.Entities.AggregateRoot;
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
	public async Task<ExportMonthlyReportResponse> Handle(ExportMonthlyReportRequest request, CancellationToken cancellationToken)
	{
		var response = new ExportMonthlyReportResponse();

		if (!await validator.ValidateAndAddErrorsAsync(request, logger, response, cancellationToken))
			return response;

		var expenses = await unitOfWork.ExpenseRepository.ListBySpecificationAsync(
			new ExpenseSpec(request.UserCode, request.Month, request.Year), cancellationToken);

		var revenues = await unitOfWork.RevenueRepository.ListBySpecificationAsync(
			new RevenueSpec(request.UserCode, request.Month, request.Year), cancellationToken);

		var generator = new ExcelReportGenerator();

		AddExpensesWorksheet(generator, expenses);
		AddRevenuesWorksheet(generator, revenues);
		AddBalanceWorksheet(generator, expenses, revenues);

		response.SetResult(
			generator.GetFileBytes(),
			$"relatorio_{request.Month:00}_{request.Year}.xlsx",
			"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

		logger.LogInformation("Relatório gerado para o usuário {UserCode}", request.UserCode);
		return response;
	}

	private static void AddExpensesWorksheet(ExcelReportGenerator generator, IEnumerable<Expense> expenses)
	{
		var worksheet = generator.AddWorksheet("Despesas");
		worksheet.AddHeaders("Descrição", "Data", "Categoria", "Valor Total", "Tipo de Pagamento", "Parcelado");

		worksheet.AddDataRows(expenses, (row, expense) =>
		{
			worksheet.Cells[row, 1].Value = expense.Description;
			worksheet.Cells[row, 2].Value = expense.StartDate.ToString("d", CultureInfo.CurrentCulture);
			worksheet.Cells[row, 3].Value = expense.Category.GetDescription();
			worksheet.Cells[row, 4].Value = expense.Payment.TotalValue;
			worksheet.Cells[row, 5].Value = expense.Payment.Type.GetDescription();
			worksheet.Cells[row, 6].Value = expense.Payment.IsInstallment ? "Sim" : "Não";

			worksheet.Cells[row, 4].ApplyCurrencyFormat();
		});

		worksheet.ApplyConditionalFormatting(
			column: 4,
			condition: cell => cell.GetValue<decimal>() > 1000,
			backgroundColor: Color.LightSalmon,
			bold: true
		);
	}

	private static void AddRevenuesWorksheet(ExcelReportGenerator generator, IEnumerable<Revenue> revenues)
	{
		var worksheet = generator.AddWorksheet("Receitas");
		worksheet.AddHeaders("Descrição", "Data Recebimento", "Tipo", "Valor", "Recorrente");

		worksheet.AddDataRows(revenues, (row, revenue) =>
		{
			worksheet.Cells[row, 1].Value = revenue.Description;
			worksheet.Cells[row, 2].Value = revenue.ReceiptDate.ToString("d", CultureInfo.CurrentCulture);
			worksheet.Cells[row, 3].Value = revenue.Type.GetDescription();
			worksheet.Cells[row, 4].Value = revenue.Amount;
			worksheet.Cells[row, 5].Value = revenue.Recurrence.IsRecurring ? "Sim" : "Não";

			worksheet.Cells[row, 4].ApplyCurrencyFormat();
		});

		worksheet.ApplyConditionalFormatting(
			column: 4,
			condition: cell => cell.GetValue<decimal>() > 5000,
			backgroundColor: Color.LightGreen,
			bold: true
		);
	}

	private static void AddBalanceWorksheet(ExcelReportGenerator generator, IEnumerable<Expense> expenses, IEnumerable<Revenue> revenues)
	{
		var worksheet = generator.AddWorksheet("Saldo");
		worksheet.AddHeaders("Resumo Financeiro Mensal", "Valor");
		worksheet.Column(1).Width = 30;

		var totalReceitas = revenues.Sum(r => r.Amount);
		var totalDespesas = expenses.Sum(e => e.Payment.TotalValue);
		var saldoFinal = totalReceitas - totalDespesas;

		var data = new List<dynamic>
		{
			new { Descricao = "Total de Receitas", Valor = totalReceitas },
			new { Descricao = "Total de Despesas", Valor = totalDespesas },
			new { Descricao = "Saldo Final", Valor = saldoFinal }
		};

		worksheet.AddDataRows(data, (row, item) =>
		{
			worksheet.Cells[row, 1].Value = item.Descricao;
			worksheet.Cells[row, 2].Value = item.Valor;
			worksheet.Cells[row, 2].ApplyCurrencyFormat();

			if (item.Descricao == "Saldo Final")
			{
				var color = item.Valor >= 0 ? Color.Green : Color.Red;
				worksheet.Cells[row, 2].Style.Font.Color.SetColor(color);
				worksheet.Cells[row, 2].Style.Font.Bold = true;
				worksheet.ApplyZebraStriping(row, headersCount: 2);
				worksheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
			}
		}, applyZebraStriping: false);
	}
}